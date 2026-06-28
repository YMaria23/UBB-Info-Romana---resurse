import { useState, useEffect } from "react";
import LoginForm from "./elements/LoginForm.jsx";
import TableJocuri from "./elements/TableJocuri.jsx";
import * as signalR from "@microsoft/signalr";
import AddBarcaForm from "./elements/AddBarcaForm.jsx";

export default function App() {
  const [user, setUser] = useState(null);
  const [pozitie, setPozitie] = useState(null);
  const [status, setStatus] = useState("");
  const [mesaj, setMesaj] = useState(null);
  
  const [punctaj, setPunctaj] = useState(0);
  const [pozitii, setPozitii] = useState("");

  useEffect(() => {
    if (user === null) return;

    // conectare la SignalR
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:55556/tHub")
        .build();

    connection.start();

    return () => connection.stop(); // cleanup la logout
  }, [user?.gameId]);

  function handleLoginSuccess(loggedUser) {
    setUser(loggedUser);
  }
  
  if (user === null) {
    return (
        <LoginForm onLoginSuccess={handleLoginSuccess}/>
    );
  }

    async function handleGuess() {
        setStatus("");

        try {
            const response = await fetch(`http://localhost:55556/api/game/runde`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    pozitie: pozitie,
                    porecla: user.username,
                    m: user.template
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error("Eroare.");
            }
            
            setUser(prev => ({ ...prev, template: data.mesaj }));
            setStatus(data.status);
            setMesaj(data.mesaj);
            
            if(data.status === "finished"){
                await handleFinish();
            }
            
        }catch(err) {
            console.log(err);
        }

    }

    async function handleFinish() {
        setStatus("");

        try {
            const response = await fetch(`http://localhost:55556/api/game/finish`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    gameId: user.gameId
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error("Eroare.");
            }
            
            setStatus(data.status);

            if(data.status === "finished") {
                setPozitii(data.pozitii);
                setPunctaj(data.punctaj);
            }
        }catch(err) {
            console.log(err);
        }

    }
  
  /*
 
   */

  return (
      <div>
          <h1>Bine ai venit {user.username}!</h1>
          <div style={{
              fontFamily: "monospace",
              fontSize: "20px",
              letterSpacing: "8px",
              lineHeight: "2",
              textAlign: "center"
          }}>
              {status==="" && user.template.map((rand, rowIndex) => (
                  <div key={rowIndex}>
                      {rand.map((cel, colIndex) => (
                          <span key={colIndex}>{cel} </span>
                      ))}
                  </div>
              ))}
              
              {status!=="" && mesaj.map((rand, rowIndex) => (
                  <div key={rowIndex}>
                      {rand.map((cel, colIndex) => (
                          <span key={colIndex}>{cel} </span>
                      ))}
                  </div>
              ))}
          </div>

          <button onClick={() => setUser(null)}>
              Logout
          </button>
          <br/>

          <TableJocuri
              gameList={user.jocuri}
          />

          <br/>
          <br/>

          {status !=="finished" && (
              <div>
                  <label>Alege pozitia (formatul: elem,elem): </label>
                  <input
                      type="text"
                      value={pozitie}
                      onChange={(e) => setPozitie(e.target.value)}
                      required
                  />
                  <button onClick={handleGuess}>Ghiceste</button>
              </div>
          )}

          {status === "finished" && (
              <div>
                  <label>Pozitiile corecte sunt: {pozitii}</label>
                  <label>Punctajul obtinut este: {punctaj}</label>
              </div>
          )}
          
          <br/>
          <br/>
          
          <AddBarcaForm/>
      </div>
  );
}