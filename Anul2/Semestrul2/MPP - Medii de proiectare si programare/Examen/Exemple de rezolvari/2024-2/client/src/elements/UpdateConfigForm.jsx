import { useState } from "react";

export default function UpdateConfigForm({ onUpdateSuccess }) {
    const [username, setUsername] = useState("");
    const [error, setError] = useState("");
    const [poz1, setPoz1] = useState("");
    const [poz2, setPoz2] = useState("");
    const [poz3, setPoz3] = useState("");
    const [poz4, setPoz4] = useState("");
    const [id, setId] = useState("");

    async function handleLogin(e) {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch("http://localhost:55556/api/game/config", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    opt1: poz1,
                    opt2: poz2,
                    opt3: poz3,
                    opt4: poz4,
                    id: id
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error("Nu exista configuratie cu acest id!");
            }

            onUpdateSuccess({
                status: data.status
            });

        } catch (err) {
            console.log(err);
            setError(err.message);
        }
    }

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Actualizeaza o configuratie: introdu id-ul si optiunile - litera, numar</h2>

            <form onSubmit={handleLogin}>
                <div style={{ marginBottom: "10px" }}>

                    <label>Id: </label>
                    <input
                        type="text"
                        value={id}
                        onChange={(e) => setId(e.target.value)}
                        required
                    />
                    
                    <label>Opt1: </label>
                    <input
                        type="text"
                        value={poz1}
                        onChange={(e) => setPoz1(e.target.value)}
                        required
                    />

                    <label>Opt2: </label>
                    <input
                        type="text"
                        value={poz2}
                        onChange={(e) => setPoz2(e.target.value)}
                        required
                    />

                    <label>Opt3: </label>
                    <input
                        type="text"
                        value={poz3}
                        onChange={(e) => setPoz3(e.target.value)}
                        required
                    />

                    <label>Opt4: </label>
                    <input
                        type="text"
                        value={poz4}
                        onChange={(e) => setPoz4(e.target.value)}
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