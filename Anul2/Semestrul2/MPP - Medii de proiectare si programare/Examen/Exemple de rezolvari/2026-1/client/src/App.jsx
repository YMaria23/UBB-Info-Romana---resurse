import { useState, useEffect } from "react";
import LoginForm from "./elements/LoginForm.jsx";
import ConfigForm from "./elements/ConfigForm.jsx";
import TableJocuri from "./elements/TableJocuri.jsx";
import AddConfigForm from "./elements/AddConfigForm.jsx";

export default function App() {
  const [user, setUser] = useState(null);
    const [filteredGames, setFilteredGames] = useState([]);
    const [randomNumber, setRandomNumber] = useState(0);
    const [turn, setTurn] = useState(0);
    const [n,setN] = useState(0);
    
    const [poreclaTurn, setPoreclaTurn] = useState(0);
    const [pozInceput, setPozInceput] = useState(0);
    const [pozFinal, setPozFinal] = useState(0);
    const [punctaj, setPunctaj] = useState(0);
    
    const [punctajFinal, setPunctajFinal] = useState(0);
    const [castigator, setCastigator] = useState("");
    const [punctajCastigator, setPunctajCastigator] = useState(0);
    const [mutari, setMutari] = useState("");

  useEffect(() => {
        if (user === null) return;

        // conectare la SignalR
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:55556/gameHub")
            .build();

        connection.on("GameChanged", () => {
            // cand serverul trimite GameChanged, actualizeaza mesajul
            setUser(prev => ({ ...prev, mesaj: "Jocul poate incepe" }));
            getTurn();
        });

      connection.on("ConfigAleasa", (confAleasa) => {
          setUser(prev => ({ ...prev, configAleasa: confAleasa }));
      });

      connection.on("NewPlayer", (data) => {
          setN(data.nrJucatori);
          setTurn(data.curent);
      });

      connection.on("NewScore", (data) => {
          setPoreclaTurn(data.porecla);
          setPozInceput(data.pozInceput);
          setPozFinal(data.pozFinal);
          setPunctaj(data.punctaj);
      });

      connection.on("FinishGame", () => {
          finishGame();
      });

        connection.start();

        return () => connection.stop(); // cleanup la logout
  }, [user?.gameId]);

  useEffect(() => {
        refreshList();
        }, [user?.username]);

  function handleLoginSuccess(loggedUser) {
    setUser(loggedUser);
  }

  if (user === null) {
    return (
        <LoginForm onLoginSuccess={handleLoginSuccess} />
    );
  }

    async function refreshList() {
      try {
          const response = await fetch(`http://localhost:55556/api/games/player/?porecla=${user.username}`, {
              method: "GET",
              headers: {
                  "Content-Type": "application/json"
              }
          });

          const data = await response.json();

          if (!response.ok) {
              throw new Error("Eroare.");
          }
          
          setFilteredGames(data);
          return data;
      }catch(err) {
          console.log(err);
      }
      
  }


    async function getTurn() {
      setTurn(0);
      setN(0);
      
        try {
            const response = await fetch(`http://localhost:55556/api/games/mutari`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error("Eroare.");
            }

            setN(data.nrJucatori);
            setTurn(data.curent);
        }catch(err) {
            console.log(err);
        }

    }
  

    async function generateNumber() {
        setPunctaj(0);
        setPozInceput(0);
        setPozFinal(0);
        setPoreclaTurn("");
        
        try {
            // nr intre 1 si 2*n
            var numar = Math.floor(Math.random() * (2 * n));
            const response = await fetch(`http://localhost:55556/api/games/mutari`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    generated: numar,
                    porecla: user.username
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error("Eroare.");
            }

            setPoreclaTurn(data.porecla);
            setPozInceput(data.pozInceput);
            setPozFinal(data.pozFinal);
            setPunctaj(data.punctaj);
        }catch(err) {
            console.log(err);
        }

    }

    async function finishGame() {
        setPunctajCastigator(0);
        setCastigator("");
        setMutari("");

        try {
            const response = await fetch(`http://localhost:55556/api/games/final`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    PlayerP: user.username
                })
            });

            const data = await response.json();
            
            if(!data)
                return;

            if (!response.ok) {
                throw new Error("Eroare.");
            }
            
            setPunctajFinal(data.punctajEu);
            setPunctajCastigator(data.punctajCastig);
            setCastigator(data.poreclaCastigator);
            setMutari(data.mutariCastig);
        }catch(err) {
            console.log(err);
        }

    }
  
  return (
      <div>
        <h1>Bine ai venit, {user.username}!</h1>
          <p>Game ID: {user.gameId}</p>
          <p>Entry order: {user.entryOrder}</p>
          <p>{user.mesaj}</p>

          {user.mesaj === "Jocul poate incepe" && (<ConfigForm user={user} onConfigSuccess={(data) => console.log(data)} />)}

        <button onClick={() => setUser(null)}>
          Logout
        </button>

          {user.configAleasa && (
              <p>Configuratia aleasa: {user.configAleasa}</p>
          )}

          {user.entryOrder - 1 === turn && !castigator && (
              <button onClick = {generateNumber} >Genereaza numar</button>
          )}

          {castigator &&(
              <div>
                  <label>Punctaj {user.username}: {punctajFinal}</label>
                  <label>Punctaj castigator - {castigator}: {punctajCastigator}</label>
                  <label>Cele mai bune mutari: {mutari}</label>
              </div>
          )}
          
          <label>Jucator curent: {poreclaTurn}</label>
          <label>Punctaj obtinut: {punctaj}</label>
          <label>Pozitie de inceput: {pozInceput}</label>
          <label>Pozitie de final: {pozFinal}</label>
          
          <TableJocuri
              gameList={filteredGames}
          />

          <AddConfigForm/>
      </div>
  );
}