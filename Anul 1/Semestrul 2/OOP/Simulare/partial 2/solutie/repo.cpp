#include "repo.h"

void Repo::citire() {
	ifstream f(file_name);

	Apartament apt{ -1,-1,"",-1 };

	while (f >> apt)
		adauga(apt);
}

void Repo::scriere() {
	ofstream g(file_name);

	for (const auto& apt : get_all())
		g << apt;

	g.close();
}

const vector<Apartament>& Repo::get_all() {
	return lista;
}

void Repo::empty() {
	ofstream g(file_name);

	g << "";
	g.close();
}

void Repo::adauga(const Apartament& apt) {
	auto it = find_if(lista.begin(), lista.end(), [apt](const Apartament& a) {
		return apt.get_id() == a.get_id();
		});

	if (it == lista.end()) {
		lista.push_back(apt);
		scriere();
	}
	else
		throw std::exception("Acest apartament exista deja in lista!\n");
}

void Repo::sterge(int id) {
	auto it = find_if(lista.begin(), lista.end(), [id](const Apartament& a) {
		return id == a.get_id();
		});
	if (it != lista.end()) {
		lista.erase(it);
		scriere();
	}
	else
		throw std::exception("Apartamentul nu a fost gasit in lista!\n");
}