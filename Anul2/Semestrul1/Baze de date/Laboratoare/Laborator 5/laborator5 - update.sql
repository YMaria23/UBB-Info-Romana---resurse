---------------- UPDATE PROCEDURES ------------------
-- Spectacole -> se face update dupa Denumire si se modifica Descrierea
CREATE PROCEDURE update_spectacole @tableName varchar(250), @denumire varchar(250), @descriere varchar(250), @flag bit OUTPUT
as begin
	-- validare: Denumire & Descriere not null
	declare @n int
	set @n = dbo.validare_string(@descriere)
	set @n += dbo.validare_string(@denumire)

	IF(@n = 0)
	BEGIN
		-- implementarea propriu-zisa
		UPDATE Spectacole
		SET Descriere = @descriere
		WHERE Denumire = @denumire

		print('UPDATE operation for table ' + @tableName);
		set @flag = 1
	END
	ELSE
	BEGIN
		print('Denumirea si Descrierea nu trebuie sa fie NULL!')
		set @flag = 0
	END
end
go

drop procedure update_spectacole

-- exemple --
declare @flag bit
EXEC update_spectacole 'Spectacole', 'Moara cu noroc', 'recomandare liceeni -> opera de BAC', @flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- denumire null
declare @flag bit
EXEC update_spectacole 'Spectacole', '', 'recomandare liceeni',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- descriere null
declare @flag bit
EXEC update_spectacole 'Spectacole', 'Moara cu noroc', '',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

select * from Spectacole

GO
-- Actori -> se face update dupa Nume, Prenume si se modifica Biografia
CREATE PROCEDURE update_actori @tableName varchar(250), @nume varchar(250), @prenume varchar(250), @biografie varchar(250), @flag bit OUTPUT
as begin
	-- validare: Nume, Prenume si Biografie nu trebuie sa fie null
	declare @n int
	set @n = dbo.validare_string(@nume)
	set @n += dbo.validare_string(@prenume)
	set @n += dbo.validare_string(@biografie)

	IF(@n = 0)
	BEGIN
		-- implementare propriu-zisa
		UPDATE Actori
		SET Biografie = @biografie
		WHERE Nume = @nume and Prenume = @prenume

		print('UPDATE operation for table ' + @tableName);
		set @flag = 1
	END
	ELSE
	BEGIN
		print('Numele, Prenumele si Biografia trebuie sa fie string-uri nenule!')
		set @flag = 0
	END
end

GO

-- exemple --
declare @flag bit
EXEC update_actori 'Actori', 'Valsan', 'Ana', 'descriere',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- nume null
declare @flag bit
EXEC update_actori 'Actori', '', 'Ana', 'altceva',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- prenume null
declare @flag bit
EXEC update_actori 'Actori', 'Valsan', '', 'altceva',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- biografie null
declare @flag bit
EXEC update_actori 'Actori', 'Valsan', 'Ana', '',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

select * from Actori


GO
-- Roluri -> se face dupa duoul de FK -> se modifica Denumirea
CREATE PROCEDURE update_roluri @tableName varchar(250), @ids int, @ida int, @denumire varchar(250), @flag bit OUTPUT
as begin
	-- validare: pt cele 2 FK + Denumirea nu trebuie sa fie null
	declare @n int
	set @n = dbo.validare_string(@denumire)

	IF(@n = 0)
	BEGIN
		declare @n1 int
		set @n = dbo.validare_fk_roluri_spectacole(@ids)
		set @n1 = dbo.validare_fk_roluri_actori(@ida)

		IF(@n >0 and @n1 > 0)
		BEGIN
			UPDATE Roluri
			SET Denumire = @denumire
			WHERE IdS = @ids and IdA = @ida

			print('UPDATE operation for table ' + @tableName);
			set @flag = 1
		END
		ELSE
		BEGIN
			print('Eroare la FK')
			set @flag = 0
		END
	END
	ELSE
	BEGIN
		print('Denumirea nu trebuie sa fie null!')
		set @flag = 0
	END
end

GO

-- exemple --
declare @flag bit
EXEC update_roluri 'Roluri', 4,1,'Secundar',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- eroare la FK
declare @flag bit
EXEC update_roluri 'Roluri', 123,1,'SECUNDAR',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- eroare la denumire
declare @flag bit
EXEC update_roluri 'Roluri', 4,1,'',@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

select * from Roluri

GO