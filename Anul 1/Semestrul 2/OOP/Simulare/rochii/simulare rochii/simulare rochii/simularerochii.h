#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_simularerochii.h"

class simularerochii : public QMainWindow
{
    Q_OBJECT

public:
    simularerochii(QWidget *parent = nullptr);
    ~simularerochii();

private:
    Ui::simularerochiiClass ui;
};

