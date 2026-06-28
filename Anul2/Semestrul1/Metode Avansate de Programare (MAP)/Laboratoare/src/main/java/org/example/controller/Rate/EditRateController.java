package org.example.controller.Rate;

import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import org.example.controller.MessageAlert;
import org.example.domain.Rata;
import org.example.domain.User;
import org.example.exceptii.NrOfArgumentsException;
import org.example.exceptii.ValidatorException;
import org.example.service.Service;
import org.example.utils.types.TipRata;
import org.example.utils.types.TipUtilizator;

import java.util.Optional;

public class EditRateController {
    @FXML
    private TextField textFieldUsername;
    @FXML
    private TextField textFieldEmail;
    @FXML
    private TextField textFieldPassword;
    @FXML
    private TextField textFieldTip;
    @FXML
    private TextField textFieldRezistenta;
    @FXML
    private TextField textFieldViteza;
    @FXML
    private TextField textFieldCard;

    private Service service;
    private Stage dialogStage;
    private Rata rata;

    public void init(Service service, Stage stage, Rata rata) {
        this.service = service;
        this.dialogStage = stage;
        this.rata = rata;

        if (null != rata) {
            setFields(rata);
        }
    }

    @FXML
    public void onSave() {
        String username = textFieldUsername.getText();
        String password = textFieldPassword.getText();
        String email = textFieldEmail.getText();
        String tip = textFieldTip.getText();
        String rezistenta = textFieldRezistenta.getText();
        String viteza = textFieldViteza.getText();
        String card = textFieldCard.getText();

        try {
            TipRata tipFinal = TipRata.valueOf(tip.toUpperCase());
            Double rezistentaFinal = Double.valueOf(rezistenta);
            Double vitezaFinal = Double.valueOf(viteza);
            Long cardFinal = Long.valueOf(card);

            if (null == this.rata)
                insertRata(username,password,email,tipFinal,rezistentaFinal,vitezaFinal,cardFinal);
            else {
                updateRata(rata.getId(), username, password, email, tipFinal, rezistentaFinal, vitezaFinal, cardFinal);
            }
        } catch (IllegalArgumentException e) {
            MessageAlert.showErrorMessage(null, "Date de intrare invalide!");
        }
    }

    private void insertRata(String username, String password, String email,TipRata tip,Double rezistenta,  Double viteza, Long card) {
        try {
            Optional<User> savedDuck = this.service.addUser(TipUtilizator.RATA,username,email,password,tip.toString(),viteza.toString(),rezistenta.toString(),card.toString());
            if (savedDuck.isPresent()) {
                dialogStage.close();
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Inserted Duck", "O rata noua a fost adaugata!");
            }
        }
        catch(ValidatorException | NrOfArgumentsException e){
            MessageAlert.showErrorMessage(null, e.getMessage());
        }
        dialogStage.close();
    }

    private void updateRata(Long id,String username, String password, String email,TipRata tip,Double rezistenta,  Double viteza, Long card) {
        try {
            Optional<User> savedDuck = this.service.update(TipUtilizator.RATA,id,username,email,password,tip.toString(),viteza.toString(),rezistenta.toString(),card.toString());
            if (savedDuck.isPresent()) {
                dialogStage.close();
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Updated Duck", "Rata selectata a fost actualizata!");
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
        textFieldTip.setText("");
        textFieldRezistenta.setText("");
        textFieldViteza.setText("");
        textFieldCard.setText("");
    }

    private void setFields(Rata rata) {
        textFieldUsername.setText(rata.getUsername());
        textFieldPassword.setText(rata.getPassword());
        textFieldEmail.setText(rata.getEmail());
        textFieldTip.setText(String.valueOf(rata.getTip()));
        textFieldRezistenta.setText(String.valueOf(rata.getRezistenta()));
        textFieldViteza.setText(String.valueOf(rata.getViteza()));
        textFieldCard.setText(String.valueOf(rata.getCard()));
    }

    @FXML
    public void onCancel() {
        dialogStage.close();
    }

}
