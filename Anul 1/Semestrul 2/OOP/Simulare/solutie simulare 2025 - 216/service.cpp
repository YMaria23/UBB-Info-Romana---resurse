#include "service.h"
#include <algorithm>

const vector<Planta>& Service::get_all_service() {
	//functie ce returneaza lista de plante
	return repo.get_all();
}

bool cmp(Planta p1, Planta p2) {
	//functie de comparare pentru sortare
	return p1.get_tip() < p2.get_tip();
}

vector<Planta> Service::sortare_tip() {
	//functia returneaza o lista noua, sortata dupa tip -> crescator
	vector<Planta> rez = repo.acces();
	sort(rez.begin(), rez.end(), cmp);

	return rez;
}

vector<Planta> Service::filtrare() {
	//functia returneaza o lista cu toate elementele ce au un necesar de apa mai mic de 300
	vector<Planta> rez;
	for (auto& elem : repo.get_all())
		if (elem.get_apa() < 300)
			rez.push_back(elem);

	return rez;
}