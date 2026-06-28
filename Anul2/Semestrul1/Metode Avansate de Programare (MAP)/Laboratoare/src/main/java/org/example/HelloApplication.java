package org.example;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.TabPane;
import javafx.scene.layout.AnchorPane;
import javafx.stage.Stage;
import org.example.controller.LoginController;
import org.example.controller.MainController;
import org.example.domain.*;
import org.example.repository.PagedRepository;
import org.example.repository.database.*;
import org.example.service.*;
import org.example.validatori.Validate;

import java.io.IOException;

public class HelloApplication extends Application{
        Validate validator;
        RepoDBUtilizatori repoUsers;
        RepoDBFriendships repoFriendships;
        PagedRepository<Card<?extends Rata>> repoCard;
        PagedRepository<Event> repoEvent;
        RepoDBUserEvent repoDBUserEvent;
        RepoDBMessages repoMessages;

        ServiceFriendship serviceFriendship;
        ServiceCard serviceCard;
        ServiceEvenimente serviceEvenimente;
        Service service;
        ServiceMessages serviceMessages;

        ServiceMain serviceMain;

        public static void main(String[] args) {
            launch(args);
        }

        @Override
        public void start(Stage primaryStage) throws IOException {
//        String fileN = ApplicationContext.getPROPERTIES().getProperty("data.tasks.messageTask");
//        messageTaskRepository = new InFileMessageTaskRepository
//                (fileN, new MessageTaskValidator());
//        messageTaskService = new MessageTaskService(messageTaskRepository);
            //messageTaskService.getAll().forEach(System.out::println);

            System.out.println("Reading data from file");
            String url = "jdbc:postgresql://localhost:5432/DuckSocialNetwork";
            String user = "postgres";
            String pass = "maria";

            validator = new Validate();
            repoUsers = new RepoDBUtilizatori(url,user, pass);
            repoFriendships = new RepoDBFriendships(url,user,pass);
            repoCard = new RepoDBCard(url,user,pass);
            repoEvent = new RepoDBEvent(url,user,pass);
            repoDBUserEvent = new RepoDBUserEvent(url,user,pass);
            repoMessages = new RepoDBMessages(url,user,pass);

            serviceFriendship = new ServiceFriendship(repoUsers,repoFriendships);
            serviceCard = new ServiceCard(repoCard,repoUsers,validator);
            serviceEvenimente = new ServiceEvenimente(repoEvent ,repoUsers,repoDBUserEvent,validator);
            service = new Service(repoUsers, serviceFriendship,serviceEvenimente,validator);
            serviceMessages = new ServiceMessages(repoMessages,service);

            serviceMain = new  ServiceMain(service,serviceFriendship,serviceCard,serviceEvenimente,serviceMessages);

            //utilizatorRepository.findAll().forEach(x-> System.out.println(x));

            initView(primaryStage);
            primaryStage.setWidth(800);
            primaryStage.show();
        }

        private void initView(Stage primaryStage) throws IOException {

            // FXMLLoader fxmlLoader = new FXMLLoader();
            //fxmlLoader.setLocation(getClass().getResource("com/example/guiex1/views/UtilizatorView.fxml"));
            /*FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/mainPan.fxml"));

            TabPane userLayout = fxmlLoader.load();
            primaryStage.setScene(new Scene(userLayout));

            MainController mainController = fxmlLoader.getController();
            mainController.setService(serviceMain);

             */

            FXMLLoader fxmlLoader = new FXMLLoader(HelloApplication.class.getResource("/login.fxml"));

            AnchorPane userLayout = fxmlLoader.load();
            primaryStage.setScene(new Scene(userLayout));

            LoginController loginController = fxmlLoader.getController();
            loginController.init(serviceMain,primaryStage);



            //RateController userController = fxmlLoader.getController();
            //userController.setUtilizatorService(service);
        }
}
