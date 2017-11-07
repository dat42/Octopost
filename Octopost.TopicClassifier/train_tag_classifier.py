# Import configuration reader
import configuration_reader

# Import libraries
import pandas
import keras
from pprint import pprint

# Import TensorFlow
import tensorflow as tf

# Import keras dependencies
from keras.preprocessing.text import Tokenizer
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import Embedding
from keras.layers import Activation
from keras.layers import Dropout
from keras.layers import GRU

# Read configuration
configuration = configuration_reader.read_configuration()
training_configuration = configuration['train']
MAX_DOCUMENT_LENGTH = training_configuration['max_document_length']
MAX_SIZE = training_configuration['max_size']
LOSS_FUNCTION = training_configuration['loss']
TENSORBOARD_DIR = configuration['tensorboard']['dir']
ACTIVATION_FUNCTION = training_configuration['activation_function']
EMBEDDING_SIZE = training_configuration['embedding_size']
LSTM_UNITS = training_configuration['lstm_units']
RECURRENT_DROPOUT = training_configuration['recurrent_dropout']
DROPOUT = training_configuration['dropout']
BATCH_SIZE = training_configuration['batchSize']
EPOCHS = training_configuration['epochs']
WORDS_FEATURE = training_configuration['wordsFeature']
MODEL_FILE_NAME = configuration['modelFileName']
LEARNING_RATE = training_configuration['learning_rate']


def get_data():
    tf.logging.set_verbosity(tf.logging.INFO)

    dbpedia = tf.contrib.learn.datasets.load_dataset('dbpedia', size='huuuuge', test_with_fake_data=False)
    x_train = pandas.Series(dbpedia.train.data[:, 1])
    y_train = pandas.Series(dbpedia.train.target)
    x_test = pandas.Series(dbpedia.test.data[:, 1])
    y_test = pandas.Series(dbpedia.test.target)

    tokenizer = Tokenizer()
    tokenizer.fit_on_texts(x_train)

    x_train_sequences = tokenizer.texts_to_sequences(x_train)
    x_test_sequences = tokenizer.texts_to_sequences(x_test)

    tokenizer = Tokenizer(num_words=MAX_DOCUMENT_LENGTH)
    X_train = tokenizer.sequences_to_matrix(x_train_sequences, mode='binary')
    X_test = tokenizer.sequences_to_matrix(x_test_sequences, mode='binary')

    num_classes = len(y_test)
    num_classes_unique = len(y_test.unique())
    print(u'Number of classes: {}'.format(num_classes_unique))

    y_train = keras.utils.to_categorical(y_train, num_classes)
    y_test = keras.utils.to_categorical(y_test, num_classes)
    print('y_train shape:', y_train.shape)
    print('y_test shape:', y_test.shape)
    return (X_train, y_train, X_test, y_test, num_classes, tokenizer)


if __name__ == '__main__':
    (X_train, y_train, X_test, y_test, num_classes, tokenizer) = get_data()

    model = Sequential()
    model.add(Embedding(MAX_SIZE, EMBEDDING_SIZE))
    model.add(GRU(LSTM_UNITS, recurrent_dropout=RECURRENT_DROPOUT))
    model.add(Dropout(DROPOUT))
    model.add(Dense(num_classes))
    model.add(Activation(ACTIVATION_FUNCTION))

    model.compile(loss=LOSS_FUNCTION,
                  optimizer=keras.optimizers.Adam(LEARNING_RATE),
                  metrics=['accuracy'])

    print(model.summary())

    tensorboard_callback = keras.callbacks.TensorBoard(log_dir=TENSORBOARD_DIR,
                                                       histogram_freq=1,
                                                       write_graph=True)

    history = model.fit(X_train, y_train,
                        batch_size=BATCH_SIZE,
                        epochs=EPOCHS,
                        shuffle=True,
                        validation_data=(X_test, y_test),
                        callbacks=[tensorboard_callback])

    score = model.evaluate(X_test, y_test,
                           batch_size=BATCH_SIZE, verbose=1)

    model.save(MODEL_FILE_NAME)

    print(u'Loss: {}'.format(score[0]))
    print(u'Accuracy: {}'.format(score[1]))

    def _predict(text):
        text_to_predict = pandas.Series([text])
        tokenizer.fit_on_texts(text_to_predict)
        text_to_predict_sequences = tokenizer.texts_to_sequences(text_to_predict)
        to_predict = tokenizer.sequences_to_matrix(text_to_predict_sequences, mode='binary')
        prediction = model.predict(to_predict)
        predicted_class_index = prediction.argmax(axis=1)
        pprint(predicted_class_index)

    while True:
        text_input = input('Enter: ')
        _predict(text_input)
