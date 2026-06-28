
def fitnessFunction(repres, adjacencyMatrix):
    # repres -> cromozom -> trebuie efectuata operatia de decoding
    communities = repres.decode()

    # CONDUCTANCE: 1/k * sum(c(i))
    # c(i) = cut(c,c_)/min(vol(c),vol(c_))
    # vol(c) -> suma tuturor gradelor nodurilor din interiorul comunitatii

    Q = 0

    # suma gradelor nodurilor din toate comunitatile
    overallSum = 0
    for node,elem in enumerate(adjacencyMatrix):
        overallSum += sum(elem)

    for community in communities:
        outsiders = 0

        # suma gradelor nodurilor din comunitate
        summ = 0
        for i in community:
            for neighbour,value in enumerate(adjacencyMatrix[i]):
                if value != 0:
                    if neighbour not in community:
                        # se numara muchiile care leaga comunitatea curenta de alta comunitate
                        outsiders += 1
                    summ += 1

        if min(summ, overallSum - summ) == 0:
            continue

        Q+= outsiders/ min(summ,overallSum - summ)

    Q /= len(communities)
    return -Q