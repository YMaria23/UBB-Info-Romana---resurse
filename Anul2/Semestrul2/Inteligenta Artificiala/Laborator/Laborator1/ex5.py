'''
Pentru un șir cu n elemente care conține valori din mulțimea {1, 2, ..., n - 1} astfel încât o singură valoare se repetă de două ori,
să se identifice acea valoare care se repetă.
De ex. în șirul [1,2,3,4,2] valoarea 2 apare de două ori.
'''

'''
    Functia returneaza valoarea din sir care se repeta.
    param: sir de nr
    return: un nr
    
    complexitate de timp: O(n)
    complexitate de spatiu: O(1)
'''
def solutie(numere):
    n = len(numere)
    suma_reala = 0
    suma_nr = (n-1)*n/2
    for numar in numere:
        suma_reala += numar

    return suma_reala - suma_nr

#TESTE
assert(solutie([1,2,3,4,2]) == 2)
assert(solutie([]) == 0)
assert(solutie([1,2,1]) == 1)
assert(solutie([3,2,1,2]) == 2)
assert(solutie([1,2,5,6,6,3,4]) == 6)
assert(solutie([1]) == 1)



# SOLUTIE CHAT GPT -> folosirea operatiei XOR
# complexitate de timp: O(n)
# complexitate de spatiu: O(1)
def gaseste_duplicat_xor(a: list[int]) -> int:
    n = len(a)
    x = 0
    for v in a:
        x ^= v
    for i in range(1, n):
        x ^= i
    return x

# Exemplul din enunț
assert gaseste_duplicat_xor([1, 2, 3, 4, 2]) == 2

# Duplicat la început
assert gaseste_duplicat_xor([1, 1, 2, 3, 4]) == 1

# Duplicat la final
assert gaseste_duplicat_xor([1, 2, 3, 4, 4]) == 4

# n mic (minim util): n=2, valori în {1}
assert gaseste_duplicat_xor([1, 1]) == 1

# Alt caz
assert gaseste_duplicat_xor([3, 1, 2, 4, 3]) == 3

# Mai mare
a = [1, 2, 3, 4, 5, 6, 7, 8, 9, 7]  # n=10, duplicat 7
assert gaseste_duplicat_xor(a) == 7

print("Toate testele au trecut ✔️")

