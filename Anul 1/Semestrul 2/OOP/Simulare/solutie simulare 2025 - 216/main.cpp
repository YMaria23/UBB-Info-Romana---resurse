#include "exam.h"
#include <QtWidgets/QApplication>
#include "ui.h"
#include "teste.h"

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);

    Teste test;
    test.run();

    Repo repo("fisier.txt");
    Service service(repo);
    UI fereastra(service);

    fereastra.show();
   
    return a.exec();
}
