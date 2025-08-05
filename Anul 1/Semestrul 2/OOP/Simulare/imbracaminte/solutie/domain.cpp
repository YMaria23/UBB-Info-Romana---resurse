#include "domain.h"
#include <string.h>

int Haine::get_id() const {
	return id;
}

string Haine::get_tip() const{
	return tip;
}

string Haine::get_culoare() const{
	return culoare;
}

double Haine::get_pret() const{
	return pret;
}

void Haine::set_id(int id_nou) {
	id = id_nou;
}

void Haine::set_tip(string tip_nou) {
	tip = tip_nou;
}

void Haine::set_culoare(string c) {
	culoare = c;
}

void Haine::set_pret(double pret_nou) {
	pret = pret_nou;
}

vector<string> split(string line, char separator) {
	stringstream ss(line);

	string part;
	vector<string> rezultat;

	while (std::getline(ss, part, separator))
		rezultat.push_back(part);

	return rezultat;
}

std::istream& operator >>(std::istream& input, Haine& haina) {
	string line;
	std::getline(input, line);

	vector<string> parts = split(line, ',');

	if (parts.size() != 4)
		return input;

	haina.set_id(stoi(parts[0]));
	haina.set_tip(parts[1]);
	haina.set_culoare(parts[2]);
	haina.set_pret(stod(parts[3]));

	return input;
}

std::ostream& operator <<(std::ostream& output,const Haine& haina) {
	output << haina.get_id() << "," << haina.get_tip() << "," << haina.get_culoare() << "," << haina.get_pret() << "\n";
	return output;
}