import random

import numpy as np


class MyLogisticRegression:
    def __init__(self):
        self.intercept_ = 0.0
        self.coef_ = []

    def shuffle_manual(self, x, y):
        # schimbam ordinea inregistrarilor in lista (shuffle) pentru a preveni ciclurile
        indices = list(range(len(x)))
        random.shuffle(indices)
        X_shuffled = [x[i] for i in indices]
        y_shuffled = [y[i] for i in indices]

        return X_shuffled, y_shuffled

    def sigmoid(self, x):
        return 1 / (1 + np.exp(-x))

    def fit(self, x, y, learningRate=0.01, noEpochs=100):
        n_features = len(x[0])

        # se initializeaza toti cei m + 1 coeficienti cu nr random (include w0)
        # se puteau initializa inclusiv cu 0, dar faceau un pas in plus fara rost
        self.coef_ = [random.random() for _ in range(n_features)]
        self.intercept_ = random.random()

        batchSize = 32

        for epoch in range(noEpochs):
            x, y = self.shuffle_manual(x, y)

            for i in range(0, len(x), batchSize):
                xBatch = x[i:i + batchSize]
                yBatch = y[i:i + batchSize]

                # se reseteaza gradientii pentru fiecare batch
                gradient_w = [0.0] * n_features
                gradient_w0 = 0.0

                current_batch_len = len(xBatch)

                for k in range(current_batch_len):
                    # se calculeaza predictia
                    # prediction_k = w0 + w1 * xk1 + ... + wn * xkn

                    prediction_k = self.intercept_
                    for j in range(n_features):
                        prediction_k += self.coef_[j] * xBatch[k][j]

                    error_k = self.sigmoid(prediction_k) - yBatch[k]

                    # se det. gradientii
                    # grad = 1/B * sum(error_k * x_k_j); k -> nr inregistrarii; j -> feature-ul
                    # grad_0 = 1/B * sum(error_k)
                    gradient_w0 += error_k
                    for j in range(n_features):
                        gradient_w[j] = gradient_w[j] + (error_k * xBatch[k][j])

                gradient_w0 /= current_batch_len

                # se actualizeaza coeficientii
                self.intercept_ -= learningRate * gradient_w0

                for j in range(n_features):
                    gradient_w[j] /= current_batch_len
                    self.coef_[j] -= learningRate * gradient_w[j]

        return self.intercept_, self.coef_

    def eval(self, xi):
        yi = self.intercept_
        for j in range(len(xi)):
            yi += self.coef_[j] * xi[j]
        return yi

    def predict(self, x):
        yComputed = [self.sigmoid(self.eval(xi)) for xi in x]
        return [1 if y > 0.5 else 0 for y in yComputed]