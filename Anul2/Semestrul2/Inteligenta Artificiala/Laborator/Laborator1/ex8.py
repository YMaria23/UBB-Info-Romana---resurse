'''
Să se genereze toate numerele (în reprezentare binară) cuprinse între 1 și n.
De ex. dacă n = 4, numerele sunt: 1, 10, 11, 100.
'''

from queue import Queue

'''
    Se va returna o lista cu toare nr binare de la 1 la n
    param: n -> nr in baza 10
    return: o lista continand toate nr de la 1 la n in binar
    
    complexitate de timp: O(n)
    complexitate de spatiu: O(n) -> chiar daca simultan, vor putea sa fie stocate aprox n/2 elem
'''
def solutie(n):
    lista_elemente = []

    coada = Queue(n)
    coada.put("1")

    for i in range(n):
        element_curent = coada.get()
        lista_elemente.append(element_curent)

        coada.put(element_curent+"0")

        #pentru a nu umple coada degeaba
        if i+2 < n:
            coada.put(element_curent+"1")

    return lista_elemente

# TESTE
assert(solutie(10) == ['1', '10', '11', '100', '101', '110', '111', '1000', '1001', '1010'])
assert(solutie(1) == ['1'])
assert(solutie(0) == [])
assert(solutie(-1) == [])
assert(solutie(4) == ['1','10','11','100'])
assert(solutie(5) == ['1','10','11','100','101'])


#SOLUTIE CHAT GPT
# complexitate de timp si de spatiu asemanatoare
from collections import deque

def gen_binare_1_la_n(n: int) -> list[str]:
    if n < 1:
        raise ValueError("n trebuie să fie >= 1")

    q = deque(["1"])
    rezultat: list[str] = []

    for _ in range(n):
        x = q.popleft()
        rezultat.append(x)
        q.append(x + "0")
        q.append(x + "1") #aici, in solutia non-AI avem mai multa eficienta pe partea de memorie in special

    return rezultat

def ruleaza_teste():
    # Exemplul din enunț
    assert gen_binare_1_la_n(4) == ["1", "10", "11", "100"]

    # n minim
    assert gen_binare_1_la_n(1) == ["1"]

    # n mic
    assert gen_binare_1_la_n(2) == ["1", "10"]
    assert gen_binare_1_la_n(3) == ["1", "10", "11"]
    assert gen_binare_1_la_n(5) == ["1", "10", "11", "100", "101"]

    # verificare de consistență: fiecare element e chiar binarul lui i+1
    out = gen_binare_1_la_n(30)
    for i, b in enumerate(out, start=1):
        assert int(b, 2) == i

    # n invalid
    try:
        gen_binare_1_la_n(0)
        assert False, "Trebuia să arunce ValueError pentru n=0"
    except ValueError:
        pass

    print("Toate testele au trecut ✔️")

ruleaza_teste()