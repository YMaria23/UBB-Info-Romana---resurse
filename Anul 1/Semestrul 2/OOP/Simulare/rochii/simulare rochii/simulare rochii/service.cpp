#include "service.h"
#include <algorithm>

void Service::inchiriaza_service(int cod) {
	repo.inchiriaza(cod);
}

const vector<Rochie>& Service::get_all_service() {
	return repo.get_all();
}

bool cmp(Rochie r1, Rochie r2) {
	return r1.get_marime() < r2.get_marime();
}

bool cmp2(Rochie r1, Rochie r2) {
	return r1.get_pret() < r2.get_pret();
}

vector<Rochie> Service::sortare_marime() {
	vector<Rochie> copie = repo.acceseaza();
	sort(copie.begin(), copie.end(), cmp);

	return copie;
}

vector<Rochie> Service::sortare_pret() {
	vector<Rochie> copie = repo.acceseaza();
	sort(copie.begin(), copie.end(), cmp2);

	return copie;
}