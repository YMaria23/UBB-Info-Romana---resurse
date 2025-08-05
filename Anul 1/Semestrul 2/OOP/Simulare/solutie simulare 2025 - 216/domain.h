#pragma once
#include <iostream>
#include <fstream>
#include <vector>
#include <string>

using namespace std;

class Planta {
private:
	int cod;
	string denumire;
	string tip;
	int apa;
public:
	Planta() = default;
	Planta(int ot_cod,string ot_den,string ot_tip,int ot_apa):cod{ot_cod},denumire{ot_den},tip{ot_tip},apa{ot_apa}{}

	~Planta(){}

	int get_cod() const;
	string get_denumire() const;
	string get_tip() const;
	int get_apa() const;

	void set_cod(int cod);
	void set_denumire(string den);
	void set_tip(string tip);
	void set_apa(int apa);
};