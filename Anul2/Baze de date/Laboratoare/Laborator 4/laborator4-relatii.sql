-- se creeaza tabelul de relatii pierdute ---
CREATE TABLE RemovedRelationships(
Id INT PRIMARY KEY IDENTITY,
NameTableLeft VARCHAR(250),
IdLeft INT,
NameTableRight VARCHAR(250),
IdRight INT,
MultiplicityDescription VARCHAR(250),
MultiplicityDate DATETIME);

GO

SELECT * FROM Costume
SELECT * FROM Actori
DROP TABLE RemovedRelationships
GO

CREATE PROCEDURE DropConstraint(
	@TableName NVARCHAR(250),
	@ConstraintName NVARCHAR(250)
)
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX);

	SET @sql = 'ALTER TABLE ' + QUOTENAME(@TableName) + 
	' DROP CONSTRAINT ' + QUOTENAME(@ConstraintName) + ';'
	 
	EXEC(@sql);
END;


GO


--------------------------------------------------------- PROCEDURA DE TRANSFORMARE 1 TO MANY -> MANY TO 1 ----------------------------------------------------

CREATE PROCEDURE TransformIntoManyToOne
(
	@ParentTable NVARCHAR(250),
	@ParentColumnId NVARCHAR(250),
	@ChildTable NVARCHAR(250),
	@ChildColumnId NVARCHAR(250),
	@FKConstraint NVARCHAR(250)
)
AS
BEGIN TRY
BEGIN TRANSACTION;

	--- crearea tabelului unde stocam valorile maxime ale cheilor ---
	CREATE TABLE #MaxValues (
		ParentId INT,
		MaxChildId INT
	);

	-- validari -- 

		IF OBJECT_ID(@ParentTable, 'U') IS NULL
	BEGIN
		PRINT 'Eroare: Tabela ' + @ParentTable + ' nu exista.';
		RETURN;
	END

	IF OBJECT_ID(@ChildTable, 'U') IS NULL
	BEGIN
		PRINT 'Eroare: Tabela ' + @ChildTable + ' nu exista.';
		RETURN;
	END

	IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = @ParentColumnId AND Object_ID = OBJECT_ID(@ParentTable))
	BEGIN
		PRINT 'Eroare: PK ' + @ParentColumnId + ' nu exista in tabela ' + @ParentTable;
		RETURN;
	END

	IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = @ChildColumnId AND Object_ID = OBJECT_ID(@ChildTable))
	BEGIN
		PRINT 'Eroare: PK ' + @ChildColumnId + ' nu exista in tabela ' + @ChildTable;
		RETURN;
	END

	--- stocam in tabelul de relatii pierdute ---
	DECLARE @sqlSave NVARCHAR(MAX) = N'
	INSERT INTO RemovedRelationships 
		(NameTableLeft, IdLeft, NameTableRight, IdRight, MultiplicityDescription, MultiplicityDate)
	SELECT 
		''' + @ParentTable + ''', 
		 ' + QUOTENAME(@ParentColumnId) + ',
		''' + @ChildTable + ''',
		 ' + QUOTENAME(@ChildColumnId) + ',
		''1:N Relationship was transformed into N:1'',
		GETDATE()
	FROM ' + QUOTENAME(@ChildTable) + ';';

	EXEC(@sqlSave)


	--- aici ne ocupam de gasirea cheii maxime ---
	DECLARE @sqlmax NVARCHAR(MAX) = '
		INSERT INTO #MaxValues (ParentId, MaxChildId)
		SELECT ' + QUOTENAME(@ParentColumnId) + ',
			   MAX(' + QUOTENAME(@ChildColumnId) + ')
		FROM ' + QUOTENAME(@ChildTable) + '
		GROUP BY ' + QUOTENAME(@ParentColumnId) + ';
	';

	EXEC(@sqlmax);


	--- aici eliminam conditia de FK si eliminam coloana corespunzatoare (din tabelul 'copil') --
	EXEC DropConstraint @TableName = @ChildTable, @ConstraintName = @FKConstraint;

	DECLARE @sqldrop NVARCHAR(MAX) =
	'ALTER TABLE ' + QUOTENAME(@ChildTable) +
	' DROP COLUMN ' + QUOTENAME(@ParentColumnId) + ';';

	EXEC(@sqldrop)

	--- adaugam coloana in tabelul 'parinte' ---
	DECLARE @sqlcreate NVARCHAR(MAX) = 
	'ALTER TABLE ' + QUOTENAME(@ParentTable) +
	' ADD ' + QUOTENAME(@ChildColumnId) + ' INT;';

	EXEC(@sqlcreate)

	--- aici adaugam datele coresp val. maxime in coloana adaugata anterior ---
	DECLARE @sqlupdate NVARCHAR(MAX) = ' UPDATE P
	SET P.' + QUOTENAME(@ChildColumnId) + ' = M.MaxChildId
	FROM ' + QUOTENAME(@ParentTable) + ' P INNER JOIN #MaxValues M 
	ON M.ParentId = P.' + QUOTENAME(@ParentColumnId) + ';';

	EXEC(@sqlupdate)

	--- adaugam constrangere de FK pe coloana adaugata --

	DECLARE @sqlfk NVARCHAR(MAX) = 
	'ALTER TABLE ' + QUOTENAME(@ParentTable) + 
	' ADD CONSTRAINT FK_'+@ParentTable +
	' FOREIGN KEY (' + QUOTENAME(@ChildColumnId) + ') REFERENCES ' +
	QUOTENAME(@ChildTable)+'('+QUOTENAME(@ChildColumnId) + ');'

	EXEC(@sqlfk)

