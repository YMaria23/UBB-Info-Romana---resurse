CREATE TABLE Versiune
(CurrentVersion INT NOT NULL);

INSERT INTO Versiune VALUES (0);

GO

----------------------------------------------------------------- DO PROCEDURES ---------------------------------------------------------------------

-------------- PROCEDURA 1 ----------------
-- modifica tipul de date al unei coloane: VARCHAR -> NVARCHAR
CREATE PROCEDURE do_vers1
AS BEGIN 
ALTER TABLE TeatruNational
ALTER COLUMN Oras NVARCHAR(100) NOT NULL

UPDATE Versiune SET CurrentVersion = 1;

PRINT 'S-a modificat tipul de date al coloanei Oras din tabelul TeatruNational din VARCHAR in NVARCHAR! Acum sunteti la versiunea 1';
END;

GO

-------------- PROCEDURA 2 -----------------
-- adauga o constrangere de valoare default pt un camp
CREATE PROCEDURE do_vers2
AS BEGIN
ALTER TABLE Spectacole
ADD CONSTRAINT df_descriere DEFAULT 'descriere default' FOR Descriere

UPDATE Versiune SET CurrentVersion = 2;

PRINT 'S-a adaugat constrangerea default df_descriere pentru coloana Descriere a tabelului Spectacole! Acum sunteti la versiunea 2';
END;


GO

-------------- PROCEDURA 3 ---------------------
-- creeaza o tabela noua
CREATE PROCEDURE do_vers3
AS BEGIN
CREATE TABLE SponsoriNationali
(IdSponsori INT PRIMARY KEY IDENTITY,
Denumire VARCHAR(100) NOT NULL,
Contributie INT NOT NULL,
IdT INT);

UPDATE Versiune SET CurrentVersion = 3;

PRINT 'S-a creat tabela SponsoriNationali! Acum sunteti la versiunea 3';
END;

GO

-------------- PROCEDURA 4 ---------------------
-- adauga un camp nou
CREATE PROCEDURE do_vers4
AS BEGIN

ALTER TABLE SponsoriNationali
ADD NumeCEO VARCHAR(100);

UPDATE Versiune SET CurrentVersion = 4;

PRINT 'S-a adaugat campul NumeCEO in tabelul SponsoriNationali! Acum sunteti la versiunea 4';
END;

GO


-------------- PROCEDURA 5 ---------------------
-- creeaza o constrangere de cheie straina
CREATE PROCEDURE do_vers5
AS BEGIN

ALTER TABLE SponsoriNationali
ADD CONSTRAINT fk_distributie_fonduri FOREIGN KEY(IdT) REFERENCES TeatruNational(IdT);

UPDATE Versiune SET CurrentVersion = 5;

PRINT 'S-a creat o constrangere de cheie straina fk_distributie_fonduri in tabelul SponsoriNationali (rel de one to many cu TeatruNational)! Acum sunteti la versiunea 5';
END;

GO

----------------------------------------------------------------- UNDO PROCEDURES ---------------------------------------------------------------------
-------------- UNDO PROCEDURA 1 ----------------
-- modifica inapoi tipul de date al unei coloane din NVARCHAR in VARCHAR
CREATE PROCEDURE undo_vers1
AS BEGIN 
ALTER TABLE TeatruNational
ALTER COLUMN Oras VARCHAR(100) NOT NULL

UPDATE Versiune SET CurrentVersion = 0;

PRINT 'S-a modificat inapoi tipul de date al coloanei Oras din tabelul TeatruNational din NVARCHAR in VARCHAR! Acum sunteti la versiunea 0';
END;

GO

-------------- UNDO PROCEDURA 2 ----------------
-- elimina o constrangere de valoare default pt un camp (cea din campul Descriere al tabelului Spectacole) 
CREATE PROCEDURE undo_vers2
AS BEGIN 

ALTER TABLE Spectacole
DROP CONSTRAINT df_descriere

