#pragma once
#include "repo.h"


class Service {
private:
	Repo& repo;
public:
	Service() = default;
	Service(Repo& repo_nou):repo{repo_nou}{}
	~Service(){}

	const vector<Planta>& get_all_service();

	vector<Planta> sortare_tip();
	vector<Planta> filtrare();
};