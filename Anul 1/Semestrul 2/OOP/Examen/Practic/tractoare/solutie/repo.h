#pragma once
#include "domain.h"

class Repo {
private:
	string nume;
	vector<Tractor> lista;
	void citire();
	void scriere();
public:
	Repo(string file) {
		nume = file;
		citire();
	}

	vector<Tractor>& acces_lista();

	void adauga(Tractor t);
	void sterge_nr(int index);
};