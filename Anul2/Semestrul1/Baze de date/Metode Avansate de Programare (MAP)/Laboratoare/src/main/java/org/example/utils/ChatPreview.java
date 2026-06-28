package org.example.utils;

public class ChatPreview {
    private Long idMainMessage;
    private String title;


    public ChatPreview(Long idMainMessage, String title) {
        this.idMainMessage = idMainMessage;
        this.title = title;
    }

    public Long getIdMainMessage() {
        return idMainMessage;
    }

    public void setIdMainMessage(Long idMainMessage) {
        this.idMainMessage = idMainMessage;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    @Override
    public String toString() {
        return title;
    }
}
