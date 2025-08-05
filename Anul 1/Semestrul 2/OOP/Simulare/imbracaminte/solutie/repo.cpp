#include "repo.h"
#include <fstream>

void Repo::citire() {
	ifstream f(file_name);

	Haine haina{ -1,"","",-1 };

	while (f >> haina)
		adaugare(haina);
}

void Repo::scriere() {
	ofstream g(file_name);

	for (const auto& haina : get_all())
		g << haina;

	g.close();
}

const vector<Haine>& Repo::get_all() {
	return lista;
}

void Repo::adaugare(const Haine& haina) {
	lista.push_back(haina);
	scriere();
}

void Repo::sterge(int id) {
	auto it = find_if(lista.begin(), lista.end(), [id](const Haine& haina) {
		return id == haina.get_id();
		});

	if (it == lista.end())
		throw std::exception("Elementul selectat nu se afla in lista!");
	else {
		lista.erase(it);
		scriere();
	}
}

vector<Haine> Repo::acces() {
	return lista;
}

void Repo::empty() {
	ofstream g(file_name);

	g << "";
	g.close();
}