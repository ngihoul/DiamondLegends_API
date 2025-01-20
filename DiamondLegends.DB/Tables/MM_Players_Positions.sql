CREATE TABLE [dbo].[MM_Players_Positions]
(
	[Id] INT IDENTITY,
	[Player] INT NOT NULL,
	[Position] INT NOT NULL,

	CONSTRAINT [PK_MM_Players_Positions] PRIMARY KEY ([id]),
	CONSTRAINT [FK_MM_Players_Positions_Player] FOREIGN KEY ([Player]) REFERENCES [Players]([Id]),
)
