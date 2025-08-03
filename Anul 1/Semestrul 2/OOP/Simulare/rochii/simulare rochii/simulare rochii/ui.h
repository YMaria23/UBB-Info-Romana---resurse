#pragma once
#include <qwidget.h>
#include <qpushbutton.h>
#include <qline.h>
#include <qlineedit.h>
#include <QWidget>
#include <qlistwidget.h>

#include "service.h"

using namespace std;

class UI :public QWidget {
private:
	Service& service;
	QListWidget* lista = new QListWidget;
	string selected;

	QLineEdit* txt_txt = new QLineEdit;

	QPushButton* btn_exit = new QPushButton("Exit");
	QPushButton* btn_inchiriere = new QPushButton("Inchiriaza");
	QPushButton* btn_sort_marime = new QPushButton("Sorteaza dupa marime");
	QPushButton* btn_sort_pret = new QPushButton("Sorteaza dupa pret");
	QPushButton* btn_no_sort = new QPushButton("Nesortat");

public:
	UI(Service& service_nou) :service{ service_nou } {
		init();
		load(service.get_all_service());
		connect_UI();
	}

	void init();
	void load(const vector<Rochie>& list);
	void connect_UI();

	void inchiriere_ui();
	void sortare_marime_ui();
	void sortare_pret_ui();
	void nesortat();
};