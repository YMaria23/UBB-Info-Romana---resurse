'''
Să se determine produsul scalar a doi vectori rari care conțin numere reale.
Un vector este rar atunci când conține multe elemente nule. Vectorii pot avea oricâte dimensiuni.
De ex. produsul scalar a 2 vectori unisimensionali [1,0,2,0,3] și [1,2,0,3,1] este 4.
'''

'''
    Functia determina produsul scalar dintre 2 vectori rari (stocati sub forma de dictionar)
    param dict: dictionar continand perechi de valori (index, valoare nenula)
    return: un nr reprezentand produsul scalar dintre cei 2 vectori rari
    
    complexitate de timp: O(k) -> k = nr de elemente nenule
    complexitate de spatiu: O(1) (nu se foloseste spatiu auxiliar) (nu se ia in considerare memoria
                            ocupata de dictionarul initial)
'''
def solutie(dict):
    sum = 0
    for key in dict:
        if len(dict[key])>1:
            sum += dict[key][0]*dict[key][1]

    return sum

#TESTE
assert(solutie({
    0:[1,1],
    1:[2],
    2:[2],
    3:[3],
    4:[3,1]
}) == 4)

assert(solutie({
    0:[1]
}) == 0)

assert(solutie({}) == 0)
assert(solutie({
    0:[1,2],
    1:[2,3],
    3:[2],
    5:[3,1],
    7:[1]
}) == 11)

assert(solutie({
    0:[1,2]
}) == 2)



# SOLUTIE CHAT GPT
# detaliu de implementare -> practic se vor parcurge doar o parte din elementele nenule (nu toate)
# complexitatea de timp este ceva mai mica
from typing import List, Dict, Iterable, Tuple

def to_sparse(v: List[float]) -> Dict[int, float]:
    """
    Transformă un vector dens v într-o reprezentare rară (dict):
      sparse[i] = v[i], doar pentru elementele nenule.

    Parametri:
      v: list[float] - vector (poate fi mare)

    Returnează:
      dict[int, float] - reprezentare rară

    Complexitate:
      Timp: O(n), Memorie: O(k) unde k = #nenule
    """
    sparse: Dict[int, float] = {}
    for i, x in enumerate(v):
        if x != 0:
            sparse[i] = float(x)
    return sparse

def dot_sparse(a: Dict[int, float], b: Dict[int, float]) -> float:
    """
    Calculează produsul scalar a·b pentru doi vectori rari reprezentați ca dict.

    Parametri:
      a, b: dict[int, float] - reprezentări rare (index -> valoare nenulă)

    Returnează:
      float - produsul scalar

    Metodă eficientă:
      Iterăm prin dicționarul mai mic și căutăm indexul în celălalt (O(1) amortizat).

    Complexitate:
      Timp: O(min(k_a, k_b)), Memorie: O(1) în plus
    """
    # iterăm prin cel mai mic dicționar
    if len(a) > len(b):
        a, b = b, a

    s = 0.0
    for idx, val in a.items():
        s += val * b.get(idx, 0.0)
    return s

# Caz: totul zero
v3 = [0, 0, 0]
v4 = [0, 0, 0]
assert dot_sparse(to_sparse(v3), to_sparse(v4)) == 0.0

# Caz: unul zero, altul nu
v5 = [0, 0, 0, 0]
v6 = [0, 5, 0, -2]
assert dot_sparse(to_sparse(v5), to_sparse(v6)) == 0.0

# Caz: numere reale (float) și negative
v7 = [0.5, 0.0, -1.5, 0.0]
v8 = [2.0, 0.0, 4.0, 0.0]
# 0.5*2 + (-1.5)*4 = 1 - 6 = -5
assert dot_sparse(to_sparse(v7), to_sparse(v8)) == -5.0