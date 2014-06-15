CREATE TABLE [dbo].[Photo]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(250) NULL, 
    [Photo] IMAGE NULL, 
    [CreatedDate] DATETIME2 NULL
)
