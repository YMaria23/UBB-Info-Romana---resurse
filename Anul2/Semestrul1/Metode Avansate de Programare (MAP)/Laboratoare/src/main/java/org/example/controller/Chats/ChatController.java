package org.example.controller.Chats;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.concurrent.Task;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;
import org.example.HelloApplication;
import org.example.controller.MessageAlert;
import org.example.controller.Persoane.EditPersonController;
import org.example.domain.messages.Message;
import org.example.domain.messages.ReplyMessage;
import org.example.observers.ObserverNew;
import org.example.service.Service;
import org.example.service.ServiceMessages;
import org.example.utils.ChatPreview;
import org.example.utils.events.ChangeEventType;
import org.example.utils.events.EntityChangeEvent;

import java.io.IOException;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

public class ChatController implements ObserverNew<EntityChangeEvent> {

    ServiceMessages service;
    Service serviceUsers;
    ObservableList<Message> model = FXCollections.observableArrayList();

    Long idUser;
    List<List<Message>> chats;

    private Label selectedMessageLabel = null;
    private Message selectedMessage = null;


    @FXML
    ListView<ChatPreview> listChats;
    @FXML
    Label labelTitle;

    @FXML
    VBox vBoxMessages;

    @FXML
    ScrollPane scrollPane;

    @FXML
    TextField textFieldReply;

    public void setServiceAndUserId(ServiceMessages service,Service serviceUsers,Long idUser) {
        this.service = service;
        this.serviceUsers = serviceUsers;
        service.addObserver(this);
        serviceUsers.addObserver(this);
        this.idUser = idUser;

        String title = "List of Chats for User " +  idUser;
        labelTitle.setText(title);

        loadChatsThread();
    }


    @Override
    public void update(EntityChangeEvent event) {
        if(event.getType() == ChangeEventType.ADD) {
            ChatPreview selected = listChats.getSelectionModel().getSelectedItem();
            loadChats(); // de modificat pe viitor

            //se reselecteaza chat-ul in noua lista de chat-uri
            if (selected != null) {
                for (ChatPreview chat : listChats.getItems()) {
                    if(chat.getIdMainMessage().equals(selected.getIdMainMessage())) {
                        selected = chat;
                        loadMessages(selected.getIdMainMessage());
                        break;
                    }
                }
            }

        }
    }

    @FXML
    public void initialize() {
        listChats.getSelectionModel().selectedItemProperty().addListener((obs, oldValue, newValue) -> {
            if (newValue != null) {
                //trebuie mai intai sa gasim mesajul cu id-ul cautat
                loadMessages(newValue.getIdMainMessage());
            }
        });
    }

    private void loadMessages(Long idMessage){
        //trebuie sa curatam mesajele anterior afisate
        vBoxMessages.getChildren().clear();

        //gasesc lista de mesaje ale chat-ului
        List<Message> mesaje = null;

        Optional<Message> message = service.findOne(idMessage);
        for(List<Message> list: chats)
            if(!list.isEmpty())
                if(list.getFirst().getId().equals(idMessage)) {
                    mesaje = list;
                    break;
                }


        //parcurgem lista ca sa adaugam mesajele in UI
        for(Message m:mesaje){
            Label bubble =  new Label(m.getMessage()+" (User " + m.getFrom() + ")");
            bubble.setWrapText(true);

            String color = (m.getFrom().equals(idUser)) ? "#bdaeff" : "#e0d9fe";
            bubble.setStyle("-fx-padding: 10; -fx-background-color: " + color + "; -fx-background-radius: 8;");

            bubble.setOnMouseClicked(event -> selectMessage(bubble, m));

            //folosim pentru aliniere
            HBox hBoxContainer = new HBox(bubble);

            if(m.getFrom().equals(idUser))
                hBoxContainer.setAlignment(Pos.CENTER_RIGHT);
            else
                hBoxContainer.setAlignment(Pos.CENTER_LEFT);

            vBoxMessages.getChildren().add(hBoxContainer);
        }

        //pt a da scroll automat la incarcarea mesajelor
        scrollPane.layout();
        scrollPane.setVvalue(1.0);
    }

