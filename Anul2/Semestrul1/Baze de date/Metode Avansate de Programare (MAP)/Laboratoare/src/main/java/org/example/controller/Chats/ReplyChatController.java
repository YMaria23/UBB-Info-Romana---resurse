package org.example.controller.Chats;

import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.TextArea;
import javafx.stage.Stage;
import org.example.controller.MessageAlert;
import org.example.domain.messages.Message;
import org.example.domain.messages.ReplyMessage;
import org.example.exceptii.NotInListException;
import org.example.service.Service;
import org.example.service.ServiceMessages;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

public class ReplyChatController {
    private ServiceMessages serviceMessages;
    private Long userId;
    private Message mainMessage;

    private Stage dialogStage;

    @FXML
    TextArea textAreaMessage;


    public void init(ServiceMessages serviceMessages, Long userId ,Message message,Stage stage){
        this.serviceMessages = serviceMessages;
        this.userId = userId;
        this.mainMessage = message;
        this.dialogStage = stage;
    }

    @FXML
    public void onSave(){
        String message = textAreaMessage.getText();

        //formez mesajul
        ReplyMessage mesaj = new ReplyMessage(userId,mainMessage.getTo(),message,LocalDateTime.now(),mainMessage.getId());

        //adaug mesajul in lista
        serviceMessages.save(mesaj);
        MessageAlert.showMessage(null, Alert.AlertType.INFORMATION,"Inserted Message","A reply was added!");
        dialogStage.close();
    }
}
