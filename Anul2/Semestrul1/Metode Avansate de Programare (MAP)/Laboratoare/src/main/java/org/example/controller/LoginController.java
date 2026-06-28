package org.example.controller;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.TabPane;
import javafx.scene.control.TextField;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.HelloApplication;
import org.example.controller.Rate.RateController;
import org.example.domain.User;
import org.example.service.Service;
import org.example.service.ServiceMain;
import org.example.utils.PasswordHasher;

import java.io.IOException;
import java.util.List;
import java.util.Objects;

public class LoginController {
    ServiceMain service;
    Stage stageMain;

    @FXML
    TextField textFieldUsername;

    @FXML
    TextField textFieldPassword;

    @FXML
    TextField textFieldTip;

    public void init(ServiceMain service,Stage stage){
        this.service = service;
        this.stageMain = stage;
    }

    public void login() throws IOException {
        String username = textFieldUsername.getText();
        String password = PasswordHasher.hashPassword(textFieldPassword.getText());
        String tip = textFieldTip.getText();

        List<User> users = service.getServiceUsers().listUsers();
        for(User user : users){
            if(user.getUsername().equals(username) && user.getPassword().equals(password)){
                if(tip.equalsIgnoreCase("administrator")) {
                    FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/mainPan.fxml"));
                    TabPane userLayout = fxmlLoader.load();

                    MainController mainController = fxmlLoader.getController();
                    mainController.setService(service);

                    stageMain.setTitle("Main Application");
                    stageMain.setScene(new Scene(userLayout));

                    return;
                }
                else if(tip.equalsIgnoreCase("normal")){
                    FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/mainUser.fxml"));
                    AnchorPane userLayout = fxmlLoader.load();

                    MainUserController mainController = fxmlLoader.getController();
                    mainController.setService(service,user.getId());

                    stageMain.setTitle("Main User Application");
                    stageMain.setScene(new Scene(userLayout));

                    return;
                }
                else
                    MessageAlert.showErrorMessage(null,"Selectati un tip valid!");
            }
        }

        MessageAlert.showErrorMessage(null,"Wrong username or password!");
    }
}
