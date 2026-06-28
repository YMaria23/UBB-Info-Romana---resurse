#Sa se normalizeze informatiile de la problema 1 si 2 folosind diferite metode de normalizare astfel:
from math import log

#problema 1 - durata anilor de studii universitare, vechimea in programare
import pandas as pd
import matplotlib.pyplot as plt
from nltk import FreqDist
import numpy as np

#incarcam datele
data = pd.read_csv('surveyDataSience.csv', low_memory=False, skiprows=[1])

mapping = {
    "Bachelor’s degree": 3,
    "Master’s degree": 5,
    "Doctoral degree": 8
}

data['studii_superioare_numeric'] = data['Q4'].map(mapping)
values = data['studii_superioare_numeric']
min_max_values = (values - values.min()) / (values.max() - values.min())

plt.figure(figsize=(10, 6))

plt.subplot(1, 2, 1)
data['studii_superioare_numeric'].value_counts().plot(kind='bar',title="Before",color="gray")
plt.xlabel('Durata studii universitare')
plt.ylabel('Frecventa')

plt.subplot(1, 2, 2)
min_max_values.value_counts().plot(kind='bar',title="After Min-Max")
plt.xlabel('Durata studii universitare')
plt.ylabel('Frecventa')
plt.show()



import re
def transform(value):
    if "<" in value:
        return 1
    if "+" in value:
        return 20
    if "-" in value:
        numbers = re.findall(r'\d+', value)
        a = int(numbers[0])
        b = int(numbers[1])

        return (a + b)/2
    return 0

data['Q6']=data['Q6'].apply(transform)
values = data['Q6']
clipping_values = values.clip(upper = 10)
min_max_values = (values - values.min()) / (values.max() - values.min())

plt.figure(figsize=(20, 6))

plt.subplot(1, 3, 1)
plt.hist(data['Q6'],rwidth=0.8)
plt.xlabel('Vechimea in programare')
plt.ylabel('Frecventa')
plt.title('Before')

plt.subplot(1, 3, 2)
plt.hist(clipping_values,rwidth=0.8)
plt.xlabel('Vechimea in programare')
plt.ylabel('Frecventa')
plt.title('After clipping')

plt.subplot(1, 3, 3)
plt.hist(min_max_values,rwidth=0.8)
plt.xlabel('Vechimea in programare')
plt.ylabel('Frecventa')
plt.title('After min-max')
plt.show()


#problema 2 - valorile pixelilor din imagini

from PIL import Image
import matplotlib.pyplot as plt
import numpy as np

img = Image.open("images/Karpaty.jpg").convert('L')
pixels = np.array(img)
mean = np.mean(pixels)
std = np.std(pixels)
z_pixels = np.array([(p-mean)/std for p in pixels])

plt.figure(figsize=(10, 4))

plt.subplot(1, 2, 1)
plt.hist(pixels.flatten(), bins=256, color='gray')
plt.title("Original Histogram")

plt.subplot(1, 2, 2)
plt.hist(z_pixels.flatten(), bins=256, color='blue')
plt.title("Standardized Histogram")

plt.show()

#problema 3 - numarul de aparitii a cuvintelor la nivelul unei propozitii.
import nltk
import unicodedata
import unidecode

with open("texts.txt", "r", encoding="utf-8") as f:
    text = f.read()

tokens= nltk.word_tokenize(text)
words = [w.lower() for w in tokens if w.isalpha()]
unique_words = set(words)
fdist = FreqDist(words)
#se creeaza un nou obiect -> va contine datele modificate
logged_fdist = FreqDist()

for word, count in fdist.items():
        logged_fdist[word] = np.log1p(count)


plt.figure(figsize=(10, 6))

plt.subplot(1, 2, 1)
fdist.plot(20, title="Top 20 cele mai frecvente cuvinte")

plt.subplot(1, 2, 2)
logged_fdist.plot(20, title="Top 20 cele mai frecvente cuvinte -> dupa log")
plt.show()