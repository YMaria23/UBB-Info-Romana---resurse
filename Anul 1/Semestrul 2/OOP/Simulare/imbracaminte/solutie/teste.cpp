#include "teste.h"
#include <assert.h>

void Teste::teste_set_get() {
	Haine haina{ 1,"pantaloni","rosii",123 };

	assert(haina.get_id() == 1);
	assert(haina.get_tip() == "pantaloni");
	assert(haina.get_culoare() == "rosii");
	assert(haina.get_pret() == 123);

	haina.set_id(2);
	assert(haina.get_id() == 2);

	haina.set_tip("bluza");
	assert(haina.get_tip() == "bluza");

	haina.set_culoare("alb");
	assert(haina.get_culoare() == "alb");

	haina.set_pret(12.3);
	assert(haina.get_pret() == 12.3);
}

void Teste::teste_adauga() {
	Repo repo("teste.txt");
	Service service(repo);

	repo.empty();

	const auto lista_1 = service.get_all_service();
	assert(lista_1.size() == 0);

	service.adaugare_service(1, "bluza", "alba", 13.4);
	service.adaugare_service(2, "pantaloni", "rosii", 234.1);

	const auto lista_2 = service.get_all_service();
	assert(lista_2.size() == 2);

	repo.empty();
}

void Teste::teste_filtrare() {
	Repo repo("teste.txt");
	Service service(repo);

	repo.empty();

	service.adaugare_service(1, "bluza", "alba", 13.4);
	service.adaugare_service(2, "pantaloni", "rosii", 234.1);
	service.adaugare_service(3, "bluza", "verde", 13.4);
	service.adaugare_service(4, "pantofi", "rosii", 234.1);

	const auto rezultat = service.filtreaza("bluza");
	assert(rezultat.size() == 2);
	assert(rezultat[0].get_id() == 1);
	assert(rezultat[1].get_id() == 3);

	repo.empty();
}

void Teste::teste_sortare() {
	Repo repo("teste.txt");
	Service service(repo);

	repo.empty();

	service.adaugare_service(1, "bluz", "alba", 132.4);
	service.adaugare_service(2, "pantaloni", "rosii", 234.1);
	service.adaugare_service(3, "bluza", "verde", 13.4);
	service.adaugare_service(4, "pantofi", "rosii", 234.2);

	const auto lista1 = service.sorteaza_tip();
	assert(lista1[0].get_id() == 1);
	assert(lista1[1].get_id() == 3);
	assert(lista1[2].get_id() == 2);
	assert(lista1[3].get_id() == 4);

	const auto lista2 = service.sorteaza_pret();
	assert(lista2[0].get_id() == 3);
	assert(lista2[1].get_id() == 1);
	assert(lista2[2].get_id() == 2);
	assert(lista2[3].get_id() == 4);

	repo.empty();
}

void Teste::run() {
	teste_set_get();
	teste_adauga();
	teste_filtrare();
	teste_sortare();
}