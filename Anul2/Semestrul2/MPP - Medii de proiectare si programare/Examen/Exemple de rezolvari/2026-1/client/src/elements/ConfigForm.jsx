import {useState,useEffect} from "react";


export default function ConfigForm({ onConfigSuccess, user }) {
    const [error, setError] = useState("");
    const [config, setConfig] = useState([]);
    const [porecla, setPorecla] = useState("");
    const [chosenConfig, setChosenConfig] = useState("");

    useEffect(() => {
        handleReceivingConfig();
    }, []);
    
    async function handleReceivingConfig() {
        setError("");
        setConfig([]);
        setPorecla("");
    
        try {
            const response = await fetch("http://localhost:55556/api/games/config", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            });
    
            const data = await response.json();
    
            if (!response.ok) {
                throw new Error("Eroare.");
            }
    
            setConfig(data.conf);
            setPorecla(data.porecla);
            
            onConfigSuccess({
                porecla: data.porecla,
                config: data.conf,
            });
    
        } catch (err) {
            console.log(err);
            setError(err.message);
        }
    }


    async function handleChoosingConfig() {
        setError("");

        try {
            const response = await fetch("http://localhost:55556/api/games/config", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    chosenConfig: config[chosenConfig-1]
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error("Eroare.");
            }

        } catch (err) {
            console.log(err);
            setError(err.message);
        }
    }

    return (
        <div style={{ textAlign: "center", marginTop: "50px" }}>
            <h2>Configuratii disponibile</h2>

            {error && <p style={{ color: "red" }}>{error}</p>}
            {config.map((conf, index) => (
                <p key={index}>{conf}</p>
            ))}
            <label>Alege jucatorul: {porecla}</label>

            {user.username === porecla &&
                (<div>
                    <label>Alege configuratia (1,2 sau 3):</label>
                    <input type="text" onChange={(e) => setChosenConfig(e.target.value)} />
                    <button onClick = {handleChoosingConfig} >Alege</button>
                </div>)
            }
        </div>
    );
}