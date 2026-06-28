'''
Se da un fisier care contine un text (format din mai multe propozitii) in limba romana - a se vedea fisierul ”data/texts.txt”.
Se cere sa se determine si sa se vizualizeze:
'''

import nltk
import unicodedata
import unidecode

with open("texts.txt", "r", encoding="utf-8") as f:
    text = f.read()

#numarul de propozitii din text;
def nr_of_sentances(text):
    sentences = nltk.sent_tokenize(text)
    return len(sentences)

print(f"Sunt {nr_of_sentances(text)} propozitii in text")

#numarul de cuvinte din text
def nr_of_words(text):
    tokens= nltk.word_tokenize(text)
    words = [w.lower() for w in tokens if w.isalpha() ] #se pastreaza numai cuvintele (isalpha() -> verifica daca toate carac. din sir sunt litere)
    return len(words)

print(f"Sunt {nr_of_words(text)} cuvinte in text")

#numarul de cuvinte diferite din text
def nr_of_unique_words(text):
    tokens = nltk.word_tokenize(text)
    words = [w.lower() for w in tokens if w.isalpha()]
    unique_words = set(words)
    return len(unique_words)

print(f"Sunt {nr_of_unique_words(text)} cuvinte diferite in text")
print()

#cel mai scurt si cel mai lung cuvant (cuvinte)
#se determina lungimea minima/maxima
def shortest_words(text):
    tokens = nltk.word_tokenize(text)
    words = [w.lower() for w in tokens if w.isalpha()]
    unique_words = set(words)

    minim = min([len(w) for w in unique_words])
    shortest_words = [w for w in unique_words if len(w) == minim]

    return shortest_words

def longest_words(text):
    tokens = nltk.word_tokenize(text)
    words = [w.lower() for w in tokens if w.isalpha()]
    unique_words = set(words)

    maxim = max([len(w) for w in unique_words])
    longest_words = [w for w in unique_words if len(w) == maxim]

    return longest_words


print("Cele mai scurte cuvinte sunt:")
print(shortest_words(text))

print("Cele mai lungi cuvinte sunt:")
print(longest_words(text))
print()

#textul fara diacritice
def text_fara_diacritice(text):
    text_normalizat = unicodedata.normalize("NFD",text)
    text_fara_diacritice = "".join([w for w in text_normalizat if unicodedata.category(w)!="Mn"])
    return text_fara_diacritice

print("Textul fara diacritice:")
print(text_fara_diacritice(text))
print()

#sinonimele celui mai lung cuvant din text
import re
from nltk.corpus import wordnet as wn
import stanza
stanza.download('ro')
nlp = stanza.Pipeline('ro', processors='tokenize,pos,lemma')

def synonymss(text):
    #normalizarea cuvantului
    if len(longest_words(text)) > 0:
        longest_word =re.sub(r'(.)\1+', r'\1', longest_words(text)[0].lower())

        synonyms = []

        #lematizarea cuvantului (ex: nu imi gaseste in dictionar "confirm", dar poate imi gaseste "confirma")
        doc = nlp(longest_word)
        final_word = ""
        for sent in doc.sentences:
            for word in sent.words:
                final_word = word.lemma
                break

        #gaseste efectiv sinonimele
        synsets = wn.synsets(final_word,lang='ron')
        for set in synsets:
            for cuv in set.lemma_names('ron'):
                if cuv != final_word:
                    synonyms.append(cuv)

        return synonyms
    return []

print(f"Sinonimele celui mai lung cuvant din text sunt:")
print(synonymss(text))



#TESTE
def teste():
    assert nr_of_sentances("Afara este cald. Ana vrea sa se joace cu Maia") == 2
    assert nr_of_sentances("afara ploua") == 1
    assert nr_of_sentances("vrea la mare. Mihai vrea la munte. Am mers si la mare si la munte!") == 3

    assert nr_of_words("afara ploua") == 2
    assert nr_of_words("Afara este cald. Ana vrea sa se joace, cu Maia") == 10
    assert nr_of_words("") == 0

    assert nr_of_unique_words("") == 0
    assert nr_of_unique_words("afara este foarte foarte cald") == 4

    assert shortest_words("afara am gasit un melc") == ["am","un"]
    assert shortest_words("afara este o garoafa") == ["o"]

    assert longest_words("afara am vazut-o pe Maria") == ["afara","maria"]
    assert longest_words("afara este rece") == ["afara"]

    assert text_fara_diacritice("Costinești este locul în care toată vara se distrează tinerii.") == "Costinesti este locul in care toata vara se distreaza tinerii."
    assert text_fara_diacritice("") == ""

    assert synonymss("Confirmaaaaaaa tata") == ['demonstra', 'dovedi', 'proba', 'stabili', 'susține', 'adeveri', 'corobora', 'întări', 'consacra', 'consfinți', 'sancționa', 'întări', 'întări', 'ratifica', 'valida']

teste()