USE Teatru2
GO

-- aici populam baza de date --

-- populare TeatruNational -- 
INSERT INTO TeatruNational(Denumire,Oras,AnInfiintare) VALUES ('Teatrul National Cluj-Napoca','Cluj-Napoca',1919);
INSERT INTO TeatruNational(Denumire,Oras,AnInfiintare) VALUES ('Teatrul National Bucuresti','Bucuresti',1852);

-- populare Spectacole -- 
INSERT INTO Spectacole(Denumire,IdT) VALUES ('Romeo si Julieta',2); -- 1 --
INSERT INTO Spectacole(Denumire,IdT) VALUES ('O scrisoare pierduta',1); -- 2 --
INSERT INTO Spectacole(Denumire,IdT) VALUES ('O noapte furtunoasa',1); -- 3 --
INSERT INTO Spectacole(Denumire,IdT) VALUES ('D’ale carnavalului',2); -- 4 --
INSERT INTO Spectacole(Denumire,IdT) VALUES ('Patima rosie',1); -- 5 --
INSERT INTO Spectacole(Denumire,IdT) VALUES ('Luceafarul',2); -- 6 --

-- populare Actori --
INSERT INTO Actori(Prenume,Nume,Specializare) VALUES ('Oana ','Pellea','drama'); -- 1--
INSERT INTO Actori(Prenume,Nume,Specializare) VALUES ('Marius ','Manole ','versatil'); -- 2 --
INSERT INTO Actori(Prenume,Nume,Specializare) VALUES ('Vasile ','Muraru','comedie'); -- 3 --
INSERT INTO Actori(Prenume,Nume,Specializare) VALUES ('Ion', 'Caramitru','drama clasica'); -- 4 --
INSERT INTO Actori(Prenume,Nume,Specializare) VALUES ('Maia ','Morgenstern','drama'); -- 5 --
INSERT INTO Actori(Prenume,Nume,Specializare) VALUES ('Sebastian ','Papaiani','comedie'); -- 6 --
INSERT INTO Actori(Prenume,Nume,Specializare) VALUES ('Oana ','Berbec','versatil'); -- 7 --

-- populare Roluri --
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Romeo',1,4);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Julieta',1,1);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Zoe',2,1);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Zaharia',2,6);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Tipatescu',2,2);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Veta',3,5);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Ruxa',5,5);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Rucsandra',6,7);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Bogdan',6,2);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Didina Mazu',4,7);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Iordache Pampon',4,3);
INSERT INTO Roluri(Denumire,IdS,IdA) VALUES ('Nae Ipingescu',3,2);


-- populare Costume --
INSERT INTO Costume(Descriere,IdA) VALUES ('rochie de epoca victoriana',1);
INSERT INTO Costume(Descriere,IdA) VALUES ('costum traditional romanesc',1);
INSERT INTO Costume(Descriere,IdA) VALUES ('tinuta moderna minimalista',7);
INSERT INTO Costume(Descriere,IdA) VALUES ('costum de servitoare comica',7);
INSERT INTO Costume(Descriere,IdA) VALUES ('costum negru elegant',4);
INSERT INTO Costume(Descriere,IdA) VALUES ('costum de taran',2);
INSERT INTO Costume(Descriere,IdA) VALUES ('rochie lungă din matase, cu corset si volane, umbreluta de dantela',5);
INSERT INTO Costume(Descriere,IdA) VALUES ('mantou lung,cizme inalte, medalion',4);
INSERT INTO Costume(Descriere,IdA) VALUES ('camasa alba lunga, brau roșu, itari, opinci, cojoc',6);


-- populare Spectatori --
INSERT INTO Spectatori(Prenume,Nume) VALUES ('Maia','Andreescu'); -- 1--
INSERT INTO Spectatori(Prenume,Nume) VALUES ('Matei','Basarab'); -- 2 --
INSERT INTO Spectatori(Prenume,Nume) VALUES ('Cristian','Presura'); -- 3 --
INSERT INTO Spectatori(Prenume,Nume) VALUES ('Andreea','Popescu'); -- 4 --
INSERT INTO Spectatori(Prenume,Nume) VALUES ('Alex','Andreescu'); -- 5 --
INSERT INTO Spectatori(Prenume,Nume) VALUES ('Mara','Ionescu'); -- 6 --
INSERT INTO Spectatori(Prenume,Nume) VALUES ('Mihaela','Andronescu'); -- 7 --


