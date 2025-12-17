
-- view pe Spectacole -> selecteaza toate denumirile spectacolelor care se joaca la teatrul 2
CREATE VIEW vw_Spectacole AS
	SELECT Denumire FROM Spectacole 
	WHERE IdT = 2

go

drop index eficientizare_view1 on Spectacole
CREATE INDEX eficientizare_view1 ON Spectacole(IdT asc, Denumire asc);
go


-- view pe Spectacole, Roluri, Actori -> afiseaza numele spectacolului si numele actorilor participanti, daca specializarea acestora este 'drama' (pt spectacolele care au cel putin 1 actor) --
CREATE VIEW vw_Spectacole_Actori AS
	SELECT S.Denumire,A.Nume,A.Prenume FROM Spectacole S INNER JOIN Roluri R ON S.IdS = R.IdS 
	INNER JOIN Actori A ON R.IdA = A.IdA 
	WHERE A.Specializare LIKE 'drama';
go

drop view vw_Spectacole_Actori

select * from vw_Spectacole_Actori

select * from Actori

drop index fk_roluri_ida ON Roluri
drop index specializare_actori ON Actori

CREATE INDEX fk_roluri_ida ON Roluri(IdA asc);
CREATE INDEX specializare_actori ON Actori(Specializare asc,Nume asc,Prenume asc);