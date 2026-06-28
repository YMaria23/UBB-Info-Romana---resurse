class MyLinearUnivariateRegression:
    def __init__(self):
        self.intercept_ = 0.0
        self.coef_ = 0.0

    def fit(self,x, y):
        n = len(x)

        sx = sum(x)
        sy = sum(y)

        sx2 = sum(i * i for i in x)

        sxy = sum(i * j for (i, j) in zip(x, y))

        w1 = (n * sxy - sx * sy) / (n * sx2 - sx * sx)
        w0 = (sy - w1 * sx) / n
        self.intercept_, self.coef_ = w0, w1

    # functia accepta 2 tipuri de reprezentari: matriceala, dar si sub forma de lista
    def predict(self, x):
        if (isinstance(x[0], list)):
            return [self.intercept_ + self.coef_ * val[0] for val in x]
        else:
            return [self.intercept_ + self.coef_ * val for val in x]

class MyLinearMultivariateRegression:
    def __init__(self):
        self.intercept_ = 0.0
        self.coef_ = []

    def constructXY(self,x,y):
        # se construiesc matricele X si Y
        X = []
        for data in x:
            # se adauga coloana de 1 pentru w0
            data = [1] + data
            X.append(data)

        Y = [[el] for el in y]

        return X,Y

    def transMat(self, x):
        # se construieste transpusa unei matrice
        T = []
        for feature in range(len(x[0])):
            vect = []
            for index in range(len(x)):
                vect.append(x[index][feature])
            T.append(vect)
        return T

    def multiply(self,x,y):
        # inmultirea a doua matrice
        m = len(x)
        n = len(x[0])
        p = len(y[0])

        if n == len(y):
            rez = []
            for line in range(m):
                # se parcurg liniile din x
                line_rez = []
                for column in range(p):
                    # se parcurg coloanele din y
                    sum = 0

                    # se calculeaza elementul de pe pozitia [line][column] din matricea finala
                    for index in range(n):
                        sum+= x[line][index] * y[index][column]
                    line_rez.append(sum)
                rez.append(line_rez)

            return rez

    def inverse(self,x):
        n = len(x)
        # se creaza matricea extinsa [x | I]
        M = []
        for i,row in enumerate(x):
            new = row.copy()
            for j in range(n):
                if i == j:
                    new.append(1)
                else:
                    new.append(0)
            M.append(new)

        for i in range(n):
            # se determina cate un pivot pe fiecare coloana (aka elementul cel mai mare dintre el curent si cele din josul sau)
            pivot_row = i
            for j in range(i + 1, n):
                #daca gasesc un element mai mare pe coloana, interschimb randurile
                if abs(M[j][i]) > abs(M[pivot_row][i]):
                    pivot_row = j

            M[i], M[pivot_row] = M[pivot_row], M[i]

            # se normalizeaza randul pivotului (pt a avea 1 pe diagonala principala)
            pivot_val = M[i][i]
            M[i] = [x / pivot_val for x in M[i]]

            # se elimina restul elementelor de pe coloana i
            # ne folosim de valoare pivotului de pe coloana respectiva pentru a face 0-uri in sus si in jos
            for k in range(n):
                if k != i:
                    # factor = elementul de pe coloana i care trebuie sa devina 0
                    factor = M[k][i]
                    # se aplica transformarea pe toata linia matricei augmentate
                    M[k] = [M[k][j] - factor * M[i][j] for j in range(2 * n)]

        # se extrage numai partea dreapta a matricei augmentate (noua inversa)
        inverse = [row[n:] for row in M]
        return inverse

    def fit(self, x, y):
        x,y = self.constructXY(x,y)

        xt = self.transMat(x)
        inv = self.inverse(self.multiply(xt,x))

        w = self.multiply(self.multiply(inv,xt),y)

        self.intercept_ = w[0][0]
        for value in w:
            self.coef_.append(value[0])

    def predict(self, x):
        rez = []
        for inreg in x:
            suma_features = sum(feature * coef for feature, coef in zip(inreg, self.coef_))
            rez.append(suma_features + self.intercept_)

        return rez