'''
Considerându-se o matrice cu n x m elemente binare (0 sau 1), să se înlocuiască cu 1 toate aparițiile elementelor egale cu 0 care sunt complet înconjurate de 1.

De ex. matricea \
[[1,1,1,1,0,0,1,1,0,1],
[1,0,0,1,1,0,1,1,1,1],
[1,0,0,1,1,1,1,1,1,1],
[1,1,1,1,0,0,1,1,0,1],
[1,0,0,1,1,0,1,1,0,0],
[1,1,0,1,1,0,0,1,0,1],
[1,1,1,0,1,0,1,0,0,1],
[1,1,1,0,1,1,1,1,1,1]]
*devine *
[[1,1,1,1,0,0,1,1,0,1],
[1,1,1,1,1,0,1,1,1,1],
[1,1,1,1,1,1,1,1,1,1],
[1,1,1,1,1,1,1,1,0,1],
[1,1,1,1,1,1,1,1,0,0],
[1,1,1,1,1,1,1,1,0,1],
[1,1,1,0,1,1,1,0,0,1],
[1,1,1,0,1,1,1,1,1,1]]
'''

'''
    Functia marcheaza plaja de 0-uri vecine ale unui element cu -1
    param matrice: matricea data
    param n: nr de linii
    param m: nr de coloane
    param i: indexul liniei
    param j: indexul coloanei
'''

def marcheaza_vecini(matrice,n,m,i,j):
    di = [1,0,-1,0]
    dj = [0,1,0,-1]

    for x in range(4):
        if 0 <= i+di[x] < n and 0 <= j + dj[x] < m:
            if matrice[i+di[x]][j+dj[x]] == 0:
                matrice[i+di[x]][j+dj[x]] = -1
                marcheaza_vecini(matrice,n,m,i+di[x],j+dj[x])


'''
    Functia inlocuieste elementele nule cu 1, daca acestea sunt complet inconjurate de 1
    param matrice: matrice
    param n: nr de linii
    param m: nr de coloane
    
    return: matrice rezultat
    
    complexitate de timp: O(n*m)
    complexitate de spatiu: O(1) -> nu s-a utilizat spatiu auxiliar (daca nu se ia in considerare stiva apelului recursiv)
'''
def solutie(matrice,n,m):
    #parcurgem conturul si marcam cu -1 zerourile care rnu vor fi inlocuite pe parcurs

    #prima linie
    for j in range(m):
        if matrice[0][j] == 0:
            matrice[0][j] = -1
            marcheaza_vecini(matrice,n,m,0,j)

    #ultima coloana
    for i in range(1,n):
        if matrice[i][m-1] == 0:
            matrice[i][m-1] = -1
            marcheaza_vecini(matrice,n,m,i,m-1)

    #ultima linie
    for j in range(m-1):
        if matrice[n-1][j] == 0:
            matrice[n-1][j] = -1
            marcheaza_vecini(matrice,n,m,n-1,j)

    #prima coloana
    for i in range(1,n-1):
        if matrice[i][0] == 0:
            matrice[i][0] = -1
            marcheaza_vecini(matrice,n,m,i,0)

    for i in range(n):
        for j in range(m):
            if matrice[i][j] == 0:
                matrice[i][j] = 1
            elif matrice[i][j] == -1:
                matrice[i][j] = 0

    return matrice


#TESTE
assert(solutie([[1,1,1,1,0,0,1,1,0,1],
                    [1,0,0,1,1,0,1,1,1,1],
                    [1,0,0,1,1,1,1,1,1,1],
                    [1,1,1,1,0,0,1,1,0,1],
                    [1,0,0,1,1,0,1,1,0,0],
                    [1,1,0,1,1,0,0,1,0,1],
                    [1,1,1,0,1,0,1,0,0,1],
                    [1,1,1,0,1,1,1,1,1,1]],8,10) == [[1,1,1,1,0,0,1,1,0,1],
                                                            [1,1,1,1,1,0,1,1,1,1],
                                                            [1,1,1,1,1,1,1,1,1,1],
                                                            [1,1,1,1,1,1,1,1,0,1],
                                                            [1,1,1,1,1,1,1,1,0,0],
                                                            [1,1,1,1,1,1,1,1,0,1],
                                                            [1,1,1,0,1,1,1,0,0,1],
                                                            [1,1,1,0,1,1,1,1,1,1]])

assert(solutie([[1,1,0,1],
                [1,0,1,1],
                [1,0,1,1],
                [1,0,1,1],
                [1,0,1,1]],5,4) == [[1,1,0,1],
                                            [1,0,1,1],
                                            [1,0,1,1],
                                            [1,0,1,1],
                                            [1,0,1,1]])

