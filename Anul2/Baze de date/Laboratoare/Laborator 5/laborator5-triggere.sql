

------------------------------------------------------- TABELUL SPECTACOLE ----------------------------------------------------------------

-- crearea tabelului de logging --

CREATE TABLE Logging_Spectacole(
IdL INT PRIMARY KEY IDENTITY,
TableName VARCHAR(250),
LIdS INT,
LDenumire VARCHAR(250),
LDescriere VARCHAR(250),
LIdT INT,
OperationType VARCHAR(50) check (OperationType IN ('UPDATE','DELETE')),
AffectedRows INT,
ExecutionDate datetime,
SLogin NVARCHAR(128));

GO
-- trigger DELETE --
CREATE TRIGGER delete_trigger_spectacole
ON Spectacole
FOR DELETE
AS BEGIN
	-- implementare propriu-zisa --
	INSERT INTO Logging_Spectacole(TableName,LIdS,LDenumire,LDescriere,LIdT,OperationType,AffectedRows,ExecutionDate,SLogin)
	SELECT 'Spectacole',deleted.IdS,deleted.Denumire,deleted.Descriere,deleted.IdT,'DELETE', @@ROWCOUNT,GETDATE(),SUSER_NAME()
	FROM deleted
END
GO

-- trigger UPDATE --
CREATE TRIGGER update_trigger_spectacole
ON Spectacole
FOR UPDATE
AS BEGIN
	-- implementare propriu-zisa --
	INSERT INTO Logging_Spectacole(TableName,LIdS,LDenumire,LDescriere,LIdT,OperationType,AffectedRows,ExecutionDate,SLogin)
	SELECT 'Spectacole',deleted.IdS,deleted.Denumire,deleted.Descriere,deleted.IdT,'UPDATE', @@ROWCOUNT,GETDATE(),SUSER_NAME()
	FROM deleted
END

select * from Logging_Spectacole


GO

------------------------------------------------------- TABELUL ACTORI ----------------------------------------------------------------

-- crearea tabelului de logging --

CREATE TABLE Logging_Actori(
IdL INT PRIMARY KEY IDENTITY,
TableName VARCHAR(250),
LIdA INT,
LNume VARCHAR(250),
LPrenume VARCHAR(250),
LSpecializare VARCHAR(250),
LBiografie VARCHAR(250),
OperationType VARCHAR(50) check (OperationType IN ('UPDATE','DELETE')),
AffectedRows INT,
ExecutionDate datetime,
SLogin NVARCHAR(128));

GO
-- trigger DELETE --
CREATE TRIGGER delete_trigger_actori
ON Actori
FOR DELETE
AS BEGIN
	-- implementare propriu-zisa --
	INSERT INTO Logging_Actori(TableName,LIdA,LNume,LPrenume,LSpecializare,LBiografie,OperationType,AffectedRows,ExecutionDate,SLogin)
	SELECT 'Actori',deleted.IdA,deleted.Nume,deleted.Prenume,deleted.Specializare,deleted.Biografie,'DELETE', @@ROWCOUNT,GETDATE(),SUSER_NAME()
	FROM deleted
END
GO

-- trigger UPDATE --
CREATE TRIGGER update_trigger_actori
ON Actori
FOR UPDATE
AS BEGIN
	-- implementare propriu-zisa --
	INSERT INTO Logging_Actori(TableName,LIdA,LNume,LPrenume,LSpecializare,LBiografie,OperationType,AffectedRows,ExecutionDate,SLogin)
	SELECT 'Actori',deleted.IdA,deleted.Nume,deleted.Prenume,deleted.Specializare,deleted.Biografie,'UPDATE', @@ROWCOUNT,GETDATE(),SUSER_NAME()
	FROM deleted
END
GO

select * from Logging_Actori





------------------------------------------------------- TABELUL ROLURI ----------------------------------------------------------------

-- crearea tabelului de logging --

CREATE TABLE Logging_Roluri(
IdL INT PRIMARY KEY IDENTITY,
TableName VARCHAR(250),
LIdS INT,
LIdA INT,
LDenumire VARCHAR(250),
OperationType VARCHAR(50) check (OperationType IN ('UPDATE','DELETE')),
AffectedRows INT,
ExecutionDate datetime,
SLogin NVARCHAR(128));

GO
-- trigger DELETE --
CREATE TRIGGER delete_trigger_roluri
ON Roluri
FOR DELETE
AS BEGIN
	-- implementare propriu-zisa --
	INSERT INTO Logging_Roluri(TableName,LIdS,LIdA,LDenumire,OperationType,AffectedRows,ExecutionDate,SLogin)
	SELECT 'Roluri',deleted.IdS,deleted.IdA,deleted.Denumire,'DELETE', @@ROWCOUNT,GETDATE(),SUSER_NAME()
	FROM deleted
END
GO

-- trigger UPDATE --
CREATE TRIGGER update_trigger_roluri
ON Roluri
FOR UPDATE
AS BEGIN
	-- implementare propriu-zisa --
	INSERT INTO Logging_Roluri(TableName,LIdS,LIdA,LDenumire,OperationType,AffectedRows,ExecutionDate,SLogin)
	SELECT 'Roluri',deleted.IdS,deleted.IdA,deleted.Denumire,'UPDATE', @@ROWCOUNT,GETDATE(),SUSER_NAME()
	FROM deleted
END
GO


select * from Logging_Roluri