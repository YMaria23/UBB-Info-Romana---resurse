#pragma once
#include "repo.h"
#include "observer.h"

class Service :public Observable {
private:
	Repo& repo;
public:
	Service(Repo& r) :repo{ r } {
		notify();
	};

	vector<Task> sortare_stare();
	void adauga(int id, string d, string s, string pr);
	vector<Task> filtrare_pr(string nume);

	void change_state(int id, string nume);
	vector<Task> filtrare_stare(string nume);
};