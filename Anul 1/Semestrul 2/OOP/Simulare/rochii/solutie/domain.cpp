#include "domain.h"

int Rochie::get_cod() const {
	return cod;
}

string Rochie::get_denumire() const {
	return denumire;
}

string Rochie::get_marime() const {
	return marime;
}

double Rochie::get_pret() const {
	return pret;
}

string Rochie::get_disponibilitate() const {
	return disponibilitate;
}

void Rochie::set_cod(int cod_nou) {
	cod = cod_nou;
}

void Rochie::set_denumire(string den_noua) {
	denumire = den_noua;
}

void Rochie::set_marime(string marime_noua) {
	marime = marime_noua;
}

void Rochie::set_pret(double pret_nou) {
	pret = pret_nou;
}

void Rochie::set_disponibilitate(string disp_n) {
	disponibilitate = disp_n;
}

vector<string> split(string input, char separator) {
	stringstream ss(input);

	vector<string> rezultat;
	string part;

	while (getline(ss, part, separator))
		rezultat.push_back(part);

	return rezultat;
}

std::istream& operator >>(std::istream& input, Rochie& rochie) {
	string linie;
	std::getline(input, linie);

	vector<string> parts = split(linie, ',');

	if (parts.size() != 5)
		return input;

	rochie.set_cod(stoi(parts[0]));
	rochie.set_denumire(parts[1]);
	rochie.set_marime(parts[2]);
	rochie.set_pret(stod(parts[3]));
	rochie.set_disponibilitate(parts[4]);

	return input;
}

std::ostream& operator <<(std::ostream& output, const Rochie& rochie) {
	output << rochie.get_cod() << "," << rochie.get_denumire() << "," << rochie.get_marime() << "," << rochie.get_pret() << "," << rochie.get_disponibilitate() << "\n";
	return output;
}