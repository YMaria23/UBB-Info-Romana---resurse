#include "repo.h"

vector<string> split(string input, char separator) {
	//functie ce separa elementele dintr-un string, dupa separator
	stringstream ss(input);

	vector<string> rezultat;
	string part;

	while (getline(ss, part, separator))
		rezultat.push_back(part);

	return rezultat;
}

void Repo::citeste() {
	//functie ce se ocupa de citirea din fisier
	ifstream f(file);

	string line;
	while (getline(f, line)) {
		vector<string> parts = split(line, ',');

		Planta elem{ -1,"","",-1 };
		elem.set_cod(stoi(parts[0]));
		elem.set_denumire(parts[1]);
		elem.set_tip(parts[2]);
		elem.set_apa(stoi(parts[3]));

		lista.push_back(elem);
	}
}

void Repo::scrie() {
	//functie ce se ocupa de scrierea in fisier
	ofstream g(file);

	for (auto& elem : lista)
		g << elem.get_cod() << "," << elem.get_denumire() << "," << elem.get_tip() << "," << elem.get_apa() << "\n";

	g.close();
}

void Repo::adauga(Planta& elem){
	//functie ce adauga un element in lista de plante
	lista.push_back(elem);
	scrie();
}

const vector<Planta>& Repo:: get_all() {
	//functie ce returneaza lista de plante -> nu poate fi modificata ulterior
	return lista;
}

vector<Planta>& Repo::acces() {
	//functie ce returneaza lista de plante -> poate fi modificata ulterior
	return lista;
}

void Repo::empty() {
	//functia sterge toate elementele din fisier
	ofstream g(file);
	g << "";

	g.close();
}