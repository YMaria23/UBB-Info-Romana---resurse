#include "teste.h"
#include <assert.h>

void Teste::testare_getteri_setteri() {
	Apartament a1{ 1,23,"strada x",23.2 };

	assert(a1.get_id() == 1);
	assert(a1.get_suprafata() == 23);
	assert(a1.get_strada() == "strada x");
	assert(a1.get_pret() == 23.2);

	a1.set_id(2);
	assert(a1.get_id() == 2);

	a1.set_suprafata(2);
	assert(a1.get_suprafata() == 2);

	a1.set_strada("x");
	assert(a1.get_strada() == "x");

	a1.set_pret(123.2);
	assert(a1.get_pret() == 123.2);
}

void Teste::testare_adaugare() {
	Repo repo("test.txt");
	Validare valid;
	Service service(repo, valid);

	repo.empty();

	const auto lista1 = service.get_all_service();
	assert(lista1.size() == 0);

	service.adauga_service(1, 234, "strada x", 123.2);
	service.adauga_service(2, 32.3, "starda y", 234);

	const auto lista2 = service.get_all_service();
	assert(lista2.size() == 2);

	try {
		service.adauga_service(-1, 234, "strada x", 123.2);
	}
	catch (const std::exception& e) {
		assert(string(e.what()) == "Date invalide!\n");
	}

	try {
		service.adauga_service(2, 32.3, "starda y", 234);
	}
	catch (const std::exception& e) {
		assert(string(e.what()) == "Acest apartament exista deja in lista!\n");
	}

	repo.empty();
}

void Teste::testare_stergere() {
	Repo repo("test.txt");
	Validare valid;
	Service service(repo, valid);

	repo.empty();
	service.adauga_service(1, 234, "strada x", 123.2);
	service.adauga_service(2, 32.3, "starda y", 234);

	const auto lista1 = service.get_all_service();
	assert(lista1.size() == 2);

	service.sterge_service(2);

	const auto lista2 = service.get_all_service();
	assert(lista2.size() == 1);

	try {
		service.sterge_service(-1);
	}
	catch (const std::exception& e) {
		assert(string(e.what()) == "Date invalide!\n");
	}

	try {
		service.sterge_service(2);
	}
	catch (const std::exception& e) {
		assert(string(e.what()) == "Apartamentul nu a fost gasit in lista!\n");
	}

	repo.empty();
}

void Teste::testare_filtrare() {
	Repo repo("test.txt");
	Validare valid;
	Service service(repo, valid);

	repo.empty();
	service.adauga_service(1, 234, "strada x", 123.2);
	service.adauga_service(2, 32.3, "starda y", 234);
	service.adauga_service(3, 100.3, "starda y", 234);

	auto lista = service.filtrare(30, 101, service.get_all_service());
	assert(lista.size() == 2);
	assert(lista[0].get_id() == 2);
	assert(lista[1].get_id() == 3);

	auto lista1 = service.filtrare_pret(124, 235);
	assert(lista1.size() == 2);
	assert(lista1[0].get_id() == 2);
	assert(lista1[1].get_id() == 3);

	repo.empty();
}

void Teste::run() {
	testare_getteri_setteri();
	testare_adaugare();
	testare_stergere();
	testare_filtrare();
}