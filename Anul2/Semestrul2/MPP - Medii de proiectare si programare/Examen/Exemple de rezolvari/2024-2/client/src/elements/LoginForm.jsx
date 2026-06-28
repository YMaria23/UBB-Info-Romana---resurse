import { useState } from "react";

export default function LoginForm({ onLoginSuccess }) {
    const [username, setUsername] = useState("");
    const [error, setError] = useState("");

    async function handleLogin(e) {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch("http://localhost:55556/api/game/join", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    playerP: username
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error("Nu s-a putut intra in joc.");
            }
            

            onLoginSuccess({
                username: username,
                lista: data.lista,
                gameId: data.gameId
            });

        } catch (err) {
            console.log(err);
            setError(err.message);
        }
    }

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Intrare in joc</h2>

            <form onSubmit={handleLogin}>
                <div style={{ marginBottom: "10px" }}>
                    <label>Porecla: </label>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>

                {error && <div style={{ color: "red", marginBottom: "10px" }}>{error}</div>}

                <button type="submit">
                    Intra in joc
                </button>
            </form>
        </div>
    );
}