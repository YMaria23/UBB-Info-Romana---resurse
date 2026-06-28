package org.example.service;

import org.example.domain.*;
import org.example.domain.rataFamily.FlyingDuck;
import org.example.domain.rataFamily.SwimmingDuck;
import org.example.domain.rataFamily.SwimmingFlyingDuck;
import org.example.exceptii.NotInListException;
import org.example.exceptii.NrOfArgumentsException;
import org.example.exceptii.ValidatorException;
import org.example.exceptii.ArgumentException;
import org.example.observers.ObservableNew;
import org.example.observers.ObserverNew;
import org.example.repository.database.RepoDBUtilizatori;
import org.example.repository.dto.Page;
import org.example.repository.dto.Pageable;
import org.example.utils.PasswordHasher;
import org.example.utils.types.TipRata;
import org.example.utils.types.TipUtilizator;
import org.example.utils.events.ChangeEventType;
import org.example.utils.events.EntityChangeEvent;
import org.example.validatori.Validate;

import java.time.LocalDate;
import java.util.*;

public class Service implements ObservableNew<EntityChangeEvent> {
    private RepoDBUtilizatori repoUsers;
    private ServiceFriendship serviceFriendship;
    private ServiceEvenimente serviceEvenimente;
    private ServiceCard serviceCard;
    private Validate validator;

    private List<ObserverNew<EntityChangeEvent>> observers = new ArrayList<>();


    public Service(RepoDBUtilizatori repoUsers, ServiceFriendship serviceFriendship, ServiceEvenimente serviceEvenimente, Validate validator) {
        this.repoUsers = repoUsers;
        this.serviceFriendship = serviceFriendship;
        this.serviceEvenimente = serviceEvenimente;
        this.validator = validator;
    }

    @Override
    public void addObserver(ObserverNew<EntityChangeEvent> e) {
        observers.add(e);
    }

    @Override
    public void removeObserver(ObserverNew<EntityChangeEvent> e) {
        observers.remove(e);
    }

    @Override
    public void notifyObservers(EntityChangeEvent e) {
        observers.stream().forEach(o -> o.update(e));
    }

    ////////////////////////////////////// USERI ////////////////////////////////////////////////////////

    /**
     * Metoda adauga un utilizator in lista
     * @param tip - tipul de utilizator pe care trebuie sa il introducem (persoana/rata)
     * @param username - sir de caractere
     * @param email - sir de caractere
     * @param password - sir de caractere
     * @param extraArguments - argumente in plus de tip sir de caractere (depinde nr lor de tipul de utilizator introdus)
     * @throws ArgumentException daca e este null
     *        AlreadyInRepoException daca e este deja in lista
     *        NumberFormatException daca nu se pot transforma anumite date din sir de caractere in numeric
     *        IllegalArgumentException daca tipul de rata introdus este unul invalid (pt cand utilizatorul este rata)
     *        ValidatorException daca id-ul este negativ sau nul, stringurile sunt nule sau emailul este invalid
     * */
    public Optional<User> addUser(TipUtilizator tip, String username, String email, String password, String... extraArguments){
        User user = null;

        if(!validator.verificaString(username) || !validator.verificaString(password))
            throw new ValidatorException("Usernameul si parola nu pot fi nule!");

        if(!validator.verificaEmail(email))
            throw new ValidatorException("Emailul nu este valid!");

        switch(tip){
            case PERSOANA -> {
                //se verifica daca avem destule argumente -> daca nu NrOfArgumentsException
                if(extraArguments.length != 5)
                    throw new NrOfArgumentsException("Wrong number of arguments");

                String nume = extraArguments[0];
                String prenume = extraArguments[1];
                LocalDate dataNasterii = LocalDate.parse(extraArguments[2]);
                String ocupatie = extraArguments[3];
                Integer nivelEmpatie =  Integer.parseInt(extraArguments[4]);

                String pass = PasswordHasher.hashPassword(password);
                user = new Persoana(username,pass,email,new ArrayList<>(),null,nume,prenume,dataNasterii,ocupatie,nivelEmpatie);

                Optional<User> saved = repoUsers.save(user);
                saved.ifPresent(m ->
                        notifyObservers(new EntityChangeEvent<>(ChangeEventType.ADD, m)));
                return saved;
            }
            case RATA ->{
                if(extraArguments.length != 4)
                    throw new NrOfArgumentsException("Wrong number of arguments");

                TipRata tipRata = TipRata.valueOf(extraArguments[0].trim().toUpperCase());
                Double viteza =  Double.parseDouble(extraArguments[1]);
                Double rezistenta = Double.parseDouble(extraArguments[2]);
                Long card = Long.parseLong(extraArguments[3]);

                String pass = PasswordHasher.hashPassword(password);

                if(tipRata == TipRata.FLYING)
                    user = new FlyingDuck(username,pass,email,new ArrayList<>(),null,tipRata,viteza,rezistenta,card);
                else if (tipRata == TipRata.SWIMMING)
                    user = new SwimmingDuck(username,pass,email,new ArrayList<>(),null,tipRata,viteza,rezistenta,card);
                else if(tipRata == TipRata.FLYING_AND_SWIMMING)
                    user = new SwimmingFlyingDuck(username,pass,email,new ArrayList<>(),null,tipRata,viteza,rezistenta,card);

                Optional<User> saved = repoUsers.save(user);
                saved.ifPresent(m ->
                        notifyObservers(new EntityChangeEvent<>(ChangeEventType.ADD, m)));
                return saved;
            }
        }
        return Optional.empty();
    }

