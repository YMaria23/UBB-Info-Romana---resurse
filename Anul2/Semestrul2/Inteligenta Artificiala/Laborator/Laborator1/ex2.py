'''
Să se determine distanța Euclideană între două locații identificate prin perechi de numere.
De ex. distanța între (1,5) și (4,1) este 5.0
'''

'''
    Functia returneaza radicalul unui numar natural, pozitiv
    param nr: numar natural, pozitiv
    return: radicalul lui nr
    exceptii: ValueError -> n este negativ
    
    complexitate de timp: O(logn) -> unde n este nr dat (aka nr)
    complexitate de spatiu: O(1)
    
    S-a utilizat metoda lui Newton–Raphson: x(n+1) = 1/2*(x(n) + S/x(n)); x(n) -> estimarea curenta a radicalului
                                                                          S -> numarul al carui radical il calculam
                                                                          (se va repeta pana ajungem la precizia dorita)
'''

def radical(nr,precizie):
    if nr < 0:
        raise ValueError('Numarul nu poate sa fie negativ!')

    if nr == 0:
        return 0

    x_curent = nr
    while True:
        x_next = 1/2 * (x_curent + nr/x_curent)
        if abs(x_next - x_curent) < precizie:
            return x_next
        x_curent = x_next


'''
    Functia returneaza distanta Euclidiana intre 2 locatii (identificare prin perechi de nr/coordonate)
    param p1: pereche de nr intregi
    param p2: pereche de nr intregi
    return: distanta Euclidiana intre p1 si p2
    
    complexitate de timp: O(logn) -> unde n este (p21 - p11)^2 + (p22 - p12)^2
    complexitate de spatiu: O(1)
'''
def solutie(p1,p2):
    return radical((p2[0]-p1[0])**2 + (p2[1]-p1[1])**2,1e-10)

#TESTE
assert radical(4,0.0000000001) == 2
assert solutie((1,5),(4,1)) == 5
assert  3.6< solutie((1,3),(4,5)) < 3.7
assert 2.2360 < solutie((-1,2),(1,1)) < 2.2361



# SOLUTIE CHAT GPT
# a fost necesar sa specific in prompt faptul ca nu se va folosi functia de radical .sqrt
"""
Determină distanța Euclidiană dintre două puncte.
Se implementează manual funcția de radical (sqrt) folosind metoda Newton.
"""


def radical(n, epsilon=1e-10):
    """
    Calculează radicalul unui număr pozitiv folosind metoda Newton.

    Parametrii:
        n (float): numărul pentru care se calculează radicalul
        epsilon (float): precizia dorită

    Returnează:
        float: sqrt(n)
    """
    if n < 0:
        raise ValueError("Radical din număr negativ")

    if n == 0:
        return 0

    x = n
    while True:
        next_x = 0.5 * (x + n / x)
        if abs(next_x - x) < epsilon:
            return next_x
        x = next_x


def distanta(p1, p2):
    """
    Determină distanța Euclidiană dintre două puncte.

    Parametrii:
        p1 (tuple): (x1, y1)
        p2 (tuple): (x2, y2)

    Returnează:
        float: distanța Euclidiană
    """
    x1, y1 = p1
    x2, y2 = p2

    suma = (x2 - x1) ** 2 + (y2 - y1) ** 2
    return radical(suma)


# TESTE
def teste():
    assert abs(radical(4) - 2) < 1e-6
    assert abs(radical(9) - 3) < 1e-6
    assert abs(radical(25) - 5) < 1e-6

    assert abs(distanta((1, 5), (4, 1)) - 5.0) < 1e-6
    assert abs(distanta((0, 0), (3, 4)) - 5.0) < 1e-6
    assert abs(distanta((2, 2), (2, 2)) - 0.0) < 1e-6
    assert abs(distanta((-1, -1), (2, 3)) - 5.0) < 1e-6


teste()

print("Toate testele au trecut!")