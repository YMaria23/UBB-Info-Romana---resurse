package org.example.controller.Chats;

import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import org.example.controller.MessageAlert;
import org.example.domain.messages.Message;
import org.example.exceptii.NotInListException;
import org.example.service.Service;
import org.example.service.ServiceMessages;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

public class NewChatController {
    private ServiceMessages serviceMessages;
    private Service serviceUsers;
    private Long userId;

    private Stage dialogStage;

    @FXML
    TextField textFieldSendTo;
    @FXML
    TextArea textAreaMessage;

    public void init(ServiceMessages serviceMessages, Service serviceUsers,Long userId,Stage stage)
    {
        this.serviceMessages = serviceMessages;
        this.serviceUsers = serviceUsers;
        this.userId = userId;
        this.dialogStage = stage;
    }

    @FXML
    public void onSave(){
        String senderss =  textFieldSendTo.getText();
        String message = textAreaMessage.getText();

        //acum dam split pe senders pt a afla cui trebuie sa trimitem mesajul
        String[] senders = senderss.split(",");
        List<Long> sendersList = new ArrayList<Long>();

        //pentru fiecare sender, se verifica daca este valid -> daca e nr si daca apare in lista de utilizatori
        for(String sender : senders){
            try{
                Long senderId = Long.parseLong(sender);
                serviceUsers.findUserById(senderId);

                //adaug in lista de senders
                sendersList.add(senderId);
            }catch(NotInListException e){
                MessageAlert.showErrorMessage(null,e.getMessage());
                return;
            }
            catch(IllegalArgumentException e){
                MessageAlert.showErrorMessage(null,"Date invalide! Trebuie ca id-urile utilizatorilor sa fie nr intregi!");
                return;
            }
        }

        //formez mesajul
        Message mesaj = new Message(userId,sendersList,message, LocalDateTime.now());

        //adaug mesajul in lista
        serviceMessages.save(mesaj);
        MessageAlert.showMessage(null,Alert.AlertType.INFORMATION,"Inserted Message","A new chat was made!");
        dialogStage.close();
    }
}
