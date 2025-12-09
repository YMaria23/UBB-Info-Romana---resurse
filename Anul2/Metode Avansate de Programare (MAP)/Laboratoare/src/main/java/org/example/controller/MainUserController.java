package org.example.controller;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.HelloApplication;
import org.example.controller.Chats.ChatController;
import org.example.domain.Persoana;
import org.example.domain.User;
import org.example.observers.ObserverNew;
import org.example.repository.dto.Page;
import org.example.repository.dto.Pageable;
import org.example.service.Service;
import org.example.service.ServiceFriendship;
import org.example.service.ServiceMain;
import org.example.utils.events.ChangeEventType;
import org.example.utils.events.EntityChangeEvent;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

public class MainUserController implements ObserverNew<EntityChangeEvent> {

    ServiceMain service;
    ObservableList<User> model = FXCollections.observableArrayList();
    Long idUser;

    private int pageSize = 2; // câte rate pe pagină
    private int currentPage = 0;

    @FXML
    TableView<User> tableUsers;
    @FXML
    TableColumn<User,Long> tableColumnId;
    @FXML
    TableColumn<User,String> tableColumnUsername;

    @FXML
    Label labelPaginaCurenta;

    public void setService(ServiceMain service,Long idUser) {
        this.service = service;
        this.idUser = idUser;
        service.getServiceUsers().addObserver(this);
        service.getServiceMessages().addObserver(this);

        initModel();
    }

    @FXML
    public void initialize() {
        tableColumnId.setCellValueFactory(new PropertyValueFactory<>("id"));
        tableColumnUsername.setCellValueFactory(new PropertyValueFactory<>("username"));

        tableUsers.setItems(model);
    }

    private void initModel() {
        List<User> users = service.getServiceUsers().listUsers();
        loadPage(0);
    }

    private void loadPage(int pageNumber) {
        List<User> users;
        Pageable pageable = new Pageable(pageNumber, pageSize);
        Page<User> page = service.getServiceUsers().findAllOnPage(pageable);
        users = new ArrayList<>((Collection) page.getElements());

        model.setAll(users);
    }

    @Override
    public void update(EntityChangeEvent event) {
        if(event.getType() == ChangeEventType.UPDATE || event.getType() == ChangeEventType.ADD) {
            currentPage = 0;
            initModel();
        }
    }

    @FXML
    public void previousPage(){
        if(currentPage > 0){
            currentPage--;
        }
        loadPage(currentPage);
        labelPaginaCurenta.setText("Current Page: " + (currentPage+1));

    }

    @FXML
    public void nextPage(){
        currentPage++;
        loadPage(currentPage);
        labelPaginaCurenta.setText("Current Page: " + (currentPage+1));
    }


    @FXML
    public void onOpenChats(ActionEvent actionEvent) throws IOException {
        if (idUser != null) {
            FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/chat.fxml"));

            AnchorPane chatContent = fxmlLoader.load();
            ChatController chatController = fxmlLoader.getController();
            chatController.setServiceAndUserId(service.getServiceMessages(), service.getServiceUsers(), idUser);

            //creeaza fereastra
            Stage stage = new Stage();
            stage.setTitle("Chats");
            stage.setScene(new Scene(chatContent));

            stage.show();
        } else {
            MessageAlert.showWarningMessage(null, "Id-ul nu poate sa fie null!");
        }
    }
}
