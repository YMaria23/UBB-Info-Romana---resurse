#include "service.h"

void Service::adauga_service(int id, double sup, string str, double pret) {
	if (validator.validare_id(id) && validator.validare_nr_real(sup) && validator.validare_nr_real(pret)) {
		Apartament apt{ id,sup,str,pret };
		repo.adauga(apt);
	}
	else
		throw std::exception("Date invalide!\n");
}

const vector<Apartament>& Service::get_all_service() {
	return repo.get_all();
}

void Service::sterge_service(int id) {
	if (validator.validare_id(id))
		repo.sterge(id);
	else
		throw std::exception("Date invalide!\n");
}

vector<Apartament> Service::filtrare(double sup1, double sup2, const vector<Apartament>& lista) {
	vector<Apartament> rezultat;
	for (auto& apt : lista)
		if (apt.get_suprafata() >= sup1 && apt.get_suprafata() <= sup2)
			rezultat.push_back(apt);
	return rezultat;
}

vector<Apartament> Service::filtrare_pret(double pret1, double pret2) {
	vector<Apartament> rezultat;
	for (auto& apt : get_all_service())
		if (apt.get_pret() >= pret1 && apt.get_pret() <= pret2)
			rezultat.push_back(apt);
	return rezultat;
}