-- populare Bilete --
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (2,12,1,1);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (2,13,1,2);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (1,11,2,1);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (1,13,2,4);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (1,14,2,5);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (5,10,3,6);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (1,21,3,7);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (1,20,4,5);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (3,13,4,6);
INSERT INTO Bilete(Randul,Loc,IdS,IdSpectator) VALUES (4,13,6,7);

-- populare Regizori --
INSERT INTO Regizori(Prenume,Nume,StilRegie) VALUES ('Radu','Afrim','experimental, vizual, poetic'); -- 1 --
INSERT INTO Regizori(Prenume,Nume,StilRegie) VALUES ('Gianina','Carbunariu','teatru documentar, social si politic');-- 2 --
INSERT INTO Regizori(Prenume,Nume,StilRegie) VALUES ('Catinca','Draganescu','teatru social contemporan'); -- 3 --
INSERT INTO Regizori(Prenume,Nume,StilRegie) VALUES ('Victor', 'Frunza','psihologic'); -- 4 --

-- populare ParticipareRegie --
INSERT INTO ParticipareRegie(RolRegizor,IdS,IdR) VALUES ('principal',1,1);
INSERT INTO ParticipareRegie(RolRegizor,IdS,IdR) VALUES ('secundar',1,4);
INSERT INTO ParticipareRegie(RolRegizor,IdS,IdR) VALUES ('principal',2,3);
INSERT INTO ParticipareRegie(RolRegizor,IdS,IdR) VALUES ('principal',3,3);
INSERT INTO ParticipareRegie(RolRegizor,IdS,IdR) VALUES ('principal',4,4);
INSERT INTO ParticipareRegie(RolRegizor,IdS,IdR) VALUES ('secundar',4,1);
INSERT INTO ParticipareRegie(RolRegizor,IdS,IdR) VALUES ('principal',5,2);
INSERT INTO ParticipareRegie(RolRegizor,IdS,IdR) VALUES ('principal',6,1);

-- populare Angajati --
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Matei','Basarabeanu','0728351902',1);
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Maria','Pantazi','0728351302',1);
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Matei','Ionescu','0718351902',1);
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Luca','Mateescu','0728951902',2);
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Manuel','Matei','0728351912',2);
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Alexandra','Basarab','0711351902',2);
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Manuela','Matei','0728351909',1);
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Adina','Mihaela','0728350902',2);
INSERT INTO Angajati(Prenume,Nume,Telefon,IdT) VALUES ('Ariana','Diapon','0728350812',2);

/*
DELETE FROM ParticipareRegie;
DELETE FROM Roluri;
DELETE FROM Costume;
DELETE FROM Bilete;
DELETE FROM Angajati;
DELETE FROM Regizori;
DELETE FROM Actori;
DELETE FROM Spectatori;
DELETE FROM Spectacole;
DELETE FROM TeatruNational;


DBCC CHECKIDENT ('TeatruNational', RESEED, 0);
DBCC CHECKIDENT ('Spectacole', RESEED, 0);
DBCC CHECKIDENT ('Actori', RESEED, 0);
DBCC CHECKIDENT ('Roluri', RESEED, 0);
DBCC CHECKIDENT ('Costume', RESEED, 0);
DBCC CHECKIDENT ('Spectatori', RESEED, 0);
DBCC CHECKIDENT ('Bilete', RESEED, 0);
DBCC CHECKIDENT ('Regizori', RESEED, 0);
DBCC CHECKIDENT ('ParticipareRegie', RESEED, 0);
DBCC CHECKIDENT ('Angajati', RESEED, 0);
*/

--select IdA,Nume from Actori


select * from Actori


