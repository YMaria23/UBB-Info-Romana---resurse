#include "ui.h"
#include <qlayout.h>
#include <qboxlayout.h>
#include <qformlayout.h>
#include <qbrush.h>
#include <qmessagebox.h>
#include <qstring.h>

void UI::init() {
	QHBoxLayout* main_ly = new QHBoxLayout;
	setLayout(main_ly);

	QHBoxLayout* stanga = new QHBoxLayout;
	QVBoxLayout* dreapta = new QVBoxLayout;

	main_ly->addLayout(stanga);
	main_ly->addLayout(dreapta);

	//stanga
	stanga->addWidget(lista);


	//dreapta
	QFormLayout* txt_ly = new QFormLayout;
	dreapta->addLayout(txt_ly);

	txt_ly->addRow("Text:", txt_txt);

	dreapta->addWidget(btn_exit);
	dreapta->addWidget(btn_inchiriere);
	dreapta->addWidget(btn_sort_marime);
	dreapta->addWidget(btn_sort_pret);
	dreapta->addWidget(btn_no_sort);
}

void UI::load(const vector<Rochie>& lista_rochii) {
	lista->clear();

	for (auto& rochie : lista_rochii) {
		string str = "cod:" + to_string(rochie.get_cod());
		str += " denumire:" + rochie.get_denumire();
		str += " marime:" + rochie.get_marime();
		str += " pret:" + to_string(rochie.get_pret());
		str += " disponibilitate:" + rochie.get_disponibilitate();

		QListWidgetItem* curent = new QListWidgetItem(QString::fromStdString(str), lista);

		const QColor color_1("green");
		const QColor color_2("red");

		QBrush brush(color_1, Qt::SolidPattern);

		if (rochie.get_disponibilitate() == "False")
			brush.setColor(color_2);

		curent->setBackground(brush);
	}

}

void UI::connect_UI() {
	QObject::connect(btn_exit, &QPushButton::clicked, [&]() {
		QMessageBox mesaj;
		mesaj.setText("Iesire din aplicatie!");
		mesaj.exec();

		close();
		});

	QObject::connect(lista, &QListWidget::itemClicked, this, [&](QListWidgetItem* item) {
		selected = item->text().toStdString();
		qDebug() << "Item:" << item->text();
		});

	QObject::connect(btn_inchiriere, &QPushButton::clicked, this, &UI::inchiriere_ui);
	QObject::connect(btn_sort_marime, &QPushButton::clicked, this, &UI::sortare_marime_ui);
	QObject::connect(btn_sort_pret, &QPushButton::clicked, this, &UI::sortare_pret_ui);
	QObject::connect(btn_no_sort, &QPushButton::clicked, this, &UI::nesortat);
}

vector<string> split_version2(string input, char separator) {
	stringstream ss(input);

	vector<string> rezultat;
	string part;

	while (getline(ss, part, separator))
		rezultat.push_back(part);

	return rezultat;
}

void UI::inchiriere_ui() {
	QMessageBox mesaj;
	if (selected == "") {
		mesaj.warning(this, "Warning", "Selectati un element!");
		return;
	}

	vector<string> parts_1 = split_version2(selected, ' ');
	vector<string> parts_2 = split_version2(parts_1[0], ':');

	try {
		service.inchiriaza_service(stoi(parts_2[1]));
	}
	catch (const std::exception& e) {
		mesaj.warning(this, "Warning", QString::fromStdString(string(e.what())));
		return;
	}

	load(service.get_all_service());
}

void UI::sortare_marime_ui() {
	vector<Rochie> rezultat = service.sortare_marime();

	load(rezultat);
}

void UI::sortare_pret_ui() {
	vector<Rochie> rezultat = service.sortare_pret();

	load(rezultat);
}

void UI::nesortat() {
	load(service.get_all_service());
}