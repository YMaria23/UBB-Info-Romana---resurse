#pragma once
#include "domain.h"

class Repo {
private:
	vector<Haine> lista;
	string file_name;
	void scriere();
	void citire();
public:
	Repo() = default;

	Repo(string nume) {
		file_name = nume;
		citire();
	}

	~Repo() {};

	void adaugare(const Haine& haina);
	void sterge(int id);
	void empty();

	const vector<Haine>& get_all();
	vector<Haine> acces();
};