COMMIT TRANSACTION;
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION;
	-- arată eroarea după rollback:
	DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
	RAISERROR(@Msg, 16, 1);
END CATCH

DROP PROCEDURE TransformIntoManyToOne

 EXEC TransformIntoManyToOne 'Actori','IdA','Costume','IdC','FK__Costume__IdA__5535A963'
 GO

 SELECT * FROM  RemovedRelationships
 select * from Actori
 select * from Costume



 --------------------------------------------------------- PROCEDURA DE TRANSFORMARE 1 TO MANY -> MANY TO MANY ----------------------------------------------------
 CREATE PROCEDURE TransformIntoManyToMany(
 @ParentTable NVARCHAR(250),
 @ParentColumnId NVARCHAR(250),
 @ChildTable NVARCHAR(250),
 @ChildColumnId NVARCHAR(250),
 @FKConstraint NVARCHAR(250)
 )
AS
BEGIN TRY
BEGIN TRANSACTION;

	-- se creaza tabelul intermediar --
	DECLARE @sqlCreateTable NVARCHAR(MAX) = '
	CREATE TABLE Leg_'+@ParentTable+'_'+@ChildTable +
	' (' + QUOTENAME(@ParentColumnId) + ' INT FOREIGN KEY REFERENCES ' +QUOTENAME(@ParentTable)+ '('+QUOTENAME(@ParentColumnId) + '),'
	+ QUOTENAME(@ChildColumnId) + ' INT FOREIGN KEY REFERENCES ' + QUOTENAME(@ChildTable) + '(' + QUOTENAME(@ChildColumnId) + '),'+
	' CONSTRAINT pk_'+@ParentTable+'_'+@ChildTable+' PRIMARY KEY ('+QUOTENAME(@ParentColumnId)+','+QUOTENAME(@ChildColumnId)+') )'

	EXEC(@sqlCreateTable)

	--- stocam in tabelul de relatii pierdute ---
	DECLARE @sqlSave NVARCHAR(MAX) = N'
	INSERT INTO RemovedRelationships 
		(NameTableLeft, IdLeft, NameTableRight, IdRight, MultiplicityDescription, MultiplicityDate)
	SELECT 
		''' + @ParentTable + ''', 
		 ' + QUOTENAME(@ParentColumnId) + ',
		''' + @ChildTable + ''',
		 ' + QUOTENAME(@ChildColumnId) + ',
		''1:N Relationship was transformed into M:N'',
		GETDATE()
	FROM ' + QUOTENAME(@ChildTable) + ';';

	EXEC(@sqlSave)

	-- se populeaza tabelul --
	DECLARE @sqlSetData NVARCHAR(MAX) = '
	INSERT INTO  Leg_'+@ParentTable+'_'+@ChildTable +' (' + QUOTENAME(@ParentColumnId) +',' + QUOTENAME(@ChildColumnId) + ')
	SELECT '+QUOTENAME(@ParentColumnId) + ',' + QUOTENAME(@ChildColumnId) + 
	' FROM ' +  QUOTENAME(@ChildTable)

	EXEC(@sqlSetData)

	-- se elimina constrangerea de foreign key --
	EXEC DropConstraint @TableName = @ChildTable, @ConstraintName = @FKConstraint;

	-- se elimina cu totul coloana --
	DECLARE @sqldrop NVARCHAR(MAX) =
	'ALTER TABLE ' + QUOTENAME(@ChildTable) +
	' DROP COLUMN ' + QUOTENAME(@ParentColumnId) + ';';

	EXEC(@sqldrop)

