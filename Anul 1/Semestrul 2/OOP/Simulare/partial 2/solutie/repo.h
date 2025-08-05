#pragma once

#include "domain.h"

class Repo {
private:
	vector<Apartament> lista;
	string file_name;
	void citire();
	void scriere();

public:
	Repo() = default;

	Repo(string nume) {
		file_name = nume;
		citire();
	}

	void adauga(const Apartament& apt);
	void sterge(int id);
	void empty();

	const vector<Apartament>& get_all();

	~Repo() {};
};
