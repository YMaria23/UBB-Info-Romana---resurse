package org.example.domain;

import org.example.domain.messages.Message;
import org.example.utils.types.TipRata;

import java.util.List;

public abstract class Rata extends User{
    private TipRata tip;
    private Double viteza;
    private Double rezistenta;
    private Long card;


    public Rata(String username, String password, String email, List<User> prieteni, List<Event> evenimente, TipRata tip, Double viteza, Double rezistenta, Long card) {
        super(username, password, email, prieteni, evenimente);
        this.tip = tip;
        this.viteza = viteza;
        this.rezistenta = rezistenta;
        this.card = card;
    }

    @Override
    void login() {
        System.out.println("Login Rata");
    }

    @Override
    void logout() {
        System.out.println("Logout Rata");
    }

    /**
     * Trimite un mesaj standard
     * */
    @Override
    void sendMessage(Message message) {
       // User u = message.getTo();
       // message.setMessage("Quack!Am terminat antrenamentul!");
       // u.receiveMessage(message);
    }

    @Override
    void receiveMessage(Message message) {
        System.out.println(message.getMessage());
    }

    public TipRata getTip() {
        return tip;
    }

    public Double getViteza() {
        return viteza;
    }

    public Double getRezistenta() {
        return rezistenta;
    }

    public Long getCard() {
        return card;
    }

    @Override
    public String toString() {
        return super.toString()+"Rata{" +
                "tip=" + tip +
                ", viteza=" + viteza +
                ", rezistenta=" + rezistenta +
                ", card=" + (card!=null ? card : "fara card")+
                '}';
    }

    @Override
    public void update(Event event) {
        System.out.println("Rata " + getId() + " got notified: ");
    }

    public void setCard(Long card) {
        this.card = card;
    }
}