    /**
     * Functia elimina un utilizator din lista
     * @param id - id-ul ratei pe care dorim sa o stergem (nr intreg)
     * @throws ArgumentException daca e este null
     * @return Optional - ce contine User-ul sters, daca acesta exista
     *                  - null daca nu se gaseste
     * */

    public Optional<User> removeUser(Long id) {
        Optional<User> u = this.repoUsers.findOne(id);

        if (id == null) {
            throw new ValidatorException("Empty ID");
        }


        if(u.isPresent()) {
            User user = u.get();
            for (User friend : new ArrayList<User>(user.getPrieteni())) {
                serviceFriendship.removeFriendships(user.getId(), friend.getId());
            }
        }

        Optional<User> deletedUser = repoUsers.delete(id);
        deletedUser.ifPresent(m ->
                notifyObservers(new EntityChangeEvent<>(ChangeEventType.DELETE, null, m)));
        return deletedUser;

        /*

        if(u.isPresent()) {
            User user = u.get();
            for (User friend : new ArrayList<User>(user.getPrieteni())) {
                serviceFriendship.removeFriendships(user.getId(), friend.getId());
            }

//        List<Event> list = serviceEvenimente.getEvents();
//        for(Event e : list){
//            List<org.example.observers.Observer> subscribers = e.getUsers();
//            try{
//                RaceEvent re = (RaceEvent)e;
//                List<Observer> lista = re.getSubscribers();
//                try{
//                    SwimmingDuck userRata = (SwimmingDuck) u;
//                    if(lista.contains(userRata))
//                        lista.remove(userRata);
//                }catch(ClassCastException ex){
//                }
//                try{
//                    SwimmingFlyingDuck userRata = (SwimmingFlyingDuck) u;
//                    if(lista.contains(userRata))
//                        lista.remove(userRata);
//                }catch(ClassCastException ex){
//                }
//            }catch(ClassCastException ex){
//            }
//            if(subscribers.contains(u))
//                e.removeObserver(u);
//        }
//
//        try{
//            Rata rata = (Rata) u;
//            Long idCard = rata.getCard();
//            Card<? extends Rata>  card = serviceCard.getCard(idCard);
//
//            SwimmingDuck sd = null;
//            FlyingDuck fd = null;
//            try {
//                sd = (SwimmingDuck)rata;
//                Card<SwimmingDuck> sc = (Card<SwimmingDuck>) card;
//                if(sc != null)
//                    sc.removeMember(sd);
//            }catch(ClassCastException e){
//                fd = (FlyingDuck)rata;
//                Card<FlyingDuck> fc = (Card<FlyingDuck>) card;
//                if (fc != null)
//                    fc.removeMember(fd);
//            }
//        }catch(ClassCastException e){
//        }

            this.repoUsers.delete(id);
        }
        else
            throw new NotInListException("Utilizatorul nu se afla in lista");

         */
    }

