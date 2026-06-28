---------------- DELETE PROCEDURES ------------------
-- Roluri 
CREATE PROCEDURE delete_roluri @tableName varchar(250), @ids int, @ida int, @flag bit OUTPUT
as begin
	-- validare FK
	declare @n1 int
	set @n1 = dbo.validare_fk_roluri_spectacole(@ids)

	declare @n2 int
	set @n2 = dbo.validare_fk_roluri_actori(@ida)

	IF(@n1 > 0 and @n2 > 0)
	BEGIN
		-- implementare propriu-zisa
		DELETE FROM Roluri
		WHERE IdS = @ids and IdA = @ida

		print('DELETE operation for table ' + @tableName);
		set @flag = 1
	END
	ELSE
	BEGIN
		print('Eroare la FK!')
		set @flag = 0
	END
end
GO

-- exemple --
declare @flag bit
EXEC delete_roluri 'Roluri', 2,1,@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- eroare la FK
declare @flag bit
EXEC delete_roluri 'Roluri', 122,1,@flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

select * from Roluri
GO


-- Spectacole 
CREATE PROCEDURE delete_spectacole @tableName varchar(250), @id int,@flag bit OUTPUT
as begin
	 -- validare: daca exista respectiva cheie in tabel
	 declare @n int
	 set @n = dbo.validare_pk_spectacole(@id)

	 IF(@n > 0)
	 BEGIN
		-- implementare propriu-zisa --
		declare @ida int

		-- se utilizeaza un cursor pentru parcurgerea selectului
		DECLARE cursor_ida CURSOR LOCAL FAST_FORWARD FOR
			SELECT IdA FROM Roluri 
			WHERE IdS = @id
		OPEN cursor_ida
		FETCH NEXT FROM cursor_ida INTO @ida
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC delete_roluri 'Roluri',@id,@ida,@flag OUTPUT
			FETCH NEXT FROM cursor_ida INTO @ida
		END
		CLOSE cursor_ida
		DEALLOCATE cursor_ida

		DELETE FROM Spectacole
		WHERE IdS = @id

		print('DELETE operation for table ' + @tableName);
		set @flag = 1
	 END
	 ELSE
	 BEGIN
		print('Eroare la PK')
		set @flag = 0
	 END
end
GO

-- exemple --
declare @flag bit
EXEC delete_spectacole 'Spectacole', 12, @flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- nu exista PK 
declare @flag bit
EXEC delete_spectacole 'Spectacole', 111, @flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

select * from Spectacole
select * from Roluri

GO


-- Actori
CREATE PROCEDURE delete_actori @tableName varchar(250), @id int,@flag bit OUTPUT
as begin
	 -- validare: daca exista respectiva cheie in tabel
	 declare @n int
	 set @n = dbo.validare_pk_actori(@id)

	 IF(@n > 0)
	 BEGIN
		-- implementare propriu-zisa --
		declare @ids int

		-- se utilizeaza un cursor pentru parcurgerea selectului
		DECLARE cursor_ids CURSOR LOCAL FAST_FORWARD FOR
			SELECT IdS FROM Roluri 
			WHERE IdA = @id
		OPEN cursor_ids
		FETCH NEXT FROM cursor_ids INTO @ids
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC delete_roluri 'Roluri',@ids,@id,@flag OUTPUT
			FETCH NEXT FROM cursor_ids INTO @ids
		END
		CLOSE cursor_ids
		DEALLOCATE cursor_ids

		DELETE FROM Actori
		WHERE IdA = @id

		print('DELETE operation for table ' + @tableName);
		set @flag = 1
	 END
	 ELSE
	 BEGIN
		print('Eroare la PK')
		set @flag = 0
	 END
end
GO

-- exemple --
declare @flag bit
EXEC delete_actori 'Actori', 8, @flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

-- nu exista PK
declare @flag bit
EXEC delete_actori 'Actori', 91, @flag OUTPUT
if @flag = 1 print('Successful operation!')
else print('Stopped execution')

select * from Actori
select * from Roluri
