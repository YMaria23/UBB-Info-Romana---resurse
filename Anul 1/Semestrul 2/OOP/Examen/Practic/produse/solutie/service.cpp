#include "service.h"
#include <algorithm>

vector<Produs> Service::sorteaza_pret() {
	vector<Produs> v = repo.acceseaza_lista();

	sort(v.begin(), v.end(), [&](Produs a, Produs b) {
		return a.get_pret() < b.get_pret();
		});

	return v;
}

void Service::adauga(int id, string nume, string tip, double pret) {
	if (nume == "" || tip == "" || !(pret >= 1 && pret <= 100))
		throw std::exception("Date invalide!");
	Produs e{ id,nume,tip,pret };
	repo.adauga(e);
	notify();
}

vector<Produs> Service::filtrare_pret(double pret) {
	vector<Produs> rez;
	vector<Produs> v = repo.acceseaza_lista();

	for (auto elem : v)
		if (elem.get_pret() <= pret)
			rez.push_back(elem);

	return rez;
}

int Service::numara_tip(string tip) {
	int nr = 0;
	vector<Produs> v = repo.acceseaza_lista();

	for (auto elem : v)
		if (elem.get_tip() == tip)
			nr++;

	return nr;
}