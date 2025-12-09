package org.example.controller.Persoane;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Label;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.HelloApplication;
import org.example.controller.Chats.ChatController;
import org.example.controller.MessageAlert;
import org.example.domain.Persoana;
import org.example.domain.Rata;
import org.example.domain.User;
import org.example.observers.ObserverNew;
import org.example.repository.dto.Page;
import org.example.repository.dto.Pageable;
import org.example.service.Service;
import org.example.service.ServiceMessages;
import org.example.utils.events.ChangeEventType;
import org.example.utils.events.EntityChangeEvent;

import java.io.IOException;
import java.time.LocalDate;
import java.util.*;

public class PersonController implements ObserverNew<EntityChangeEvent> {
    Service service;
    ServiceMessages serviceMessages;
    ObservableList<Persoana> model = FXCollections.observableArrayList();


    private int pageSize = 2; // câte rate pe pagină
    private int currentPage = 0;


    @FXML
    TableView<Persoana> tableView;
    @FXML
    TableColumn<Persoana, Long> tableColumnId;
    @FXML
    TableColumn<Persoana, String> tableColumnUsername;
    @FXML
    TableColumn<Persoana, String> tableColumnEmail;
    @FXML
    TableColumn<Rata, String> tableColumnPassword;
    @FXML
    TableColumn<Persoana, String> tableColumnNume;
    @FXML
    TableColumn<Persoana, String> tableColumnPrenume;
    @FXML
    TableColumn<Persoana, LocalDate> tableColumnData;
    @FXML
    TableColumn<Persoana, String> tableColumnOcupatie;
    @FXML
    TableColumn<Persoana, Integer> tableColumnEmpatie;


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

        tableColumnNume.setCellValueFactory(new PropertyValueFactory<>("nume"));
        tableColumnPrenume.setCellValueFactory(new PropertyValueFactory<>("prenume"));
        tableColumnData.setCellValueFactory(new PropertyValueFactory<>("dataNasterii"));
        tableColumnOcupatie.setCellValueFactory(new PropertyValueFactory<>("ocupatie"));
        tableColumnEmpatie.setCellValueFactory(new PropertyValueFactory<>("nivelEmpatie"));

        tableView.setItems(model);
    }

    private void initModel() {
        List<Persoana> persoane = service.listUsers().stream()
                .filter(u -> u instanceof Persoana) // ia doar persoanele
                .map(u -> (Persoana) u)            // le convertește la Persoane
                .toList();                              // le pune in lista

        loadPage(0);
    }

    private void loadPage(int pageNumber) {
        List<Persoana> persoane;
        Pageable pageable = new Pageable(pageNumber, pageSize);
        Page<User> page = service.findAllPersoaneOnPage(pageable);
        persoane = new ArrayList<>((Collection) page.getElements());

        model.setAll(persoane);
    }

    @Override
    public void update(EntityChangeEvent event) {
        if(event.getType() == ChangeEventType.UPDATE || event.getType() == ChangeEventType.ADD) {
            currentPage = 0;
            initModel();
        }
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
        FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/editPerson.fxml"));
        Scene scene = new Scene(fxmlLoader.load());

        var stage = new Stage();
        stage.setTitle("Add/Edit Person");
        stage.setScene(scene);

        Persoana selected = tableView.getSelectionModel().getSelectedItem();
        EditPersonController controller = fxmlLoader.getController();
        controller.init(service, stage, selected);

        stage.show();
    }

    @FXML
    public void onDelete(ActionEvent actionEvent) throws IOException {
        Persoana person =  (Persoana) tableView.getSelectionModel().getSelectedItem();
        if(person != null){
            Optional<User> removedPerson = service.removeUser(person.getId());
            if(removedPerson.isPresent()){
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Delete", "Persoana a fost stearsa!");
                initModel();
            }
        }
        else{
            MessageAlert.showWarningMessage(null,"Nu ati selectat persoana pe care doriti sa o stergeti!");
        }
    }


    @FXML
    public void onOpenChats(ActionEvent actionEvent) throws IOException {
        Persoana person =  (Persoana) tableView.getSelectionModel().getSelectedItem();
        if(person != null){
            FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/chat.fxml"));

            AnchorPane chatContent = fxmlLoader.load();
            ChatController chatController = fxmlLoader.getController();
            chatController.setServiceAndUserId(serviceMessages,service,person.getId());

            //creeaza fereastra
            Stage stage = new Stage();
            stage.setTitle("Chats");
            stage.setScene(new Scene(chatContent));

            stage.show();
        }
        else{
            MessageAlert.showWarningMessage(null,"Nu ati selectat o persoana!");
        }
    }
}
