CREATE DATABASE Teatru2
GO

USE Teatru2;
GO

-- 1 --
-- relatie de one to many cu Spectacole - 2
-- relatie de one to many cu Angajati - 10
CREATE TABLE TeatruNational
(IdT INT PRIMARY KEY IDENTITY,
Denumire VARCHAR(100) NOT NULL UNIQUE,
Oras VARCHAR(100) NOT NULL,
AnInfiintare INT NOT NULL
)

-- 2 --
-- relatie de one to many cu Teatru - 1
-- relatie de many to many cu Actori - 3
-- relatie de many to many cu Spectatori - 6 
-- relatie de many to many cu Regizori - 8
CREATE TABLE Spectacole
(IdS INT PRIMARY KEY IDENTITY,
Denumire VARCHAR(100) NOT NULL,
Descriere VARCHAR(100),
IdT INT FOREIGN KEY REFERENCES TeatruNational(IdT)
)

-- 3 -- 
-- relatie de many to many cu Spectacole - 2
-- relatie de one to many cu Costume - 5
CREATE TABLE Actori
(IdA INT PRIMARY KEY IDENTITY,
Nume VARCHAR(100) NOT NULL,
Prenume VARCHAR(100) NOT NULL,
Specializare VARCHAR(100),
Biografie VARCHAR(300)
)

-- 4 -- 
--relatie intre 2 si 3 (Spectacole si Actori)
CREATE TABLE Roluri
(IdS INT FOREIGN KEY REFERENCES Spectacole(IdS),
IdA INT FOREIGN KEY REFERENCES Actori(IdA),
Denumire VARCHAR(300) NOT NULL,
CONSTRAINT pk_Roluri PRIMARY KEY (IdS,IdA)
)

-- 5 --
-- relatie de one to many cu Actori - 3
CREATE TABLE Costume
(IdC INT PRIMARY KEY IDENTITY,
Descriere VARCHAR(300),
IdA INT FOREIGN KEY REFERENCES Actori(IdA)
)

-- 6 --
-- relatie de many to many cu Spectacole - 2
CREATE TABLE Spectatori
(IdSpectator INT PRIMARY KEY IDENTITY,
Nume VARCHAR(100) NOT NULL,
Prenume VARCHAR(100) NOT NULL
)

-- 7 --
-- legatura intre 6 si 2 (Spectatori si Spectacole)
CREATE TABLE Bilete
(IdS INT FOREIGN KEY REFERENCES Spectacole(IdS),
IdSpectator INT FOREIGN KEY REFERENCES Spectatori(IdSpectator),
Randul INT NOT NULL,
Loc INT NOT NULL,
CONSTRAINT uq_RandSiLoc UNIQUE(Randul,Loc),
CONSTRAINT pk_Bilete PRIMARY KEY(Ids,IdSpectator)
)

-- 8 --
-- relatie de many to many cu Spectacole - 2
CREATE TABLE Regizori
(IdR INT PRIMARY KEY IDENTITY,
Nume VARCHAR(100) NOT NULL,
Prenume VARCHAR(100) NOT NULL,
StilRegie VARCHAR(100),
Premii VARCHAR(200)
)

-- 9 --
-- legatura dintre 2 si 8 (Spectacole si Regizori)
CREATE TABLE ParticipareRegie
(IdS INT FOREIGN KEY REFERENCES Spectacole(IdS),
IdR INT FOREIGN KEY REFERENCES Regizori(IdR),
RolRegizor VARCHAR(300) NOT NULL,
CONSTRAINT pk_ParticipareRegie PRIMARY KEY(IdS,IdR)
)

-- 10 --
-- relatie de one to many cu Teatru - 1
CREATE TABLE Angajati
(IdAngajat INT PRIMARY KEY IDENTITY,
IdT INT FOREIGN KEY REFERENCES TeatruNational(IdT),
Nume VARCHAR(100) NOT NULL,
Prenume VARCHAR(100) NOT NULL,
Telefon VARCHAR(100) NOT NULL,
CONSTRAINT chk_telefon CHECK(telefon LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
)


