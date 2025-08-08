#pragma once
#include "domain.h"

class Repo {
private:
	string nume;
	vector<Task> lista;
	void scriere();
	void citire();
public:
	Repo(string file) {
		nume = file;
		citire();
	}

	vector<Task>& acces_lista();
	void adauga(Task t);
	void change_state(int index, string nume);
	void empty();
};