#pragma once

#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>

using namespace std;

class Apartament {
private:
	int id;
	double suprafata;
	string strada;
	double pret;

public:
	Apartament() = default;
	Apartament(int id_nou, double sup_noua, string str_noua, double pret_nou) : id{ id_nou }, suprafata{ sup_noua }, strada{ str_noua }, pret{ pret_nou } {

	};

	//Apartament(Apartament& apt) : id{ apt.id }, suprafata{ apt.suprafata }, strada{ apt.strada }, pret{ apt.pret } {

	//}/


	int get_id() const;
	double get_suprafata() const;
	string get_strada() const;
	double get_pret() const;

	void set_id(int id);
	void set_suprafata(double sup);
	void set_strada(string str);
	void set_pret(double pret);

	friend std::istream& operator >>(std::istream& input, Apartament& apt);

	~Apartament() {

	}
};

std::ostream& operator <<(std::ostream& output, const Apartament& apt);