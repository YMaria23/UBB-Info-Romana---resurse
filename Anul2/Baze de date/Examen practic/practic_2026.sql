create database PapuciData
go

use PapuciData
go

create table TipuriPapuci(
IdT int primary key identity,
Denumire varchar(250) not null,
Sezon varchar(250),
Descriere varchar(250),
constraint ck_sezon check(Sezon in ('Vara','Toamna','Primavara','Iarna','All season'))
)

create table Branduri(
IdB int primary key identity,
Nume varchar(250) not null,
Logo varchar(250),
Stil varchar(250),
Culori varchar(250),
Preferinte varchar(250),
AnFondare int,
Tara varchar(250),
constraint ck_an check(AnFondare between 1500 and 2026)
)

create table Papuci(
IdP int primary key identity,
Denumire varchar(250) not null,
Model varchar(250),
Culoare varchar(250),
Pret real,
IdT int foreign key references TipuriPapuci(IdT),
IdB int foreign key references Branduri(IdB))

create table Copii(
IdC int primary key identity,
Nume varchar(250),
Prenume varchar(250),
Gen varchar(1),
DataNastere date,
constraint ck_gen check(Gen in ('M','F'))
)

create table Comenzi(
IdC int foreign key references Copii(IdC),
IdP int foreign key references Papuci(IdP),
DataAchizitie date,
Disponibilitate varchar(2),
constraint pk_comenzi primary key (IdC,IdP),
constraint ck_disponibilitate check (Disponibilitate in ('da','nu'))
)



-- populare baza
insert into TipuriPapuci(Denumire,Sezon) values ('de casa','Iarna')
insert into TipuriPapuci(Denumire,Sezon) values ('pufosi','Primavara')
insert into TipuriPapuci(Denumire,Sezon) values ('de plaja','Vara')

insert into Branduri(Nume,Logo,Culori,AnFondare,Tara) values ('Zara','Z','alb, negru',2000,'Spania')
insert into Branduri(Nume,Logo,Culori,AnFondare,Tara) values ('H&M','H','alb, rosu',2010,'Germania')


insert into Papuci(Denumire,Culoare,Pret,IdT,IdB) values ('Garal','roz',30.5,2,1)
insert into Papuci(Denumire,Culoare,Pret,IdT,IdB) values ('Jarameia','albastru',15.7,3,2)
insert into Papuci(Denumire,Culoare,Pret,IdT,IdB) values ('Ionic','galben',20.9,3,1)

insert into Copii(Nume,Prenume,Gen,DataNastere) values ('Ionescu','Ana','F','2000-01-09')
insert into Copii(Nume,Prenume,Gen,DataNastere) values ('Ionescu','Maria','F','2010-02-11')
insert into Copii(Nume,Prenume,Gen,DataNastere) values ('Parter','Alexandru','M','2012-11-09')

insert into Comenzi(IdC,IdP,DataAchizitie,Disponibilitate) values (1,1,'2026-01-14','da')
insert into Comenzi(IdC,IdP,DataAchizitie,Disponibilitate) values (1,3,'2026-05-14','da')
insert into Comenzi(IdC,IdP,DataAchizitie,Disponibilitate) values (2,1,'2026-01-13','da')
insert into Comenzi(IdC,IdP,DataAchizitie,Disponibilitate) values (3,2,'2026-01-14','da')

select * from Comenzi
go

create procedure adaugaPapuci @idCopil int, @idPapuci int, @Data date, @Disponibilitate varchar(2)
as begin
	declare @count int

	select @count = count(*) from Comenzi
	where IdC = @idCopil and IdP = @idPapuci

	if @count = 0
	begin
		-- inseamna ca nu exista si trebuie adaugat
		insert into Comenzi(IdC,IdP,DataAchizitie,Disponibilitate) values (@idCopil,@idPapuci,@Data,@Disponibilitate)
		print('S-a adaugat o pereche de papuci! (inregistrare adaugata)')
	end
	else
	begin
		-- inseamna ca exista inregistrarea => trebuie doar actualizata
		update Comenzi
		set DataAchizitie = @Data, Disponibilitate = @Disponibilitate
		where IdC = @idCopil and IdP = @idPapuci

		print('S-a actualizat o inregistrare!')
	end
end

exec adaugaPapuci 2,1,'2026-02-07','da'
exec adaugaPapuci 2,2,'2026-01-23','da'

delete from Comenzi where IdP = 2 and IdC = 2
go

create view denumirePapuciNepopulari as
	select P.Denumire from Papuci P
	inner join Comenzi CO on CO.IdP = P.IdP
	inner join Copii C on C.IdC = CO.IdC
	group by P.IdP,P.Denumire having count(CO.IdC) = (select min(Y.NrPapuci) 
														from( 
														select count(R1.IdP) as NrPapuci from Comenzi R1
														group by R1.IdC) Y)

go

select * from denumirePapuciNepopulari
