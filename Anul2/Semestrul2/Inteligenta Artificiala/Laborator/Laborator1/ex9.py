'''
Considerându-se o matrice cu n x m elemente întregi și o listă cu perechi formate din coordonatele a
2 căsuțe din matrice ((p,q) și (r,s)), să se calculeze suma elementelor din sub-matricile identificate
de fieare pereche.

De ex, pt matricea
[[0, 2, 5, 4, 1],
[4, 8, 2, 3, 7],
[6, 3, 4, 6, 2],
[7, 3, 1, 8, 3],
[1, 5, 7, 9, 4]]
și lista de perechi ((1, 1) și (3, 3)), ((2, 2) și (4, 4)), suma elementelor din prima sub-matrice este 38,
iar suma elementelor din a 2-a sub-matrice este 44.
'''

'''
    Functia calculeaza suma elementelor din submatricele primite
    param mat: matricea cu elemente
    param lista: lista de perechi ce indica submatricele
    param n: nr de linii al matricei
    param m: nr de coloane al matricei
    return: lista sumelor submatricelor
    
    complexitate de timp: O(n*m)
    complexitate de spatiu: O(1) -> nu se utilizeaza spatiu auxiliar
'''
def solutie(mat,lista,n,m):
    #se construieste matricea de sume partiale
    for i in range(n):
        for j in range(m):
            if i > 0:
                mat[i][j] += mat[i-1][j]
            if j > 0:
                mat[i][j] += mat[i][j-1]
                if i > 0:
                    mat[i][j] -= mat[i-1][j-1]

    rezultat = []
    for elem in lista:
        colt_stanga_sus = elem[0]
        colt_dreapta_jos = elem[1]

        if 0 <= colt_stanga_sus[0] < n and 0 <= colt_dreapta_jos[0] < n and 0<=colt_stanga_sus[1] < m and 0 <= colt_dreapta_jos[1] < m and colt_stanga_sus[0] <= colt_dreapta_jos[0] and colt_stanga_sus[1] <= colt_dreapta_jos[1]:
            suma = mat[colt_dreapta_jos[0]][colt_dreapta_jos[1]]
            if colt_stanga_sus[0] > 0:
                suma -= mat[colt_stanga_sus[0]-1][colt_dreapta_jos[1]]
            if colt_stanga_sus[1] > 0:
                suma-= mat[colt_dreapta_jos[0]][colt_stanga_sus[1]-1]
            if colt_stanga_sus[1] > 0 and colt_stanga_sus[0] > 0:
                suma+=mat[colt_stanga_sus[0]-1][colt_stanga_sus[1]-1]

            rezultat.append(suma)

    return rezultat

mat = [ [0, 2, 5, 4, 1],
        [4, 8, 2, 3, 7],
        [6, 3, 4, 6, 2],
        [7, 3, 1, 8, 3],
        [1, 5, 7, 9, 4]]

#TESTE
assert(solutie(mat,[((2,2),(4,4)),((0,0),(4,4)),((1,1),(3,3)),((1,2),(3,4))],5,5)==[44,105,38,36])
assert(solutie([[0,1,2],
                [1,1,1]],[((0,0),(0,0)),((1,0),(1,1)),((2,3),(1,3))],2,3) == [0,2])
assert(solutie([[]],[((0,0),(0,0))],0,0) == [])


#SOLUTIE CHAT GPT
# complexitate de timp -> O(m*n)
# complexitatea de spatiu este mai mare -> O(m*n) -> utilizeaza o matrice de sume partiale auxiliara
from typing import List, Tuple

Coord = Tuple[int, int]
Pair = Tuple[Coord, Coord]


def build_prefix_sum(matrix: List[List[int]]) -> List[List[int]]:
    """
    Construiește matricea de sume prefixate 2D (n+1)x(m+1), unde ps[i][j]
    = suma elementelor din submatricea [0..i-1][0..j-1].

    Complexitate: O(n*m) timp, O(n*m) memorie.
    """
    if not matrix or not matrix[0]:
        return [[0]]

    n, m = len(matrix), len(matrix[0])
    ps = [[0] * (m + 1) for _ in range(n + 1)]

    for i in range(1, n + 1):
        row_sum = 0
        for j in range(1, m + 1):
            row_sum += matrix[i - 1][j - 1]
            ps[i][j] = ps[i - 1][j] + row_sum

    return ps


def rect_sum(ps: List[List[int]], p: int, q: int, r: int, s: int) -> int:
    """
    Returnează suma elementelor din submatricea cu colțuri (p,q) și (r,s),
    considerând indecși 0-based, inclusiv capetele.

    ps este matricea de prefix (n+1)x(m+1).

    Complexitate: O(1).
    """
    top = min(p, r)
    left = min(q, s)
    bottom = max(p, r)
    right = max(q, s)

    # conversie la coordonate în ps (shift cu +1)
    t, l, b, rr = top, left, bottom + 1, right + 1
    return ps[b][rr] - ps[t][rr] - ps[b][l] + ps[t][l]


def submatrices_sums(matrix: List[List[int]], pairs: List[Pair]) -> List[int]:
    """
    Pentru fiecare pereche de coordonate, calculează suma submatricei.

    Complexitate: O(n*m + k).
    """
    ps = build_prefix_sum(matrix)
    return [rect_sum(ps, p, q, r, s) for ((p, q), (r, s)) in pairs]


# ---------------------- TESTE (assert) ----------------------

def _test_example():
    mat = [
        [0, 2, 5, 4, 1],
        [4, 8, 2, 3, 7],
        [6, 3, 4, 6, 2],
        [7, 3, 1, 8, 3],
        [1, 5, 7, 9, 4]
    ]
    pairs = [((1, 1), (3, 3)), ((2, 2), (4, 4))]
    # conform enunț: 38 și 44
    assert submatrices_sums(mat, pairs) == [38, 44]


def _test_single_cell():
    mat = [
        [1, 2],
        [3, 4]
    ]
    assert submatrices_sums(mat, [((0, 0), (0, 0))]) == [1]
    assert submatrices_sums(mat, [((1, 1), (1, 1))]) == [4]


def _test_reversed_corners():
    mat = [
        [1, 2, 3],
        [4, 5, 6],
        [7, 8, 9]
    ]
    # submatricea dintre (0,0) și (1,1): 1+2+4+5 = 12
    assert submatrices_sums(mat, [((1, 1), (0, 0))]) == [12]
    # submatricea dintre (2,2) și (0,1): (rânduri 0..2, col 1..2) = 2+3+5+6+8+9=33
    assert submatrices_sums(mat, [((2, 2), (0, 1))]) == [33]


def _test_negative_values():
    mat = [
        [1, -1],
        [-2, 3]
    ]
    # totul: 1-1-2+3 = 1
    assert submatrices_sums(mat, [((0, 0), (1, 1))]) == [1]
    # linia 1: -2 + 3 = 1
    assert submatrices_sums(mat, [((1, 0), (1, 1))]) == [1]


def run_tests():
    _test_example()
    _test_single_cell()
    _test_reversed_corners()
    _test_negative_values()
    print("All tests passed!")


if __name__ == "__main__":
    run_tests()