/********************************************************************************
** Form generated from reading UI file 'simularerochii.ui'
**
** Created by: Qt User Interface Compiler version 6.9.1
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_SIMULAREROCHII_H
#define UI_SIMULAREROCHII_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QToolBar>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_simularerochiiClass
{
public:
    QMenuBar *menuBar;
    QToolBar *mainToolBar;
    QWidget *centralWidget;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *simularerochiiClass)
    {
        if (simularerochiiClass->objectName().isEmpty())
            simularerochiiClass->setObjectName("simularerochiiClass");
        simularerochiiClass->resize(600, 400);
        menuBar = new QMenuBar(simularerochiiClass);
        menuBar->setObjectName("menuBar");
        simularerochiiClass->setMenuBar(menuBar);
        mainToolBar = new QToolBar(simularerochiiClass);
        mainToolBar->setObjectName("mainToolBar");
        simularerochiiClass->addToolBar(mainToolBar);
        centralWidget = new QWidget(simularerochiiClass);
        centralWidget->setObjectName("centralWidget");
        simularerochiiClass->setCentralWidget(centralWidget);
        statusBar = new QStatusBar(simularerochiiClass);
        statusBar->setObjectName("statusBar");
        simularerochiiClass->setStatusBar(statusBar);

        retranslateUi(simularerochiiClass);

        QMetaObject::connectSlotsByName(simularerochiiClass);
    } // setupUi

    void retranslateUi(QMainWindow *simularerochiiClass)
    {
        simularerochiiClass->setWindowTitle(QCoreApplication::translate("simularerochiiClass", "simularerochii", nullptr));
    } // retranslateUi

};

namespace Ui {
    class simularerochiiClass: public Ui_simularerochiiClass {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_SIMULAREROCHII_H
