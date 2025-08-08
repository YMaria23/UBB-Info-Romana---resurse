#pragma once
#include "service.h"
#include <qwidget.h>
#include <qpushbutton.h>
#include <qboxlayout.h>
#include <qtableview.h>
#include <qformlayout.h>
#include <qlineedit.h>
#include <qcombobox.h>
#include <qpainter.h>

class MyTable :public QAbstractTableModel {
private:
	Service& service;
	vector<Tractor> v;
	string selected_text;
public:
	MyTable(Service& s, vector<Tractor> l, string t) : service{ s }, v{ l }, selected_text{ t } {};

	int rowCount(const QModelIndex& parent = QModelIndex()) const override {
		return v.size();
	}

	int columnCount(const QModelIndex& parent = QModelIndex()) const override {
		return 5;
	}

	QVariant data(const QModelIndex& index, int role = Qt::DisplayRole) const override {
		if (role == Qt::DisplayRole) {
			const Tractor t = v[index.row()];
			switch (index.column()) {
			case 0:
				return t.get_id();
			case 1:
				return QString::fromStdString(t.get_denumire());
			case 2:
				return QString::fromStdString(t.get_tip());
			case 3:
				return t.get_nr();
			case 4:
				return service.filtrare_tip(t.get_tip()).size();
			}
		}
		if (role == Qt::BackgroundRole) {
			const Tractor t = v[index.row()];
			if (t.get_tip() == selected_text) {
				return QBrush(Qt::red);
			}
		}
		return QVariant{};
	}

	QVariant headerData(int section, Qt::Orientation orientation, int role = Qt::DisplayRole) const override{
		if (role == Qt::DisplayRole && orientation == Qt::Horizontal) {
			switch (section) {
			case 0:
				return "ID";
			case 1:
				return "DESCRIERE";
			case 2:
				return "TIP";
			case 3:
				return "NR ROTI";
			case 4:
				return "NR TRACTOARE";
			}
		}
		if (role == Qt::DisplayRole && orientation == Qt::Vertical)
			return section + 1;
		return QVariant{};
	}
};

class Figura :public QWidget {
	Q_OBJECT

private:
	Service& service;
	int nr;
signals:
	void clicked();
public:
	Figura(Service& s, int n) : service{ s }, nr{ n } {
		repaint();
	};

	void paintEvent(QPaintEvent* ev) override {
		qDebug() << "si pe aici\n";
		QPainter p{ this };
		int x = 15;
		int y = 10;
		for (int i = 0; i < nr; i++) {
			p.drawEllipse({ x,15 }, y, y);
			x += 25;
		}
	}

	void mousePressEvent(QMouseEvent* mouse) override {
		emit clicked();
		change_nr();
		repaint();
	}

	void set_nr(int n) {
		nr = n;
	}

	void change_nr() {
		nr -= 2;
	}
};

class UI :public QWidget {
private:
	Service& service;

	QTableView* tabel = new QTableView;
	MyTable* model = nullptr;

	QLineEdit* id_txt = new QLineEdit;
	QLineEdit* descriere_txt = new QLineEdit;
	QLineEdit* tip_txt = new QLineEdit;
	QLineEdit* nr_txt = new QLineEdit;

	QPushButton* adauga_btn = new QPushButton("Adauga");

	QComboBox* dropdown = new QComboBox;

	string selected_text;
	int nr_selected;
	int id_selected;

	Figura* fig = new Figura(service,0);
	
public:
	UI(Service& s) : service{ s } {
		init();
		load(service.sortare_denumire());
		connect_data();
	}

	void init();
	void load(vector<Tractor> v);
	void connect_data();
};