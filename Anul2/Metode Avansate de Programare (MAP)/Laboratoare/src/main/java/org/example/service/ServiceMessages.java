package org.example.service;

import javafx.util.Pair;
import org.example.domain.User;
import org.example.domain.messages.Message;
import org.example.domain.messages.ReplyMessage;
import org.example.exceptii.NotInListException;
import org.example.observers.ObservableNew;
import org.example.observers.ObserverNew;
import org.example.repository.database.RepoDBMessages;
import org.example.utils.events.ChangeEventType;
import org.example.utils.events.EntityChangeEvent;

import java.util.*;
import java.util.stream.Collectors;

public class ServiceMessages implements ObservableNew<EntityChangeEvent> {
    RepoDBMessages repoMessages;
    Service serviceUsers;

    private List<ObserverNew<EntityChangeEvent>> observers = new ArrayList<>();

    public ServiceMessages(RepoDBMessages repoMessages, Service serviceUsers)
    {
        this.repoMessages = repoMessages;
        this.serviceUsers = serviceUsers;
    }

    public Optional<Message> findOne(Long id){
        return repoMessages.findOne(id);
    }

    /**
     * Functia salveaza un mesaj
     * @param message
     * @return Optional - de mesaj, daca acesta a putut fi salvat
     *                  - empty, in caz contrar
     * @throws org.example.exceptii.NotInListException daca - user-ul care trimite mesajul sau cei care primesc nu exista
     *                                                      - mesajul catre care vrea sa dea reply nu exista
     */
    public Optional<Message> save(Message message){
        serviceUsers.findUserById(message.getFrom());

        for(Long destinatar: message.getTo())
            serviceUsers.findUserById(destinatar);


        if(message instanceof ReplyMessage){
            Long replyId =  ((ReplyMessage)message).getReplyMessage();
            if(repoMessages.findOne(replyId).isEmpty())
                throw new NotInListException("There is no such reply");
        }

        Optional<Message> saved = repoMessages.save(message);
        saved.ifPresent(m ->
                notifyObservers(new EntityChangeEvent<>(ChangeEventType.ADD, m)));

        return saved;
    }

    private Boolean containsUser(Long idUser, Message message){
        if(message.getFrom().equals(idUser))
            return true;

        for(Long destinatar: message.getTo())
            if(destinatar.equals(idUser))
                return true;

        return false;
    }


    /**
     * Returneaza lista tuturor chat-urilor (mesajele din chat-uri sunt ordonate cronologic)
     * @param userId
     * @return
     */
    public List<List<Message>> groupChatsPerUser(Long userId){
        List<List<Message>> listOfChats = new ArrayList<List<Message>>();
        List<Long> markedMessages = new ArrayList<>();
        List<Message> allMessages = repoMessages.findAll();

        for(Message message: allMessages){
            if(containsUser(userId, message) &&  !markedMessages.contains(message.getId()) && message instanceof Message) {
                //incep functia care imi gaseste chat-ul
                Pair<List<Message>, List<Long>> result = findChatByMessage(message, new ArrayList<>(markedMessages),allMessages);
                List<Message> currentChat = orderChat(result.getKey());
                markedMessages = result.getValue();

                //si adaug chat-ul in lista de chaturi
                listOfChats.add(currentChat);
            }

        }

        return listOfChats;
    }


    public Pair<List<Message>,List<Long>> findChatByMessage(Message message, List<Long> markedMessages,List<Message> allMessages){
        //mesajul de la care incepem este cel principal (cel care a deschis discutia) => cautam toate mesajele care au dat reply la el (si pt fiecare dupa reply ul

        markedMessages.add(message.getId());
        List<Message> chat = new ArrayList<>();
        //List<Message> allMessages = repoMessages.findAll();

        for(Message message2: allMessages){
            if(message2 instanceof ReplyMessage && !markedMessages.contains(message2.getId())){
                ReplyMessage replyMessage = (ReplyMessage)message2;
                if(replyMessage.getReplyMessage().equals(message.getId())){
                    Pair<List<Message>, List<Long>> result = findChatByMessage(message2,markedMessages,allMessages);
                    //chat = result.getKey();
                    //markedMessages = result.getValue();

                    chat.addAll(result.getKey());
                    markedMessages.addAll(result.getValue());

                    //chat.add(message2);
                    //markedMessages.add(message2.getId());
                }
            }
        }

        chat.add(message);
        markedMessages.add(message.getId());

        return new Pair<>(chat, markedMessages);
    }

    public List<Message> orderChat(List<Message> chat){
        List<Message> sorted = chat.stream()
                .sorted(Comparator.comparing(Message::getDate))
                .collect(Collectors.toList());

        return sorted;
    }

    public List<Message> findAll(){
        return repoMessages.findAll();
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

    public Message findBaseMessage(Message message){
        if(message instanceof ReplyMessage){
            ReplyMessage replyMessage = (ReplyMessage)message;
            Long idParent =  replyMessage.getReplyMessage();
            Optional<Message> messageParent = findOne(idParent);

            if(messageParent.isPresent()){
                return findBaseMessage(messageParent.get());
            }
        }

        return message;
    }
}
