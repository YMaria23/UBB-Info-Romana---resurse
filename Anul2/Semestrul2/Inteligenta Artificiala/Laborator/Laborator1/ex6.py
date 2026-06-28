'''
Pentru un șir cu n numere întregi care conține și duplicate, să se determine elementul majoritar (care apare de mai mult de n / 2 ori).
De ex. 2 este elementul majoritar în șirul [2,8,7,2,2,5,2,3,1,2,2].
'''

'''
    Functia determina elementul majoritar din lista
    param: lista de elemente
    return: elementul cerut
    
    complexitate de timp: O(n)
    complexitate de spatiu: O(1) -> se va folosi algoritmul lui Boyer Moore pentru determinarea elementului majoritar
'''

def solutie(lista):
    n = len(lista)

    candidat = None
    vot = 0
    for elem in lista:
        if candidat is None or vot == 0:
            candidat = elem
            vot = 1
        elif candidat == elem:
            vot += 1
        elif candidat !=elem:
            vot -=1

    #acum se verifica daca este majoritar (apare de cel putin n/2+1 ori)
    aparitii = 0
    for elem in lista:
        if elem == candidat:
            aparitii += 1

    if aparitii > n//2 :
        return candidat

    return None

#TESTE
assert(solutie([2,2,3,3]) == None)
assert(solutie([2,8,7,2,2,5,2,3,1,2,2]) == 2)
assert(solutie([3,3]) == 3)
assert(solutie([3]) == 3)
assert(solutie([]) is None)
assert(solutie([1,2,1,2,1,2,2]) == 2)
assert(solutie([1,2,1,2]) == None)


#SOLUTIE CHAT GPT
#foloseste algoritmul lui Boyer–Moore
#aceleasi complexitati
#s-a pus test inclusiv pe nr negative (nu e neaparat caz 'extrem' totusi)
from typing import List, Optional


def majority_element(nums: List[int]) -> Optional[int]:
    """
    Determină elementul majoritar (apare de mai mult de n/2 ori) folosind
    algoritmul Boyer–Moore Majority Vote.

    Returnează:
      - elementul majoritar dacă există
      - None dacă nu există element majoritar

    Complexitate:
      - timp: O(n)
      - memorie: O(1)
    """
    if not nums:
        return None

    # 1) găsește candidat
    candidate = None
    count = 0
    for x in nums:
        if count == 0:
            candidate = x
            count = 1
        elif x == candidate:
            count += 1
        else:
            count -= 1

    # 2) verifică dacă e majoritar
    assert candidate is not None  # nums nu e gol
    freq = 0
    for x in nums:
        if x == candidate:
            freq += 1

    return candidate if freq > len(nums) // 2 else None


# ---------------------- TESTE (assert) ----------------------

def _test_example():
    nums = [2, 8, 7, 2, 2, 5, 2, 3, 1, 2, 2]
    assert majority_element(nums) == 2


def _test_no_majority():
    nums = [1, 2, 3, 1, 2, 3]
    assert majority_element(nums) is None


def _test_all_same():
    nums = [5, 5, 5, 5]
    assert majority_element(nums) == 5


def _test_single_element():
    assert majority_element([42]) == 42


def _test_even_length_majority():
    nums = [9, 9, 1, 9]
    # 9 apare de 3 ori, n=4, 3 > 2 => majoritar
    assert majority_element(nums) == 9


def _test_empty():
    assert majority_element([]) is None


def run_tests():
    _test_example()
    _test_no_majority()
    _test_all_same()
    _test_single_element()
    _test_even_length_majority()
    _test_empty()
    print("All tests passed!")


if __name__ == "__main__":
    run_tests()