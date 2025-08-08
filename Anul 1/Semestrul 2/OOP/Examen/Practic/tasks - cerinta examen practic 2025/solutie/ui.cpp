#include "ui.h"
#include <qmessagebox.h>

void UI::init() {
	QHBoxLayout* main_ly = new QHBoxLayout;
	setLayout(main_ly);

	QVBoxLayout* st_ly = new QVBoxLayout;
	QVBoxLayout* dr_ly = new QVBoxLayout;

	main_ly->addLayout(st_ly);
	main_ly->addLayout(dr_ly);

	st_ly->addWidget(tabel);

	QFormLayout* txt_ly = new QFormLayout;
	txt_ly->addRow("Id", id_txt);
	txt_ly->addRow("Descriere", descriere_txt);
	txt_ly->addRow("Stare", stare_txt);
	txt_ly->addRow("Programatori", pr_txt);

	dr_ly->addLayout(txt_ly);
	dr_ly->addWidget(adauga_btn);
	dr_ly->addWidget(reload_btn);
	dr_ly->addWidget(progra);
}

void UI::load(vector<Task> t) {
	model = new MyTable(t);
	tabel->setModel(model);
}

void UI::connect_data() {
	QObject::connect(adauga_btn, &QPushButton::clicked, [&]() {
		string id = id_txt->text().toStdString();
		string descr = descriere_txt->text().toStdString();
		string stare = stare_txt->text().toStdString();
		string pr = pr_txt->text().toStdString();

		QMessageBox mesaj;

		int id_final;
		try {
			id_final = stoi(id);
		}
		catch (exception&) {
			mesaj.warning(this, "Warning", "Id-ul trebuie sa fie nr intreg!");
			return;
		}

		try {
			service.adauga(id_final, descr, stare, pr);
		}
		catch (exception& e) {
			mesaj.warning(this, "Warning", e.what());
			return;
		}

		load(service.sortare_stare());
		});

	QObject::connect(progra, &QLineEdit::textChanged, [&]() {
		string nume = progra->text().toStdString();
		load(service.filtrare_pr(nume));
		});

	QObject::connect(reload_btn, &QPushButton::clicked, [&]() {
		load(service.sortare_stare());
		});
}


void Aditional::init() {
	setWindowTitle(QString::fromStdString(nume));
	QHBoxLayout* main_ly = new QHBoxLayout;
	setLayout(main_ly);

	QVBoxLayout* st_ly = new QVBoxLayout;
	QVBoxLayout* dr_ly = new QVBoxLayout;

	main_ly->addLayout(st_ly);
	main_ly->addLayout(dr_ly);

	st_ly->addWidget(tabel);

	dr_ly->addWidget(open);
	dr_ly->addWidget(inprogress);
	dr_ly->addWidget(closed);
}

void Aditional::load(vector<Task> v) {
	model = new MyTable(v);
	tabel->setModel(model);
}

void Aditional::connect_data() {
	QObject::connect(tabel, &QTableView::clicked, [&](const QModelIndex& in) {
		int row = (long)in.row();
		QModelIndex i = tabel->model()->index(row, 0);
		QVariant item = tabel->model()->data(i);


		QMessageBox mesaj;

		try {
			id_selected = item.toInt();
		}
		catch (exception&) {
			mesaj.warning(this, "Warning", "Nu a fost selectat corect!");
		}
		});

	QObject::connect(open, &QPushButton::clicked, [&]() {
		service.change_state(id_selected, "open");
		load(service.filtrare_stare(nume));
		});

	QObject::connect(inprogress, &QPushButton::clicked, [&]() {
		service.change_state(id_selected, "inprogress");
		load(service.filtrare_stare(nume));
		});

	QObject::connect(closed, &QPushButton::clicked, [&]() {
		service.change_state(id_selected, "closed");
		load(service.filtrare_stare(nume));
		});
}
