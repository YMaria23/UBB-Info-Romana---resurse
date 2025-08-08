#include "domain.h"

int Task::get_id() const {
	//Functia returneaza id-ul Task-ului
	return id;
}

string Task::get_descriere() const {
	//Functia returneaza descrierea Task-ului
	return descriere;
}

string Task::get_stare() const {
	//Functa returneaza starea in care se afla task-ul
	return stare;
}

vector<string> Task::get_programatori() const {
	//Functia returneaza nr de programatori care lucreaza la task-ul dat
	return programatori;
}

void Task::set_id(int i) {
	//Se schimba valoarea id-ului curent cu cea a unui id dat (nr intreg)
	id = i;
}

void Task::set_descriere(string d) {
	//Se schimba descrierea curent cu una data (sir de caractere)
	descriere = d;
}

void Task::set_stare(string s) {
	//Se schimba starea curenta cu una data (sir de caractere)
	stare = s;
}

void Task::set_programatori(vector<string> v) {
	//Se schimba set-ul de programatori cu altul dat (sir de siruri de caractere)
	programatori = v;
}