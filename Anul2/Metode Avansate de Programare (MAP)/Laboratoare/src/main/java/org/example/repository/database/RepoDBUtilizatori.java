package org.example.repository.database;

import lombok.RequiredArgsConstructor;
import org.example.domain.*;
import org.example.domain.rataFamily.FlyingDuck;
import org.example.domain.rataFamily.SwimmingDuck;
import org.example.domain.rataFamily.SwimmingFlyingDuck;
import org.example.exceptii.ArgumentException;
import org.example.repository.PagedRepository;
import org.example.repository.dto.Page;
import org.example.repository.dto.Pageable;
import org.example.utils.types.TipRata;

import java.sql.*;
import java.sql.Date;
import java.util.*;
import java.util.function.BiFunction;

@RequiredArgsConstructor
public class RepoDBUtilizatori implements PagedRepository<User> {
        private final String url;
        private final String username;
        private final String password;

        @Override
        public Optional<User> findOne(Long id) {
            if(id == null)
                throw new ArgumentException("Id-ul nu poate sa fie null!");

            try (Connection connection = DriverManager.getConnection(url, username, password)) {
                var statement = connection.prepareStatement("SELECT * FROM Users WHERE idu = ?");
                statement.setLong(1, id);
                ResultSet resultSet = statement.executeQuery();

                if (!resultSet.next()) {
                    return Optional.empty();
                }

                var user = getUser(connection, resultSet);
                if (user == null) {
                    throw new RuntimeException("Date inconsistente: user " + id + " există în Users, dar nu are rând în tabela asociată (Persoane/Rate)");
                }

                return Optional.of(user);
            } catch (SQLException e) {
                throw new RuntimeException("Eroare la căutarea userului: " + id, e);
            }
        }

    @Override
        public List<User> findAll() {
            try (Connection connection = DriverManager.getConnection(url, username, password)) {
                var statement = connection.prepareStatement("SELECT * FROM Users");
                ResultSet resultSet = statement.executeQuery();

                List<User> users = new ArrayList<>();
                while (resultSet.next()) {
                    var user = getUser(connection,resultSet);
                    users.add(user);
                }
                return users;
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }
        }

    @Override
    public Optional<User> save(User user) {
            if(user == null)
                throw new ArgumentException("User-ul nu poate sa fie null!");

            try (Connection connection = DriverManager.getConnection(url, username, password)) {
                // Inserare în tabela USERS
                PreparedStatement stmtUser = connection.prepareStatement(
                        "INSERT INTO users (username, pass, email, tip) VALUES (?, ?, ?, ?)",
                        Statement.RETURN_GENERATED_KEYS
                );

                stmtUser.setString(1, user.getUsername());
                stmtUser.setString(2, user.getPassword());
                stmtUser.setString(3, user.getEmail());

                String tip = (user instanceof Persoana) ? "PERSOANA" : "RATA";
                stmtUser.setString(4, tip);

                stmtUser.executeUpdate();

                // Obține id-ul generat automat din USERS
                try (ResultSet keys = stmtUser.getGeneratedKeys()) {
                    if (keys.next()) {
                        long generatedId = keys.getLong(1);
                        user.setId(generatedId);
                    } else {
                        throw new SQLException("Eroare: nu s-a generat ID pentru user.");
                    }
                }

                // Inserare suplimentară în tabelele specifice
                if (user instanceof Persoana userPers) {
                    PreparedStatement stmtPers = connection.prepareStatement(
                            "INSERT INTO persoane (idp, nume, prenume, dataNasterii, ocupatie, nivelEmpatie) VALUES (?, ?, ?, ?, ?, ?)"
                    );
                    stmtPers.setLong(1, userPers.getId());
                    stmtPers.setString(2, userPers.getNume());
                    stmtPers.setString(3, userPers.getPrenume());
                    stmtPers.setDate(4, Date.valueOf(userPers.getDataNasterii()));
                    stmtPers.setString(5, userPers.getOcupatie());
                    stmtPers.setInt(6, userPers.getNivelEmpatie());
                    stmtPers.executeUpdate();
                }

                else if (user instanceof Rata userRata) {
                    PreparedStatement stmtRata = connection.prepareStatement(
                            "INSERT INTO rate (idr, tip, viteza, rezistenta) VALUES (?, ?, ?, ?)"
                    );
                    stmtRata.setLong(1, userRata.getId());
                    stmtRata.setString(2, userRata.getTip().toString());
                    stmtRata.setDouble(3, userRata.getViteza());
                    stmtRata.setDouble(4, userRata.getRezistenta());
                    stmtRata.executeUpdate();
                }

            } catch (SQLException e) {
                return Optional.empty();
            }
            return Optional.of(user);
    }


