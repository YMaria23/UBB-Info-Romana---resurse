#include "repo.h"

vector<string> split(string input, char separator) {
	stringstream ss(input);
	vector<string> rez;
	string part;

	while (getline(ss, part, separator)) {
		rez.push_back(part);
	}

	return rez;
}

void Repo::citire() {
	ifstream f(nume);
	string line;

	while (getline(f, line)) {
		vector<string> parts = split(line, ',');

		Tractor t{ -1,"","",-1 };
		t.set_id(stoi(parts[0]));
		t.set_denumire(parts[1]);
		t.set_tip(parts[2]);
		t.set_nr(stoi(parts[3]));

		lista.push_back(t);
	}
}

void Repo::scriere() {
	ofstream g(nume);

	for (auto elem : lista)
		g << elem.get_id() << "," << elem.get_denumire() << "," << elem.get_tip() << "," << elem.get_nr() << endl;

	g.close();
}

vector<Tractor>& Repo::acces_lista() {
	return lista;
}

void Repo::adauga(Tractor t) {
	for (auto elem : lista)
		if (elem.get_id() == t.get_id())
			throw::exception("Un tractor cu acelasi id se afla deja in lista!");

	lista.push_back(t);
	scriere();
}

void Repo::sterge_nr(int index) {
	int nr = lista[index].get_nr();
	lista[index].set_nr(nr - 2);
	scriere();
}