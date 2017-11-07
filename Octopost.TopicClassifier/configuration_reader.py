import json

def read_configuration():
    with open('configuration.json') as data_file:    
        return json.load(data_file)
