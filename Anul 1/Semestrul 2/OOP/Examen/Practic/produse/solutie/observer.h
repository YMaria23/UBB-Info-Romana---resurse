#pragma once
#include <algorithm>
#include <vector>
using namespace std;

class Observer {
public:
	virtual void update() = 0;
};

class Observable {
	vector<Observer*> obs;
protected:
	void notify() {
		for (auto o : obs)
			o->update();
	}
public:
	void addObserver(Observer* o) {
		obs.push_back(o);
	}

	void removeObserver(Observer* o) {
		obs.erase(remove(obs.begin(), obs.end(), o));
	}
};