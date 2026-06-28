def fitnessFunction(repres, adjacencyMatrix):
    # repres -> cromozom -> trebuie efectuata operatia de decoding
    communities = repres.decode()
    m = sum(sum(row) for row in adjacencyMatrix) / 2

    # MODULARITY: sum(eii - ai^2) -> simplist
    # eii -> nr de muchii care se afla in comunitatea curenta
    # ai -> nr de muchii care au cel putin o extremitate in comunitatea curenta

    # MODULARITY: 1/(2*m) * sum_in_same_module(aij - ki*kj/(2*m)) -> explicativ
    # aij -> elementul de pe linia i, coloana j din matricea de adiacenta
    # ki -> gradul nodului i
    # m -> nr de muchii din graf/retea
    # varianta cu 1/2m -> oricate module (1/4m -> strict pe doua module)

    Q = 0

    for community in communities:
        for i in community:
            for j in community:
                ki = sum(adjacencyMatrix[i])
                kj = sum(adjacencyMatrix[j])

                Q += adjacencyMatrix[i][j] - ki * kj / (2 * m)

    Q *= 1/(2*m)
    return Q

