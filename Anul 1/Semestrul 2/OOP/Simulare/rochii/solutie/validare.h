#pragma once
#include "domain.h"

class Validare {
public:
	bool validare_cod(int cod);
	bool validare_pret(double pret);
	bool validare_disponibilitate(string disp);
};