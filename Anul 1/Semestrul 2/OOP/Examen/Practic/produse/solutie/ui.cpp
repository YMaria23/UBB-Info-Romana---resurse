#include "ui.h"
#include <qmessagebox.h>
#include <qbrush.h>
#include <map>

void UI::init() {
	QHBoxLayout* main_ly = new QHBoxLayout;
	setLayout(main_ly);

	QVBoxLayout* st_ly = new QVBoxLayout;
	QVBoxLayout* dr_ly = new QVBoxLayout;

	main_ly->addLayout(st_ly);
	main_ly->addLayout(dr_ly);

	st_ly->addWidget(tabel);

	QFormLayout* txt_ly = new QFormLayout;
	dr_ly->addLayout(txt_ly);
	txt_ly->addRow("Id", id_txt);
	txt_ly->addRow("Nume", nume_txt);
	txt_ly->addRow("Tip", tip_txt);
	txt_ly->addRow("Pret", pret_txt);
	txt_ly->addRow("Valoare Slider", slider_txt);

	dr_ly->addWidget(adauga_btn);
	dr_ly->addWidget(slider);
	slider->setSingleStep(5);

}

void UI::load(const vector<Produs>& v) {
	model = new MyTable{ v, new_value };
	tabel->setModel(model);
}

void UI::connect_data() {
	QObject::connect(adauga_btn, &QPushButton::clicked, [&]() {
		QMessageBox mesaj;

		string id = id_txt->text().toStdString();
		string nume = nume_txt->text().toStdString();
		string tip = tip_txt->text().toStdString();
		string pret = pret_txt->text().toStdString();

		int id_final;
		double pret_final;

		try {
			id_final = stoi(id);
		}
		catch (exception&) {
			mesaj.warning(this, "Warning", "Id-ul trebuie sa fie un nr intreg!");
			return;
		}

		try {
			pret_final = stod(pret);
		}
		catch (exception&) {
			mesaj.warning(this, "Warning", "Pretul trebuie sa fie un nr real!");
			return;
		}

		try {
			service.adauga(id_final, nume, tip, pret_final);
		}
		catch (exception& e) {
			mesaj.warning(this, "Warning", e.what());
		}

		load(service.sorteaza_pret());
		});

	QObject::connect(slider, &QSlider::valueChanged, [&]() {
		new_value = slider->value();
		slider_txt->setText(QString::fromStdString(to_string(new_value)));
		load(service.sorteaza_pret());
		});
}

void UI::open() {
	vector<Produs> lista = service.sorteaza_pret();

	for (auto elem : lista) {
		if (v.find(elem.get_tip()) == v.end()) {
			TipWidget* tip = new TipWidget(service, elem.get_tip());
			tip->show();

			v.insert({ elem.get_tip(), 1 });
		}
	}
}