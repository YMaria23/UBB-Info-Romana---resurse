#pragma once

#include "repo.h"
#include "validator.h"

class Service {
private:
	Repo& repo;
	Validare validator;
public:
	Service() = default;
	Service(Repo& ot, Validare ot_valid) : repo{ ot }, validator{ ot_valid } {};
	~Service() {};

	void adauga_service(int id, double sup, string str, double pret);
	void sterge_service(int id);
	vector<Apartament> filtrare(double sup1, double sup2, const vector<Apartament>& lista);
	vector<Apartament> filtrare_pret(double pret1, double pret2);

	const vector<Apartament>& get_all_service();
};
