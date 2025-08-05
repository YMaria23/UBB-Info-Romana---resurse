#pragma once
#include "repo.h"
#include "validare.h"

class Service {
private:
	Repo& repo;
	Validare validator;
public:
	Service() = default;
	Service(Repo& repo_nou, Validare valid) : repo{ repo_nou }, validator{ valid } {};
	~Service() {};

	void inchiriaza_service(int cod);
	const vector<Rochie>& get_all_service();

	vector<Rochie> sortare_marime();
	vector<Rochie> sortare_pret();
};