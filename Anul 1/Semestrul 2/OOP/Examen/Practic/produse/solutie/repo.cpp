#include "repo.h"
#include <fstream>

vector<string> split(string input, char separator) {
	stringstream ss(input);
	string part;
	vector<string> rez;

	while (getline(ss, part, separator))
		rez.push_back(part);

	return rez;
}

void Repo::citire() {
	ifstream f(nume);
	string linie;

	while (getline(f, linie)) {
		vector<string> parts = split(linie, ',');

		Produs p{ -1,"","",-1 };
		p.set_id(stoi(parts[0]));
		p.set_nume(parts[1]);
		p.set_tip(parts[2]);
		p.set_pret(stod(parts[3]));

		lista.push_back(p);
	}
}

void Repo::scriere() {
	ofstream g(nume);

	for (auto elem : lista)
		g << elem.get_id() << "," << elem.get_nume() << "," << elem.get_tip() << "," << elem.get_pret() << endl;

	g.close();
}

vector<Produs>& Repo::acceseaza_lista() {
	return lista;
}

void Repo::adauga(Produs e) {
	for (auto elem : lista)
		if (elem.get_id() == e.get_id())
			throw std::exception("Exista deja un element cu acest id in lista!\n");

	lista.push_back(e);
	scriere();
}