COMMIT TRANSACTION;
END TRY

BEGIN CATCH
ROLLBACK TRANSACTION
	DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
	RAISERROR(@Msg,16,1);
END CATCH

DROP PROCEDURE TransformIntoManyToMany


SELECT * FROM Leg_Actori_Costume

EXEC TransformIntoManyToMany 'TeatruNational','IdT','Angajati','IdAngajat','FK__Angajati__IdT__6383C8BA'
GO

SELECT name 
FROM sys.foreign_keys
WHERE parent_object_id = OBJECT_ID('Angajati');




 --------------------------------------------------------- PROCEDURA DE TRANSFORMARE MANY TO MANY -> 1 TO MANY ----------------------------------------------------
 CREATE PROCEDURE TransformIntoOneToMany_FromManyToMany(
 @ParentTable NVARCHAR(250),
 @ParentColumnId NVARCHAR(250),
 @ChildTable NVARCHAR(250),
 @ChildColumnId NVARCHAR(250),
 @LegTable NVARCHAR(250)
 )
 AS
BEGIN TRY
BEGIN TRANSACTION;
	
	-- se adauga coloana noua in tabelul din dreapta (cel care va ramane cu relatia de ~N)
	DECLARE @sqlAddColumn NVARCHAR(MAX) = 'ALTER TABLE ' + QUOTENAME(@ChildTable) +
	' ADD ' + QUOTENAME(@ParentColumnId) + ' INT'

	EXEC(@sqlAddColumn)

	--- stocam in tabelul de relatii pierdute ---
	DECLARE @sqlSave NVARCHAR(MAX) = N'
	INSERT INTO RemovedRelationships 
		(NameTableLeft, IdLeft, NameTableRight, IdRight, MultiplicityDescription, MultiplicityDate)
	SELECT 
		''' + @ParentTable + ''', 
		 p.' + QUOTENAME(@ParentColumnId) + ',
		''' + @ChildTable + ''',
		 c.' + QUOTENAME(@ChildColumnId) + ',
		''M:N Relationship was transformed into 1:N'',
		GETDATE()
	FROM ' + QUOTENAME(@ParentTable) + ' p INNER JOIN ' + QUOTENAME(@LegTable) + ' l ON '+
	'p.' + QUOTENAME(@ParentColumnId) + ' = l.'+ QUOTENAME(@ParentColumnId) + ' INNER JOIN '+
	QUOTENAME(@ChildTable) + ' c ON c.' + QUOTENAME(@ChildColumnId) + ' = l.'+ QUOTENAME(@ChildColumnId) + ';';

	EXEC(@sqlSave)

	-- adaugam noile id-uri in coloana adaugata -> mai exact, id-urile maxime ale tabelului din stanga --
	DECLARE @sqlSetData NVARCHAR(MAX) = 'UPDATE Child SET Child.' +
	QUOTENAME(@ParentColumnId) + ' = ' +
	'(SELECT MAX(Parent.' + QUOTENAME(@ParentColumnId) + ') FROM ' +
	QUOTENAME(@ParentTable) + ' Parent INNER JOIN ' + QUOTENAME(@LegTable) + ' Leg ON ' +
	' Leg.' + QUOTENAME(@ParentColumnId) + ' = Parent.' + QUOTENAME(@ParentColumnId) +
	'  WHERE Leg.' + QUOTENAME(@ChildColumnId) + ' = Child.' + QUOTENAME(@ChildColumnId) + ' ) ' +
	' FROM ' + QUOTENAME(@ChildTable) + ' Child;';

	EXEC(@sqlSetData)

	--- adaugam constrangere de FK pe coloana adaugata --
	DECLARE @sqlfk NVARCHAR(MAX) = 
	'ALTER TABLE ' + QUOTENAME(@ChildTable) +  
	' ADD CONSTRAINT FK_'+@ChildTable +
	' FOREIGN KEY (' + QUOTENAME(@ParentColumnId) + ') REFERENCES ' +
	QUOTENAME(@ParentTable)+'('+QUOTENAME(@ParentColumnId) + ');'

	EXEC(@sqlfk)

	-- stergem tabela de legatura --
	DECLARE @sqlDropTable NVARCHAR(MAX) = ' DROP TABLE ' + QUOTENAME(@LegTable)
	EXEC(@sqlDropTable)

