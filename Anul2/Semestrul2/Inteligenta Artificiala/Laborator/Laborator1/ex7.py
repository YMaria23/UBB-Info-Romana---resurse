'''
Să se determine al k-lea cel mai mare element al unui șir de numere cu n elemente (k < n).
De ex. al 2-lea cel mai mare element din șirul [7,4,6,3,9,1] este 7.
'''

'''
    Functia determina al k-lea cel mai mare element al unui sir
    param lista: lista cu elemente (nr intregi)
    param k: nr natural
    return: al k-lea cel mai mare element
    
    complexitate de timp: O(nlogk) -> se parcurg toate elementele + operatiile de pe heap (k -> dimensiunea heap-ului)
    complexitate de spatiu: O(k) -> spatiu aditional
'''
import heapq

def solutie(lista,k):
    if k < 1:
        return None

    heap = []
    dim_heap = 0
    for elem in lista:
        if dim_heap < k:
            heapq.heappush(heap,elem)
            dim_heap += 1
        else:
            if elem > heap[0]:
                heapq.heappop(heap)
                heapq.heappush(heap,elem)

    return heap[0]

#TESTE
assert solutie([2,3,6,1,4],2) == 4
assert solutie([2,3,6,1,4],-1) is None
assert solutie([2,3,6,1,4],3) == 3
assert solutie([1],1) == 1
assert solutie([],0) is None
assert solutie([3,4,2,1,4],2) == 4


#SOLUTIE CHAT GPT
#complexitatea de timp medie este O(n), dar worst case va da O(n^2)
from typing import List
import random


def kth_largest_quickselect(arr: List[int], k: int) -> int:
    """
    Determină al k-lea cel mai mare element dintr-un șir (k < n).
    k este 1-based: k=1 -> cel mai mare, k=2 -> al 2-lea cel mai mare, etc.

    Complexitate medie: O(n), memorie: O(1) extra (în afară de modificarea listei).
    Atenție: funcția modifică lista (lucrează in-place).
    """
    n = len(arr)
    if n == 0:
        raise ValueError("Șirul nu poate fi gol.")
    if not (1 <= k < n + 1):
        raise ValueError("k trebuie să fie în [1, n].")

    # Transformăm problema: al k-lea cel mai mare == elementul cu index (n-k) dacă lista ar fi sortată crescător
    target_index = n - k

    left, right = 0, n - 1

    def partition(l: int, r: int, pivot_index: int) -> int:
        pivot_value = arr[pivot_index]
        # mut pivot la final
        arr[pivot_index], arr[r] = arr[r], arr[pivot_index]
        store = l
        for i in range(l, r):
            if arr[i] < pivot_value:
                arr[store], arr[i] = arr[i], arr[store]
                store += 1
        # mut pivot la locul final
        arr[store], arr[r] = arr[r], arr[store]
        return store

    while left <= right:
        pivot_index = random.randint(left, right)
        pivot_final = partition(left, right, pivot_index)

        if pivot_final == target_index:
            return arr[pivot_final]
        elif pivot_final < target_index:
            left = pivot_final + 1
        else:
            right = pivot_final - 1

    # logic, nu ar trebui să ajungă aici
    raise RuntimeError("Eroare internă Quickselect.")

# ------------------- TESTE (assert) -------------------

def _tests():
    # exemplul din enunț
    a = [7, 4, 6, 3, 9, 1]
    assert kth_largest_quickselect(a.copy(), 2) == 7

    # k = 1 (cel mai mare)
    assert kth_largest_quickselect(a.copy(), 1) == 9

    # k = n (cel mai mic)
    assert kth_largest_quickselect(a.copy(), len(a)) == 1

    # elemente negative
    b = [-5, -1, -3, -2]
    assert kth_largest_quickselect(b.copy(), 1) == -1
    assert kth_largest_quickselect(b.copy(), 2) == -2
    assert kth_largest_quickselect(b.copy(), 3) == -3
    assert kth_largest_quickselect(b.copy(), 4) == -5

    # duplicate (k-lea cel mai mare în ordinea valorilor, cu duplicate incluse)
    c = [5, 1, 5, 2, 5]
    # sort desc: [5,5,5,2,1]
    assert kth_largest_quickselect(c.copy(), 1) == 5
    assert kth_largest_quickselect(c.copy(), 2) == 5
    assert kth_largest_quickselect(c.copy(), 3) == 5
    assert kth_largest_quickselect(c.copy(), 4) == 2
    assert kth_largest_quickselect(c.copy(), 5) == 1

    # liste mici
    d = [2, 1]
    assert kth_largest_quickselect(d.copy(), 1) == 2
    assert kth_largest_quickselect(d.copy(), 2) == 1

_tests()