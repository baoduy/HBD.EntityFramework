CREATE TABLE [dbo].[ParentChildren]
(
	[ParentId] INT NOT NULL ,
	[ChildrenId] INT NOT NULL, 
	[CreatedBy]  nvarchar(100)  NOT NULL,
    [CreatedOn]  DATETIME       NOT NULL,
    [UpdatedBy]  nvarchar(100)  NULL,
    [UpdatedOn]  DATETIME       NULL,
    [RowVersion] ROWVERSION     NULL,
    CONSTRAINT [PK_ParentChildrent] PRIMARY KEY ([ParentId], [ChildrenId]), 
    CONSTRAINT [FK_ParentChildrent_Parent] FOREIGN KEY ([ParentId]) REFERENCES [Parents]([Id]), 
    CONSTRAINT [FK_ParentChildrent_Childrent] FOREIGN KEY ([ChildrenId]) REFERENCES [Children](Id) 
)
