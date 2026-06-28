import {useState,useEffect} from "react";


function ActivityRow({joc}){
    return (
        <tr>
            <td>{joc.id}</td>
            <td>{joc.puncte}</td>
            <td>{joc.pozitia}</td>
            <td>{joc.runde}</td>
        </tr>
    );
}

export default function TableJocuri({ gameList }) {
        let rows = [];
        gameList.forEach(function(joc) {
            rows.push(<ActivityRow joc={joc}  key={joc.id} />);
        });


        return (
            <div className="ActivityTable">
                <table className="center">
                    <thead>
                    <tr>
                        <th>Id</th>
                        <th>Puncte Obtinute</th>
                        <th>Pozitia</th>
                        <th>Runde</th>
                    </tr>
                    </thead>
                    <tbody>{rows}</tbody>
                </table>

            </div>
        );
}