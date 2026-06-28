import random
from Chromosome import Chromosome
from ChromosomePonderat import ChromosomePonderat


class GA:
    def __init__(self, param=None, problParam=None):
        self.__param = param
        self.__problParam = problParam
        self.__population = []

        self.transformIntoListOfNeighboursAndWeights()

    @property
    def population(self):
        return self.__population


    def transformIntoListOfNeighboursAndWeights(self):
        adjacencyMatrix = self.__problParam['adjacencyMatrix']

        neighbours = []
        weights = []
        for line in adjacencyMatrix:
            indexNeighbours = []
            indexWeights = []
            for index,elem in enumerate(line):
                if elem != 0:
                    indexNeighbours.append(index)
                    indexWeights.append(elem)

            neighbours.append(indexNeighbours)
            weights.append(indexWeights)

        self.__problParam['listOfNeighbours'] = neighbours
        self.__problParam['listOfWeights'] = weights

    def initialisation(self):
        for _ in range(0, self.__param['popSize']):
            c = ChromosomePonderat(self.__problParam)
            self.__population.append(c)

    def selection(self):
        pos1 = random.randint(0, self.__param['popSize'] - 1)
        pos2 = random.randint(0, self.__param['popSize'] - 1)
        if (self.__population[pos1].fitness > self.__population[pos2].fitness):
            return pos1
        else:
            return pos2

    def oneGeneration(self):
        newPop = []
        for _ in range(self.__param['popSize']):
            p1 = self.__population[self.selection()]
            p2 = self.__population[self.selection()]
            off = p1.crossover(p2)

            mutationProbability = random.random()

            if mutationProbability < self.__param['mutProb']:
                off.mutation()

            newPop.append(off)

        self.__population = newPop
        self.evaluation()

    def oneGenerationElitism(self):
        newPop = [self.bestChromosome()]
        for _ in range(self.__param['popSize'] - 1):
            p1 = self.__population[self.selection()]
            p2 = self.__population[self.selection()]
            off = p1.crossover(p2)

            mutationProbability = random.random()

            if mutationProbability < self.__param['mutProb']:
                off.mutation()

            newPop.append(off)
        self.__population = newPop
        self.evaluation()

    def oneGenerationSteadyState(self):
        for _ in range(self.__param['popSize']):
            p1 = self.__population[self.selection()]
            p2 = self.__population[self.selection()]
            off = p1.crossover(p2)

            # mutatia se aplica cu o anumita probabilitate
            if random.random() < self.__param['mutProb']:
                off.mutation()

            off.fitness = self.__problParam['function'](off)

            worstIndex = self.__population.index(self.worstChromosome())

            if off.fitness > self.__population[worstIndex].fitness:
                # se inlocuieste cel mai "prost" cromozom, cu cel generat acum (daca cel actual este mai bun)
                self.__population[worstIndex] = off

    def evaluation(self):
        for c in self.__population:
            c.fitness = self.__problParam['function'](c)

    def bestChromosome(self):
        best = self.__population[0]
        for c in self.__population:
            if (c.fitness > best.fitness):
                best = c
        return best

    def worstChromosome(self):
        worst = self.__population[0]
        for c in self.__population:
            if (c.fitness < worst.fitness):
                worst = c
        return worst

    def run(self):
        self.initialisation()
        self.evaluation()

        for _ in range(self.__param['noGen']):
            self.oneGenerationElitism()

        # se va returna cea mai buna solutie
        return self.bestChromosome()