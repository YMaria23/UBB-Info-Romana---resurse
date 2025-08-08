#pragma once
#include <iostream>
#include <sstream>
#include <string>

using namespace std;

class Produs {
private:
	int id;
	string nume;
	string tip;
	double pret;
public:
	Produs(int id_ot, string n, string t, double pret_ot) : id{ id_ot }, nume{ n }, tip{ t }, pret{ pret_ot } {};

	int get_id() const;
	string get_nume() const;
	string get_tip() const;
	double get_pret() const;

	void set_id(int id);
	void set_nume(string nume);
	void set_tip(string tip);
	void set_pret(double pret);

	int nr_vocale() const;
};