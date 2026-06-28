import { useState } from "react";

export default function AddBarcaForm() {
    const [username, setUsername] = useState("");
    const [error, setError] = useState("");
    const [poz1, setPoz1] = useState("");
    const [poz2, setPoz2] = useState("");
    const [poz3, setPoz3] = useState("");

    async function handleLogin(e) {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch("http://localhost:55556/api/game/barci", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    poz1: poz1,
                    poz2: poz2,
                    poz3: poz3
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
            <h2>Adauga o barca (pentru fiecare pozitie, formatul este: elem,elem)</h2>

            <form onSubmit={handleLogin}>
                <div style={{ marginBottom: "10px" }}>
                    <label>Poz1: </label>
                    <input
                        type="text"
                        value={poz1}
                        onChange={(e) => setPoz1(e.target.value)}
                        required
                    />

                    <label>Poz2: </label>
                    <input
                        type="text"
                        value={poz2}
                        onChange={(e) => setPoz2(e.target.value)}
                        required
                    />

                    <label>Poz3: </label>
                    <input
                        type="text"
                        value={poz3}
                        onChange={(e) => setPoz3(e.target.value)}
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