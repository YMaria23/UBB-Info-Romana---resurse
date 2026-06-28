import {useState,useEffect} from "react";


function ActivityRow({joc}){
    return (
        <tr>
            <td>{joc.poreclaJucator}</td>
            <td>{joc.oraInceput}</td>
            <td>{joc.punctaj}</td>
        </tr>
    );
}

export default function TableClasament({ gameList}) {
    let rows = [];
    (gameList ?? []).forEach(function(joc) {
        rows.push(<ActivityRow joc={joc}  key={joc.id}/>);
    });


    return (
        <div className="ActivityTable">
            <table className="center">
                <thead>
                <tr>
                    <th>Jucator</th>
                    <th>Ora Inceput</th>
                    <th>Punctaj</th>
                </tr>
                </thead>
                <tbody>{rows}</tbody>
            </table>

        </div>
    );
}