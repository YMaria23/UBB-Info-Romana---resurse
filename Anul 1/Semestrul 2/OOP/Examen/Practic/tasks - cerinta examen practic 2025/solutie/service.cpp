#include "service.h"
#include <algorithm>

vector<Task> Service::sortare_stare() {
	/*Functia sorteaza lista de task-uri si o returneaza
	*/
	vector<Task> v = repo.acces_lista();

	sort(v.begin(), v.end(), [&](Task a, Task b) {
		return a.get_stare() < b.get_stare();
		});

	return v;
}

vector<string> split2(string input, char sep) {
	/*Functie ce imparte in siruri de caractere, dupa un separator
	* Oarametrii: input -> sir de caractere
	*			  sep -> caracter
	*/
	stringstream ss(input);
	string part;
	vector<string> rez;

	while (getline(ss, part, sep))
		rez.push_back(part);

	return rez;
}

void Service::adauga(int id, string d, string s, string pr) {
	/*Functia adauga un task in lista
	* Parametrii: id -> nr intreg
	*			  d -> sir de caractere
	*			  s -> sir de caractere
	*			  pr -> sir de caractere
	* Exceptii: de tip std::exception daca:
	* -nr de programatori este mai mare decat 4 sau mai mic decat 1
	* -descrierea este un sir de caractere vid sau starea nu ia una dintre cele 3 valori permise "open","closed","inprogress"
	* -id-ul nu e unic
	*/
	vector<string> p = split2(pr, ',');
	if (!(p.size() >= 1 && p.size() <= 4))
		throw std::exception("Nr de programatori trebuie sa fie intre 1 si 4!");
	if (d == "" || (s != "open" && s != "inprogress" && s != "closed"))
		throw std::exception("Date invalide!");

	Task t(id, d, s, p);
	repo.adauga(t);
	notify();
}

vector<Task> Service::filtrare_pr(string nume) {
	/*Functia returneaza un vector ce contine numai task-urile ce au programatori ale caror nume contin sirul de caractere dat
	* Parametrii: nume -> sir de caractere
	*/
	vector<Task> v = sortare_stare();
	vector<Task> rez;

	for (auto elem : v) {
		vector<string> pr = elem.get_programatori();
		for (auto names : pr)
			if (names.find(nume) < pr.size()) {
				int ok = 1;
				for (auto e : rez)
					if (e.get_id() == elem.get_id())
						ok = 0;

				//auto it = find(pr.begin(), pr.end(), nume);
				//if (it != pr.end())
				if (ok)
					rez.push_back(elem);
			}
	}

	return rez;
}

void Service::change_state(int id, string nume) {
	/*Functia schimba starea task-ului cu id-ul dat
	* Parametrii: id -> nr intreg
	*			  nume -> sir de caractere
	*/
	vector<Task> v = repo.acces_lista();

	for (int i = 0; i < v.size(); i++)
		if (v[i].get_id() == id) {
			repo.change_state(i, nume);
			break;
		}
	notify();
}

vector<Task> Service::filtrare_stare(string nume) {
	/*Functia returneaza o lista ce contine numai task-urile cu stare data
	* Prametrii: nume -> sir de caractere
	*/
	vector<Task> v = repo.acces_lista();
	vector<Task> rez;

	for (auto elem : v)
		if (elem.get_stare() == nume)
			rez.push_back(elem);

	return rez;
}