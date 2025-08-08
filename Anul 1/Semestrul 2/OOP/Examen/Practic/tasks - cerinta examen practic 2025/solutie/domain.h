#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <sstream>

using namespace std;

class Task {
private:
	int id;
	string descriere;
	vector<string> programatori;
	string stare;
public:
	Task(int i, string d, string s, vector<string> pr) : id{ i }, descriere{ d }, stare{ s }, programatori{ pr } {};

	int get_id() const;
	string get_descriere() const;
	string get_stare() const;
	vector<string> get_programatori() const;

	void set_id(int id);
	void set_descriere(string d);
	void set_stare(string s);
	void set_programatori(vector<string> pr);
};