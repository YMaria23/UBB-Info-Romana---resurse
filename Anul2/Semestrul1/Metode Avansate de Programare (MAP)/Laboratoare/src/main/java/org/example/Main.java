package org.example;

import org.example.config.Config;
import org.example.domain.*;
import org.example.repository.*;
import org.example.repository.database.*;
import org.example.service.*;
import org.example.validatori.Validate;

public class Main{
    public static void main(String[] args) {

        /*
        String url = "jdbc:postgresql://localhost:5432/DuckSocialNetwork";
        String user = "postgres";
        String pass = "maria";

        Citire toolCitire = new Citire();
        List<User> users = toolCitire.citeste("fisier.csv");

        List<Friendship> friendships = new ArrayList<Friendship>();
        List<Card<? extends Rata>> carduri = new ArrayList<>();
        List<Event> events = new ArrayList<>();

        Validate validator = new Validate();

        //repository.Repository<User> repoUsers = new RepositoryClasic<User>(users);
        RepoDBUtilizatori repoUsers = new RepoDBUtilizatori(url,user,pass);

        //repository.Repository<Friendship> repoFriendships = new RepositoryFriendship(friendships);
        RepoDBFriendships repoFriendships = new RepoDBFriendships(url,user,pass);

        //Repository<Card<? extends Rata>> repoCard = new RepositoryCard(carduri);
        PagedRepository<Card<?extends Rata>> repoCard = new RepoDBCard(url,user,pass);

        //Repository<Event> repoEvent = new RepositoryEvent(events);
        PagedRepository<Event> repoEvent = new RepoDBEvent(url,user,pass);
        RepoDBUserEvent repoDBUserEvent = new RepoDBUserEvent(url,user,pass);

        ServiceFriendship serviceFriendship = new ServiceFriendship(repoUsers,repoFriendships);
        ServiceCard serviceCard = new ServiceCard(repoCard,repoUsers,validator);
        ServiceEvenimente serviceEvenimente = new ServiceEvenimente(repoEvent ,repoUsers,repoDBUserEvent,validator);
        Service service = new Service(repoUsers, serviceFriendship,serviceEvenimente,validator);

        ServiceMain serviceMain = new  ServiceMain(service,serviceFriendship,serviceCard,serviceEvenimente);

        UI runner = new UI(serviceMain);
        runner.run();

         */
        String url = Config.getProperties().getProperty("db.url");
        String user = Config.getProperties().getProperty("db.username");
        String pass = Config.getProperties().getProperty("db.password");
        /*

        Validate validator = new Validate();

        //repository.Repository<User> repoUsers = new RepositoryClasic<User>(users);
        RepoDBUtilizatori repoUsers = new RepoDBUtilizatori(url,user,pass);

        //repository.Repository<Friendship> repoFriendships = new RepositoryFriendship(friendships);
        RepoDBFriendships repoFriendships = new RepoDBFriendships(url,user,pass);

        //Repository<Card<? extends Rata>> repoCard = new RepositoryCard(carduri);
        PagedRepository<Card<?extends Rata>> repoCard = new RepoDBCard(url,user,pass);

        //Repository<Event> repoEvent = new RepositoryEvent(events);
        PagedRepository<Event> repoEvent = new RepoDBEvent(url,user,pass);
        RepoDBUserEvent repoDBUserEvent = new RepoDBUserEvent(url,user,pass);

        ServiceFriendship serviceFriendship = new ServiceFriendship(repoUsers,repoFriendships);
        ServiceCard serviceCard = new ServiceCard(repoCard,repoUsers,validator);
        ServiceEvenimente serviceEvenimente = new ServiceEvenimente(repoEvent ,repoUsers,repoDBUserEvent,validator);
        Service service = new Service(repoUsers, serviceFriendship,serviceEvenimente,validator);


        RepoDBMessages repoM = new RepoDBMessages(url,user,pass);
        ServiceMessages serviceM = new ServiceMessages(repoM,service);

        /*
        ArrayList<Long> l =  new ArrayList<>();
        l.add(2L);
        l.add(15L);

        Message m = new Message(5L,l,"buna", LocalDateTime.now());
        serviceM.save(m);

        l.remove(15L);
        l.add(5L);

        ReplyMessage mr = new ReplyMessage(15L,l,"buna si voua",LocalDateTime.now(),m.getId());
        serviceM.save(mr);


        serviceM.findAll().forEach(System.out::println);
         */


        //serviceM.groupChatsPerUser(22L).forEach(System.out::println);


        Repository<User> userFileRepository3 = new RepoDBUtilizatori(url,user, pass);

        userFileRepository3.findAll().forEach(x-> System.out.println(x));

        HelloApplication.main(args);
    }
}
