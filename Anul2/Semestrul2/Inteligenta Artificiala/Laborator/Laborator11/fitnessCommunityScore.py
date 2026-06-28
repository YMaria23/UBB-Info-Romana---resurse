def fitnessFunction(repres, adjacencyMatrix):
    # repres -> cromozom -> trebuie efectuata operatia de decoding
    communities = repres.decode()

    # COMMUNITY SCORE: 1/k * sum(f(i))
    # f(i) = m_in / (m_in + m_out)^alpha
    # m_in -> nr de muchii complet cuprinse in comunitatea curenta
    # m_out -> nr de muchii care au EXACT o extremitate in comunitatea curenta
    # alpha -> controleaza penalitatea pe care o primeste o comunitate atunci cand are muchii "exterioare"

    Q = 0
    alpha = 0.5

    for community in communities:
        # m_out
        outsiders = 0

        # m_in
        insiders = 0

        for i in community:
            for neighbour,value in enumerate(adjacencyMatrix[i]):
                if value != 0:
                    if neighbour not in community:
                        outsiders += 1
                    else:
                        insiders += 1

        # deoarece este vorba de muchii interne, acestea se vor numara de doua ori: de fiecare data, in cadrul calculului pentru cate o extremitate
        insiders = insiders//2
        total_edges = outsiders + insiders

        if total_edges > 0:
            Q += insiders / (total_edges ** alpha)

    Q /= len(communities)
    return Q