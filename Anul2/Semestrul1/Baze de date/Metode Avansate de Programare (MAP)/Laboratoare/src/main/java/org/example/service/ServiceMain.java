package org.example.service;

public class ServiceMain {
    private Service serviceUsers;
    private ServiceFriendship serviceFriendship;
    private ServiceCard serviceCard;
    private ServiceEvenimente serviceEvenimente;
    private ServiceMessages serviceMessages;

    public ServiceMain(Service serviceUsers, ServiceFriendship serviceFriendship, ServiceCard serviceCard, ServiceEvenimente serviceEvenimente, ServiceMessages serviceMessages) {
        this.serviceUsers = serviceUsers;
        this.serviceFriendship = serviceFriendship;
        this.serviceCard = serviceCard;
        this.serviceEvenimente = serviceEvenimente;
        this.serviceMessages = serviceMessages;

        //serviceFriendship.rebuildFriendLists();
    }

    public Service getServiceUsers() {
        return serviceUsers;
    }

    public ServiceFriendship getServiceFriendship() {
        return serviceFriendship;
    }

    public ServiceCard getServiceCard() {
        return serviceCard;
    }

    public ServiceEvenimente getServiceEvenimente() {
        return serviceEvenimente;
    }

    public ServiceMessages getServiceMessages() {
        return serviceMessages;
    }
}
