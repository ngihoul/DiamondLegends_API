CREATE TABLE [dbo].[Rosters]
(
	[Id] INT IDENTITY,
	[Player] INT NOT NULL,
	[Team] INT NOT NULL,
	[AddedAt] DATETIME NOT NULL,

	CONSTRAINT [PK_Rosters] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Rosters_Player] FOREIGN KEY ([Player]) REFERENCES [Players]([Id]),
	CONSTRAINT [FK_Rosters_Team] FOREIGN KEY ([Team]) REFERENCES [Teams]([Id])
)
