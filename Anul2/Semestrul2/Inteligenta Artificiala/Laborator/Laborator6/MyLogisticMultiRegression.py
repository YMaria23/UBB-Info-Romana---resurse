import random

import numpy as np


class MyLogisticMultiRegression:
    def __init__(self):
        self.intercept_ = []
        self.coef_ = []
        self.classes_ = []

    def begin(self,y):
        self.classes_ = np.unique(y)
        nr_clase = len(self.classes_)

        cy = [0] * len(y)
        for index in range(len(y)):
            line = [0] * nr_clase
            for l in range(len(self.classes_)):
                if self.classes_[l] == y[index]:
                    line[l] = 1
                    break
            cy[index] = line

        return nr_clase, cy

    def shuffle_manual(self, x, y):
        # schimbam ordinea inregistrarilor in lista (shuffle) pentru a preveni ciclurile
        indices = list(range(len(x)))
        random.shuffle(indices)
        X_shuffled = [x[i] for i in indices]
        y_shuffled = [y[i] for i in indices]

        return X_shuffled, y_shuffled

    def sigmoid(self, x):
        return 1 / (1 + np.exp(-x))

    def softmax(self, z):
        # z este un vector de scoruri, ex: [2.0, 1.0, 0.1]

        # se scade un maxim pentru a nu da overflow
        exp_z = np.exp(z - np.max(z))
        sum_exp_z = np.sum(exp_z)

        return exp_z / sum_exp_z


    def fit(self, x, y, learningRate=0.01, noEpochs=100):
        n_features = len(x[0])
        nr_clase, y = self.begin(y)

        # se initializeaza toti cei m + 1 coeficienti cu nr random (include w0)
        # se puteau initializa inclusiv cu 0, dar faceau un pas in plus fara rost
        self.coef_ = [[random.random() for _ in range(n_features)] for _ in range(nr_clase)]
        self.intercept_ = [random.random() for _ in range(nr_clase)]

        batchSize = 32

        for epoch in range(noEpochs):
            x, y = self.shuffle_manual(x, y)

            for i in range(0, len(x), batchSize):
                xBatch = x[i:i + batchSize]
                yBatch = y[i:i + batchSize]

                # initializare
                grad_w = [[0.0] * n_features for _ in range(nr_clase)]
                grad_w0 = [0.0] * nr_clase

                for k in range(len(xBatch)):
                    # se calculeaza z_k pentru toate clasele

                    z_k = []
                    for c in range(nr_clase):
                        # score -> valoarea prezisa pentru clasa respectiva
                        score = self.intercept_[c]
                        for j in range(n_features):
                            score += self.coef_[c][j] * xBatch[k][j]
                        z_k.append(score)

                    # se aplica softmax (pentru a obtine probabilitate)
                    p_k = self.softmax(np.array(z_k))

                    # se calculeaza eroarea + gradientul
                    for c in range(nr_clase):
                        error_kc = p_k[c] - yBatch[k][c]

                        grad_w0[c] += error_kc
                        for j in range(n_features):
                            grad_w[c][j] += error_kc * xBatch[k][j]

                # update pentru coeficienti
                for c in range(nr_clase):
                    self.intercept_[c] -= learningRate * (grad_w0[c] / len(xBatch))
                    for j in range(n_features):
                        self.coef_[c][j] -= learningRate * (grad_w[c][j] / len(xBatch))

        return self.intercept_, self.coef_

    def predict(self, x):
        predictions = []

        for xi in x:
            # se calculeaza scorurile pentru fiecare clasa
            z = []
            for c in range(len(self.classes_)):
                score = self.intercept_[c]
                for j in range(len(xi)):
                    score += self.coef_[c][j] * xi[j]
                z.append(score)

            # se va lua elementul cu argumentul cel mai mare
            # se putea aplica softmax si aici, pentru a obtine probabilitati propriu-zise, dar rezultatul nu s-ar fi schimbat
            index_max = np.argmax(z)
            predictions.append(self.classes_[index_max])
        return predictions