#pragma once
#include <qwidget.h>
#include <qpushbutton.h>
#include <qtableview.h>
#include <qboxlayout.h>
#include "service.h"
#include <qabstractitemview.h>
#include <qlineedit.h>
#include <qformlayout.h>
#include <qslider.h>
#include <qbrush.h>
#include <qlabel.h>
#include "observer.h"

class MyTable :public QAbstractTableModel {
private:
	vector<Produs> lista;
	double new_value;
public:
	MyTable(const vector<Produs>& l, double cv) :lista{ l }, new_value{ cv } {};

	int rowCount(const QModelIndex& parent = QModelIndex()) const override {
		return lista.size();
	}

	int columnCount(const QModelIndex& parent = QModelIndex()) const override {
		return 5;
	}

	QVariant data(const QModelIndex& index, int role = Qt::DisplayRole) const override {
		if (role == Qt::DisplayRole) {
			const Produs p = lista[index.row()];
			switch (index.column()) {
			case 0:
				return QString::fromStdString(to_string(p.get_id()));
			case 1:
				return QString::fromStdString(p.get_nume());
			case 2:
				return QString::fromStdString(p.get_tip());
			case 3:
				return QString::fromStdString(to_string(p.get_pret()));
			case 4:
				return QString::fromStdString(to_string(p.nr_vocale()));
			}
		}

		if (role == Qt::BackgroundRole) {
			const Produs p = lista[index.row()];
			if (p.get_pret() <= new_value)
				return QBrush(Qt::red);
		}
		return QVariant{};
	}

	QVariant headerData(int section, Qt::Orientation orientation, int role = Qt::DisplayRole) const override {
		if (role == Qt::DisplayRole && orientation == Qt::Horizontal) {
			switch (section) {
			case 0:
				return "ID";
			case 1:
				return "NUME";
			case 2:
				return "TIP";
			case 3:
				return "PRET";
			case 4:
				return "NR VOCALE";
			}
		}
		else if (role == Qt::DisplayRole && orientation == Qt::Vertical) {
			return section + 1;
		}
		return QVariant{};
	}
};

class UI :public QWidget, public Observer {
private:
	Service& service;

	QTableView* tabel = new QTableView;
	MyTable* model = nullptr;

	QLineEdit* id_txt = new QLineEdit;
	QLineEdit* nume_txt = new QLineEdit;
	QLineEdit* tip_txt = new QLineEdit;
	QLineEdit* pret_txt = new QLineEdit;
	QLineEdit* slider_txt = new QLineEdit;

	QPushButton* adauga_btn = new QPushButton("Adauga");

	QSlider* slider = new QSlider(Qt::Horizontal);
	double new_value;

	map<string, int> v;

public:
	UI(Service& s) : service{ s } {
		init();
		load(service.sorteaza_pret());
		connect_data();
		service.addObserver(this);

		open();
	}

	void init();
	void load(const vector<Produs>& v);
	void connect_data();
	void open();

	void update() override {
		load(service.sorteaza_pret());
		open();
	}

	~UI() {
		service.removeObserver(this);
	}
};

class TipWidget :public QWidget, public Observer {
private:
	Service& service;
	string nume;
	QLabel* tip_txt = new QLabel;
public:
	TipWidget(Service& s, string tip) : service{ s }, nume{ tip } {
		QHBoxLayout* main_ly = new QHBoxLayout;
		setLayout(main_ly);
		main_ly->addWidget(tip_txt);
		service.addObserver(this);

		setWindowTitle(QString::fromStdString(nume));
		update();
	};

	void update() override {
		int nr = service.numara_tip(nume);
		tip_txt->setText(QString::fromStdString(to_string(nr)));
	}

	~TipWidget() {
		service.removeObserver(this);
	}
};