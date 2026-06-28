import { useState } from "react";

export default function AddConfigForm() {
    const [username, setUsername] = useState("");
    const [error, setError] = useState("");
    const [config, setConfig] = useState("");
    const [n, setN] = useState("");

    async function handleLogin(e) {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch("http://localhost:55556/api/games/configs", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    n: n,
                    chosenConfig: config
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error("Datele sunt incorecte");
            }

        } catch (err) {
            console.log(err);
            setError(err.message);
        }
    }

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Adauga o configurare</h2>

            <form onSubmit={handleLogin}>
                <div style={{ marginBottom: "10px" }}>
                    <label>N: </label>
                    <input
                        type="text"
                        value={n}
                        onChange={(e) => setN(e.target.value)}
                        required
                    />

                    <label>Valori: </label>
                    <input
                        type="text"
                        value={config}
                        onChange={(e) => setConfig(e.target.value)}
                        required
                    />
                </div>

                {error && <div style={{ color: "red", marginBottom: "10px" }}>{error}</div>}

                <button type="submit">
                    Adauga
                </button>
            </form>
        </div>
    );
}