#pragma once
#include "repo.h"

class Service {
private:
	Repo& repo;
public:
	Service() = default;

	Service(Repo& repo_nou): repo{repo_nou}{}

	~Service(){}

	void adaugare_service(int id,string tip,string culoare,double pret);
	void sterge_service(int id);
	const vector<Haine>& get_all_service();

	vector<Haine> filtreaza(string tip);
	vector<Haine> sorteaza_tip();
	vector<Haine> sorteaza_pret();
};