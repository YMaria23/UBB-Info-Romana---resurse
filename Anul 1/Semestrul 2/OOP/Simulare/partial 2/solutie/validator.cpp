#include "validator.h"

bool Validare::validare_id(int id) {
	return id > 0;
}

bool Validare::validare_nr_real(double nr) {
	return nr > 0;
}