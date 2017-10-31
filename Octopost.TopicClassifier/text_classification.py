from __future__ import absolute_import
from __future__ import division
from __future__ import print_function
import argparse
import sys
from sklearn import metrics

import numpy as np
import pandas
import tensorflow as tf

from flask import Flask
from flask import jsonify
from flask import request

FLAGS = None
MAX_DOCUMENT_LENGTH = 10
EMBEDDING_SIZE = 50
n_words = 0
MAX_LABEL = 15
WORDS_FEATURE = 'words'


def estimator_spec_for_softmax_classification(logits, labels, mode):
    predicted_classes = tf.argmax(logits, 1)
    if mode == tf.estimator.ModeKeys.PREDICT:
        return tf.estimator.EstimatorSpec(
            mode=mode,
            predictions={
                'class': predicted_classes,
                'prob': tf.nn.softmax(logits)
            })

    one_hot_labels = tf.one_hot(labels, MAX_LABEL, 1, 0)
    loss = tf.losses.softmax_cross_entropy(onehot_labels=one_hot_labels, logits=logits)
    if mode == tf.estimator.ModeKeys.TRAIN:
        optimizer = tf.train.AdamOptimizer(learning_rate=0.075)
        train_op = optimizer.minimize(loss, global_step=tf.train.get_global_step())
        return tf.estimator.EstimatorSpec(mode, loss=loss, train_op=train_op)

    eval_metric_ops = {
        'accuracy': tf.metrics.accuracy(
            labels=labels, predictions=predicted_classes)
    }
    return tf.estimator.EstimatorSpec(
        mode=mode, loss=loss, eval_metric_ops=eval_metric_ops)


def rnn_model(features, labels, mode):
    word_vectors = tf.contrib.layers.embed_sequence(
        features[WORDS_FEATURE],
        vocab_size=n_words,
        embed_dim=EMBEDDING_SIZE)
    word_list = tf.unstack(word_vectors, axis=1)
    cell = tf.contrib.rnn.GRUCell(EMBEDDING_SIZE)
    _, encoding = tf.contrib.rnn.static_rnn(cell, word_list, dtype=tf.float32)
    logits = tf.layers.dense(encoding, MAX_LABEL, activation=None)
    return estimator_spec_for_softmax_classification(
        logits=logits, labels=labels, mode=mode)


def main(args):
    global n_words
    if (FLAGS.test_with_fake_data):
        print('Testing with fake data...')
    dbpedia = tf.contrib.learn.datasets.load_dataset('dbpedia', test_with_fake_data=FLAGS.test_with_fake_data)
    x_train = pandas.Series(dbpedia.train.data[:, 1])
    y_train = pandas.Series(dbpedia.train.target)
    x_test = pandas.Series(dbpedia.test.data[:, 1])
    y_test = pandas.Series(dbpedia.test.target)

    vocab_processor = tf.contrib.learn.preprocessing.VocabularyProcessor(MAX_DOCUMENT_LENGTH)

    x_transform_train = vocab_processor.fit_transform(x_train)
    x_transform_test = vocab_processor.transform(x_test)

    x_train = np.array(list(x_transform_train))
    x_test = np.array(list(x_transform_test))

    n_words = len(vocab_processor.vocabulary_)
    print('Total words: %d' % n_words)

    # Build model
    model_fn = rnn_model
    classifier = tf.estimator.Estimator(model_fn=model_fn, model_dir='./model')

    # Train.
    if not FLAGS.server:
        print('Training model...')
        summary_hook = tf.train.SummarySaverHook(
            save_secs=2,
            output_dir='./data',
            scaffold=tf.train.Scaffold())
        train_input_fn = tf.estimator.inputs.numpy_input_fn(
            x={WORDS_FEATURE: x_train},
            y=y_train,
            batch_size=len(x_train),
            num_epochs=None,
            shuffle=True)

        classifier.train(input_fn=train_input_fn, steps=2000, hooks=[summary_hook])

    # Predict.
    test_input_fn = tf.estimator.inputs.numpy_input_fn(
        x={WORDS_FEATURE: x_test},
        y=y_test,
        num_epochs=7,
        shuffle=False)

    # Score with tensorflow.
    scores = classifier.evaluate(input_fn=test_input_fn, steps=10000)
    print('Accuracy (tensorflow): {0:f}'.format(scores['accuracy']))
    def _predict(text):
        text_to_predict = text
        text_to_predict = pandas.Series(text_to_predict)
        text_to_predict = vocab_processor.fit_transform(text_to_predict)
        text_to_predict = np.array(list(text_to_predict))
        test_fn = tf.estimator.inputs.numpy_input_fn(
            x={WORDS_FEATURE: text_to_predict},
            num_epochs=1,
            shuffle=False)
        prediction = list(classifier.predict(input_fn=test_fn))
        predicted_classes = [p['class'] for p in prediction]
        return predicted_classes
    return _predict


def get_class_dictionary():
    class_map = {}
    classes_file = open('./dbpedia_data/dbpedia_csv/classes.txt', 'r')
    i = 1
    while True:
        line = classes_file.readline()
        if not line: break
        class_map[i] = line.replace('\n', '')
        i += 1
    return class_map


def start_server():
    print('Starting web server...')
    f = main([])
    class_map = get_class_dictionary()
    app = Flask(__name__)

    @app.route('/classes')
    def get_classes():
        return jsonify(class_map)

    @app.route('/prediction')
    def predict_class():
        text = request.args['text']
        prediction = f(text)
        print(prediction)
        first = prediction[0]
        return_obj = {}
        return_obj['prediction_value'] = first.item()
        return_obj['prediction_text'] = class_map[first.item()]
        return jsonify(return_obj)

    app.run(host='0.0.0.0', port=9001)


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument(
        '--test_with_fake_data',
        default=False,
        help='Test the example code with fake data.',
        action='store_true')
    parser.add_argument(
        '--server',
        default=False,
        help='Determines whether to run the server')
    FLAGS, unparsed = parser.parse_known_args()
    if FLAGS.server:
        start_server()
    else:
        tf.app.run(main=main, argv=[sys.argv[0]] + unparsed)
