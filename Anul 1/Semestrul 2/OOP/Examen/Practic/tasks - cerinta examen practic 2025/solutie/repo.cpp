#include "repo.h"

vector<string> split(string input, char sep) {
	//Functie ce sparge un sir in mai multe siruri de caractere, dupa un separator dat
	//input -> sir de caractere
	//sep -> caracter
	stringstream ss(input);
	string part;
	vector<string> rez;

	while (getline(ss, part, sep))
		rez.push_back(part);

	return rez;
}

void Repo::citire() {
	//Functie ce citeste date dintr-un fisier dat si introduce toate task-urile intr-o lista de taskuri

	ifstream f(nume);
	string line;

	while (getline(f, line)) {
		vector<string> parts = split(line, ',');

		Task t{ -1,"","",{""} };

		t.set_id(stoi(parts[0]));
		t.set_descriere(parts[1]);
		t.set_stare(parts[2]);

		vector<string> pr;
		for (int i = 3; i < parts.size(); i++)
			pr.push_back(parts[i]);

		t.set_programatori(pr);

		lista.push_back(t);
	}
}

void Repo::scriere() {
	//Functie ce scrie intr-un fisier, dintr-o lista de task-uri

	ofstream g(nume);

	for (auto elem : lista) {
		g << elem.get_id() << "," << elem.get_descriere() << "," << elem.get_stare() << ",";
		for (int i = 0; i < elem.get_programatori().size(); i++) {
			g << elem.get_programatori()[i];
			if (i != elem.get_programatori().size() - 1)
				g << ",";
		}
		g << endl;
	}

	g.close();
}

vector<Task>& Repo::acces_lista() {
	//Functia returneaza o lista de task-uri

	return lista;
}

void Repo::adauga(Task t) {
	/*Functia adauga un task in lista, daca aceste nu se regaseste deja acolo
	* Exceptii:: de tip std::exception pt cazul in care in lista se regaseste un task cu acelasi id cu al celuia pe care dorim sa-l adaugam
	* (la final, se incarca lista de task-uri in fisier)
	*/
	for (auto elem : lista)
		if (elem.get_id() == t.get_id())
			throw std::exception("Deja se afla un element cu acest id in lista!");

	lista.push_back(t);
	scriere();
}

void Repo::change_state(int index, string nume) {
	/*Functia schimba starea task-ului de la indexul "index"
	* Parametrii: index -> nr intreg
	*			  nume -> sir de caractere
	*/

	lista[index].set_stare(nume);
	scriere();
}

void Repo::empty() {
	ofstream g(nume);
	g << "";
	g.close();
}