package org.example.repository.database;

import org.example.domain.Card;
import org.example.domain.Event;
import org.example.domain.cardFamily.CardInotatoare;
import org.example.domain.cardFamily.CardZburatoare;
import org.example.domain.messages.Message;
import org.example.domain.messages.ReplyMessage;
import org.example.exceptii.ArgumentException;

import java.lang.reflect.Type;
import java.sql.*;
import java.util.ArrayList;
import java.util.Optional;
import java.util.List;

public class RepoDBMessages {
    private final String url;
    private final String username;
    private final String password;

    public RepoDBMessages(String url, String username, String password) {
        this.url = url;
        this.username = username;
        this.password = password;
    }

    public Optional<Message> findOne(Long id) {
        if( id == null )
            throw new ArgumentException("Id-ul nu poate sa fie null");

        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            var statement = connection.prepareStatement("SELECT * FROM Messages WHERE idm = ?");
            statement.setLong(1, id);
            ResultSet resultSet = statement.executeQuery();

            if (!resultSet.next()) {
                return Optional.empty();
            }

            var message = getMessage(resultSet);
            return Optional.of(message);
        } catch (SQLException e) {
            throw new RuntimeException("Eroare la căutarea mesajului: " + id, e);
        }
    }

    public List<Message> findAll() {
        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            var statement = connection.prepareStatement("SELECT * FROM Messages ORDER BY idm");
            ResultSet resultSet = statement.executeQuery();

            List<Message> messages = new ArrayList<>();
            while (resultSet.next()) {
                var message = getMessage(resultSet);
                messages.add(message);
            }
            return messages;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public Optional<Message> save(Message message) {
        if(message == null)
            throw new ArgumentException("Mesajul dat nu poate sa fie null!");
        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            PreparedStatement st = connection.prepareStatement(
                    "INSERT INTO Messages (sender, mess, dtime,idreply) VALUES (?, ?, ?, ?)",
                    Statement.RETURN_GENERATED_KEYS
            );

            st.setLong(1,message.getFrom());
            st.setString(2,message.getMessage());
            st.setTimestamp(3,Timestamp.valueOf(message.getDate()));

            if( message instanceof  ReplyMessage ){
                ReplyMessage replyMessage = (ReplyMessage)message;
                st.setLong(4,replyMessage.getReplyMessage());
            }
            else
                st.setNull(4, Types.BIGINT);

            st.executeUpdate();

            //modificam id-ul
            ResultSet keys = st.getGeneratedKeys();
            if (keys.next()) {
                long id = keys.getLong(1);
                message.setId(id);
            }

            // acum trebuie sa salvam si inregistrarile pentru destinatari
            List<Long> destinatari = message.getTo();

            for (Long destinatar : destinatari) {
                PreparedStatement st2 = connection.prepareStatement("INSERT INTO Destinatari (idU,idm) VALUES (?, ?)");

                st2.setLong(1, destinatar);
                st2.setLong(2, message.getId());

                st2.executeUpdate();
            }

        } catch (SQLException e) {
            return Optional.empty();
        }
        return Optional.of(message);
    }

    private Message getMessage(ResultSet resultSet) throws SQLException {
        var id = resultSet.getLong("idm");
        var sender = resultSet.getLong("sender");
        var message = resultSet.getString("mess");
        var date = resultSet.getTimestamp("dtime").toLocalDateTime();
        Long reply =  resultSet.getLong("idreply");
        if(resultSet.wasNull())
            reply = null;

        List<Long> destinatari = getRecipientsForMessage(id);

        Message mesaj = null;

        if(reply == null){
            mesaj = new Message(sender,destinatari,message,date);
        }
        else
            mesaj = new ReplyMessage(sender,destinatari,message,date,reply);

        mesaj.setId(id);
        return mesaj;
    }

    public List<Long> getRecipientsForMessage(Long messageId) throws SQLException {

        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            String sql = "SELECT idU FROM Destinatari WHERE idm = ?";
            var statement = connection.prepareStatement(sql);

            statement.setLong(1, messageId);

            ResultSet rs = statement.executeQuery();
            List<Long> recipients = new ArrayList<>();

            while (rs.next()) {
                long id = rs.getLong("idU");
                recipients.add(id);
            }

            return recipients;
        }
    }
}
