#include "teste.h"
#include <assert.h>

void Teste::teste_get_set() {
	Rochie rochie{ 123,"rochie albastra","M",123,"True" };

	assert(rochie.get_cod() == 123);
	assert(rochie.get_denumire() == "rochie albastra");
	assert(rochie.get_marime() == "M");
	assert(rochie.get_pret() == 123);
	assert(rochie.get_disponibilitate() == "True");

	rochie.set_cod(124);
	assert(rochie.get_cod() == 124);

	rochie.set_denumire("alb");
	assert(rochie.get_denumire() == "alb");

	rochie.set_marime("S");
	assert(rochie.get_marime() == "S");

	rochie.set_pret(124);
	assert(rochie.get_pret() == 124);

	rochie.set_disponibilitate("False");
	assert(rochie.get_disponibilitate() == "False");
}

void Teste::teste_adauga() {
	Repo repo("test.txt");

	repo.empty();
	const auto& lista_1 = repo.get_all();
	assert(lista_1.size() == 0);

	Rochie rochie{ 123,"rochie albastra","M",123,"True" };
	repo.adauga(rochie);

	const auto& lista_2 = repo.get_all();
	assert(lista_2.size() == 1);
	assert(lista_2[0].get_cod() == 123);

	try {
		repo.adauga(rochie);
	}
	catch (const std::exception& e) {
		assert(string(e.what()) == "Acesta rochie se afla deja in lista!\n");
	}

	repo.empty();
}

void Teste::teste_inchiriere() {
	Repo repo("test.txt");
	Validare valid;
	Service service(repo, valid);

	repo.empty();

	Rochie rochie{ 123,"rochie albastra","M",123,"True" };
	repo.adauga(rochie);

	Rochie rochie_1{ 124,"rochie albastra","S",123,"False" };
	repo.adauga(rochie_1);

	Rochie rochie_2{ 125,"rochie albastra","L",123,"True" };
	repo.adauga(rochie_2);

	service.inchiriaza_service(123);
	const auto lista_1 = service.get_all_service();

	assert(lista_1[0].get_disponibilitate() == "False");

	try {
		service.inchiriaza_service(126);
	}
	catch (const std::exception& e) {
		assert(string(e.what()) == "Nu ati selectat o rochie existenta!");
	}

	try {
		service.inchiriaza_service(124);
	}
	catch (const std::exception& e) {
		assert(string(e.what()) == "Nu ati selectat o rochie existenta!");
	}

	repo.empty();
}

void Teste::teste_sortare() {
	Repo repo("test.txt");
	Validare valid;
	Service service(repo, valid);

	repo.empty();

	Rochie rochie{ 123,"rochie albastra","M",183,"True" };
	repo.adauga(rochie);

	Rochie rochie_1{ 124,"rochie albastra","S",134,"False" };
	repo.adauga(rochie_1);

	Rochie rochie_2{ 125,"rochie albastra","L",101,"True" };
	repo.adauga(rochie_2);

	const auto lista_marime = service.sortare_marime();
	assert(lista_marime[0].get_cod() == 125);
	assert(lista_marime[1].get_cod() == 123);
	assert(lista_marime[2].get_cod() == 124);


	const auto lista_pret = service.sortare_pret();
	assert(lista_pret[0].get_cod() == 125);
	assert(lista_pret[1].get_cod() == 124);
	assert(lista_pret[2].get_cod() == 123);

	repo.empty();
}

void Teste::run() {
	teste_get_set();
	teste_adauga();
	teste_inchiriere();
	teste_sortare();
}