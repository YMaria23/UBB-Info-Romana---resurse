#include "ui.h"
#include <qformlayout.h>
#include <qmessagebox.h>
#include <qbrush.h>
#include <qcolor.h>

void UI::init() {
	QHBoxLayout* main_ly = new QHBoxLayout();
	setLayout(main_ly);

	QVBoxLayout* stanga = new QVBoxLayout;
	QVBoxLayout* dreapta = new QVBoxLayout;

	main_ly->addLayout(stanga);
	main_ly->addLayout(dreapta);

	//partea stanga
	stanga->addWidget(lista);



	//partea dreapta
	QFormLayout* type_ly = new QFormLayout;
	dreapta->addLayout(type_ly);

	type_ly->addRow("Id:", txt_id);
	type_ly->addRow("Suprafata:", txt_sup);
	type_ly->addRow("Strada:", txt_str);
	type_ly->addRow("Pret:", txt_pret);
	type_ly->addRow("Suprafata1:", txt_sup1);
	type_ly->addRow("Suprafata2:", txt_sup2);
	type_ly->addRow("Pret1:", txt_pret1);
	type_ly->addRow("Pret2:", txt_pret2);

	dreapta->addWidget(btn_exit);
	dreapta->addWidget(btn_adauga);
	dreapta->addWidget(btn_sterge);
	dreapta->addWidget(btn_fil_sup);
	dreapta->addWidget(btn_fil_pret);
	dreapta->addWidget(btn_reload);
}

void UI::load(const vector<Apartament>& vect) {
	lista->clear();

	for (const auto& apt : vect) {
		string str = "id:" + to_string(apt.get_id());
		str += " suprafata:" + to_string(apt.get_suprafata());
		str += " strada:" + apt.get_strada();
		str += " pret:" + to_string(apt.get_pret());

		QListWidgetItem* curent = new QListWidgetItem(QString::fromStdString(str), lista);
		//lista->addItem(QString::fromStdString(str));
		//QListWidgetItem* curent = lista->currentItem();

		const QColor color_1(QString::fromStdString("red"));
		const QColor color_2(QString::fromStdString("green"));

		QBrush brush(color_2, Qt::SolidPattern);

		if (apt.get_id() % 2 == 0)
			brush.setColor(color_1);

		curent->setBackground(brush);
	}
}

void UI::connect() {
	QObject::connect(btn_exit, &QPushButton::clicked, [&]() {
		QMessageBox exit_msg;

		exit_msg.setText("Iesire din aplicatie!");
		exit_msg.exec();

		close();
		});

	QObject::connect(btn_adauga, &QPushButton::clicked, this, &UI::adauga_ui);
	QObject::connect(btn_sterge, &QPushButton::clicked, this, &UI::sterge_ui);
	QObject::connect(btn_fil_sup, &QPushButton::clicked, this, &UI::filtrare_ui);
	QObject::connect(btn_fil_pret, &QPushButton::clicked, this, &UI::filtrare_pret_ui);
	QObject::connect(btn_reload, &QPushButton::clicked, this, &UI::reload_ui);
}

void UI::reload_ui() {
	load(service.get_all_service());
}

void UI::clear_box() {
	txt_id->clear();
	txt_sup->clear();
	txt_str->clear();
	txt_pret->clear();
	txt_sup1->clear();
	txt_sup2->clear();
	txt_pret1->clear();
	txt_pret2->clear();
}

void UI::adauga_ui() {
	string id = txt_id->text().toStdString();
	string sup = txt_sup->text().toStdString();
	string str = txt_str->text().toStdString();
	string pret = txt_pret->text().toStdString();

	int id_valid;
	double pret_valid, sup_valida;

	QMessageBox mesaj;

	try {
		id_valid = stoi(id);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Id-ul trebuie sa fie un nr intreg!");
		return;
	}

	try {
		pret_valid = stod(pret);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Pretul trebuie sa fie un nr real!");
		return;
	}

	try {
		sup_valida = stod(sup);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Suprafata trebuie sa fie un nr real!");
		return;
	}

	try {
		service.adauga_service(id_valid, sup_valida, str, pret_valid);
	}
	catch (const std::exception& e) {
		mesaj.warning(this, "Warning", QString::fromStdString(string(e.what())));
		return;
	}

	clear_box();
	load(service.get_all_service());
}

void UI::sterge_ui() {
	string id = txt_id->text().toStdString();

	QMessageBox mesaj;
	int id_valid;

	try {
		id_valid = stoi(id);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Id-ul trebuie sa fie un nr intreg!");
		return;
	}

	try {
		service.sterge_service(id_valid);
	}
	catch (const std::exception& e) {
		mesaj.warning(this, "Warning", QString::fromStdString(string(e.what())));
		return;
	}

	clear_box();
	load(service.get_all_service());
}

void UI::filtrare_ui() {
	string sup1 = txt_sup1->text().toStdString();
	string sup2 = txt_sup2->text().toStdString();

	QMessageBox mesaj;
	double sup1_valid;
	double sup2_valid;

	try {
		sup1_valid = stod(sup1);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Suprafata trebuie sa fie un nr real!");
		return;
	}

	try {
		sup2_valid = stod(sup2);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Suprafata trebuie sa fie un nr real!");
		return;
	}

	clear_box();
	load(service.filtrare(sup1_valid, sup2_valid, service.get_all_service()));
}

void UI::filtrare_pret_ui() {
	string sup1 = txt_sup1->text().toStdString();
	string sup2 = txt_sup2->text().toStdString();

	string pret1 = txt_pret1->text().toStdString();
	string pret2 = txt_pret2->text().toStdString();

	QMessageBox mesaj;
	double sup1_valid, sup2_valid;
	double pret1_valid, pret2_valid;

	bool ok = true;

	try {
		sup1_valid = stod(sup1);
	}
	catch (invalid_argument&) {
		ok = false;
	}

	try {
		sup2_valid = stod(sup2);
	}
	catch (invalid_argument&) {
		ok = false;
	}

	try {
		pret1_valid = stod(pret1);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Pretul trebuie sa fie un nr real!");
		return;
	}

	try {
		pret2_valid = stod(pret2);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Pretul trebuie sa fie un nr real!");
		return;
	}

	vector<Apartament> rezultat = service.filtrare_pret(pret1_valid, pret2_valid);
	if (ok == true)
		load(service.filtrare(sup1_valid, sup2_valid, rezultat));
	else
		load(rezultat);

	clear_box();
}