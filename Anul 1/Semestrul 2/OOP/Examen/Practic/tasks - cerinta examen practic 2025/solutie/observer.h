#pragma once
#include <iostream>
#include <vector>

using namespace std;

class Observer {
public:
	virtual void update() = 0;
};

class Observable {
private:
	vector<Observer*> lista;
protected:
	void notify() {
		for (auto o : lista)
			o->update();
	}
public:
	void addObserver(Observer* o) {
		lista.push_back(o);
	}

	void removeObserver(Observer* o) {
		lista.erase(remove(lista.begin(), lista.end(), o));
	}
};