    private void selectMessage(Label bubble, Message message) {

        // 1. Deselectăm mesajul anterior
        if (selectedMessageLabel != null) {
            String color = (selectedMessage.getFrom().equals(idUser)) ? "#bdaeff" : "#e0d9fe";
            selectedMessageLabel.setStyle("-fx-padding: 10; -fx-background-color: " + color + "; -fx-background-radius: 8;");
        }

        // 2. Selectăm mesajul curent (vizual)
        bubble.setStyle("-fx-padding: 10; -fx-background-color: #aac9ff; -fx-background-radius: 8;");

        // 3. Salvăm selecția
        selectedMessageLabel = bubble;
        selectedMessage = message;

        System.out.println("Ai selectat mesajul cu id = " + message.getId());
    }

    private void loadChats(){
        //gaseste lista de chat-uri pt userul dat
        chats = service.groupChatsPerUser(idUser);
        ObservableList<ChatPreview> chatPreviews = FXCollections.observableArrayList();

        for(int i = 1; i<=chats.size(); i++){
            String title = "Chat " + i;
            List<Message> chat = chats.get(i-1);

            if(!chat.isEmpty()) {
                Message firstMessage = chat.get(0);
                ChatPreview newChat = new ChatPreview(firstMessage.getId(), title);

                chatPreviews.add(newChat);
            }
        }

        listChats.setItems(chatPreviews);
    }


    /// //////////////////////////////////////// DE UITAT PESTE CUM FUNCTIONEAZA ////////////////////////////////////
    private void loadChatsThread() {

        //se creeaza taskul care rulează în background
        Task<ObservableList<ChatPreview>> task = new Task<>() {
            @Override
            protected ObservableList<ChatPreview> call() {

                //efectiv ce se va rula pe thread-ul nou creat
                chats = service.groupChatsPerUser(idUser);
                ObservableList<ChatPreview> chatPreviews = FXCollections.observableArrayList();

                for (int i = 1; i <= chats.size(); i++) {
                    List<Message> chat = chats.get(i - 1);

                    if (!chat.isEmpty()) {
                        Message firstMessage = chat.get(0);
                        chatPreviews.add(new ChatPreview(firstMessage.getId(), "Chat " + i));
                    }
                }

                return chatPreviews;
            }
        };

        //se actualizeaza in UI
        task.setOnSucceeded(event -> {
            ObservableList<ChatPreview> chatPreviews = task.getValue();
            listChats.setItems(chatPreviews);
        });

        //se porneste thread-ul
        new Thread(task).start();
    }

    @FXML
    public void onStartNewChat(ActionEvent actionEvent) throws IOException {
        FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/newChat.fxml"));
        Scene scene = new Scene(fxmlLoader.load());

        var stage = new Stage();
        stage.setTitle("Start a new chat");
        stage.setScene(scene);

        NewChatController controller = fxmlLoader.getController();
        controller.init(service,serviceUsers,idUser,stage);

        stage.show();
    }

    private void clearText(){
        textFieldReply.setText("");
    }

    @FXML
    public void onReply(ActionEvent actionEvent) throws IOException {
        /*
        Message m = selectedMessage;
        if(m == null) {
            MessageAlert.showErrorMessage(null, "Selectati un mesaj!");
            return;
        }

        FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/replyChat.fxml"));
        Scene scene = new Scene(fxmlLoader.load());

        var stage = new Stage();
        stage.setTitle("Reply to a message");
        stage.setScene(scene);

        ReplyChatController controller = fxmlLoader.getController();
        controller.init(service,idUser,m,stage);

        stage.show();
        */

        Message m = selectedMessage;
        ChatPreview selected = listChats.getSelectionModel().getSelectedItem();

        if (m == null) {
            //daca nu s-a selectat niciun mesaj, automat se pune ca reply la mesajul de baza (care a inceput conversatia)
            Long idMain = selected.getIdMainMessage();
            Optional<Message> mainMessage = service.findOne(idMain);

            m =  mainMessage.get();
        }

        if (selected != null) {
            Long idMainMessage = selected.getIdMainMessage();
            Optional<Message> mainMessage = service.findOne(idMainMessage);

            String message = textFieldReply.getText();

            ReplyMessage mesaj = new ReplyMessage(idUser, new ArrayList<>(mainMessage.get().getTo()), message, LocalDateTime.now(), m.getId());

            service.save(mesaj);
            MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Inserted Message", "A reply was added!");

            clearText();
        }

    }

}
