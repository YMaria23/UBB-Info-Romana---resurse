#include "ui.h"
#include <qmessagebox.h>
#include <algorithm>

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
	txt_ly->addRow("Tip", tip_txt);
	txt_ly->addRow("Nr roti", nr_txt);

	dr_ly->addLayout(txt_ly);
	dr_ly->addWidget(adauga_btn);
	dr_ly->addWidget(dropdown);
	
	dr_ly->addWidget(fig);
}

void UI::load(vector<Tractor> v) {
	model = new MyTable(service, service.sortare_denumire(),selected_text);
	tabel->setModel(model);

	dropdown->clear();
	vector<string> tips;
	for (auto elem : service.sortare_denumire()) {
		auto it = std::find(tips.begin(), tips.end(), elem.get_tip());
		if (it == tips.end()) {
			tips.push_back(elem.get_tip());
			dropdown->addItem(QString::fromStdString(elem.get_tip()));
		}
	}

	fig->set_nr(nr_selected);
	fig->repaint();
}

void UI::connect_data(){
	QObject::connect(adauga_btn, &QPushButton::clicked, [&]() {
		string id = id_txt->text().toStdString();
		string desc = descriere_txt->text().toStdString();
		string tip = tip_txt->text().toStdString();
		string nr = nr_txt->text().toStdString();

		int id_final, nr_final;
		QMessageBox mesaj;

		try {
			id_final = stoi(id);
		}
		catch (exception&) {
			mesaj.warning(this, "Warning", "Id-ul trebuie sa fie nr intreg!");
			return;
		}

		try {
			nr_final = stoi(nr);
		}
		catch (exception&) {
			mesaj.warning(this, "Warning", "Nr de roti trebuie sa fie un nr intreg!");
			return;
		}

		try {
			service.adauga(id_final, desc, tip, nr_final);
		}
		catch (exception& e) {
			mesaj.warning(this, "Warning", e.what());
			return;
		}

		load(service.sortare_denumire());
		});

	QObject::connect(dropdown, &QComboBox::activated, [&](int index) {
		selected_text = dropdown->itemText(index).toStdString();
		load(service.sortare_denumire());
		});

	QObject::connect(tabel, &QTableView::clicked, [&](const QModelIndex& index) {
		int row = (long)index.row();
		QModelIndex i = tabel->model()->index(row, 3);
		QVariant item = tabel->model()->data(i);

		QModelIndex id = tabel->model()->index(row, 0);
		QVariant id_item = tabel->model()->data(id);

		try {
			nr_selected = item.toInt();
		}
		catch (exception&) {

		}

		try {
			id_selected = id_item.toInt();
		}
		catch (exception&) {

		}

		load(service.sortare_denumire());
		});

	QObject::connect(fig, &Figura::clicked, [&]() {
		service.scade_nr(id_selected);
		load(service.sortare_denumire());
		});
}