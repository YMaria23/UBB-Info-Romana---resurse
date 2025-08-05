#include "repo.h"
#include <fstream>

void Repo::citire() {
	ifstream f(name_file);

	Rochie rochie_noua{ -1,"","",-1,"" };

	while (f >> rochie_noua)
		adauga(rochie_noua);
}

void Repo::scriere() {
	ofstream g(name_file);

	for (const auto& rochie : get_all())
		g << rochie;

	g.close();
}

void Repo::adauga(Rochie& rochie_noua) {
	auto it = find_if(lista.begin(), lista.end(), [rochie_noua](const Rochie& rochie) {
		return rochie_noua.get_cod() == rochie.get_cod();
		});

	if (it == lista.end()) {
		lista.push_back(rochie_noua);
		scriere();
	}
	else
		throw std::exception("Acesta rochie se afla deja in lista!\n");
}

const vector<Rochie>& Repo::get_all() {
	return lista;
}

void Repo::inchiriaza(int cod) {
	for (auto& rochie : lista)
		if (rochie.get_cod() == cod && rochie.get_disponibilitate() == "True") {
			rochie.set_disponibilitate("False");
			scriere();
			return;
		}

	throw std::exception("Nu ati selectat o rochie existenta!");
}

void Repo::empty() {
	ofstream g(name_file);

	g << "";
	g.close();
}

vector<Rochie> Repo::acceseaza() {
	return lista;
}