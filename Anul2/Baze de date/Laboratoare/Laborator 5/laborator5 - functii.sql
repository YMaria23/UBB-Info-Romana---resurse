---------------- FUNCTII DE VALIDARE ------------------
-- pt string not null
CREATE FUNCTION validare_string(@param varchar(250))
returns int
as begin
	declare @r int = 0
	if( @param is null or @param = '')
		set @r = 1
	return @r
end
GO

declare @n int
set @n = dbo.validare_string('ceva')
select @n

GO

-- pt FK (relatia Spectacole-Teatru)
CREATE FUNCTION validare_fk_spectacole_teatru(@valoare_fk int)
returns int
as begin
	declare @count int;
	SELECT @count = COUNT(*) FROM dbo.TeatruNational WHERE IdT = @valoare_fk
	return @count
end
GO

declare @n int
set @n = dbo.validare_fk_spectacole_teatru(2)
select @n

GO

-- pt FK (relatia Roluri-Spectacole)
CREATE FUNCTION validare_fk_roluri_spectacole(@valoare_fk int)
returns int
as begin
	declare @count int;
	SELECT @count = COUNT(*) FROM dbo.Spectacole WHERE IdS = @valoare_fk
	return @count
end
GO

declare @n int
set @n = dbo.validare_fk_roluri_spectacole(200)
select @n

GO

-- pt FK (relatia Roluri-Actori)
CREATE FUNCTION validare_fk_roluri_actori(@valoare_fk int)
returns int
as begin
	declare @count int;
	SELECT @count = COUNT(*) FROM dbo.Actori WHERE IdA = @valoare_fk
	return @count
end
GO

declare @n int
set @n = dbo.validare_fk_roluri_actori(100)
select @n

GO

-- pt PK Roluri
CREATE FUNCTION validare_pk_roluri(@id1 int, @id2 int)
returns int
as begin
	declare @count int
	SELECT @count = COUNT(*) FROM dbo.Roluri WHERE IdS = @id1 and IdA = @id2
	return @count
end
GO

declare @n int
set @n = dbo.validare_pk_roluri(4,1)
select @n

GO

-- pt PK Spectacole
CREATE FUNCTION validare_pk_spectacole(@id int)
returns int
as begin
	declare @count int
	SELECT @count = COUNT(*) FROM dbo.Spectacole WHERE IdS = @id
	return @count
end
GO

declare @n int
set @n = dbo.validare_pk_spectacole(411)
select @n

GO


-- pt PK Actori
CREATE FUNCTION validare_pk_actori(@id int)
returns int
as begin
	declare @count int
	SELECT @count = COUNT(*) FROM dbo.Actori WHERE IdA = @id
	return @count
end
GO

declare @n int
set @n = dbo.validare_pk_actori(4)
select @n

GO