    @Override
        public Optional<User> update(User user) {
            if(user == null)
                throw new ArgumentException("User-ul nu poate sa fie null!");

            try (Connection connection = DriverManager.getConnection(url, username, password)) {

                // actualizam tabela principala USERS
                String tip = (user instanceof Persoana) ? "PERSOANA" : "RATA";

                var stmtUser = connection.prepareStatement(
                        "UPDATE Users SET username = ?, pass = ?, email = ?, tip = ? WHERE idu = ?"
                );
                stmtUser.setString(1, user.getUsername());
                stmtUser.setString(2, user.getPassword());
                stmtUser.setString(3, user.getEmail());
                stmtUser.setString(4, tip);
                stmtUser.setLong(5, user.getId());
                stmtUser.executeUpdate();

                // actualizam in functie de tipul concret al user-ului
                if (user instanceof Persoana userPers) {
                    var stmtPers = connection.prepareStatement(
                            "UPDATE Persoane SET nume = ?, prenume = ?, dataNasterii = ?, ocupatie = ?, nivelEmpatie = ? WHERE idp = ?"
                    );
                    stmtPers.setString(1, userPers.getNume());
                    stmtPers.setString(2, userPers.getPrenume());
                    stmtPers.setDate(3, Date.valueOf(userPers.getDataNasterii()));
                    stmtPers.setString(4, userPers.getOcupatie());
                    stmtPers.setInt(5, userPers.getNivelEmpatie());
                    stmtPers.setLong(6, userPers.getId());
                    var rez = stmtPers.executeUpdate();

                    return rez > 0 ? Optional.of(user) : Optional.empty();
                }

                else if (user instanceof Rata userRata) {
                    var stmtRata = connection.prepareStatement(
                            "UPDATE Rate SET tip = ?, viteza = ?, rezistenta = ?, card = ? WHERE idr = ?"
                    );
                    stmtRata.setString(1, userRata.getTip().toString());
                    stmtRata.setDouble(2, userRata.getViteza());
                    stmtRata.setDouble(3, userRata.getRezistenta());

                    if(userRata.getCard() == null)
                        stmtRata.setNull(4, Types.BIGINT);
                    else
                        stmtRata.setLong(4, userRata.getCard());

                    stmtRata.setLong(5, userRata.getId());
                    var rez = stmtRata.executeUpdate();

                    return rez > 0 ? Optional.of(user) : Optional.empty();
                }

            } catch (SQLException e) {
                throw new RuntimeException("Eroare la actualizarea utilizatorului", e);
            }
            return Optional.empty();
        }


    @Override
        public Optional<User> delete(Long id) {
            if(id == null)
                throw new ArgumentException("Id-ul nu poate sa fie null!");

            try (Connection connection = DriverManager.getConnection(url, username, password)) {
                var user = findOne(id);
                if (user.isPresent()) {

                    var statement = connection.prepareStatement("DELETE FROM Users WHERE idu = ?");
                    statement.setLong(1, id);

                    if(user.get() instanceof Persoana) {
                        var statementPers = connection.prepareStatement("DELETE FROM Persoane WHERE idp = ?");
                        statementPers.setLong(1, id);
                        statementPers.execute();
                    }
                    else if (user.get() instanceof Rata) {
                        var statementRata = connection.prepareStatement("DELETE FROM Rate WHERE idr = ?");
                        statementRata.setLong(1, id);
                        statementRata.executeUpdate();
                    }

                    statement.executeUpdate();
                    //return rez > 0 ? Optional.empty() : friendship;
                }
                return user;
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }
        }

