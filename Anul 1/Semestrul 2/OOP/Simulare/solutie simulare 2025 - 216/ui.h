#pragma once
#include "service.h"
#include <qwidget.h>
#include <qtablewidget>
#include <qlabel.h>
#include <qboxlayout.h>
#include <qpushbutton.h>

class UI :public QWidget {
private:
	Service& service;
	QTableWidget* tabel = new QTableWidget;

	QPushButton* btn_exit = new QPushButton("Exit");
	QPushButton* btn_sort = new QPushButton("Sorteaza dupa tip");
	QPushButton* btn_fil = new QPushButton("Filtreaza dupa necesar apa");
	QPushButton* btn_reset = new QPushButton("Reset");

	QLabel* label = new QLabel("");
public:
	UI(Service& service_nou) :service{ service_nou } {
		init();
		load(service.get_all_service());
		connect_ui();
	}

	void init();
	void load(const vector<Planta>& list);
	void connect_ui();
};
