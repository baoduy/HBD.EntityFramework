

PRINT N'Merging static data to [Children]';
GO

SET IDENTITY_INSERT dbo.[Children] ON
GO

MERGE INTO [dbo].[Children] AS [Target]
USING(VALUES
	 (
			 1, 'View', 'System', GETDATE(), NULL, NULL
	 ),
	 (
			 2, 'Edit', 'System', GETDATE(), NULL, NULL
	 ),
	 (
			 3, 'Update', 'System', GETDATE(), NULL, NULL
	 ),
	 (
			 4, 'Delete', 'System', GETDATE(), NULL, NULL
	 )) AS [Source]([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn])
ON [Target].[Id] = [Source].[Id]
WHEN MATCHED
	  THEN UPDATE SET  [Name] = [Source].[Name], [CreatedBy] = [Source].[CreatedBy], [CreatedOn] = [Source].[CreatedOn], [UpdatedBy] = [Source].[UpdatedBy], [UpdatedOn] = [Source].[UpdatedOn]
WHEN NOT MATCHED BY TARGET
	  THEN
	  INSERT([Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn])
	  VALUES
(
			 [Id], [Name], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]
);

GO
SET IDENTITY_INSERT dbo.[Children] OFF
GO

PRINT N'Completed merge static data to [Children]';