#pragma once
#include "domain.h"

class Repo {
private:
	vector<Rochie> lista;
	string name_file;
	void scriere();
	void citire();
public:
	Repo() = default;
	Repo(string nume) {
		name_file = nume;
		citire();
	}

	~Repo() {};

	void adauga(Rochie& rochie_noua);
	void inchiriaza(int cod);

	void empty();

	const vector<Rochie>& get_all();
	vector<Rochie> acceseaza();
};
