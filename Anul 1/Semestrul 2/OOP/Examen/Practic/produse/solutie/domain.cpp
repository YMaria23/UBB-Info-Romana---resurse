#include "domain.h"

int Produs::get_id() const {
	return id;
}

string Produs::get_nume() const {
	return nume;
}

string Produs::get_tip() const {
	return tip;
}

double Produs::get_pret() const {
	return pret;
}

void Produs::set_id(int id_nou) {
	id = id_nou;
}

void Produs::set_nume(string n) {
	nume = n;
}

void Produs::set_tip(string t) {
	tip = t;
}

void Produs::set_pret(double p) {
	pret = p;
}

int Produs::nr_vocale() const {
	int nr = 0;
	for (int i = 0; i < nume.size(); i++)
		if (nume[i] == 'a' || nume[i] == 'A' || nume[i] == 'e' || nume[i] == 'E' || nume[i] == 'i' || nume[i] == 'I' || nume[i] == 'o' || nume[i] == 'O' || nume[i] == 'u' || nume[i] == 'U')
			nr++;
	return nr;
}