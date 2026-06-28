import random
import numpy as np
import math
from ANN import ANN


class CNN:
    def __init__(self, iterations, learningRate, num_filters, filter_size,layers,neurons):
        self.max_nr_of_iterations = iterations
        self.learning_rate = learningRate
        self.num_filters = num_filters
        self.filter_size = filter_size  # acel F din teorie
        self.early_stop = 20
        self.tolerance = 0.0001

        self.layers = layers
        self.neurons_per_layer = neurons

        # filtrele vor avea 4 dimensiuni: nr total de filtre (K), inaltimea + latimea (F x F), adancimea (D)
        # adancimea corespunde cu "feature-urile" datelor de intrare
        self.filters = []
        self.bias_filters = []

        self.lista_ponderi = []


    def relu(self, x):
        return np.maximum(0, x)

    def sigmoid(self, x):
        return 1 / (1 + np.exp(-x))

    '''
    def init_filters(self, depth):
        # se initializeaza aleator K filtre de F x F x D (adancime)
        self.filters = np.random.uniform(-0.1, 0.1,(self.num_filters, self.filter_size, self.filter_size, depth))
        self.bias_filters = np.zeros(self.num_filters)
    '''

    def init_network(self, input_depth, flattened_size, num_outputs):
        # se initializeaza aleator K filtre de F x F x D (adancime)
        scale_filters = np.sqrt(2.0 / (self.filter_size * self.filter_size * input_depth))
        self.filters = np.random.randn(self.num_filters, self.filter_size, self.filter_size,
                                       input_depth) * scale_filters
        self.bias_filters = np.zeros(self.num_filters)

        # se initializeaza ponderile (pas pentru ANN -> FC layer)
        # [dim_aplatizată, n1, n2, ..., outputs]
        structura = [flattened_size] + self.neurons_per_layer + [num_outputs]

        self.lista_ponderi = []
        for i in range(len(structura) - 1):
            # +1 pentru bias
            fan_in = structura[i] + 1
            m = np.random.randn(fan_in, structura[i + 1]) * np.sqrt(2.0 / fan_in)
            self.lista_ponderi.append(m)

    def convolution_layer(self, image_input):
        # image_input are forma [64, 64, 3]
        H, W, D = image_input.shape
        F = self.filter_size
        # padding
        P = 1

        padded = np.pad(image_input, ((P, P), (P, P), (0, 0)), mode='constant')


        # (N + 2P - F) / S + 1 -> pentru un S mai mare se reduce volumul de calcul pentru straturile exterioare, insa rezultatele nu sunt atat de "fine"
        H_out = (H + 2*P - F) + 1
        W_out = (W + 2*P - F) + 1

        # harta (de harti) de activare
        # 3D -> inaltime, latime, nr_filtre
        activation_maps = np.zeros((H_out, W_out, self.num_filters))

        for k in range(self.num_filters):
            for i in range(H_out):
                for j in range(W_out):
                    # se extrage acea fereastra -> receptive field
                    # pe linie: de la i la i + F
                    # pe coloana: de la j la j + F
                    # pe adancime -> se ia tot
                    window = padded[i:i + F, j:j + F, :]

                    # produs scalar intre filtru si window
                    suma = np.sum(window * self.filters[k])

                    # se aplica functia de activare
                    activation_maps[i, j, k] = self.relu(suma + self.bias_filters[k])

        return activation_maps

    def max_pooling_layer(self,harta_activare, size=2, stride=2):
        # harta de activare are 2 dimensiuni
        h, w = harta_activare.shape

        # se calculeaza dimensiunile noii harti reduse
        new_h = (h - size) // stride + 1
        new_w = (w - size) // stride + 1

        pooled_map = np.zeros((new_h, new_w))

        for i in range(0, new_h):
            for j in range(0, new_w):
                # se defineste fereatra de stride x stride (in cazul de fata 2 x 2)
                start_i = i * stride
                start_j = j * stride
                fereastra = harta_activare[start_i: start_i + size, start_j: start_j + size]

                # se pastreaza doar cea mai mare valoare -> max pooling
                pooled_map[i, j] = np.max(fereastra)

        return pooled_map

    def max_pooling_with_mask(self, conv_out, size=2, stride=2):
        # harta (de harti) de activare are 3 dimensiuni
        # contine toate hartile de activare (care au 2D)
        H, W, K = conv_out.shape

        # se calculeaza dimensiunile noii harti reduse
        new_h = (H - size) // stride + 1
        new_w = (W - size) // stride + 1

        pooled_out = np.zeros((new_h, new_w, K))

        # masca are aceeasi dimensiune cu intrarea initiala (va contine 1 pt pixelii care merg mai departe si cauzeaza eroarea si 0 pt ceilalti)
        # este utila in backpropagation atunci cand vrem sa vedem cine este "vinovat" pentru eroare
        mask = np.zeros((H, W, K))

        for k in range(K):
            for i in range(new_h):
                for j in range(new_w):
                    # se defineste fereatra de stride x stride (in cazul de fata 2 x 2)
                    start_i, start_j = i * stride, j * stride
                    window = conv_out[start_i:start_i + size, start_j:start_j + size, k]

                    # se pastreaza doar cea mai mare valoare -> max pooling
                    m = np.max(window)
                    pooled_out[i, j, k] = m

                    # se determina unde a fost gasit acel maxim in window
                    # argmax genereaza doar indexul liniar
                    # unravel_index determina coordonatele
                    idx = np.unravel_index(np.argmax(window), window.shape)

                    # se marcheaza pixelul care va "trece mai departe" in analiza
                    mask[start_i + idx[0], start_j + idx[1], k] = 1

        return pooled_out, mask

    def ann_forward(self, flattened_input):
        # se adauga feature-urile ca output pentru stratul de intrare
        current_output = np.insert(flattened_input, 0, 1)

        # lista cu outputurile de pe fiecare strat
        outputs_straturi = [current_output]
        # pentru stratul de intrare nu avem net
        nets = []


        for i in range(len(self.lista_ponderi)):
            # se calculeaza net-ul de pe stratul curent
            net = np.dot(current_output, self.lista_ponderi[i])
            nets.append(net)

            # activarea neuronului
            if i == len(self.lista_ponderi) - 1:
                # pentru ultimul strat, functia de activare este sigmoidul
                current_output = self.sigmoid(net)
            else:
                # pentru straturile ascunse, functia de activare este ReLU
                current_output = self.relu(net)
                # se adauga bias
                current_output = np.insert(current_output, 0, 1)

            outputs_straturi.append(current_output)

        return outputs_straturi, nets

    def ann_backprop(self, outputs_straturi, nets, initial_error):
        gradienti_ann = []

        # eroarea de pe stratul de iesire este eroarea initiala (cea data de output-ul efectiv)
        erori_strat_curent = np.array(initial_error, dtype=float).flatten()

        # se parcurg straturile invers
        for i in range(len(self.lista_ponderi) - 1, -1, -1):
            output_anterior = outputs_straturi[i]

            # dim matricei de ponderi curente
            nr_sursa = len(output_anterior)
            nr_dest = len(erori_strat_curent)

            # initializarea matricii de gradienti
            grad_matrice = np.zeros((nr_sursa, nr_dest))

            for s in range(nr_sursa):
                for d in range(nr_dest):
                    grad_matrice[s][d] = output_anterior[s] * erori_strat_curent[d]

            gradienti_ann.insert(0, grad_matrice)

            # propagarea erorii
            if i > 0:
                # stratul anterior spre cel curent (fara bias)
                ponderi_fara_bias = self.lista_ponderi[i][1:, :]

                # se calculeaza eroarea propagata (eroare = sum(w*eroare))
                eroare_propagata = np.zeros(len(nets[i - 1]))
                for neuron_ant in range(len(nets[i - 1])):
                    suma_eroare = 0
                    for neuron_curr in range(len(erori_strat_curent)):
                        suma_eroare += erori_strat_curent[neuron_curr] * ponderi_fara_bias[neuron_ant][neuron_curr]

                    # vedem daca neuronul este activ sau nu
                    derivata_relu = 1 if nets[i - 1][neuron_ant] > 0 else 0
                    eroare_propagata[neuron_ant] = suma_eroare * derivata_relu

                erori_strat_curent = eroare_propagata

        # erorile pentru invatarea filtrelor
        # se folosesc ponderile de pe primul strat
        ponderi_intrare = self.lista_ponderi[0][1:, :]
        eroare_input_ann = np.zeros(ponderi_intrare.shape[0])

        for pixel_idx in range(ponderi_intrare.shape[0]):
            suma_px = 0
            for neuron_strat1 in range(len(erori_strat_curent)):
                suma_px += erori_strat_curent[neuron_strat1] * ponderi_intrare[pixel_idx][neuron_strat1]
            eroare_input_ann[pixel_idx] = suma_px

        return gradienti_ann, eroare_input_ann

    def update_filters_weights(self, img, d_conv):
        F = self.filter_size
        # d_conv are forma (H_out, W_out, K)

        P = 1

        padded_img = np.pad(img, ((P, P), (P, P), (0, 0)), mode='constant')

        for k in range(self.num_filters):
            for i in range(F):
                for j in range(F):
                    # adancime  3 (RGB)
                    for d in range(img.shape[2]):
                        # se extrage regiunea/ window-ul aftectat de pixelul respectiv + inmultire cu eroare
                        region = padded_img[i: i + d_conv.shape[0], j: j + d_conv.shape[1], d]
                        gradient = np.sum(region * d_conv[:, :, k])

                        # actualizarea filtrului
                        self.filters[k, i, j, d] += self.learning_rate * gradient

            # bias-ul a contribuit la toate output-urile -> ia din vina de la toti
            self.bias_filters[k] += self.learning_rate * np.sum(d_conv[:, :, k])

    def train_step(self, img, target):
        # stratul convolutiv
        conv_out = self.convolution_layer(img)

        # stratul de pooling -> foloseste max pooling
        pool_out, mask = self.max_pooling_with_mask(conv_out)

        # trebuie aplatizat pentru a reveni cu logica de ANN
        flattened = pool_out.flatten()
        outputs_straturi, nets = self.ann_forward(flattened)

        # eroarea stratului de iesire (bazata pe functia de activare: sigmoid)
        predicitie = outputs_straturi[-1][0]
        eroare_iesire = (target - predicitie) * predicitie * (1 - predicitie)
        loss = 0.5 * (target - predicitie) ** 2

        # backprop prin straturile ANN (logica asemanatoare cu fit-ul de la ANN)
        gradienti_ann, eroare_input_ann = self.ann_backprop(outputs_straturi, nets, eroare_iesire)

        # actualizarea ponderilor din ANN
        for i in range(len(self.lista_ponderi)):
            self.lista_ponderi[i] += self.learning_rate * gradienti_ann[i]

        # partea de invatare pentru filtre
        # ann-ul da datele aplatizate -> ne trebuie in forma pool-ului calculat anterior
        d_pool = eroare_input_ann.reshape(pool_out.shape)
        d_pool_expanded = np.repeat(np.repeat(d_pool, 2, axis=0), 2, axis=1)

        # s-a distribuit eroarea prin masca
        d_conv = d_pool_expanded * mask * (conv_out > 0)
        self.update_filters_weights(img, d_conv)

        return loss


    def fit(self, inputs, outputs):
        # inputs are forma (shape-ul) [N, 64, 64, 3]; N -> nr de inregistrari
        depth = inputs.shape[3]

        conv_h = inputs.shape[1]
        conv_w = inputs.shape[2]

        pool_h = conv_h // 2
        pool_w = conv_w // 2

        flattened_size = pool_h * pool_w * self.num_filters

        num_outputs = 1 if len(np.unique(outputs)) <= 2 else len(np.unique(outputs))

        self.init_network(depth,flattened_size,num_outputs)

        iteration = 1
        loss_vechi = math.inf
        early_stop_cnt = 0

        while iteration <= self.max_nr_of_iterations and early_stop_cnt < self.early_stop:
            loss_epoca = 0

            idx = np.random.permutation(len(inputs))
            inputs = inputs[idx]
            outputs = outputs[idx]

            for img, target in zip(inputs, outputs):
                loss_inregistrare = self.train_step(img, target)
                loss_epoca += loss_inregistrare

            loss_epoca /= len(inputs)
            if abs(loss_vechi - loss_epoca) < self.tolerance:
                early_stop_cnt += 1
            else:
                early_stop_cnt = 0

            loss_vechi = loss_epoca

            print(f"Iteratia {iteration}, Loss: {loss_epoca:.10f}")

            # s-a incheiat o iteratie
            iteration += 1


    def predict(self, inputs):
        predictions = []
        for img in inputs:
            # strat convolutiv
            conv_out = self.convolution_layer(img)

            # strat de pooling
            # nu am nevoie de masca aici
            pool_out, _ = self.max_pooling_with_mask(conv_out)
            flattened = pool_out.flatten()

            outputs_straturi, _ = self.ann_forward(flattened)
            predictions.append(outputs_straturi[-1][0])
            #predictions.append(outputs_straturi[-1])
        return np.array(predictions)