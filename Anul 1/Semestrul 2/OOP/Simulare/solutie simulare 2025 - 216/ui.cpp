#include "ui.h"
#include <qmessagebox.h>
#include <qbrush.h>

void UI::init() {
	QHBoxLayout* main_ly = new QHBoxLayout;
	setLayout(main_ly);

	QVBoxLayout* stanga = new QVBoxLayout;
	QVBoxLayout* dreapta = new QVBoxLayout;

	main_ly->addLayout(stanga);
	main_ly->addLayout(dreapta);

	//stanga
	stanga->addWidget(tabel);

	stanga->addWidget(label);

	//dreapta
	dreapta->addWidget(btn_exit);
	dreapta->addWidget(btn_sort);
	dreapta->addWidget(btn_fil);
	dreapta->addWidget(btn_reset);

}

void UI::load(const vector<Planta>& list) {
	tabel->clear();

	tabel->setRowCount((int)list.size()+1);
	tabel->setColumnCount(4);

	QTableWidgetItem* cod = new QTableWidgetItem();
	cod->setText("Cod");
	tabel->setItem(0, 0, cod);

	QTableWidgetItem* den = new QTableWidgetItem();
	den->setText("Denumire");
	tabel->setItem(0, 1, den);

	QTableWidgetItem* tip = new QTableWidgetItem;
	tip->setText("Tip");
	tabel->setItem(0, 2, tip);

	QTableWidgetItem* apa = new QTableWidgetItem;
	apa->setText("Necesar apa");
	tabel->setItem(0, 3,apa);

	int linie = 1;

	for (auto& elem : list) {
		QBrush brush("blue",Qt::SolidPattern);

		if (elem.get_tip() == "tropicala")
			brush.setColor(Qt::cyan);
		else if (elem.get_tip() == "suculenta")
			brush.setColor(Qt::magenta);
		else if (elem.get_tip() == "aeriana")
			brush.setColor(Qt::red);
		else if (elem.get_tip() == "infloritoare")
			brush.setColor(Qt::darkGray);

		QTableWidgetItem* cod_n = new QTableWidgetItem();
		cod_n->setText(QString::fromStdString(to_string(elem.get_cod())));
		tabel->setItem(linie, 0, cod_n);
		cod_n->setBackground(brush);

		QTableWidgetItem* den_n = new QTableWidgetItem;
		den_n->setText(QString::fromStdString(elem.get_denumire()));
		tabel->setItem(linie, 1, den_n);
		den_n->setBackground(brush);

		QTableWidgetItem* tip_n = new QTableWidgetItem;
		tip_n->setText(QString::fromStdString(elem.get_tip()));
		tabel->setItem(linie, 2, tip_n);
		tip_n->setBackground(brush);

		QTableWidgetItem* apa_n = new QTableWidgetItem;
		apa_n->setText(QString::fromStdString(to_string(elem.get_apa())));
		tabel->setItem(linie, 3, apa_n);
		apa_n->setBackground(brush);

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

	QObject::connect(tabel, &QTableWidget::itemClicked, [&](QTableWidgetItem* elem) {
		int linie = elem->row();
		QTableWidgetItem* cod_apa = tabel->item(linie, 3);
		string apa = cod_apa->text().toStdString();
		int apa_v = stoi(apa);

		if (apa_v > 300)
			label->setText("Aceasta planta necesita udare zilnica!");
		else
			label->setText("");
	});

	QObject::connect(btn_sort, &QPushButton::clicked, [&]() {
		vector<Planta> rez = service.sortare_tip();
		load(rez);
		});

	QObject::connect(btn_fil, &QPushButton::clicked, [&]() {
		vector<Planta> rez = service.filtrare();
		load(rez);
		});

	QObject::connect(btn_reset, &QPushButton::clicked, [&]() {
		load(service.get_all_service());
		});
}