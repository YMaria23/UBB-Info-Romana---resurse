import csv
import os
import random
import re
from contextlib import nullcontext
import gensim


def data_aumentation(message,model,nr_of_modified_words = 3):
    # tokenizare
    words = re.split(r'[ .,\n()!?;]+',message)
    new_words = words.copy()

    # selectia cuvintelor tinta (practic eliminarea cuvintelor de legatura)

    candidates = [word for word in words if len(word) > 3]
    if len(candidates) < nr_of_modified_words:
        return message

    selected_words = random.sample(candidates,nr_of_modified_words)

    # parcurgerea cuvintelor pe care urmeaza sa le inlocuim
    nr_of_modified_words = 0
    for word in selected_words:
        # determinarea indexului
        index = words.index(word)

        # determinarea celui mai similar vecin
        # se verifica mai intai daca exista cuvantul respectiv in dictionar
        if word in model:
            similar_words = model.most_similar(word,topn=1)
            new_word = similar_words[0][0]
            nr_of_modified_words += 1
        else:
            print(f"Cuvantul '{word}' nu a fost gasit in vocabular. Trecem mai departe.")
            continue

        new_words[index] = new_word

    new_message = ' '.join(new_words)
    return  new_message if nr_of_modified_words > 1 else message


def data_balancing():
    # incarcarea datelor
    crtDir =  os.getcwd()
    fileName = os.path.join(crtDir, '', 'reviews_mixed.csv')

    data = []
    with open(fileName) as csv_file:
        csv_reader = csv.reader(csv_file, delimiter=',')
        line_count = 0
        for row in csv_reader:
            if line_count == 0:
                dataNames = row
            else:
                data.append(row)
            line_count += 1

    inputs = [data[i][0] for i in range(len(data))]
    outputs = [data[i][1] for i in range(len(data))]
    labelNames = list(set(outputs))

    # se verifica distributia
    distribution = []

    for labelName in labelNames:
        distribution.append([outputs.count(labelName), labelName])

    max_pair = max(distribution)

    max_distribution = max_pair[0]
    max_label = max_pair[1]

    # initializarea modelului word2vec
    modelPath = os.path.join(crtDir, '', 'GoogleNews-vectors-negative300.bin')
    word2vecModel300 = gensim.models.KeyedVectors.load_word2vec_format(modelPath, binary=True)

    with open(fileName, mode='a', encoding='utf-8', newline='') as f:
        writer = csv.writer(f)
        for labelName in labelNames:
            if labelName != max_label:
                # trebuie aduse la acelasi nr de exemple ca label-ul cu cele mai multe inregistrari
                # se va face acest lucru prin augmentare
                all_examples = [inputt for inputt,outputt in zip(inputs, outputs) if outputt == labelName]

                # cate exemple mai trebuie sa "generez" pana ajung la nr dorit
                nr_of_wanted_examples = max_distribution - len(all_examples)

                for index in range(nr_of_wanted_examples):
                    # partea efectiva de augmentare
                    original = random.choice(all_examples)
                    augmented = data_aumentation(original,word2vecModel300)

                    if augmented != original:
                        # inseamna ca putem sa il adaugam la setul de date
                        writer.writerow([augmented,labelName])


data_balancing()