import random
import numpy as np


class Chromosome:
    def __init__(self, problParam=None):
        self.__problParam = problParam

        # reprezentantul pentru o solutie
        self.__repres = self.generateChromosome()
        self.__fitness = 0.0

    # pentru structura cromozomului ( = solutie posibila), se va utiliza Locus-based Adjacency
    def generateChromosome(self):
        neighbours = self.__problParam['listOfNeighbours']
        chromosome = []

        for node in range(len(neighbours)):
            # se determina random vecinul cu care il "cupleaza"
            neighbour = random.choice(neighbours[node])
            chromosome.append(neighbour)

        return chromosome

    @property
    def repres(self):
        return self.__repres

    @property
    def fitness(self):
        return self.__fitness

    @repres.setter
    def repres(self, l=None):
        if l is None:
            self.__repres = self.generateChromosome()
        else:
            self.__repres = l

    @fitness.setter
    def fitness(self, fit=0.0):
        self.__fitness = fit

    # se va utiliza Standard uniform crossover -> se potriveste cu locus-based representation
    def crossover(self, c):
        # se creeaza o masca binara, de dimensiune n (cate noduri sunt in retea)
        binaryMask = np.random.choice([1, 0], size= len(self.__repres))

        offspring = np.zeros(len(self.__repres),dtype=int)

        for index in range(len(self.__repres)):
            if binaryMask[index] == 1:
                offspring[index] = c.repres[index]
            else:
                offspring[index] = self.__repres[index]

        newChromosome = Chromosome(self.__problParam)
        newChromosome.repres = offspring.tolist()

        return newChromosome


    # se va schimba vecinul unui nod, cu un altul
    def mutation(self):
        # se determina pozitia pe care se va face schimbarea (carui nod ii schimbam vecinul)
        pos = random.randint(0, len(self.__repres) - 1)

        # se determina vecinul cu care va fi inlocuit vecinul vechi
        newNeighbour = random.choice(self.__problParam['listOfNeighbours'][pos])
        self.__repres[pos] = newNeighbour

    def decode(self):
        n = len(self.__repres)
        visited = [False] * n

        # se vor salva comunitatiile sub forma unei liste de liste (fiecare lista interioara va contine nodurile unei comunitati)
        communities = []

        for node in range(n):
            if not visited[node]:
                community = []
                stack = [node]
                while stack:
                    curr = stack.pop()
                    if not visited[curr]:
                        visited[curr] = True
                        community.append(curr)
                        neighbour = self.__repres[curr]
                        if not visited[neighbour]:
                            stack.append(neighbour)
                communities.append(community)

        return communities

    def __str__(self):
        return '\nChromo: ' + str(self.__repres) + ' has fit: ' + str(self.__fitness)

    def __repr__(self):
        return self.__str__()

    def __eq__(self, c):
        return self.__repres == c.__repres and self.__fitness == c.__fitness