CREATE TABLE [dbo].[Positions]
(
	[Id] INT IDENTITY,
	[Player] INT NOT NULL,
	[Position] INT NOT NULL,

	CONSTRAINT [PK_Positions] PRIMARY KEY ([id]),
	CONSTRAINT [FK_Positions_Player] FOREIGN KEY ([Player]) REFERENCES [Players]([Id]),
)
