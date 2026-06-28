package org.example.domain.messages;

import org.example.utils.idGenerators.MessagesIdGenerator;

import java.time.LocalDateTime;
import java.util.List;

public class Message {
    private Long id;
    private Long from;
    private List<Long> to;
    private String message;
    private LocalDateTime date;

    public Message(Long from, List<Long> to, String message, LocalDateTime date) {
        this.id = MessagesIdGenerator.nextId();
        this.from = from;
        this.to = to;
        this.message = message;
        this.date = date;
    }

    @Override
    public String toString() {
        return "Message{" +
                "id=" + id +
                ", from=" + from +
                ", to=" + to +
                ", message='" + message + '\'' +
                ", date=" + date +
                '}';
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Long getFrom() {
        return from;
    }

    public void setFrom(Long from) {
        this.from = from;
    }

    public List<Long> getTo() {
        return to;
    }

    public void setTo(List<Long> to) {
        this.to = to;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public LocalDateTime getDate() {
        return date;
    }

    public void setDate(LocalDateTime date) {
        this.date = date;
    }
}
