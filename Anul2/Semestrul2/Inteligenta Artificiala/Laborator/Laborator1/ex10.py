'''
Considerându-se o matrice cu n x m elemente binare (0 sau 1) sortate crescător pe linii,
să se identifice indexul liniei care conține cele mai multe elemente de 1.

De ex. în matricea
[[0,0,0,1,1],
[0,1,1,1,1],
[0,0,1,1,1]]
a doua linie conține cele mai multe elemente 1.
'''

'''
    Functie ce returneaza indexul liniei cu cele mai multe elemente de 1.
    param: matricea de elemente
    param: n -> nr de linii al matricei
    param: m -> nr de coloane al matrici
    return: un nr (indexul cerut)
    
    complexitate de timp: O(n+m)
    complexitate de spatiu: O(1)
'''

def solutie(matrice,n,m):
    #se va merge in "scara" de la sf linie spre inceput
    i = 0
    for j in range(m-1,-1,-1):
        copie = i
        while matrice[i][j] != 1 and i < n-1:
            i+=1
        if matrice[i][j] == 0:
            i = copie
    return i+1 #pentru a afisa a cata linie este (nu indexul propriu-zis)

#TESTE
assert(solutie([[0,0,0,1,1],
                        [0,1,1,1,1],
                        [0,0,1,1,1]], 3,5) == 2)

assert(solutie([[0,0,0],
                        [0,0,0]], 2,3) == 1)

assert(solutie([[1,1,1],
                        [1,1,1]], 2,3) == 1)

assert(solutie([[0,1,1]],1,3) == 1)

assert(solutie([[0,1,1],
                        [0,0,1],
                        [1,1,1]],3,3) == 3)

assert(solutie([[0,1,1],
                        [0,1,1],
                        [0,0,1]],3,3) == 1)


# SOLUTIE CHAT GPT
def linia_cu_cei_mai_multi_1(A: list[list[int]]) -> int:
    if not A or not A[0]:
        raise ValueError("Matricea trebuie să aibă cel puțin o linie și o coloană.")

    n, m = len(A), len(A[0])

    j = m - 1           # pornim din dreapta sus
    best_row = 0        # prima linie ca default (tie-break)

    for i in range(n):
        # mergem la stânga cât timp avem 1 pe linia i
        while j >= 0 and A[i][j] == 1:
            j -= 1
            best_row = i

    return best_row

def ruleaza_teste():
    # Exemplul din enunț
    A = [
        [0,0,0,1,1],
        [0,1,1,1,1],
        [0,0,1,1,1],
    ]
    assert linia_cu_cei_mai_multi_1(A) == 1

    # Toate 0 -> tie, prima linie
    A = [
        [0,0,0],
        [0,0,0],
    ]
    assert linia_cu_cei_mai_multi_1(A) == 0

    # Toate 1 -> tie, prima linie
    A = [
        [1,1],
        [1,1],
        [1,1],
    ]
    assert linia_cu_cei_mai_multi_1(A) == 0

    # Câștigă ultima linie
    A = [
        [0,0,1],
        [0,1,1],
        [1,1,1],
    ]
    assert linia_cu_cei_mai_multi_1(A) == 2

    # Egalitate între linii -> prima
    A = [
        [0,1,1,1],  # 3 de 1
        [0,1,1,1],  # 3 de 1
        [0,0,1,1],  # 2 de 1
    ]
    assert linia_cu_cei_mai_multi_1(A) == 0

    # O singură linie
    A = [[0,0,0,1,1,1]]
    assert linia_cu_cei_mai_multi_1(A) == 0

    # O singură coloană
    A = [
        [0],
        [1],
        [1],
        [0],  # atenție: nu ar fi sortat crescător față de 0/1 per linie e ok (linia e lungime 1)
    ]
    # linii cu 1: index 1 și 2 -> tie -> 1
    assert linia_cu_cei_mai_multi_1(A) == 1

    print("Toate testele au trecut ✔️")

ruleaza_teste()