#pragma once
#include <QWidget>
#include <qpushbutton.h>
#include <qlayout.h>
#include <qlabel.h>
#include <qlist.h>
#include <qlistwidget.h>
#include <qlineedit.h>

#include "service.h"

class UI :public QWidget {
private:
	Service& service;
	QListWidget* lista = new QListWidget;

	//user input
	QLineEdit* txt_id = new QLineEdit;
	QLineEdit* txt_sup = new QLineEdit;
	QLineEdit* txt_str = new QLineEdit;
	QLineEdit* txt_pret = new QLineEdit;
	QLineEdit* txt_sup1 = new QLineEdit;
	QLineEdit* txt_sup2 = new QLineEdit;
	QLineEdit* txt_pret1 = new QLineEdit;
	QLineEdit* txt_pret2 = new QLineEdit;

	//butoane
	QPushButton* btn_exit = new QPushButton("Exit");
	QPushButton* btn_reload = new QPushButton("Reload");
	QPushButton* btn_adauga = new QPushButton("Adauga");
	QPushButton* btn_sterge = new QPushButton("Sterge");
	QPushButton* btn_fil_sup = new QPushButton("Filtreaza suprafata");
	QPushButton* btn_fil_pret = new QPushButton("Filtreaza pret");

public:
	UI(Service& ot) :service(ot) {
		init();
		load(service.get_all_service());
		connect();
	}

	void init();
	void load(const vector<Apartament>& vect);
	void connect();

	void clear_box();
	void reload_ui();

	void adauga_ui();
	void sterge_ui();
	void filtrare_ui();
	void filtrare_pret_ui();
};
