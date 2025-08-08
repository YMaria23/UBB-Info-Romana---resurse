#include <QtWidgets/QApplication>
#include "ui.h"
#include "test.h"

int main(int argc, char* argv[])
{
    QApplication app(argc, argv);

    Testare t;
    t.run_teste();

    Repo repo("fisier.txt");
    Service service(repo);
    UI fereastra(service);
    fereastra.show();

    return app.exec();
}
