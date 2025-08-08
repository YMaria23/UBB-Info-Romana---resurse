#include "domain.h"

int Tractor::get_id() const {
	return id;
}

string Tractor::get_denumire() const {
	return denumire;
}

string Tractor::get_tip() const {
	return tip;
}

int Tractor::get_nr() const {
	return nr;
}

void Tractor::set_id(int id_n) {
	id = id_n;
}

void Tractor::set_denumire(string d) {
	denumire = d;
}

void Tractor::set_tip(string t) {
	tip = t;
}

void Tractor::set_nr(int n) {
	nr = n;
}