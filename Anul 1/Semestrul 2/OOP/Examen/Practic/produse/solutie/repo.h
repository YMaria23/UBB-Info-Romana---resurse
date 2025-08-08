#pragma once
#include "domain.h"
#include <vector>

class Repo {
private:
	vector<Produs> lista;
	string nume;
	void citire();
	void scriere();
public:
	Repo(string file) {
		nume = file;
		citire();
	}

	vector<Produs>& acceseaza_lista();
	void adauga(Produs e);
};