package org.example.controller.Persoane;

import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import org.example.controller.MessageAlert;
import org.example.domain.Persoana;
import org.example.domain.User;
import org.example.exceptii.NrOfArgumentsException;
import org.example.exceptii.ValidatorException;
import org.example.service.Service;
import org.example.utils.types.TipUtilizator;

import java.time.LocalDate;
import java.util.Optional;

public class EditPersonController {

    @FXML
    private TextField textFieldUsername;
    @FXML
    private TextField textFieldPassword;
    @FXML
    private TextField textFieldEmail;
    @FXML
    private TextField textFieldNume;
    @FXML
    private TextField textFieldPrenume;
    @FXML
    private TextField textFieldData;
    @FXML
    private TextField textFieldOcupatie;
    @FXML
    private TextField textFieldEmpatie;

    private Service service;
    private Stage dialogStage;
    private Persoana person;

    public void init(Service service, Stage stage, Persoana person) {
        this.service = service;
        this.dialogStage = stage;
        this.person = person;

        if (null != person) {
            setFields(person);
        }
    }

    @FXML
    public void onSave() {
        String username = textFieldUsername.getText();
        String password = textFieldPassword.getText();
        String email = textFieldEmail.getText();
        String nume = textFieldNume.getText();
        String prenume = textFieldPrenume.getText();
        String data = textFieldData.getText();
        String ocupatie = textFieldOcupatie.getText();
        String empatie = textFieldEmpatie.getText();

        try {
            LocalDate dataFinala = LocalDate.parse(data);
            Integer empatieFinala = Integer.valueOf(empatie);

            if (null == this.person)
                insertPerson(username,password,email,nume,prenume,data,ocupatie,empatie);
            else {
                updatePerson(person.getId(),username, password, email,nume,prenume,data,ocupatie,empatie);
            }
        } catch (IllegalArgumentException e) {
            MessageAlert.showErrorMessage(null, "Date de intrare invalide!");
        }
    }

    private void insertPerson(String username, String password, String email,String nume, String prenume, String data, String ocupatie, String empatie) {
        try {
            Optional<User> savedPerson = this.service.addUser(TipUtilizator.PERSOANA,username,email,password,nume,prenume,data,ocupatie,empatie);
            if (savedPerson.isPresent()) {
                dialogStage.close();
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Inserted Person", "O noua persoana a fost inserata!");
            }
        }
        catch(ValidatorException | NrOfArgumentsException e){
            MessageAlert.showErrorMessage(null, e.getMessage());
        }
        dialogStage.close();
    }

    private void updatePerson(Long id,String username, String password, String email,String nume, String prenume, String data, String ocupatie, String empatie) {
        try {
            Optional<User> savedPerson = this.service.update(TipUtilizator.PERSOANA,id,username,email,password,nume,prenume,data,ocupatie,empatie);
            if (savedPerson.isPresent()) {
                dialogStage.close();
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Updated Person", "Persoana selectata a fost actualizata!");
            }
        } catch (ValidatorException | NrOfArgumentsException e) {
            MessageAlert.showErrorMessage(null, e.getMessage());
        }
        dialogStage.close();
    }

    private void clearFields() {
        textFieldUsername.setText("");
        textFieldPassword.setText("");
        textFieldEmail.setText("");
        textFieldNume.setText("");
        textFieldPrenume.setText("");
        textFieldData.setText("");
        textFieldOcupatie.setText("");
        textFieldEmpatie.setText("");
    }

    private void setFields(Persoana person) {
        textFieldUsername.setText(person.getUsername());
        textFieldPassword.setText(person.getPassword());
        textFieldEmail.setText(person.getEmail());
        textFieldNume.setText(person.getNume());
        textFieldPrenume.setText(person.getPrenume());
        textFieldPrenume.setText(person.getPrenume());
        textFieldData.setText(String.valueOf(person.getDataNasterii()));
        textFieldOcupatie.setText(person.getOcupatie());
        textFieldEmpatie.setText(String.valueOf(person.getNivelEmpatie()));
    }

    @FXML
    public void onCancel() {
        dialogStage.close();
    }

}
