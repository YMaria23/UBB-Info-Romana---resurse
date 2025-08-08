#include <QtWidgets/QApplication>
#include "ui.h"

int main(int argc, char* argv[])
{
    QApplication app(argc, argv);

    Repo repo("fisier.txt");
    Service service(repo);
    UI fereastra(service);
    fereastra.show();

    return app.exec();
}