    public Optional<User> update(TipUtilizator tip, Long id, String username, String email, String password, String... extraArguments) {
        User user = null;

        if(id == null)
            throw new ValidatorException("Id-ul nu poate sa fie null!");

        if(!validator.validareId(id))
            throw new ValidatorException("Id-ul nu poate sa fie negativ!");

        if(!validator.verificaString(username) || !validator.verificaString(password))
            throw new ValidatorException("Usernameul si parola nu pot fi nule!");

        if(!validator.verificaEmail(email))
            throw new ValidatorException("Emailul nu este valid!");

        switch(tip){
            case PERSOANA -> {
                //se verifica daca avem destule argumente -> daca nu NrOfArgumentsException
                if(extraArguments.length != 5)
                    throw new NrOfArgumentsException("Wrong number of arguments");

                String nume = extraArguments[0];
                String prenume = extraArguments[1];
                LocalDate dataNasterii = LocalDate.parse(extraArguments[2]);
                String ocupatie = extraArguments[3];
                Integer nivelEmpatie =  Integer.parseInt(extraArguments[4]);

                String pass = PasswordHasher.hashPassword(password);
                user = new Persoana(username,pass,email,new ArrayList<>(),null,nume,prenume,dataNasterii,ocupatie,nivelEmpatie);
                user.setId(id);

                Optional<User> saved = repoUsers.update(user);
                saved.ifPresent(m ->
                        notifyObservers(new EntityChangeEvent<>(ChangeEventType.UPDATE, m)));
                return saved;
            }
            case RATA ->{
                if(extraArguments.length != 4)
                    throw new NrOfArgumentsException("Wrong number of arguments");

                TipRata tipRata = TipRata.valueOf(extraArguments[0].trim().toUpperCase());
                Double viteza =  Double.parseDouble(extraArguments[1]);
                Double rezistenta = Double.parseDouble(extraArguments[2]);
                Long card = Long.parseLong(extraArguments[3]);

                String pass = PasswordHasher.hashPassword(password);

                if(tipRata == TipRata.FLYING)
                    user = new FlyingDuck(username,pass,email,new ArrayList<>(),null,tipRata,viteza,rezistenta,card);
                else if (tipRata == TipRata.SWIMMING)
                    user = new SwimmingDuck(username,pass,email,new ArrayList<>(),null,tipRata,viteza,rezistenta,card);
                else if(tipRata == TipRata.FLYING_AND_SWIMMING)
                    user = new SwimmingFlyingDuck(username,pass,email,new ArrayList<>(),null,tipRata,viteza,rezistenta,card);

                user.setId(id);

                Optional<User> saved = repoUsers.update(user);
                saved.ifPresent(m ->
                        notifyObservers(new EntityChangeEvent<>(ChangeEventType.UPDATE, m)));
                return saved;
            }
        }
        return Optional.empty();
    }

    /**
     * Metoda gaseste un utilizator dupa id-ul sau
     * @param id - id-ul utilizatorului
     * @return utilizatorul cautat
     * @throws ArgumentException daca id este null
     *        NotInListException daca nu exista un utilizator cu id-ul respectiv
     * */
    public User findUserById(Long id){
        if(repoUsers.findOne(id).isPresent())
            return repoUsers.findOne(id).get();
        else
            throw new NotInListException("User not found!");
    }

    /**
     * @return lista de utilizatori
     * */
    public List<User> listUsers(){
        return (ArrayList)repoUsers.findAll();
    }

    /**
     * @return lista de utilizatori paginata
     * */
    public Page<User> findAllOnPage(Pageable pageable){
        return repoUsers.findAllOnPage(pageable);
    }

    /**
     * @return lista de rate paginata (origare ar fi tipul ales)
     * */
    public Page<User> findAllRateOnPage(Pageable pageable,TipRata tip){

        if(tip!=null)
            return repoUsers.findAllRateTypeOnPage(pageable,tip.toString());
        else
            return repoUsers.findAllRateOnPage(pageable);
    }

    /**
     * @return lista de persoane paginata
     * */
    public Page<User> findAllPersoaneOnPage(Pageable pageable){
        return repoUsers.findAllPersOnPage(pageable);
    }

}
