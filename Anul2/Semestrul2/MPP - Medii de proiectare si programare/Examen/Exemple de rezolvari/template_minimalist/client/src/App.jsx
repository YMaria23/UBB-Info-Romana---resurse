import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

// import: npm install @microsoft/signalr

export default function App() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    if (user === null) return;

    // conectare la SignalR
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:55556/tHub")
        .build();

    connection.on("GameChanged", () => {
      // cand serverul trimite GameChanged, actualizeaza mesajul
      setUser(prev => ({ ...prev, mesaj: "Jocul poate incepe" }));
      getTurn();
    });

    connection.start();

    return () => connection.stop(); // cleanup la logout
  }, [user?.gameId]);

  function handleLoginSuccess(loggedUser) {
    setUser(loggedUser);
  }

  /*
  if (user === null) {
    return (
        <LoginForm onLoginSuccess={handleLoginSuccess}/>
    );
  }
   */
  
  
  /* {user.username}
  <p>{user.mesaj}</p>

        <button onClick={() => setUser(null)}>
          Logout
        </button>
   */

  return (
      <div>
        <h1>Bine ai venit!</h1>
      </div>
  );
}