COMMIT TRANSACTION;
END TRY

BEGIN CATCH
ROLLBACK TRANSACTION
	DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
	RAISERROR(@Msg,16,1);
END CATCH

DROP PROCEDURE TransformIntoOneToMany_FromManyToMany

EXEC TransformIntoOneToMany_FromManyToMany 'Spectacole','IdS','Regizori','IdR','ParticipareRegie'
GO

select * from RemovedRelationships
GO



 --------------------------------------------------------- PROCEDURA DE TRANSFORMARE 1 TO MANY -> 1 TO 1 ----------------------------------------------------
 CREATE PROCEDURE TransformIntoOneToOne(
 	@ParentTable NVARCHAR(250),
	@ParentColumnId NVARCHAR(250),
	@ChildTable NVARCHAR(250),
	@ChildColumnId NVARCHAR(250),
	@PKConstraint NVARCHAR(250)
)
 AS
BEGIN TRY
BEGIN TRANSACTION;
	--- stocam in tabelul de relatii pierdute ---
	DECLARE @sqlSave NVARCHAR(MAX) = N'
	INSERT INTO RemovedRelationships 
		(NameTableLeft, IdLeft, NameTableRight, IdRight, MultiplicityDescription, MultiplicityDate)
	SELECT 
		''' + @ParentTable + ''', 
		 ' + QUOTENAME(@ParentColumnId) + ',
		''' + @ChildTable + ''',
		 ' + QUOTENAME(@ChildColumnId) + ',
		''1:N Relationship was transformed into 1:1'',
		GETDATE()
	FROM ' + QUOTENAME(@ChildTable) + ';';

	EXEC(@sqlSave)

	-- tabel pentru pastrarea legaturilor de tip id maxim --
	CREATE TABLE #MaxValues (
		ParentId INT,
		MaxChildId INT
	);

	-- determinarea id-urilor maxime --
	DECLARE @sqlmax NVARCHAR(MAX) = '
		INSERT INTO #MaxValues (ParentId, MaxChildId)
		SELECT ' + QUOTENAME(@ParentColumnId) + ',
			   MAX(' + QUOTENAME(@ChildColumnId) + ')
		FROM ' + QUOTENAME(@ChildTable) + '
		GROUP BY ' + QUOTENAME(@ParentColumnId) + ';
	';

	EXEC(@sqlmax);

	-- crearea unui tabel cu toate inregistrarile din tabelul din stanga care nu au legaturi --
	CREATE TABLE #ParentWithoutChild (
		ParentId INT
	);

	-- popularea tabelului creat anterior --
	DECLARE @sqlParentWithoutChild NVARCHAR(MAX) = 'INSERT INTO #ParentWithoutChild(ParentId) SELECT P.' + QUOTENAME(@ParentColumnId) + ' FROM ' +
	QUOTENAME(@ParentTable) + ' P LEFT JOIN ' + QUOTENAME(@ChildTable) + ' C 
	 ON P.' + QUOTENAME(@ParentColumnId) + ' = C.' + QUOTENAME(@ParentColumnId) + 
	 ' WHERE C.'+ QUOTENAME(@ParentColumnId) + ' is null;';

	EXEC(@sqlParentWithoutChild)


	-- stergem constrangerea de cheie primara de pe tabela din dreapta --
	EXEC DropConstraint @TableName = @ChildTable, @ConstraintName = @PKConstraint;

	-- setarea id-ului de legatura din tabelul din stanga pe null, daca nu are cheia maxima --
	DECLARE @sqlSetNull NVARCHAR(MAX) = 'UPDATE c 
	SET c.' + QUOTENAME(@ParentColumnId) + ' = NULL ' +
	' FROM ' + QUOTENAME(@ChildTable) + ' c INNER JOIN #MaxValues m ON m.ParentId = c.' + QUOTENAME(@ParentColumnId) +
	' WHERE c.' + QUOTENAME(@ChildColumnId) + ' <> m.MaxChildId;';

	EXEC(@sqlSetNull)

	-- se face asocierea dintre inregistrarile din tabelul din dreapta ramase fara legatura si cele din tabelul din stanga ramase fara legatura --
	-- numerotarea inregistrarilor din #ParentWithoutChild --
	SELECT ROW_NUMBER() OVER(ORDER BY ParentId) AS rn, ParentId
	INTO #p
	FROM #ParentWithoutChild;

	-- creeaza tabel auxiliar --
	CREATE TABLE #c (
    rn INT,
    ChildId INT
	);

	-- numerotarea inregistrarilor din tabelul din dreapta (Child) ramasi fara legatura --
	DECLARE @sqlChildRowNumber NVARCHAR(MAX) = 'INSERT INTO #c (rn, ChildId) SELECT 
    ROW_NUMBER() OVER (ORDER BY ' + QUOTENAME(@ChildColumnId) + ') AS rn, '+
    QUOTENAME(@ChildColumnId) + ' FROM ' + QUOTENAME(@ChildTable) +
	' WHERE ' + QUOTENAME(@ParentColumnId) + ' is null';

	EXEC(@sqlChildRowNumber)

	-- asocierea dintre tabele --
	DECLARE @sqlAssos NVARCHAR(MAX) = ' UPDATE Child
	SET Child.' + QUOTENAME(@ParentColumnId) +' = P.ParentId ' +
	' FROM ' + QUOTENAME(@ChildTable) + ' Child' +
	' INNER JOIN #c c ON Child.' + QUOTENAME(@ChildColumnId) + ' = c.ChildId' +
	' INNER JOIN #p P ON P.rn = c.rn;';
	
	EXEC(@sqlAssos)

	-- se determina cate inregistrari din tabela dreapta au ramas fara legatura --
	DECLARE @nr INT;
	DECLARE @sqlCount NVARCHAR(MAX) = N'SELECT @nrOut = COUNT(*) FROM ' + QUOTENAME(@ChildTable) + ' WHERE ' + QUOTENAME(@ParentColumnId) + ' is null;';

	EXEC sp_executesql
    @sqlCount,
    N'@nrOut INT OUTPUT',
    @nrOut = @nr OUTPUT;


	-- crearea inregistrarilor in plus pentru 'copiii' ramasi fara 'parinte' --
	WHILE @nr > 0
	BEGIN
		DECLARE @idNou INT;
		
		DECLARE @sqlInsert NVARCHAR(MAX) = 'INSERT INTO ' + QUOTENAME(@ParentTable) + ' DEFAULT VALUES; SELECT @id = SCOPE_IDENTITY()'; 
		EXEC sp_executesql
		@sqlInsert,
		N'@id INT OUTPUT',
		@id = @idNou OUTPUT;


		DECLARE @sqlTop NVARCHAR(MAX) = 'UPDATE TOP(1) C  SET C.' + QUOTENAME(@ParentColumnId) + ' = @id ' + 
		' FROM ' + QUOTENAME(@ChildTable) + ' C WHERE ' + QUOTENAME(@ParentColumnId) + ' is null;';

		EXEC sp_executesql
		@sqlTop,
		N'@id INT',
		@id = @idNou;

		SET @nr -= 1;
	END

COMMIT TRANSACTION;
END TRY

BEGIN CATCH
ROLLBACK TRANSACTION
	DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
	RAISERROR(@Msg,16,1);
END CATCH

 EXEC TransformIntoOneToOne 'Actori','IdA','Costume','IdC','PK__Costume__C49600037AB84B84'

 select * from Actori