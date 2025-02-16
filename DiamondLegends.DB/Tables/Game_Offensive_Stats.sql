﻿CREATE TABLE [dbo].[Game_Offensive_Stats]
(
	[Id] INT IDENTITY,
	[Game] INT NOT NULL,
	Player INT NOT NULL,
	[Order] INT NOT NULL,
	Position INT NOT NULL,
	PA INT NOT NULL DEFAULT 0,
	AB INT NOT NULL DEFAULT 0,
	R INT NOT NULL DEFAULT 0,
	H INT NOT NULL DEFAULT 0,
	[2B] INT NOT NULL DEFAULT 0,
	[3B] INT NOT NULL DEFAULT 0,
	HR INT NOT NULL DEFAULT 0,
	RBI INT NOT NULL DEFAULT 0,
	BB INT NOT NULL DEFAULT 0,
	IBB INT NOT NULL DEFAULT 0,
	SO INT NOT NULL DEFAULT 0,
	SB INT NOT NULL DEFAULT 0,
	CS INT NOT NULL DEFAULT 0

	CONSTRAINT PK_Game_Offensive_Stats PRIMARY KEY ([Id]),
	CONSTRAINT FK_Game_Offensive_Stats_Games_Game FOREIGN KEY (Game) REFERENCES [Games]([Id]),
	CONSTRAINT FK_Game_Offensive_Stats_Players_Player FOREIGN KEY (Player) REFERENCES [Players]([Id])
)
