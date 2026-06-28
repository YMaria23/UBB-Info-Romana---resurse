import {useState,useEffect} from "react";


function ActivityRow({joc, chooseFunc}){
    return (
        <tr>
            <td>{joc}</td>
            <td><button onClick={() => chooseFunc(joc)}>Alege</button></td>
        </tr>
    );
}

export default function TableJocuri({ gameList, chooseFunction }) {
        let rows = [];
        let functie=chooseFunction;
        gameList.forEach(function(joc, index) {
            rows.push(<ActivityRow joc={joc} chooseFunc={functie} key={index}/>);
        });


        return (
            <div className="ActivityTable">
                <table className="center">
                    <thead>
                    <tr>
                        <th>Puncte</th>
                        <th>Actiune</th>
                    </tr>
                    </thead>
                    <tbody>{rows}</tbody>
                </table>

            </div>
        );
}