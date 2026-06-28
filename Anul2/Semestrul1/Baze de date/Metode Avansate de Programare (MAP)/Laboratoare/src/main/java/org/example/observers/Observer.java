package org.example.observers;

import org.example.domain.Event;

public interface Observer {
    /**
     * va trimite mai departe messajul catre observatori => acestia afisand-ul pe ecran
     * */
    void update(Event event);
}
