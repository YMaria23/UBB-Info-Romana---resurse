#pragma once
#include <iostream>
#include <vector>
#include <string>
#include <fstream>
#include <sstream>
#include <algorithm>

using namespace std;

class Tractor {
private:
	int id;
	string denumire;
	string tip;
	int nr;
public:
	Tractor(int id_nou, string d, string t, int n) : id{ id_nou }, denumire{ d }, tip{ t }, nr{ n } {

	}

	int get_id() const;
	string get_denumire() const;
	string get_tip() const;
	int get_nr() const;

	void set_id(int id);
	void set_denumire(string den);
	void set_tip(string tip);
	void set_nr(int nr);
};