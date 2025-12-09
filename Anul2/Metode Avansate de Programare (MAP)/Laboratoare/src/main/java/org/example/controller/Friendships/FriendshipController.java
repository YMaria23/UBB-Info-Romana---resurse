package org.example.controller.Friendships;

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
import javafx.stage.Stage;
import org.example.HelloApplication;
import org.example.controller.MessageAlert;
import org.example.controller.Persoane.EditPersonController;
import org.example.domain.Friendship;
import org.example.domain.Persoana;
import org.example.domain.User;
import org.example.observers.ObserverNew;
import org.example.repository.dto.Page;
import org.example.repository.dto.Pageable;
import org.example.service.Service;
import org.example.service.ServiceFriendship;
import org.example.utils.events.ChangeEventType;
import org.example.utils.events.EntityChangeEvent;

import java.io.IOException;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

public class FriendshipController implements ObserverNew<EntityChangeEvent> {
    ServiceFriendship service;
    Service serviceUsers;
    ObservableList<Friendship> model = FXCollections.observableArrayList();


    private int pageSize = 2; // câte rate pe pagină
    private int currentPage = 0;


    @FXML
    TableView<Friendship> tableView;
    @FXML
    TableColumn<Friendship, Long> tableColumnId;
    @FXML
    TableColumn<Friendship, Long> tableColumnUser1;
    @FXML
    TableColumn<Friendship, Long> tableColumnUser2;

    @FXML
    Label labelPaginaCurenta;


    public void setFriendshipService(ServiceFriendship service,Service serviceUsers) {
        this.service = service;
        this.serviceUsers = serviceUsers;
        service.addObserver(this);
        serviceUsers.addObserver(this);
        initModel();
    }

    @FXML
    public void initialize() {
        tableColumnId.setCellValueFactory(new PropertyValueFactory<>("id"));
        tableColumnUser1.setCellValueFactory(new PropertyValueFactory<>("user1"));
        tableColumnUser2.setCellValueFactory(new PropertyValueFactory<>("user2"));

        tableView.setItems(model);
    }

    private void initModel() {
        List<Friendship> friendships = service.listFriendships();

        loadPage(0);
    }

    private void loadPage(int pageNumber) {
        List<Friendship> friendships;
        Pageable pageable = new Pageable(pageNumber, pageSize);
        Page<Friendship> page = service.findFriendshipsOnPage(pageable);

        Iterable<Friendship> iterable = page.getElements();
        friendships = new ArrayList<>();
        iterable.forEach(friendships::add);
        //friendships = new ArrayList<>(page.getElements());

        model.setAll(friendships);
    }

    @Override
    public void update(EntityChangeEvent event) {
        if(event.getType() == ChangeEventType.ADD || event.getType() == ChangeEventType.UPDATE || event.getType() == ChangeEventType.DELETE){
            currentPage = 0;
            initModel();
        }
    }

    @FXML
    public void handleFilterRata(){
        currentPage = 0;
        loadPage(0);
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
        FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/editFriendship.fxml"));
        Scene scene = new Scene(fxmlLoader.load());

        var stage = new Stage();
        stage.setTitle("Add/Edit Friendship");
        stage.setScene(scene);

        Friendship selected = tableView.getSelectionModel().getSelectedItem();
        EditFriendshipController controller = fxmlLoader.getController();
        controller.init(service, stage, selected);

        stage.show();
    }

    @FXML
    public void onDelete(ActionEvent actionEvent) throws IOException {
        Friendship friendship =  tableView.getSelectionModel().getSelectedItem();
        if(friendship != null){
            Optional<Friendship> removedFriendship = service.removeFriendships(friendship.getUser1(), friendship.getUser2());
            if(removedFriendship.isPresent()){
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Delete", "Prietenia a fost stearsa!");
                initModel();
            }
        }
        else{
            MessageAlert.showWarningMessage(null,"Nu ati selectat prietenia pe care doriti sa o stergeti!");
        }
    }

    @FXML
    public void onNrComunitati(ActionEvent actionEvent) throws IOException {
        int nr = service.nrComunitati();
        MessageAlert.showMessage(null,Alert.AlertType.INFORMATION,"Nr Comunitati","Nr comunitatilor este: "+nr);
    }

    @FXML
    public void onCeaMaiSociabilaComunitate(ActionEvent actionEvent) throws IOException {
        List<Long> comunitate = service.ceaMaiSociabilaComunitate();
        String text = comunitate.stream()
                .map(Object::toString)
                .collect(Collectors.joining(", "));

        MessageAlert.showMessage(null,Alert.AlertType.INFORMATION,"Cea mai sociabila comunitate","Cea mai sociabila comunitate este formata din userii cu id-urile: "+text);
    }

}
