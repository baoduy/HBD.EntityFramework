CREATE TABLE [dbo].[Parents]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[Name] nvarchar(100) not null,
	[CreatedBy]  nvarchar(100)  NOT NULL,
    [CreatedOn]  DATETIME       NOT NULL,
    [UpdatedBy]  nvarchar(100)  NULL,
    [UpdatedOn]  DATETIME       NULL,
    [RowVersion] ROWVERSION     NULL,
)
