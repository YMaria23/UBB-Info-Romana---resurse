import { useState, useEffect, useCallback } from "react";
import LoginForm from "./elements/LoginForm.jsx";
import TableJocuri from "./elements/TableJocuri.jsx"
import * as signalR from "@microsoft/signalr";
import UpdateConfigForm from "./elements/UpdateConfigForm.jsx";
import TableClasament from "./elements/TableClasament.jsx";

// import: npm install @microsoft/signalr

export default function App() {
  const [user, setUser] = useState(null);
  const [status, setStatus] = useState("");
  const [alegere, setAlegere] = useState("");
  
  const [punctaj, setPunctaj] = useState(-1);
  const [alegereJoc, setAlegereJoc] = useState("");
  
  const [lista, setLista] = useState(null);
  const [punctajFinal, setPunctajFinal] = useState(0);
  
  const [statusGame, setStatusGame] = useState("");

  const clasament = useCallback(async () => {
        const response = await fetch(`http://localhost:55556/api/game/clasament`);
        const data = await response.json();
        setLista(data);
        }, []);

  useEffect(() => {
    if (user === null) return;

    // conectare la SignalR
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:55556/tHub")
        .build();

    connection.on("FinishGame", () => {
          clasament();
    });

    connection.start();

    return () => connection.stop(); // cleanup la logout
  }, [user?.gameId,clasament]);

  function handleLoginSuccess(loggedUser) {
    setUser(loggedUser);
  }

    function handleUpdateSuccess(data) {
        setStatus(data.status);
    }
  
  if (user === null) {
    return (
        <LoginForm onLoginSuccess={handleLoginSuccess}/>
    );
  }

  function handleChoose(joc) {
        setAlegere(joc);
  }
  
  async function playGame() {
      try {
          const response = await fetch(`http://localhost:55556/api/game/runde`, {
              method: "POST",
              headers: {
                  "Content-Type": "application/json"
              },
              body: JSON.stringify({
                  opt: alegere,
                  porecla: user.username
              })
          });

          const data = await response.json();

          if (!response.ok) {
              throw new Error("Eroare.");
          }

          setPunctaj(data.punctaj);
          setAlegereJoc(data.alegereJoc)
          setStatusGame(data.status);

          if(data.status === "finished"){
              await finishGame();
          }

      }catch(err) {
          console.log(err);
      }
  }
  
  async function finishGame(){
      try {
          const response = await fetch(`http://localhost:55556/api/game/finish`, {
              method: "POST",
              headers: {
                  "Content-Type": "application/json"
              },
              body: JSON.stringify({
                  porecla: user.username,
                  gameId: user.gameId
              })
          });

          const data = await response.json();

          if (!response.ok) {
              throw new Error("Eroare.");
          }
          
          setPunctajFinal(data.punctaj);
          setLista(data.lista);

      }catch(err) {
          console.log(err);
      }
  }
  
  
  
  /* {user.username}
  <p>{user.mesaj}</p>

        <button onClick={() => setUser(null)}>
          Logout
        </button>
   */

  return (
      <div>
        <h1>Bine ai venit {user.username}!</h1>
        <button onClick={() => setUser(null)}>
          Logout
        </button>

        <TableJocuri
            gameList={user.lista}
            chooseFunction={handleChoose}
        />
          
          <br/><br/>
          <UpdateConfigForm onUpdateSuccess={handleUpdateSuccess}/>
          {status === "updated" && (
              <label>Modificare realizata cu succes!</label>
          )}
          
          <button onClick={() => playGame()}>Server Generates</button>
          <div>
              <p>Punctaj: {punctaj}</p>
              <p>Alegerea jocului: {alegereJoc}</p>
          </div>
          
          <br/><br/><br/>
          {statusGame === "finished" &&(
              <div>
                  <h2>Game Finished!</h2>
                  <p>Punctaj: {punctajFinal}</p>
              </div>
          )}
          
          <TableClasament gameList = {lista}/>
          
      </div>
  );
}