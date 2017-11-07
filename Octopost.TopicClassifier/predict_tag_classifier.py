import configuration_reader
import train_tag_classifier

import pandas
import keras
import sys

if __name__ == '__main__':
    configuration = configuration_reader.read_configuration()
    (_, _, _, _, _, tokenizer) = train_tag_classifier.get_data()
    model_path = configuration['modelFileName']

    model = keras.models.load_model(model_path)
    
    while True:
        text = input('Enter the text: ')
        text_to_predict = pandas.Series([text])
        tokenizer.fit_on_texts(text_to_predict)
        text_to_predict_sequences = tokenizer.texts_to_sequences(text_to_predict)
        to_predict = tokenizer.sequences_to_matrix(text_to_predict_sequences, mode='binary')
        prediction = model.predict(to_predict)
        print(prediction)
        predicted_class = model.predict_classes(to_predict)
        print(predicted_class)
