package org.example.controller.Rate;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.HelloApplication;
import org.example.controller.Chats.ChatController;
import org.example.controller.MessageAlert;
import org.example.domain.Rata;
import org.example.domain.User;
import org.example.observers.ObserverNew;
import org.example.repository.dto.Page;
import org.example.repository.dto.Pageable;
import org.example.service.Service;
import org.example.service.ServiceMessages;
import org.example.utils.types.TipRata;
import org.example.utils.events.ChangeEventType;
import org.example.utils.events.EntityChangeEvent;
import javafx.collections.ObservableList;
import javafx.collections.FXCollections;

import java.io.IOException;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Optional;

import javafx.event.ActionEvent;

public class RateController implements ObserverNew<EntityChangeEvent> {
    Service service;
    ServiceMessages serviceMessages;
    ObservableList<Rata> model = FXCollections.observableArrayList();


    private int pageSize = 2; // câte rate pe pagină
    private int currentPage = 0;


    @FXML
    TableView<Rata> tableView;
    @FXML
    TableColumn<Rata, Long> tableColumnId;
    @FXML
    TableColumn<Rata, String> tableColumnUsername;
    @FXML
    TableColumn<Rata, String> tableColumnEmail;
    @FXML
    TableColumn<Rata, String> tableColumnPassword;
    @FXML
    TableColumn<Rata, TipRata> tableColumnTipR;
    @FXML
    TableColumn<Rata, Double> tableColumnViteza;
    @FXML
    TableColumn<Rata, Double> tableColumnRezistenta;
    @FXML
    TableColumn<Rata, Long> tableColumnCard;


    @FXML
    ComboBox<TipRata> comboBoxTipRata;


    @FXML
    Label labelPaginaCurenta;


    public void setUtilizatorService(Service service,ServiceMessages serviceMessages) {
        this.service = service;
        this.serviceMessages = serviceMessages;
        service.addObserver(this);
        serviceMessages.addObserver(this);
        initModel();
    }

    @FXML
    public void initialize() {
        tableColumnId.setCellValueFactory(new PropertyValueFactory<>("id"));
        tableColumnUsername.setCellValueFactory(new PropertyValueFactory<>("username"));
        tableColumnPassword.setCellValueFactory(new PropertyValueFactory<>("password"));
        tableColumnEmail.setCellValueFactory(new PropertyValueFactory<>("email"));

        tableColumnTipR.setCellValueFactory(new PropertyValueFactory<>("tip"));
        tableColumnViteza.setCellValueFactory(new PropertyValueFactory<>("viteza"));
        tableColumnRezistenta.setCellValueFactory(new PropertyValueFactory<>("rezistenta"));
        tableColumnCard.setCellValueFactory(new PropertyValueFactory<>("card"));

        tableView.setItems(model);

        comboBoxTipRata.getItems().add(null);
        comboBoxTipRata.getItems().addAll(TipRata.values());
    }

    private void initModel() {
        List<Rata> rate = service.listUsers().stream()
                .filter(u -> u instanceof Rata) // ia doar ratele
                .map(u -> (Rata) u)             // le convertește la Rata
                .toList();                           // le pune in lista

        loadPage(0);
    }

    private void loadPage(int pageNumber) {
        TipRata tip = comboBoxTipRata.getValue();


        List<Rata> rate;
        Pageable pageable = new Pageable(pageNumber, pageSize);
        Page<User> page = service.findAllRateOnPage(pageable,tip);
        rate = new ArrayList<>((LinkedList) page.getElements());

        model.setAll(rate);
    }

    @Override
    public void update(EntityChangeEvent event) {
        if (event.getType() == ChangeEventType.ADD || event.getType() == ChangeEventType.UPDATE) {
            currentPage = 0;
            initModel();
        }

    }

    @FXML
    public void handleFilterRata(){
        currentPage = 0;
        loadPage(0);
    }

    public void handleDeleteUtilizator(ActionEvent actionEvent) {
        User user = (User) tableView.getSelectionModel().getSelectedItem();
        if (user != null) {
            service.removeUser(user.getId());
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
    public void onEdit(ActionEvent actionEvent) throws IOException {
        FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/editDuck.fxml"));
        Scene scene = new Scene(fxmlLoader.load());

        var stage = new Stage();
        stage.setTitle("Add/Edit Rata");
        stage.setScene(scene);

        Rata selected = tableView.getSelectionModel().getSelectedItem();
        EditRateController controller = fxmlLoader.getController();
        controller.init(service, stage, selected);

        stage.show();
    }

    @FXML
    public void onDelete(ActionEvent actionEvent) throws IOException {
        Rata rata =  (Rata) tableView.getSelectionModel().getSelectedItem();
        if(rata != null){
            Optional<User> removedRata = service.removeUser(rata.getId());
            if(removedRata.isPresent()){
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Delete", "Rata a fost stearsa!");
                initModel();
            }
        }
        else{
            MessageAlert.showWarningMessage(null,"Nu ati selectat rata pe care doriti sa o stergeti!");
        }
    }

    @FXML
    public void onOpenChats(ActionEvent actionEvent) throws IOException {
        Rata rata =  (Rata) tableView.getSelectionModel().getSelectedItem();
        if(rata != null){
            FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/chat.fxml"));

            AnchorPane chatContent = fxmlLoader.load();
            ChatController chatController = fxmlLoader.getController();
            chatController.setServiceAndUserId(serviceMessages,service,rata.getId());

            //creeaza fereastra
            Stage stage = new Stage();
            stage.setTitle("Chats");
            stage.setScene(new Scene(chatContent));

            stage.show();
        }
        else{
            MessageAlert.showWarningMessage(null,"Nu ati selectat o rata!");
        }
    }
}