UPDATE Versiune SET CurrentVersion = 1;

PRINT 'S-a eliminat constrangerea de valoare default df_descriere de pe campul Descriere al tabelului Spectacole! Acum sunteti la versiunea 1';
END;

GO

-------------- UNDO PROCEDURA 3 ---------------------
-- elimina tabela SponsoriNationali
CREATE PROCEDURE undo_vers3
AS BEGIN
DROP TABLE SponsoriNationali;

UPDATE Versiune SET CurrentVersion = 2;

PRINT 'S-a eliminat tabela SponsoriNationali! Acum sunteti la versiunea 2';
END;

GO


-------------- UNDO PROCEDURA 4 ---------------------
-- elimina campul NumeCEO din tabelul SponsoriNationali
CREATE PROCEDURE undo_vers4
AS BEGIN

ALTER TABLE SponsoriNationali
DROP COLUMN NumeCEO;

UPDATE Versiune SET CurrentVersion = 3;

PRINT 'S-a eliminat campul NumeCEO din tabelul SponsoriNationali! Acum sunteti la versiunea 3';
END;

GO

-------------- UNDO PROCEDURA 5 ---------------------
-- elimina constrangerea de cheie straina fk_distributie_fonduri
CREATE PROCEDURE undo_vers5
AS BEGIN

ALTER TABLE SponsoriNationali
DROP CONSTRAINT fk_distributie_fonduri;

UPDATE Versiune SET CurrentVersion = 4;

PRINT 'S-a eliminat constrangerea de cheie straina fk_distributie_fonduri din tabelul SponsoriNationali! Acum sunteti la versiunea 4';
END;

GO



--------------------------------------------------- PROCEDURA MAIN --------------------------------------------------
CREATE PROCEDURE main @versiuneDorita INT
AS BEGIN

IF ( @versiuneDorita > 5 OR @versiuneDorita < 0 )
BEGIN
	RAISERROR ('Versiunea ceruta nu este una valida! Versiuni valide: 0,1,2,3,4,5',16,1);
	RETURN;
END;

ELSE
BEGIN
	DECLARE @versiuneCurenta INT;
	SET @versiuneCurenta = (SELECT CurrentVersion FROM Versiune);

	IF @versiuneCurenta = @versiuneDorita
		PRINT 'Deja va aflati la versiunea ' + CAST(@versiuneCurenta AS VARCHAR)
	
	WHILE @versiuneCurenta < @versiuneDorita
	BEGIN
		PRINT 'Suntem la versiunea ' + CAST(@versiuneCurenta AS VARCHAR) + ' si inaintam catre urmatoarea';

		IF @versiuneCurenta = 0
			EXEC do_vers1;
		ELSE IF @versiuneCurenta = 1
			EXEC do_vers2
		ELSE IF @versiuneCurenta = 2
			EXEC do_vers3
		ELSE IF @versiuneCurenta = 3
			EXEC do_vers4
		ELSE IF @versiuneCurenta = 4
			EXEC do_vers5

		SET @versiuneCurenta = @versiuneCurenta + 1;
	END

	WHILE @versiuneCurenta > @versiuneDorita
	BEGIN
		PRINT 'Suntem la versiunea ' + CAST(@versiuneCurenta AS VARCHAR) + ' si mergem catre antecedenta';

		IF @versiuneCurenta = 1
			EXEC undo_vers1;
		ELSE IF @versiuneCurenta = 2
			EXEC undo_vers2
		ELSE IF @versiuneCurenta = 3
			EXEC undo_vers3
		ELSE IF @versiuneCurenta = 4
			EXEC undo_vers4
		ELSE IF @versiuneCurenta = 5
			EXEC undo_vers5

		SET @versiuneCurenta = @versiuneCurenta - 1;
	END
	
	PRINT 'Sunteti la versiunea dorita!'
END;

END;
GO

EXEC main 1;
