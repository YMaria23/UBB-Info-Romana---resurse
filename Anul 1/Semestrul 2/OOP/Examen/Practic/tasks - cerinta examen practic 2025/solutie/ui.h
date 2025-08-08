#pragma once
#include "service.h"
#include <qwidget.h>
#include <qpushbutton.h>
#include <qboxlayout.h>
#include <qtableview.h>
#include <qlineedit.h>
#include <qformlayout.h>

class MyTable :public QAbstractTableModel {
private:
	vector<Task> v;
public:
	MyTable(vector<Task> l) : v{ l } {};

	int rowCount(const QModelIndex& parent = QModelIndex()) const override {
		return v.size();
	}

	int columnCount(const QModelIndex& parent = QModelIndex()) const override {
		return 4;
	}

	QVariant data(const QModelIndex& index, int role = Qt::DisplayRole) const override {
		if (role == Qt::DisplayRole) {
			const Task t = v[index.row()];
			switch (index.column()) {
			case 0:
				return t.get_id();
			case 1:
				return QString::fromStdString(t.get_descriere());
			case 2:
				return QString::fromStdString(t.get_stare());
			case 3:
				return t.get_programatori().size();
			}
		}
		return QVariant{};
	}

	QVariant headerData(int section, Qt::Orientation orientation, int role = Qt::DisplayRole) const override {
		if (orientation == Qt::Horizontal && role == Qt::DisplayRole) {
			switch (section) {
			case 0:
				return "ID";
			case 1:
				return "DESCRIERE";
			case 2:
				return "STARE";
			case 3:
				return "NR PR";
			}
		}
		if (orientation == Qt::Vertical && role == Qt::DisplayRole)
			return section + 1;
		return QVariant{};
	}
};

class Aditional :public QWidget, public Observer {
private:
	Service& service;
	string nume;

	QTableView* tabel = new QTableView;
	MyTable* model = nullptr;

	QPushButton* open = new QPushButton("open");
	QPushButton* inprogress = new QPushButton("inprogress");
	QPushButton* closed = new QPushButton("closed");

	int id_selected;

public:
	Aditional(Service& s, string n) : service{ s }, nume{ n } {
		service.addObserver(this);
		init();
		load(service.filtrare_stare(nume));
		connect_data();
	}

	void init();
	void load(vector<Task> v);
	void connect_data();

	void update() {
		load(service.filtrare_stare(nume));
	}

	~Aditional() {
		service.removeObserver(this);
	}
};


class UI :public QWidget, public Observer {
private:
	Service& service;

	QTableView* tabel = new QTableView;
	MyTable* model = nullptr;

	QLineEdit* id_txt = new QLineEdit;
	QLineEdit* descriere_txt = new QLineEdit;
	QLineEdit* stare_txt = new QLineEdit;
	QLineEdit* pr_txt = new QLineEdit;

	QPushButton* adauga_btn = new QPushButton("Adauga");
	QPushButton* reload_btn = new QPushButton("Reload Data");

	QLineEdit* progra = new QLineEdit;


public:
	UI(Service& s) : service{ s } {
		service.addObserver(this);

		init();
		load(service.sortare_stare());
		connect_data();

		Aditional* open1 = new Aditional(service, "open");
		open1->show();

		Aditional* inprogress1 = new Aditional(service, "inprogress");
		inprogress1->show();

		Aditional* closed1 = new Aditional(service, "closed");
		closed1->show();

	}

	void init();
	void load(vector<Task> v);
	void connect_data();

	void update() {
		load(service.sortare_stare());
	}

	~UI() {
		service.removeObserver(this);
	}
};