-- interogari --
USE Teatru2
GO

-- se adauga indecsi pt optimizarea interogarilor --
CREATE INDEX index_nume_asc_pren_asc ON Actori (Nume ASC, Prenume ASC);
CREATE INDEX index_specializare_asc_full_nume_asc ON Actori (Specializare ASC,Nume ASC, Prenume ASC);
CREATE INDEX index_den_asc ON Spectacole (Denumire ASC);

-- 1.afiseaza numele spectacolelor care au cel putin un spectator --
SELECT Denumire FROM Spectacole 
WHERE IdS IN (SELECT IdS FROM Bilete WHERE IdSpectator IN (SELECT IdSpectator FROM Spectatori));

-- 2.afiseaza numele spectacolului si nr de actori (pt spectacolele care au cel putin 2 actori) --
SELECT S.Denumire, COUNT(A.IdA) nr_actori FROM Spectacole S INNER JOIN Roluri R ON S.IdS = R.IdS 
INNER JOIN Actori A ON R.IdA = A.IdA 
GROUP BY S.IdS, S.Denumire HAVING COUNT(A.IdA) >= 2;

-- 3.afiseaza numele si prenumele actorilor si nr spectacolelor in care joaca, daca acesta este mai mare sau egal cu 2 --
SELECT A.Nume, A.Prenume, COUNT(S.IdS) Nr_spectacole FROM Actori A INNER JOIN Roluri R ON A.IdA = R.IdA
INNER JOIN Spectacole S ON R.IdS = S.IdS
GROUP BY A.IdA, A.Nume,A.Prenume HAVING COUNT(S.IdS) >= 2;

-- 4.afiseaza numele spectacolului si stilul/specializarea + nume si prenume regizorului principal --
SELECT S.Denumire, R.Nume,R.Prenume,R.StilRegie FROM Spectacole S INNER JOIN ParticipareRegie P ON S.IdS = P.IdS
INNER JOIN Regizori R ON P.IdR = R.IdR WHERE P.RolRegizor = 'principal';

-- 5.afiseaza numele + prenumele actorilor (specializati pe drama/versatil) si ale regizorilor care lucreaza impreuna (relatii "neinrudite" => trebuie distinct) --
SELECT DISTINCT A.Nume, A.Prenume, R.Nume, R.Prenume FROM Spectacole S INNER JOIN Roluri Rol ON S.IdS=Rol.IdS
INNER JOIN Actori A ON Rol.IdA = A.IdA
INNER JOIN ParticipareRegie P ON S.IdS = P.IdS
INNER JOIN Regizori R ON P.IdR = R.IdR
WHERE A.Specializare LIKE '%drama%' OR A.Specializare='versatil';

-- 6.afiseaza numele + prenumele actorilor (specializati pe drama), costumele si spectacolele la care acestia participa --
SELECT A.Nume, A.Prenume, C.Descriere, S.Denumire FROM Actori A INNER JOIN Costume C ON A.IdA=C.IdA
INNER JOIN Roluri R ON A.IdA=R.IdA
INNER JOIN Spectacole S ON S.IdS = R.IdS
WHERE A.Specializare LIKE '%drama%';

-- 7.afiseaza denumirea teatrului, nr de angajati si nr de spectacole --
SELECT T.Denumire,
(SELECT COUNT(*) FROM Spectacole S WHERE S.IdT = T.IdT) AS Numar_Spectacole,
(SELECT COUNT(*) FROM Angajati A WHERE A.IdT = T.IdT) As Numar_Angajati
FROM TeatruNational T;

-- 8.afiseaza descrierea costumelor si rolurile actorilor (nume+prenume) (fie ca au costume, fie ca nu) care se specializeaza pe comedie --
SELECT A.Nume, A.Prenume, C.Descriere AS Descriere_Costume, R.Denumire AS Rol FROM Actori A 
LEFT JOIN Costume C ON A.IdA = C.IdA
LEFT JOIN Roluri R ON A.IdA = R.IdA
WHERE A.Specializare = 'drama';

-- 9.afiseaza numele + prenumele spectatorilor si nr de spectacole la care iau parte --
SELECT Spec.Nume, Spec.Prenume, COUNT(S.IdS) Numar_Spectacole FROM Spectatori Spec LEFT JOIN Bilete B ON Spec.IdSpectator = B.IdSpectator
LEFT JOIN Spectacole S ON S.IdS = B.IdS
GROUP BY Spec.IdSpectator,Spec.Nume, Spec.Prenume;

-- 10.afiseaza specializarile distincte ale actorilor --
SELECT DISTINCT Specializare FROM Actori
