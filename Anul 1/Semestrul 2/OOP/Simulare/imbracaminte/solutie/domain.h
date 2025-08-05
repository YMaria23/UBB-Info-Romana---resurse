#pragma once
#include <iostream>
#include <sstream>
#include <vector>
#include <string>

using namespace std;

class Haine {
private:
	int id;
	string tip;
	string culoare;
	double pret;
public:
	Haine() = default;

	Haine(int id_nou,string tip_nou,string c_noua,double pret_nou) : id{id_nou}, tip{tip_nou}, culoare{c_noua}, pret{pret_nou}{}

	~Haine() {};

	int get_id() const;
	string get_tip() const;
	string get_culoare() const;
	double get_pret() const;

	void set_id(int id);
	void set_tip(string tip);
	void set_culoare(string c);
	void set_pret(double pret);

	friend std::istream& operator >>(std::istream& input, Haine& haina);

};

std::ostream& operator <<(std::ostream& output, const Haine& haina);