#include "validare.h"

bool Validare::validare_cod(int cod) {
	return cod > 0;
}

bool Validare::validare_pret(double pret) {
	return pret > 0;
}

bool Validare::validare_disponibilitate(string disp) {
	return disp == "True" || disp == "False";
}