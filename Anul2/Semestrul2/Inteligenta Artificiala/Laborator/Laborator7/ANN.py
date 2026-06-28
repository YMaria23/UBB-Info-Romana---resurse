import random
import numpy as np
import math

class ANN:
    def __init__(self,itterations,layers,neurons,learningRate):
        self.max_nr_of_itterations = itterations
        self.nr_of_layers = layers
        self.nr_of_neurons_per_layer = neurons
        self.learning_rate = learningRate
        self.toleranta = 0.0001

        self.early_stop = 20
        self.lista_ponderi = []

    def relu(self,x):
        return np.maximum(0,x)

    def sigmoid(self,x):
        return 1/(1+np.exp(-x))

    def fit(self,inputs,outputs):
        # formarea vectorului care ne spune cati neuroni sunt pe fiecare strat
        nr_neuroni_strat_intrare = len(inputs[0])
        nr_neuroni_strat_iesire = len(np.unique(outputs))

        if nr_neuroni_strat_iesire == 2:
            nr_neuroni_strat_iesire = 1

        self.nr_of_neurons_per_layer.insert(0,nr_neuroni_strat_intrare)
        self.nr_of_neurons_per_layer.append(nr_neuroni_strat_iesire)

        # initializarea ponderilor -> o sa fie nr_straturi - 1 matrici de ponderi, corespunzatoare legaturilor dintre straturi
        lista_ponderi = []
        for layer in range(self.nr_of_layers + 1):
            # se adauga 1 pentru bias
            strat_sursa = self.nr_of_neurons_per_layer[layer] + 1
            strat_destinatie = self.nr_of_neurons_per_layer[layer + 1]

            matrice_ponderi = []
            #std_dev = np.sqrt(2 / strat_sursa)
            for linie in range(strat_sursa):
                coloana = [random.uniform(-0.1, 0.1) for _ in range(strat_destinatie)]
                #coloana = [random.gauss(0, std_dev) for _ in range(strat_destinatie)]
                matrice_ponderi.append(coloana)

            #lista_ponderi_per_straturi = [random.random() for _ in range(strat_sursa * strat_destinatie)]

            # se adauga lista de ponderi la lista totala
            lista_ponderi.append(matrice_ponderi)


        early_stop_parameter = 0
        loss_vechi = math.inf
        iteration = 1

        # conditie de oprire pe baza de nr maxim de iteratii + early stop (bazat pe loss -> MSE)
        while iteration <= self.max_nr_of_itterations and early_stop_parameter < self.early_stop:
            loss_epoca = 0

            for input,output in zip(inputs,outputs):
                # pentru bias se adauga feature-ul 1
                x = [1] + list(input)

                # tinem minte lista de liste de ponderi (lista de straturi, lista de perechi (net, output))
                mem_retea = []

                # se adauga stratul de intrare
                mem_retea_strat_intrare = []
                for elem in x:
                    mem_retea_strat_intrare.append([None,elem])

                mem_retea.append(mem_retea_strat_intrare)

                # se activeaza neuronii de pe layerele ascunse
                for layer in range(self.nr_of_layers):
                    x_new = []
                    mem_retea_strat = []

                    # se adauga val pentru bias si in memorie
                    mem_retea_strat.append([None,1])

                    for neuron in range(self.nr_of_neurons_per_layer[layer+1]):
                        # se calculeaza net-ul -> suma ponderata
                        net = 0
                        for i in range(len(x)):
                            net += x[i] * lista_ponderi[layer][i][neuron]

                        # aplic ReLU
                        output_neuron = self.relu(net)

                        # adaug la vectorul de outputuri de pe layerul curent
                        x_new.append(output_neuron)

                        # se adauga perechea formata din net si output la stratul curent
                        mem_retea_strat.append([net,output_neuron])

                    # pentru bias se adauga "feature-ul" 1
                    x_new.insert(0,1)
                    x = x_new.copy()

                    # se salveaza stratul curent
                    mem_retea.append(mem_retea_strat)

                # se activeaza neuronii de pe layerul de iesire
                mem_retea_strate_iesire = []
                ultimul_layer_ascuns = self.nr_of_layers
                for neuron in range(self.nr_of_neurons_per_layer[-1]):
                    # se calculeaza net-ul -> suma ponderata
                    net = 0
                    for i in range(len(x)):
                        net += x[i] * lista_ponderi[ultimul_layer_ascuns][i][neuron]

                    # aplic sigmoid
                    output_neuron = self.sigmoid(net)

                    mem_retea_strate_iesire.append([net,output_neuron])

                mem_retea.append(mem_retea_strate_iesire)


                predictie = mem_retea[-1][0][1]

                loss_inregistrare = 0.5 * (output - predictie) ** 2
                loss_epoca += loss_inregistrare


                # se ajusteaza ponderile
                # se stabilesc erorile de pe stratul de iesire
                erori = []

                for neuron in range(self.nr_of_neurons_per_layer[-1]):
                    # se calculeaza eroarea
                    output_neuron = mem_retea[-1][neuron][1]

                    # s-a folosit functia sigmoid
                    eroare = (output - output_neuron) * (1 - output_neuron) * output_neuron

                    erori.append(eroare)

                # se modifica ponderile ce leaga ultimul strat ascuns de stratul de iesire
                for neuron_ascuns in range(self.nr_of_neurons_per_layer[-2] + 1):
                    # se determina output-ul neuronului de pe stratul ascuns
                    output_neuron_ascuns = mem_retea[-2][neuron_ascuns][1]
                    for neuron in range(self.nr_of_neurons_per_layer[-1]):
                        gradient_pondere = self.learning_rate * erori[neuron] * output_neuron_ascuns

                        # actualizarea "oficiala" a ponderii
                        lista_ponderi[-1][neuron_ascuns][neuron] += gradient_pondere

                # backprop prin toate straturile ascunse: de la ultimul la primul
                for layer in range(self.nr_of_layers - 1, -1, -1):
                    erori_curente = []

                    # calcul erori pentru stratul ascuns curent
                    # mem_retea[layer + 1] = stratul ascuns curent (cu bias pe pozitia 0)
                    for neuron in range(1, self.nr_of_neurons_per_layer[layer + 1] + 1):
                        net = mem_retea[layer + 1][neuron][0]

                        eroare = 0
                        for neuron_superior in range(len(erori)):
                            eroare_superioara = erori[neuron_superior]
                            pondere = lista_ponderi[layer + 1][neuron][neuron_superior]
                            eroare += eroare_superioara * pondere

                        # derivata ReLU
                        eroare *= (1 if net > 0 else 0)
                        erori_curente.append(eroare)

                    # actualizare ponderi dintre stratul anterior si stratul curent
                    for neuron_anterior in range(self.nr_of_neurons_per_layer[layer] + 1):
                        output_neuron_anterior = mem_retea[layer][neuron_anterior][1]

                        for neuron_dest in range(len(erori_curente)):
                            gradient_pondere = self.learning_rate * erori_curente[neuron_dest] * output_neuron_anterior
                            lista_ponderi[layer][neuron_anterior][neuron_dest] += gradient_pondere

                    erori = erori_curente.copy()

            loss_epoca /= len(inputs)
            if abs(loss_vechi - loss_epoca) < self.toleranta:
                early_stop_parameter += 1
            else:
                early_stop_parameter = 0

            loss_vechi = loss_epoca

            print(f"Iteratia {iteration}, Loss: {loss_epoca:.10f}")

            # s-a incheiat o iteratie
            iteration += 1

        self.lista_ponderi = lista_ponderi


    def predict(self, inputs):
        predictions = []

        for input_data in inputs:
            # se adauga feature-ul 1 corespunzator bias-ului
            # x -> valorile neuronilor de pe stratul de intrare
            x = [1] + list(input_data)

            # parcurgerea straturilor ascunse
            for layer in range(self.nr_of_layers):
                x_new = []
                # se adauga 1 pentru bias (la fel ca pe stratul de intrare)
                x_new.append(1)

                # se calculeaza rezultatul (output-ul)
                for neuron_dest in range(self.nr_of_neurons_per_layer[layer + 1]):
                    net = 0
                    for i in range(len(x)):
                        net += x[i] * self.lista_ponderi[layer][i][neuron_dest]

                    x_new.append(self.relu(net))

                x = x_new.copy()

            # parcurgerea stratului de iesire
            idx_iesire = self.nr_of_layers
            iesiri_finale = []

            for neuron_dest in range(self.nr_of_neurons_per_layer[-1]):
                net = 0
                for i in range(len(x)):
                    net += x[i] * self.lista_ponderi[idx_iesire][i][neuron_dest]

                iesiri_finale.append(self.sigmoid(net))

            # returneaza rezultatul corespunzator tipului de clasificare ales
            if len(iesiri_finale) == 1:
                predictions.append(iesiri_finale[0])

            else:
                predictions.append(iesiri_finale)

        return predictions