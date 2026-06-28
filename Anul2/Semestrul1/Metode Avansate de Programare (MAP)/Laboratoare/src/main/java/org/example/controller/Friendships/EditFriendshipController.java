package org.example.controller.Friendships;

import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import org.example.controller.MessageAlert;
import org.example.domain.Friendship;
import org.example.exceptii.AlreadyInRepoException;
import org.example.exceptii.NotInListException;
import org.example.exceptii.NrOfArgumentsException;
import org.example.exceptii.ValidatorException;
import org.example.service.ServiceFriendship;

import java.util.Optional;

public class EditFriendshipController {

    @FXML
    private TextField textFieldIdUser1;
    @FXML
    private TextField textFieldIdUser2;

    private ServiceFriendship service;
    private Stage dialogStage;
    private Friendship friendship;

    public void init(ServiceFriendship service, Stage stage, Friendship friendship) {
        this.service = service;
        this.dialogStage = stage;
        this.friendship = friendship;

        if (null != friendship) {
            setFields(friendship);
        }
    }

    @FXML
    public void onSave() {
        String user1 = textFieldIdUser1.getText();
        String user2 = textFieldIdUser2.getText();

        try {
            Long user1Final = Long.valueOf(user1);
            Long user2Final = Long.valueOf(user2);

            if (null == this.friendship)
                insertFriendship(user1Final,user2Final);
            else
                updateFriendship(user1Final,user2Final);
        } catch (IllegalArgumentException e) {
            MessageAlert.showErrorMessage(null, "Date de intrare invalide!");
        }
    }

    private void insertFriendship(Long user1, Long user2) {
        try {
            Optional<Friendship> savedFriendship = this.service.addFriendships(user1, user2);
            if (savedFriendship.isPresent()) {
                dialogStage.close();
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Inserted Friendship", "O noua prietenie a fost inserata!");
            }
        }
        catch(ValidatorException | NrOfArgumentsException | NotInListException | AlreadyInRepoException e){
            MessageAlert.showErrorMessage(null, e.getMessage());
        }
        dialogStage.close();
    }

    private void updateFriendship(Long user1,Long user2) {
        try {
            Optional<Friendship> savedFriendship = this.service.updateFriendships(friendship.getUser1(), friendship.getUser2(),user1,user2);
            if (savedFriendship.isPresent()) {
                dialogStage.close();
                MessageAlert.showMessage(null, Alert.AlertType.INFORMATION, "Updated Friendship", "Prietenia selectata a fost actualizata!");
            }
            else
                MessageAlert.showWarningMessage(null,"Nu a fost gasita!");
        } catch (ValidatorException | NrOfArgumentsException | NotInListException e) {
            MessageAlert.showErrorMessage(null, e.getMessage());
        }
        dialogStage.close();
    }

    private void clearFields() {
        textFieldIdUser1.clear();
        textFieldIdUser2.clear();
    }

    private void setFields(Friendship friendship) {
        textFieldIdUser1.setText(friendship.getUser1().toString());
        textFieldIdUser2.setText(friendship.getUser2().toString());
    }

    @FXML
    public void onCancel() {
        dialogStage.close();
    }
}
