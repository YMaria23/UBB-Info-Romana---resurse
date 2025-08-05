#include "domain.h"

int Apartament::get_id() const {
	return id;
}

double Apartament::get_suprafata() const {
	return suprafata;
}

string Apartament::get_strada() const {
	return strada;
}

double Apartament::get_pret() const {
	return pret;
}

void Apartament::set_id(int id_nou) {
	id = id_nou;
}

void Apartament::set_suprafata(double sup) {
	suprafata = sup;
}

void Apartament::set_strada(string str) {
	strada = str;
}

void Apartament::set_pret(double pret_nou) {
	pret = pret_nou;
}

vector<string> split(string input, char separator) {
	stringstream ss(input);
	vector<string> rezultat;

	string part;

	while (std::getline(ss, part, separator))
		rezultat.push_back(part);

	return rezultat;
}

std::istream& operator >>(std::istream& input, Apartament& apt) {
	string line;
	std::getline(input, line);

	vector<string> parts = split(line, ',');

	if (parts.size() != 4)
		return input;

	apt.set_id(stoi(parts[0]));
	apt.set_suprafata(stod(parts[1]));
	apt.set_strada(parts[2]);
	apt.set_pret(stod(parts[3]));

	return input;

}

std::ostream& operator <<(std::ostream& output, const Apartament& apt) {
	output << apt.get_id() << "," << apt.get_suprafata() << "," << apt.get_strada() << "," << apt.get_pret() << "\n";
	return output;
}