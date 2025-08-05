#include "teste.h"
#include <assert.h>

void Teste::test_get_set() {
	Planta p{ -1,"","",-1 };

	p.set_cod(3);
	assert(p.get_cod() == 3);

	p.set_denumire("planta");
	assert(p.get_denumire() == "planta");

	p.set_tip("suculenta");
	assert(p.get_tip() == "suculenta");

	p.set_apa(290);
	assert(p.get_apa() == 290);
}

void Teste::test_filtrare_sortare() {
	Repo repo("test.txt");
	Service service(repo);

	repo.empty();

	Planta p1{ 1,"planta","tip1",200 };
	repo.adauga(p1);

	Planta p2{ 4,"planta 2","tip2",400 };
	repo.adauga(p2);

	Planta p3{ 5,"planta 3","tip3",100 };
	repo.adauga(p3);

	const auto lista1 = service.sortare_tip();
	assert(lista1[0].get_cod() == 1);
	assert(lista1[1].get_cod() == 4);
	assert(lista1[2].get_cod() == 5);

	const auto lista2 = service.filtrare();
	assert(lista2[0].get_cod() == 1);
	assert(lista2[1].get_cod() == 5);

	repo.empty();
}

void Teste::run() {
	test_get_set();
	test_filtrare_sortare();
}