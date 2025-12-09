package org.example.domain.messages;

import org.example.domain.User;

import java.time.LocalDateTime;
import java.util.List;

public class ReplyMessage extends Message{
    Long replyMessage;

    public ReplyMessage(Long from, List<Long> to, String message, LocalDateTime date, Long replyMessage) {
        super(from, to, message, date);
        this.replyMessage = replyMessage;
    }

    public Long getReplyMessage() {
        return replyMessage;
    }

    public void setReplyMessage(Long replyMessage) {
        this.replyMessage = replyMessage;
    }

    @Override
    public String toString() {
        return "ReplyMessage{" +
                super.toString() +
                "replyMessage=" + replyMessage +
                '}';
    }
}
