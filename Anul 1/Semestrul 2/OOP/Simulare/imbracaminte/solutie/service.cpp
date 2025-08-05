#include "service.h"
#include <algorithm>

void Service::adaugare_service(int id,string tip, string culoare, double pret) {
	Haine haina{ id,tip,culoare,pret };
	repo.adaugare(haina);
}

void Service::sterge_service(int id) {
	repo.sterge(id);
}

const vector<Haine>& Service::get_all_service() {
	return repo.get_all();
}

vector<Haine> Service::filtreaza(string tip) {
	vector<Haine> rezultat;

	for (auto& haina : get_all_service())
		if (haina.get_tip() == tip)
			rezultat.push_back(haina);

	return rezultat;
}

bool cmp(Haine haina1, Haine haina2) {
	return haina1.get_tip() < haina2.get_tip();
}

bool cmp2(Haine haina1, Haine haina2) {
	return haina1.get_pret() < haina2.get_pret();
}

vector<Haine> Service::sorteaza_tip() {
	vector<Haine> rez = repo.acces();

	sort(rez.begin(), rez.end(), cmp);
	return rez;
}

vector<Haine> Service::sorteaza_pret() {
	vector<Haine> rez = repo.acces();

	sort(rez.begin(), rez.end(), cmp2);
	return rez;
}