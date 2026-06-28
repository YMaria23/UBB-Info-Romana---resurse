


package org.example.repository.database;

import org.example.domain.Card;
import org.example.domain.Friendship;

import java.util.*;
import java.sql.*;

import lombok.RequiredArgsConstructor;
import org.example.exceptii.ArgumentException;
import org.example.repository.PagedRepository;
import org.example.repository.Repository;
import org.example.repository.dto.Page;
import org.example.repository.dto.Pageable;

@RequiredArgsConstructor
public class RepoDBFriendships implements PagedRepository<Friendship> {

    private final String url;
    private final String username;
    private final String password;

    @Override
    public Optional<Friendship> findOne(Long id) {
        if(id == null)
            throw new ArgumentException("Id-ul nu poate sa fie null");

        try (Connection connection = DriverManager.getConnection(url, username, password);
             PreparedStatement st = connection.prepareStatement(
                     "SELECT idf, user1, user2 FROM Friendships WHERE idf = ?")) {

            st.setLong(1, id);
            try (ResultSet rs = st.executeQuery()) {
                if (rs.next()) {
                    return Optional.of(getFriendship(rs));
                }
            }
            return Optional.empty();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public List<Friendship> findAll() {
        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            var statement = connection.prepareStatement("SELECT * FROM Friendships ORDER BY idF");
            ResultSet resultSet = statement.executeQuery();

            List<Friendship> friendships = new ArrayList<>();
            while (resultSet.next()) {
                var user = getFriendship(resultSet);
                friendships.add(user);
            }
            return friendships;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public Optional<Friendship> save(Friendship friendship) {
        if(friendship == null)
            throw new ArgumentException("Prietenia nu poate sa fie null");

        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            long a = Math.min(friendship.getUser1(), friendship.getUser2());
            long b = Math.max(friendship.getUser1(), friendship.getUser2());

            PreparedStatement st = connection.prepareStatement(
                    "INSERT INTO Friendships (user1, user2) VALUES (?, ?)",
                    Statement.RETURN_GENERATED_KEYS
            );
            st.setLong(1, a);
            st.setLong(2, b);

            st.executeUpdate();

            try (ResultSet keys = st.getGeneratedKeys()) {
                if (keys.next()) {
                    long generatedId = keys.getLong(1); // prima coloană, fără nume!
                    friendship.setId(generatedId);
                    System.out.println("INSERT OK, id generat = " + generatedId);
                } else {
                    System.out.println("Nu s-a generat niciun ID!");
                }
            }

        } catch (SQLException e) {
            return Optional.empty();
        }
        return Optional.of(friendship);
    }

    @Override
    public Optional<Friendship> update(Friendship friendship) {
        if(friendship == null)
            throw new ArgumentException("Prietenia nu poate sa fie null");

        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            var statement = connection.prepareStatement("UPDATE Friendships SET user1 = ?, user2 = ? WHERE idF = ?");
            statement.setLong(1, friendship.getUser1());
            statement.setLong(2, friendship.getUser2());
            statement.setLong(3, friendship.getId());

            var rez = statement.executeUpdate();

            return rez > 0 ? Optional.of(friendship) : Optional.empty();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public Optional<Friendship> delete(Long id) {
        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            var statement = connection.prepareStatement("DELETE FROM friendships WHERE idf = ?");

            Optional<Friendship> friendship = findOne(id);

            if(friendship.isPresent()) {
                statement.setLong(1, id);
                int rez = statement.executeUpdate();
                System.out.println("Delete friendship id=" + id + " -> " + rez + " rows");
            }
            return friendship;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private static Friendship getFriendship(ResultSet rs) throws SQLException {
        long id = rs.getLong("idf");
        long u1 = rs.getLong("user1");
        long u2 = rs.getLong("user2");

        Friendship f = new Friendship(u1, u2);
        f.setId(id); // suprascrie orice id generat în constructor
        return f;
    }

    @Override
    public Page<Friendship> findAllOnPage(Pageable pageable) {
        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            var totalNumberOfFriendships = count(connection);
            List<Friendship> friendshipsOnPage = totalNumberOfFriendships > 0 ? findAllOnPage(connection, pageable) : List.of();
            return new Page<>(friendshipsOnPage, totalNumberOfFriendships);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private List<Friendship> findAllOnPage(Connection connection, Pageable pageable) {
        ResultSet resultSet;
        try (var statement = connection.prepareStatement("SELECT * FROM Friendships limit ? offset ?")) {
            statement.setInt(1,pageable.getPageSize());
            statement.setInt(2,pageable.getPageSize()*pageable.getPageNUmber());
            resultSet = statement.executeQuery();
            List<Friendship> friendships = new LinkedList<>();
            while (resultSet.next()) {
                var friendship = getFriendship(resultSet);
                friendships.add(friendship);
            }
            return friendships;
        }  catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private int count(Connection connection){
        try (var statement = connection.prepareStatement("SELECT COUNT(*) AS count FROM Friendships")) {
            var result =  statement.executeQuery();
            return result.next() ? result.getInt("count") : 0;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

}
