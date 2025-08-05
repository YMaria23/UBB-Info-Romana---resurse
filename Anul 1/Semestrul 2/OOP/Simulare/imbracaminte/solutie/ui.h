#pragma once
#include "service.h"
#include <qwidget.h>
#include <qpushbutton.h>
#include <qboxlayout.h>
#include <qlistwidget.h>
#include <qtablewidget.h>
#include <qlineedit.h>

class UI:public QWidget {
private:
	Service& service;
	QTableWidget* tabel = new QTableWidget;

	QLineEdit* txt_tip = new QLineEdit;

	string elems;

	QPushButton* btn_exit = new QPushButton("Exit");
	QPushButton* btn_sterge = new QPushButton("Sterge");
	QPushButton* btn_fil = new QPushButton("Filtreaza");
	QPushButton* btn_sort_pret = new QPushButton("Sorteaza dupa pret");
	QPushButton* btn_sort_tip = new QPushButton("Sorteaza dupa tip");
	QPushButton* btn_no_sort = new QPushButton("Nesortat");

public:
	UI(Service& service_nou) : service{ service_nou } {
		init();
		load(service.get_all_service());
		connect_ui();
	}

	void init();
	void load(const vector<Haine>& vector);
	void connect_ui();

	void filtreaza_ui();
	void sort_pret_ui();
	void sort_tip_ui();
	void no_sort_ui();
	void sterge_ui();
};