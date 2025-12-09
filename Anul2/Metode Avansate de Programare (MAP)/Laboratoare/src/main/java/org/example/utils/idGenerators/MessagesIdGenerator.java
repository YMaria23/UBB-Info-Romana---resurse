package org.example.utils.idGenerators;

public class MessagesIdGenerator {
    private static Long idCurent = 0L;

    /**
     * Genereaza id-uri unice pentru clasa Message & ReplyMessage
     * */
    public static synchronized Long nextId() {
        return ++idCurent;
    }
}
