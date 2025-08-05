#pragma once

#include "domain.h"

class Validare {
public:
	Validare() = default;
	~Validare() {};

	bool validare_id(int id);
	bool validare_nr_real(double nr);
};