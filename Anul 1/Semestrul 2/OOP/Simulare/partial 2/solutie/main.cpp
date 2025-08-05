#include "partial2.h"
#include <QtWidgets/QApplication>
#include "teste.h"
#include "ui.h"

/*
* in acest proiect, stergerea este facuta dupa id (nu dupa selectare din lista)
*/

int main(int argc, char* argv[])
{
    QApplication a(argc, argv);


    Repo repository("fisier.txt");
    Validare valid;
    Service service(repository, valid);
    UI fereastra(service);

    Teste test;
    test.run();

    fereastra.show();

    return a.exec();
}

