#include "domain.h"

int Planta::get_cod() const {
	//returneaza codul elementului
	return cod;
}

string Planta::get_denumire() const {
	//returneaza denumirea elementului
	return denumire;
}

string Planta::get_tip() const {
	//returneaza tipul elementului
	return tip;
}

int Planta::get_apa() const {
	//retureneaza necesarul de apa al elementului
	return apa;
}

void Planta::set_cod(int cod_nou) {
	//schimba valoarea codului curent
	cod = cod_nou;
}

void Planta::set_denumire(string den) {
	//schimba valoarea denumirii curente
	denumire = den;
}

void Planta::set_tip(string tip_n) {
	//schimba valoarea tipului curent
	tip = tip_n;
}

void Planta::set_apa(int apa_n) {
	//schimba necesarul de apa curent
	apa = apa_n;
}