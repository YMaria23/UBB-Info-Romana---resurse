package org.example.controller;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.control.Tab;
import javafx.scene.layout.AnchorPane;
import org.example.controller.Friendships.FriendshipController;
import org.example.controller.Persoane.PersonController;
import org.example.controller.Rate.RateController;
import org.example.service.ServiceMain;

import java.io.IOException;

public class MainController{
    @FXML private Tab TabDuck;
    @FXML private Tab TabPerson;
    @FXML private Tab TabFriendship;

    private ServiceMain service;

    public void setService(ServiceMain service) {
        this.service = service;
        loadTabs();
    }

    private void loadTabs(){
        try {
            //load pt tab-ul de rate
            FXMLLoader loaderDuck = new FXMLLoader(getClass().getResource("/duckTab.fxml"));
            AnchorPane duckContent = loaderDuck.load();
            RateController duckController = loaderDuck.getController();
            duckController.setUtilizatorService(service.getServiceUsers(),service.getServiceMessages());
            TabDuck.setContent(duckContent);

            //load pt tab-ul de persoane
            FXMLLoader loaderPersoana = new FXMLLoader(getClass().getResource("/personTab.fxml"));
            AnchorPane personContent = loaderPersoana.load();
            PersonController personController = loaderPersoana.getController();
            personController.setUtilizatorService(service.getServiceUsers(),service.getServiceMessages());
            TabPerson.setContent(personContent);


            //load pt tab-ul de friendships
            FXMLLoader loaderFriendships = new FXMLLoader(getClass().getResource("/friendshipTab.fxml"));
            AnchorPane friendshipContent = loaderFriendships.load();
            FriendshipController friendshipController = loaderFriendships.getController();
            friendshipController.setFriendshipService(service.getServiceFriendship(),service.getServiceUsers());
            TabFriendship.setContent(friendshipContent);


        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @FXML
    public void initialize() {
    }
}