assert(solutie([[0,0,0],
                [0,0,0],
                [0,0,0]],3,3) == [[0,0,0],
                                        [0,0,0],
                                        [0,0,0]])

assert(solutie([[1,1,1,1,1],
                [1,0,1,0,1],
                [1,0,1,0,0],
                [1,1,1,0,1],
                [1,1,1,1,1]],5,5) == [[1,1,1,1,1],
                                            [1,1,1,0,1],
                                            [1,1,1,0,0],
                                            [1,1,1,0,1],
                                            [1,1,1,1,1]])

assert(solutie([[1,1,1,1]],1,4) == [[1,1,1,1]])
assert(solutie([[0]],1,1) == [[0]])



#SOLUTIE CHAT GPT
#complexitatea de spatiu este mai mare -> se foloseste de coada pentru a marca 0-urile de pe margine
#poate avea avantaj fata de recursivitate pe matrici mai mari
from collections import deque

def umple_zero_inconjurate(A: list[list[int]]) -> list[list[int]]:
    if not A or not A[0]:
        return A

    n, m = len(A), len(A[0])
    q = deque()

    # marcăm 0-urile "safe" cu valoarea 2 (temporar)
    def push_if_zero(i: int, j: int) -> None:
        if 0 <= i < n and 0 <= j < m and A[i][j] == 0:
            A[i][j] = 2
            q.append((i, j))

    # 1) punem în coadă toate 0-urile de pe margine
    for j in range(m):
        push_if_zero(0, j)
        push_if_zero(n - 1, j)
    for i in range(n):
        push_if_zero(i, 0)
        push_if_zero(i, m - 1)

    # 2) BFS: marcăm toate 0-urile conectate la margine
    while q:
        i, j = q.popleft()
        push_if_zero(i - 1, j)
        push_if_zero(i + 1, j)
        push_if_zero(i, j - 1)
        push_if_zero(i, j + 1)

    # 3) flip: 0 (închis) -> 1, iar 2 (safe) -> 0
    for i in range(n):
        for j in range(m):
            if A[i][j] == 0:
                A[i][j] = 1
            elif A[i][j] == 2:
                A[i][j] = 0

    return A

def ruleaza_teste():
    # Exemplul din enunț
    A = [
        [1,1,1,1,0,0,1,1,0,1],
        [1,0,0,1,1,0,1,1,1,1],
        [1,0,0,1,1,1,1,1,1,1],
        [1,1,1,1,0,0,1,1,0,1],
        [1,0,0,1,1,0,1,1,0,0],
        [1,1,0,1,1,0,0,1,0,1],
        [1,1,1,0,1,0,1,0,0,1],
        [1,1,1,0,1,1,1,1,1,1],
    ]
    expected = [
        [1,1,1,1,0,0,1,1,0,1],
        [1,1,1,1,1,0,1,1,1,1],
        [1,1,1,1,1,1,1,1,1,1],
        [1,1,1,1,1,1,1,1,0,1],
        [1,1,1,1,1,1,1,1,0,0],
        [1,1,1,1,1,1,1,1,0,1],
        [1,1,1,0,1,1,1,0,0,1],
        [1,1,1,0,1,1,1,1,1,1],
    ]
    assert umple_zero_inconjurate([row[:] for row in A]) == expected

    # Toate 1 -> neschimbat
    B = [[1,1],[1,1]]
    assert umple_zero_inconjurate([row[:] for row in B]) == B

    # Toate 0 -> toate sunt conectate la margine -> neschimbat
    C = [[0,0,0],[0,0,0]]
    assert umple_zero_inconjurate([row[:] for row in C]) == C

    # Un 0 închis complet (centru)
    D = [
        [1,1,1],
        [1,0,1],
        [1,1,1],
    ]
    assert umple_zero_inconjurate([row[:] for row in D]) == [
        [1,1,1],
        [1,1,1],
        [1,1,1],
    ]

    # 0 conectat la margine prin lanț -> rămâne 0
    E = [
        [1,0,1,1],
        [1,0,1,1],
        [1,0,0,1],
        [1,1,1,1],
    ]
    assert umple_zero_inconjurate([row[:] for row in E]) == E

    # Egalitate / caz mic 1x1
    assert umple_zero_inconjurate([[0]]) == [[0]]
    assert umple_zero_inconjurate([[1]]) == [[1]]

    print("Toate testele au trecut ✔️")

ruleaza_teste()

