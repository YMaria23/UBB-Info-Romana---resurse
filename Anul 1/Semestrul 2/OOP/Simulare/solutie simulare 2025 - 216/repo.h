#pragma once
#include "domain.h"
#include <sstream>

class Repo {
private:
	vector<Planta> lista;
	string file;
	void scrie();
	void citeste();
public:
	Repo() = default;
	Repo(string nume) {
		file = nume;
		citeste();
	}
	~Repo(){}

	void adauga(Planta& elem);
	const vector<Planta>& get_all();
	vector<Planta>& acces();

	void empty();
};