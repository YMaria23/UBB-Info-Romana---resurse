---------------- SELECT PROCEDURES ------------------
-- Spectacole
CREATE PROCEDURE select_spectacole @tableName varchar(250)
as
begin
	select * from Spectacole
	print 'SELECT operation for table ' + @tableName
end

EXEC select_spectacole 'Spectacole'
GO

-- Actori
CREATE PROCEDURE select_actori @tableName varchar(250)
as
begin
	select * from Actori
	print 'SELECT operation for table ' + @tableName
end

EXEC select_actori 'Actori'
GO

-- Roluri
CREATE PROCEDURE select_roluri @tableName varchar(250)
as
begin
	select * from Roluri
	print 'SELECT operation for table ' + @tableName
end

EXEC select_roluri 'Roluri'
GO


---------------- INSERT PROCEDURES ------------------
-- Spectacole
CREATE PROCEDURE insert_spectacole @tableName varchar(250), @denumire varchar(250), @descriere varchar(250),@idt INT, @nrRows INT
as begin
	-- validare pt: 1.denumire != null, 2.FK --
	declare @n int
	set @n = dbo.validare_string(@denumire)

	IF( @n = 0)
	BEGIN
		set @n = dbo.validare_fk_spectacole_teatru(@idt)
		IF(@n > 0)
		BEGIN
			-- de aici incepe rezolvarea propriu-zisa --
			declare @nr int = 1

			WHILE @nr <= @nrRows
			BEGIN
				insert into Spectacole(Denumire,Descriere,IdT) values (@denumire,@descriere,@idt)
				set @nr = @nr + 1
			END

			set @nr = @nr-1
			print('INSERT operation for table '+ @tableName + ' -> rows affected: '+ CAST(@nr AS varchar(10)));
		END
		ELSE
		BEGIN
			print('Nu exista leg de FK')
		END
	END
	ELSE
	BEGIN
		print('Denumirea trebuie sa nu fie null');
	END
end

-- exemple --
-- problema la FK
EXEC insert_spectacole 'Spectacole','Lapusneanul','recomandare liceeni',20,1
-- nu are denumire
EXEC insert_spectacole 'Spectacole','','recomandare liceeni',20,1
EXEC insert_spectacole 'Spectacole','Punguta cu doi bani','',2,2

select * from spectacole

GO

-- Actori
CREATE PROCEDURE insert_actori @tableName varchar(250), @nume varchar(250), @prenume varchar(250),@specializare varchar(250), @biografie varchar(250), @flag bit OUTPUT
as begin
	-- validare pt: 1.nume != null, prenume != null, specializare != null
	declare @n int
	set @n = dbo.validare_string(@nume)
	set @n += dbo.validare_string(@prenume)
	set @n += dbo.validare_string(@specializare)

	IF( @n = 0)
	BEGIN
		-- de aici incepe rezolvarea propriu-zisa --
		insert into Actori(Nume,Prenume,Specializare,Biografie) values (@nume,@prenume,@specializare,@biografie)
		print('INSERT operation for table '+ @tableName);
		set @flag = 1
	END
	ELSE
	BEGIN
		print('Numele,Prenumele si Specializare trebuie sa nu fie null');
		set @flag = 0
	END
end
go

-- exemple --
declare @flag bit
EXEC insert_actori 'Actori','Valsan','Amalia','drama',NULL,@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- nu are specializare
declare @flag bit
EXEC insert_actori 'Actori','Valsan','Maria','','',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- nu are nume
declare @flag bit
EXEC insert_actori 'Actori','','Maria','drama','',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- nu are prenume
declare @flag bit
EXEC insert_actori 'Actori','Valsan','','drama','',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

select * from Actori

GO


-- Roluri
CREATE PROCEDURE insert_roluri @tableName varchar(250), @ids int, @ida int, @denumire varchar(250)
as begin
	-- validari: 2 validari pt FK + validare ca denumirea sa nu fie null + validare ca nu mai exista aceeasi inregistrare (PK)
	declare @n1 int
	set @n1 = dbo.validare_string(@denumire)

	IF(@n1 = 0)
	BEGIN
		declare @n2 int
		set @n1 = dbo.validare_fk_roluri_spectacole(@ids)
		set @n2 = dbo.validare_fk_roluri_actori(@ida)

		IF(@n1 > 0 and @n2 > 0)
		BEGIN
			set @n1 = dbo.validare_pk_roluri(@ids,@ida)

			IF(@n1 = 0)
			BEGIN
				-- implementare propriu-zisa --
				insert into Roluri(IdS,IdA,Denumire) values (@ids,@ida,@denumire)
				print('INSERT operation for table ' + @tableName);
			END
			ELSE
			BEGIN
				print('Exista deja o inregistrare la fel!')
			END
		END
		ELSE
		BEGIN
			print('Eroare la FK!')
		END
	END
	ELSE
	BEGIN
		print('Denumirea trebuie sa nu fie nula!')
	END
end

-- exemple 
-- exista deja inregistrarea
EXEC insert_roluri 'Roluri',4,1,'PRINCIPAL'
-- nu exista FK
EXEC insert_roluri 'Roluri',4,10,'PRINCIPAL'
-- nu exista FK
EXEC insert_roluri 'Roluri',11,10,'PRINCIPAL'
EXEC insert_roluri 'Roluri',2,7, 'secundar'

select * from Roluri
select * from Actori

GO




---- SPECTACOLE --

EXEC select_spectacole 'Spectacole'
GO
EXEC insert_spectacole 'Spectacole','Punguta cu doi bani','',2,2

declare @flag bit
EXEC update_spectacole 'Spectacole', 'Moara cu noroc', 'recomandare liceeni -> opera de BAC', @flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

--declare @flag bit
EXEC delete_spectacole 'Spectacole', 12, @flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')




---- ACTORI ----
EXEC select_actori 'Actori'
GO

declare @flag bit
EXEC insert_actori 'Actori','Valsan','Amalia','drama',NULL,@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

declare @flag bit
EXEC update_actori 'Actori', 'Valsan', 'Ana', 'descriere',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')


declare @flag bit
EXEC delete_actori 'Actori', 8, @flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')


---- ROLURI ---
EXEC select_roluri 'Roluri'
GO

EXEC insert_roluri 'Roluri',2,7, 'secundar'

declare @flag bit
EXEC update_roluri 'Roluri', 4,1,'Secundar',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

declare @flag bit
EXEC delete_roluri 'Roluri', 2,1,@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

