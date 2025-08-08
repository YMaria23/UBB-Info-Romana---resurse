#include "test.h"
#include <assert.h>

void Testare::test_sortare() {
	Repo repo("test.txt");
	Service service(repo);

	repo.empty();
	repo.adauga({ 1,"d1","open",{"p1"} });
	repo.adauga({ 3,"d2","closed",{"p1","p2"} });
	repo.adauga({ 2,"d1","inprogress",{"p1"} });

	vector<Task> rez = service.sortare_stare();
	assert(rez.size() == 3);
	assert(rez[0].get_id() == 3);
	assert(rez[1].get_id() == 2);
	assert(rez[2].get_id() == 1);

	repo.empty();
}

void Testare::test_filtrare() {
	Repo repo("test.txt");
	Service service(repo);

	repo.empty();
	repo.adauga({ 3,"d2","closed",{"p1","p2"} });
	repo.adauga({ 4,"d2","closed",{"cv"} });
	repo.adauga({ 2,"d1","inprogress",{"p1"} });
	repo.adauga({ 1,"d1","open",{"p1"} });

	vector<Task> rez = service.filtrare_pr("p");
	assert(rez.size() == 3);
	assert(rez[0].get_id() == 3);
	assert(rez[1].get_id() == 2);
	assert(rez[2].get_id() == 1);

	vector<Task> rez2 = service.filtrare_pr("2");
	assert(rez2.size() == 1);
	assert(rez[0].get_id() == 3);

	vector<Task> rez3 = service.filtrare_stare("closed");
	assert(rez3.size() == 2);
	assert(rez3[0].get_id() == 3);
	assert(rez3[1].get_id() == 4);

	repo.empty();
}

void Testare::test_rest() {
	Repo repo("test.txt");
	Service service(repo);

	repo.empty();
	repo.adauga({ 1,"d1","open",{"p1"} });
	repo.adauga({ 3,"d2","closed",{"p1","p2"} });
	repo.adauga({ 2,"d1","inprogress",{"p1"} });

	try {
		service.adauga(11, "cv", "altceva", { "p1" });
		assert(false);
	}
	catch (exception&) {
		assert(true);
	}

	try {
		service.adauga(11, "", "open", { "p1" });
		assert(false);
	}
	catch (exception&) {
		assert(true);
	}

	try {
		service.adauga(1, "cv", "open", { "p1" });
		assert(false);
	}
	catch (exception&) {
		assert(true);
	}

	try {
		service.adauga(11, "cv", "open", {});
		assert(false);
	}
	catch (exception&) {
		assert(true);
	}
	repo.empty();

	service.change_state(3, "open");
	vector<Task> acces = repo.acces_lista();
	assert(acces[1].get_stare() == "open");

	Task cv{ 1,"cv","open",{"p1"} };
	cv.set_id(2);
	assert(cv.get_id() == 2);

	cv.set_descriere("altceva");
	assert(cv.get_descriere() == "altceva");

	cv.set_programatori({ "p1","p" });
	repo.empty();
}

void Testare::run_teste() {
	test_sortare();
	test_filtrare();
	test_rest();
}