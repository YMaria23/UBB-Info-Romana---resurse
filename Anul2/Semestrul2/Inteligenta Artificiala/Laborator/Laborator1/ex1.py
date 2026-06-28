#Să se determine ultimul (din punct de vedere alfabetic) cuvânt care poate apărea într-un text care conține mai multe
#cuvinte separate prin ” ” (spațiu).
#De ex. ultimul (dpdv alfabetic) cuvânt din ”Ana are mere rosii si galbene” este cuvântul "si".


'''
    Functia determina cel mai mare cuvand d.p.d.v. alfabetic
    param: text -> sir de caractere
    return: cuvantul cerut -> sir de caractere

    complexitate de timp: O(n) -> n = nr de caractere din text
    complexitate de spatiu: O(m) (d.p.d.v. al spatiului auxiliar utilizat) m = nr de cuvinte din text
'''

def solutie1(text):
    cuvinte = text.split() #O(n) -> n reprezentand nr de caractere din text

    ultimul = ""
    for i in cuvinte: #O(m) -> m = nr de cuvinte din text
        if ultimul.upper() < i.upper():
            ultimul = i

    return ultimul


'''
    Functia determina cel mai mare cuvand d.p.d.v. alfabetic
    param: text -> sir de caractere
    return: cuvantul cerut -> sir de caractere

    complexitate de timp: O(n) -> n = nr de caractere din text
    complexitate de spatiu: O(1) (d.p.d.v. al spatiului auxiliar utilizat)
'''
def solutie2(text):
    ultimul=""
    cuvant = ""
    for caracter in text:
        if caracter == ' ':
            if cuvant != "" and cuvant.upper() > ultimul.upper():
                ultimul = cuvant
            cuvant = ""
        else:
            cuvant += caracter

    if cuvant != "" and cuvant.upper() > ultimul.upper():
        ultimul = cuvant

    return ultimul


# TESTE
assert(solutie1("Ana are mere si pere") == "si")
assert(solutie1("Ana are  mere    si  pere") == "si")
assert(solutie1(" ") == "")
assert(solutie1("Ana are mere si pere si multe zvonuri") == "zvonuri")
assert(solutie1("ana aba ara aZa")=="aZa")

assert(solutie2("Ana are mere si pere") == "si")
assert(solutie2("Ana are  mere    si  pere") == "si")
assert(solutie2(" ") == "")
assert(solutie2("Ana are mere si pere si multe zvonuri") == "zvonuri")
assert(solutie2("ana aba ara aZa ")=="aZa")


#SOLUTIE CHAT GPT -> foarte asemanatoare cu solutia2
def ultimul_cuvant_alfabetic(text):
    maxim = ""
    cuvant_curent = ""

    for ch in text:
        if ch != " ":
            cuvant_curent += ch
        else:
            if cuvant_curent:
                if cuvant_curent.lower() > maxim.lower():
                    maxim = cuvant_curent
                cuvant_curent = ""

    # verificăm ultimul cuvânt (dacă textul nu se termină cu spațiu)
    if cuvant_curent:
        if cuvant_curent.lower() > maxim.lower():
            maxim = cuvant_curent

    return maxim

# Exemplul din enunț
assert ultimul_cuvant_alfabetic("Ana are mere rosii si galbene") == "si"

# Un singur cuvânt
assert ultimul_cuvant_alfabetic("banana") == "banana"

# Majuscule vs minuscule
assert ultimul_cuvant_alfabetic("Zebra ana mere") == "Zebra"

# Spații multiple
assert ultimul_cuvant_alfabetic("ana    are   pere") == "pere"

# Ultimul alfabetic la început
assert ultimul_cuvant_alfabetic("zoo ana are mere") == "zoo"

# Toate identice
assert ultimul_cuvant_alfabetic("ana ana ana") == "ana"

# Cuvinte apropiate alfabetic
assert ultimul_cuvant_alfabetic("car casa cas") == "casa"

# Spațiu la final
assert ultimul_cuvant_alfabetic("ana are pere ") == "pere"

# Șir gol
assert ultimul_cuvant_alfabetic("") == ""

# Doar spații
assert ultimul_cuvant_alfabetic("     ") == ""