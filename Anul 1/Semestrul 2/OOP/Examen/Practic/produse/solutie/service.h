#pragma once
#include "repo.h"
#include "observer.h"

class Service : public Observable {
private:
	Repo& repo;
public:
	Service(Repo& r) :repo{ r } {
		notify();
	};

	vector<Produs> sorteaza_pret();
	void adauga(int id, string nume, string tip, double pret);

	vector<Produs> filtrare_pret(double pret);
	int numara_tip(string tip);
};