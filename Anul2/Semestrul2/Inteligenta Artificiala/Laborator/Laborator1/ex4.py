'''
Să se determine cuvintele unui text care apar exact o singură dată în acel text.
De ex. cuvintele care apar o singură dată în ”ana are ana are mere rosii ana" sunt: 'mere' și 'rosii'.
'''

'''
    Fuctia returneaza o lista continand toate cuvintele ce apar o singura data intr-un text
    param: text -> textul dat (sir de caractere)
    return: lista cu cuvintele cerute -> lista de siruri de caractere
    
    complexitate de timp: O(n) -> n = nr de caractere din text
    complexitate de spatiu: O(m) -> m = nr de cuvinte distincte din text
'''
def solutie(text):
    #textul va fi parcurs caracter cu caracter pentru a nu folosi memorie in plus (ca in cazul cu split)
    dictionar: dict = {}
    cuvant = ""

    #O(n) -> se parcurge tot textul (n = nr de caractere din text)
    for caracter in text:
        if caracter in " .,;":
            if cuvant != "":
                if cuvant.lower() in dictionar:
                    dictionar[cuvant.lower()] += 1
                else:
                    dictionar[cuvant.lower()] = 1
            cuvant = ""
        else:
            cuvant += caracter

    if cuvant!="":
        dictionar[cuvant.lower()] += dictionar.get(cuvant.lower(), 0) + 1
        # if cuvant.lower() in dictionar:
        #     dictionar[cuvant.lower()] += 1
        # else:
        #     dictionar[cuvant.lower()] = 1

    #O(m) -> se parcurg toate cuvintele (m = nr de cuvinte distincte din text)
    lista_cuvinte = []
    for cuvant in dictionar:
        if dictionar[cuvant] == 1:
            lista_cuvinte.append(cuvant)

    return lista_cuvinte

#TESTE
assert(solutie("ana vrea la mare") == ["ana","vrea","la","mare"])
assert(solutie("ana  mere . la  ana ,") == ["mere","la"])
assert(solutie("") == [])
assert(solutie(" , . ;") == [])
assert(solutie("ana maria  ana maria ") == [])
assert(solutie("ana are ana are mere rosii ana") == ["mere","rosii"])
assert(solutie("a") == ["a"])
assert(solutie(";., a ") == ["a"])
assert(solutie(" Ana a vrut ana ") == ["a","vrut"])



#SOLUTIE CHAT GPT
# complexitate de timp : O(n) -> n = nr de caractere din text
# complexitate de spatiu : O(k) -> k = nr de cuvinte din text
def cuvinte_unice(text: str) -> list[str]:
    cuvinte = text.split()
    freq: dict[str, int] = {}
    first_form: dict[str, str] = {}

    # 1) numărăm aparițiile (case-insensitive)
    for w in cuvinte:
        key = w.lower()
        freq[key] = freq.get(key, 0) + 1
        if key not in first_form:
            first_form[key] = w  # păstrăm forma primei apariții

    # 2) colectăm doar cele cu frecvență 1, în ordinea apariției
    rezultat: list[str] = []
    added: set[str] = set()
    for w in cuvinte:
        key = w.lower()
        if freq[key] == 1 and key not in added:
            rezultat.append(first_form[key])
            added.add(key)

    return rezultat

# Exemplul din enunț
assert cuvinte_unice("ana are ana are mere rosii ana") == ["mere", "rosii"]

# Spații multiple
assert cuvinte_unice("ana   are   ana   are   mere") == ["mere"]

# Un singur cuvânt
assert cuvinte_unice("mere") == ["mere"]

# Toate cuvintele apar de 2 ori -> rezultat gol
assert cuvinte_unice("ana are ana are") == []

# Șir gol
assert cuvinte_unice("") == []

# Doar spații
assert cuvinte_unice("     ") == []

# Case-insensitive (Ana = ana)
assert cuvinte_unice("Ana are ana ARE mere") == ["mere"]

# Păstrează ordinea apariției
assert cuvinte_unice("z x y x") == ["z", "y"]

# Punctuația este considerată parte din cuvânt (conform separării doar după spațiu)
assert cuvinte_unice("mere, rosii mere") == ["mere,", "rosii","mere"]

print("Toate testele au trecut ✔️")