#include "service.h"

vector<Tractor> Service::sortare_denumire() {
	vector<Tractor> v = repo.acces_lista();

	sort(v.begin(), v.end(), [&](Tractor a, Tractor b) {
		return a.get_denumire() < b.get_denumire();
		});

	return v;
}

vector<Tractor> Service::filtrare_tip(string nume) {
	vector<Tractor> v = repo.acces_lista();
	vector<Tractor> rez;

	for (auto elem : v)
		if (elem.get_tip() == nume)
			rez.push_back(elem);

	return rez;
}

void Service::adauga(int id, string d, string t, int nr) {
	if (d == "" || t == "" || !(nr % 2 == 0 && nr >= 2 && nr <= 16))
		throw exception("Date invalide!");

	Tractor add{ id,d,t,nr };
	repo.adauga(add);
}

void Service::scade_nr(int id) {
	vector<Tractor> v = repo.acces_lista();

	for (int i = 0; i < v.size(); i++)
		if (v[i].get_id() == id) {
			repo.sterge_nr(i);
			break;
		}
}