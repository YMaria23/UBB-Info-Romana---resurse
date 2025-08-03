#pragma once
#include <iostream>
#include <vector>
#include <string>
#include <sstream>

using namespace std;

class Rochie {
private:
	int cod;
	string denumire;
	string marime;
	double pret;
	string disponibilitate;

public:
	Rochie() = default;
	Rochie(int ot_nou, string ot_den, string ot_m, double ot_pret, string ot_d) : cod{ ot_nou }, denumire{ ot_den }, marime{ ot_m }, pret{ ot_pret }, disponibilitate{ ot_d } {

	};


	~Rochie() {};

	int get_cod() const;
	string get_denumire() const;
	string get_marime() const;
	double get_pret() const;
	string get_disponibilitate() const;

	void set_cod(int cod);
	void set_denumire(string den);
	void set_marime(string marime);
	void set_pret(double pret);
	void set_disponibilitate(string disp);

	friend std::istream& operator >>(std::istream& input, Rochie& rochie);
};

std::ostream& operator <<(std::ostream& output, const Rochie& rochie);