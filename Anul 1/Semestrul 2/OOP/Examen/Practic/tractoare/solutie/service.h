#pragma once
#include "repo.h"

class Service {
private:
	Repo& repo;
public:
	Service(Repo& r) : repo{ r } {};

	vector<Tractor> sortare_denumire();
	vector<Tractor> filtrare_tip(string tip);

	void adauga(int id, string den, string tip, int nr);
	void scade_nr(int id);
};