        private static User getUser(Connection connection, ResultSet resultSet) throws SQLException {
            var id = resultSet.getLong("idu");
            var username = resultSet.getString("username");
            var password = resultSet.getString("pass");
            var email = resultSet.getString("email");
            var tip = resultSet.getString("tip");

            if ("PERSOANA".equalsIgnoreCase(tip)) {
                try (PreparedStatement ps = connection.prepareStatement("SELECT nume, prenume, dataNasterii, ocupatie, nivelEmpatie FROM Persoane WHERE idp = ?")) {
                    ps.setLong(1, id);

                    try(ResultSet pr = ps.executeQuery()) {
                        if(pr.next()) {
                            var nume = pr.getString("nume");
                            var prenume = pr.getString("prenume");
                            var date = pr.getDate("dataNasterii").toLocalDate();
                            var ocupatie = pr.getString("ocupatie");
                            var nivelEmpatie = pr.getInt("nivelEmpatie");

                            Persoana pers = new Persoana(username, password, email, new ArrayList<>(), new ArrayList<>(), nume, prenume, date, ocupatie, nivelEmpatie);
                            pers.setId(id);
                            return pers;
                        }
                    }
                }
            } else if ("RATA".equalsIgnoreCase(tip)) {
                try (PreparedStatement ps = connection.prepareStatement("SELECT tip,viteza,rezistenta,card FROM Rate WHERE idr=?")) {
                    ps.setLong(1, id);

                    try(ResultSet rr = ps.executeQuery()) {
                        if(rr.next()) {
                            var tipRataString = rr.getString("tip");
                            TipRata tipRata = TipRata.valueOf(tipRataString.toUpperCase());

                            var viteza = rr.getDouble("viteza");
                            var rezistenta = rr.getDouble("rezistenta");
                            Long card = null;
                            try{
                                card = rr.getLong("card");
                            }catch(NullPointerException e){

                            }

                            if (tipRata == TipRata.SWIMMING) {
                                SwimmingDuck rata = new SwimmingDuck(username, password, email, new ArrayList<>(), new ArrayList<>(), tipRata, viteza, rezistenta, card);
                                rata.setId(id);
                                return rata;
                            }

                            if (tipRata == TipRata.FLYING) {
                                FlyingDuck rata = new FlyingDuck(username, password, email, new ArrayList<>(), new ArrayList<>(), tipRata, viteza, rezistenta, card);
                                rata.setId(id);
                                return rata;
                            }

                            if(tipRata == TipRata.FLYING_AND_SWIMMING){
                                SwimmingFlyingDuck rata = new SwimmingFlyingDuck(username, password, email, new ArrayList<>(), new ArrayList<>(), tipRata, viteza, rezistenta, card);
                                rata.setId(id);
                                return rata;
                            }
                        }
                    }
                }
            }
            return null;
        }

