#a)Sa se stabileasca:

import pandas as pd
#incarcam datele
data = pd.read_csv('surveyDataSience.csv', low_memory=False, skiprows=[1])

#numarul de respondenti (de la care s-au colectate informatiile)
number_of_participants = data.shape[0]
print("Number of Participants: ", number_of_participants)


# x = data[data["Q3"]]

#numar si tipul informatiilor (atributelor, proprietatilor) detinute pentru un respondent
number_of_attributes = data.shape[1]
print("Number of Attributes: ", number_of_attributes)

#numarul de respondenti pentru care se detin date complete
complete_data = data.dropna(axis=0, how='any') #se putea lasa si () -> by default parametrii sunt cei pusi explicit
number_of_participants_complete_data = complete_data.shape[0]
print("Number of Participants Complete: ", number_of_participants_complete_data)
print()

'''
- durata medie a anilor de studii superioare pentru acesti respondenti (cea efectiva
sau cea estimata)
'''
#se foloseste un map pentru a inlocui numele ultimei diplome obtinute cu un nr de ani
mapping = {
    "Bachelor’s degree": 3,
    "Master’s degree": 5,
    "Doctoral degree": 8
}

data['studii_superioare_numeric'] = data['Q4'].map(mapping)
average_uni_years = data['studii_superioare_numeric'].mean()
print("Durata medie a anilor de studii superioare pentru toti respondentii:",average_uni_years)

'''
- durata medie a anilor de studii pentru respondentii din Romania
'''
#obtinem respondentii din Romania
romanian_data = data[data['Q3'] == 'Romania']
print("Durata medie a anilor de studii superioare pentru respondentii din Romania:",romanian_data['studii_superioare_numeric'].mean())

'''
- durata medie a anilor de studii pentru respondentii din Romania care sunt femei.
'''
romanian_women_data = romanian_data[romanian_data['Q2'] == 'Woman']
print("Durata medie a anilor de studii superioare pentru respondentii din Romania care sunt femei:",romanian_women_data['studii_superioare_numeric'].mean())
print()

#numarul de respondenti femei din Romania pentru care se detin date complete
nr_complete_data_female_romania = romanian_women_data.dropna().shape[0]
print("Numarul de respondenti femei din Romania pentru care se detin date complete:",nr_complete_data_female_romania)
print()

'''
- numarul de femei din Romania care programeaza in Python
- intervalul de varsta cu cele mai multe femei care programeaza in Python? 
- Dar in C++? 

Comparati rezultatele obtinute pentru cele doua limbaje de programare.
'''
romanian_women_python_data = romanian_women_data[romanian_women_data['Q7_Part_1'] == 'Python']
print("Numarul de femei din Romania care programeaza in Python:", romanian_women_python_data.shape[0])

python_programmers = data[data['Q7_Part_1'] == 'Python']
women_python_programmers = python_programmers[python_programmers['Q2'] == 'Woman']

if women_python_programmers.shape[0] > 0:
    aux_data = (women_python_programmers
                .groupby('Q1')
                .agg(count=('Q1','count'))
                .reset_index() #pentru ca altfel nu-l mai vede pe Q1 coloana -> ci doar indexul dupa care s-a realizat gruparea
                )
    maxim = aux_data['count'].max()
    print("Intervalul de varsta cu cele mai multe femei ce programeaza in Python:",aux_data[aux_data['count'] == maxim].Q1.iloc[0])


c_plus_programmers = data[data['Q7_Part_1'] == 'C++']
women_c_plus_programmers = c_plus_programmers[c_plus_programmers['Q2'] == 'Woman']

if women_c_plus_programmers.shape[0] > 0:
    aux_data = (women_c_plus_programmers
                .groupby('Q1')
                .agg(count=('Q1','count'))
                .reset_index() #pentru ca altfel nu-l mai vede pe Q1 coloana -> ci doar indexul dupa care s-a realizat gruparea
                )
    maxim = aux_data['count'].max()
    print("Intervalul de varsta cu cele mai multe femei ce programeaza in C++:",aux_data[aux_data['count'] == maxim].Q1.iloc[0])
print()

'''
domeniul de valori posibile si valorile extreme pentru fiecare atribut/proprietate (feature).
In cazul proprietatilor nenumerice, cate valori posibile are fiecare astfel de proprietate
'''

#se vor selecta proprietatile numerice
numeric_features = data.select_dtypes(include='number')
extreme_values = numeric_features.agg(['min', 'max'])
print(extreme_values)

#se va realiza analiza pentru proprietatile nenumerice
nonnumerical_features = data.select_dtypes(exclude='number')

for col in nonnumerical_features:
    print(col, "->", data[col].nunique()) #nunique -> numara cate inregistrari unice sunt
print()

'''
- transformati informatiile despre vechimea in programare in numar de ani (folositi in locul intervalului, mijlocul acestuia)

- calculati momentele de ordin 1 si 2 pentru aceasta variabila (minim, maxim, media, deviatia standard, mediana). 

Ce se poate spune despre aceasta variabila?
'''
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
programming_years = data['Q6']

print("Momentele de ordin 1 si 2 pentru vechimea in programare:")
print("Minim:",programming_years.min())
print("Maxim:",programming_years.max())
print("Media:",programming_years.mean())
print("Deviatia standard:",programming_years.std())
print("Mediana:",programming_years.median())


#b)Sa se vizualizeze:
import matplotlib.pyplot as plt

#distributia respondentilor care programeaza in Python pe categorii de varsta
python_programmers = data[data['Q7_Part_1'] == 'Python']

python_programmers['Q1'].value_counts().plot(kind='bar')
plt.xlabel('Categorii de varsta')
plt.ylabel('Frecventa')
plt.title('Python & Ages Histogram')
plt.show()

#distributia respondentilor din Romania care programeaza in Python pe categorii de varsta
romanian_python_programmers = data[data['Q3'] == 'Romania']

romanian_python_programmers['Q1'].value_counts().plot(kind='bar')
plt.xlabel('Categorii de varsta')
plt.ylabel('Frecventa')
plt.title('Romanian Python Programmers & Ages Histogram')
plt.show()

#distributia respondentilor femei din Romania care programeaza in Python pe categorii de varsta

romanian_women_python_data['Q1'].value_counts().plot(kind='bar')
plt.xlabel('Categorii de varsta')
plt.ylabel('Frecventa')
plt.title('Romanian Women Python Programmers & Ages Histogram')
plt.show()

'''
respondentii care pot fi considerati "outlieri" din punct de vedere al vechimii in programare
(puteti folositi un boxplot pentru a identifica aceste valori)
'''
plt.boxplot(data['Q6'])
plt.title("Programming Experience Distribution")
plt.ylabel("Years")
plt.show()