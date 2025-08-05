#include "ui.h"
#include <qformlayout.h>
#include <qobject.h>
#include <qmessagebox.h>

void UI::init() {
	QHBoxLayout* main_ly = new QHBoxLayout;
	setLayout(main_ly);

	QVBoxLayout* stanga = new QVBoxLayout;
	QVBoxLayout* dreapta = new QVBoxLayout;

	main_ly->addLayout(stanga);
	main_ly->addLayout(dreapta);

	//partea stanga
	stanga->addWidget(tabel);

	
	//partea dreapta
	QFormLayout* text_ly = new QFormLayout;
	dreapta->addLayout(text_ly);

	text_ly->addRow("Tip", txt_tip);

	dreapta->addWidget(btn_exit);
	dreapta->addWidget(btn_sterge);
	dreapta->addWidget(btn_fil);
	dreapta->addWidget(btn_sort_pret);
	dreapta->addWidget(btn_sort_tip);
	dreapta->addWidget(btn_no_sort);
}

void UI::load(const vector<Haine>& vector) {
	//tabel->clear();
	qDebug() <<"Lungime lista:"<< vector.size() << "\n";

	tabel->setColumnCount(4);
	tabel->setRowCount((int)vector.size() + 1);

	QTableWidgetItem* id = new QTableWidgetItem(QString::fromStdString("Id"));
	tabel->setItem(0, 0, id);

	QTableWidgetItem* tip = new QTableWidgetItem(QString::fromStdString("Tip"));
	tabel->setItem(0, 1, tip);

	QTableWidgetItem* culoare_n = new QTableWidgetItem(QString::fromStdString("Culoare"));
	tabel->setItem(0, 2, culoare_n);

	QTableWidgetItem* pret = new QTableWidgetItem(QString::fromStdString("Pret"));
	tabel->setItem(0, 3, pret);

	int linie = 1;

	for (auto& haina : vector) {

		string culoare;
		if (haina.get_culoare() == "albastru")
			culoare = "blue";
		else if (haina.get_culoare() == "galben")
			culoare = "yellow";
		else if (haina.get_culoare() == "verde")
			culoare = "green";
		else
			culoare = "red";

		QBrush brush(QString::fromStdString(culoare), Qt::SolidPattern);

		QTableWidgetItem* id_nou = new QTableWidgetItem(QString::fromStdString(to_string(haina.get_id())));
		id_nou->setBackground(brush);
		tabel->setItem(linie, 0, id_nou);

		QTableWidgetItem* tip_nou = new QTableWidgetItem(QString::fromStdString(haina.get_tip()));
		tip_nou->setBackground(brush);
		tabel->setItem(linie, 1, tip_nou);

		QTableWidgetItem* culoare_noua = new QTableWidgetItem(QString::fromStdString(haina.get_culoare()));
		culoare_noua->setBackground(brush);
		tabel->setItem(linie, 2, culoare_noua);

		QTableWidgetItem* pret_nou = new QTableWidgetItem(QString::fromStdString(to_string(haina.get_pret())));
		pret_nou->setBackground(brush);
		tabel->setItem(linie, 3, pret_nou);

		linie++;
	}
}

void UI::connect_ui() {
	QObject::connect(btn_exit, &QPushButton::clicked, [&]() {
		QMessageBox mesaj;
		mesaj.setText("Iesire din aplicatie!");
		mesaj.exec();

		close();
		});

	QObject::connect(tabel, &QTableWidget::itemClicked, [&](QTableWidgetItem* item) {
		int row = item->row();
		QTableWidgetItem* id = tabel->item(row, 0);
		elems= id->text().toStdString();
		qDebug() << "Tipul selectat: " << elems << "\n";
		});

	QObject::connect(btn_sterge, &QPushButton::clicked, this, &UI::sterge_ui);
	QObject::connect(btn_fil, &QPushButton::clicked,this, &UI::filtreaza_ui);
	QObject::connect(btn_sort_pret, &QPushButton::clicked, this, &UI::sort_pret_ui);
	QObject::connect(btn_sort_tip, &QPushButton::clicked, this, &UI::sort_tip_ui);
	QObject::connect(btn_no_sort, &QPushButton::clicked, this, &UI::no_sort_ui);

}

void UI::filtreaza_ui() {
	string tip = txt_tip->text().toStdString();

	vector<Haine> rezultat = service.filtreaza(tip);
	load(rezultat);
}

void UI::sort_pret_ui() {
	vector<Haine> rez = service.sorteaza_pret();
	load(rez);
}

void UI::sort_tip_ui() {
	vector<Haine> rez = service.sorteaza_tip();
	load(rez);
}

void UI::no_sort_ui() {
	load(service.get_all_service());
}

void UI::sterge_ui() {
	QMessageBox mesaj;
	int id;
	try {
		id = stoi(elems);
	}
	catch (invalid_argument&) {
		mesaj.warning(this, "Warning", "Selectati un element valid!");
		return;
	}
	try {
		service.sterge_service(id);
	}
	catch (const std::exception& e) {
		mesaj.warning(this, "Warning", QString::fromStdString(string(e.what())));
		return;
	}

	load(service.get_all_service());
}