    private Page<User> findOnPage(Pageable pageable, String command, BiFunction<Connection, Pageable, List<User>> pageFunction){
        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            var totalNumberOfUsers = count(connection,command);
            List<User> usersOnPage = totalNumberOfUsers > 0 ? pageFunction.apply(connection, pageable) : List.of();
            return new Page<>(usersOnPage, totalNumberOfUsers);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    private List<Object> toSql(String tip) {
        if (tip == null) {
            return Collections.emptyList();
        }

        List<String> conditions = new ArrayList<>();
        List<Object> params = new ArrayList<>();
        if(tip.equalsIgnoreCase("RATA")) {
            conditions.add("where tip=?");
            params.add(tip);
        }else{
            conditions.add("inner join Rate R ON U.idu = R.idR where R.tip = ?");
            params.add(tip);
        }

        return params;
    }


    @Override
    public Page<User> findAllOnPage(Pageable pageable) {
        return findOnPage(pageable,"SELECT COUNT(*) AS count FROM Users",this::findAllOnPage);
    }

    private List<User> findAllOnPage(Connection connection, Pageable pageable) {
        ResultSet resultSet;
        try (var statement = connection.prepareStatement("SELECT * FROM Users limit ? offset ?")) {
            statement.setInt(1,pageable.getPageSize());
            statement.setInt(2,pageable.getPageSize()*pageable.getPageNUmber());
            resultSet = statement.executeQuery();
            List<User> users = new LinkedList<>();
            while (resultSet.next()) {
                var user = getUser(connection,resultSet);
                users.add(user);
            }
            return users;
        }  catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public Page<User> findAllRateOnPage(Pageable pageable){
            return findOnPage(pageable,"SELECT COUNT(*) AS count FROM Users WHERE tip = 'RATA'",this::findAllRateOnPageAux);
    }

    private List<User> findAllRateOnPageAux(Connection connection, Pageable pageable) {
        ResultSet resultSet;
        try (var statement = connection.prepareStatement("SELECT * FROM Users WHERE tip=? limit ? offset ?")) {
            statement.setInt(2,pageable.getPageSize());
            statement.setInt(3,pageable.getPageSize()*pageable.getPageNUmber());
            statement.setString(1,"RATA");

            resultSet = statement.executeQuery();
            List<User> users = new LinkedList<>();
            while (resultSet.next()) {
                var user = getUser(connection,resultSet);
                users.add(user);
            }
            return users;
        }  catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public Page<User> findAllPersOnPage(Pageable pageable) {
        return findOnPage(pageable,"SELECT COUNT(*) AS count FROM Users WHERE tip = 'PERSOANA'",this::findAllPersOnPageAux);
    }

    private List<User> findAllPersOnPageAux(Connection connection, Pageable pageable) {
        ResultSet resultSet;
        try (var statement = connection.prepareStatement("SELECT * FROM Users WHERE tip=? limit ? offset ?")) {
            statement.setInt(2,pageable.getPageSize());
            statement.setInt(3,pageable.getPageSize()*pageable.getPageNUmber());
            statement.setString(1,"PERSOANA");

            resultSet = statement.executeQuery();
            List<User> users = new LinkedList<>();
            while (resultSet.next()) {
                var user = getUser(connection,resultSet);
                users.add(user);
            }
            return users;
        }  catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public Page<User> findAllRateTypeOnPage(Pageable pageable,String tip) {
        try (Connection connection = DriverManager.getConnection(url, username, password)) {
            String sql = String.format(
                    "SELECT COUNT(*) AS count FROM Users U INNER JOIN Rate R ON U.idu = R.idr WHERE R.tip = '%s'",
                    tip
            );

            var totalNumberOfUsers = count(connection,sql);
            List<User> usersOnPage = totalNumberOfUsers > 0 ? findAllRateTypeOnPageAux(connection, pageable,tip) : List.of();
            return new Page<>(usersOnPage, totalNumberOfUsers);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private List<User> findAllRateTypeOnPageAux(Connection connection, Pageable pageable,String tip) {
        ResultSet resultSet;
        try (var statement = connection.prepareStatement("SELECT * FROM Users U INNER JOIN Rate R ON U.idu = R.idr WHERE R.tip = ? limit ? offset ?")) {
            statement.setInt(2,pageable.getPageSize());
            statement.setInt(3,pageable.getPageSize()*pageable.getPageNUmber());
            statement.setString(1,tip);

            resultSet = statement.executeQuery();
            List<User> users = new LinkedList<>();
            while (resultSet.next()) {
                var user = getUser(connection,resultSet);
                users.add(user);
            }
            return users;
        }  catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private int count(Connection connection, String comanda){
        try (var statement = connection.prepareStatement(comanda)) {
            var result =  statement.executeQuery();
            return result.next() ? result.getInt("count") : 0;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

}

