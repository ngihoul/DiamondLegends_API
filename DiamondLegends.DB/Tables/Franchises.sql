CREATE TABLE [dbo].[Franchises]
(
	[Id] INT IDENTITY,
	[Owner] INT NOT NULL,
	Team INT NOT NULL,
	League INT NOT NULL,
	Season INT NOT NULL,
	CurrentDay INT NOT NULL DEFAULT 0,
	Budget BIGINT NOT NULL DEFAULT 0

  CONSTRAINT PK_Franchises PRIMARY KEY (Id),
  CONSTRAINT FK_Franchises_Users_Owner FOREIGN KEY ([Owner]) REFERENCES Users(Id),
  CONSTRAINT FK_Franchises_Teams_Team FOREIGN KEY ([Team]) REFERENCES Teams(Id),
  CONSTRAINT FK_Franchises_Leagues_League FOREIGN KEY (League) REFERENCES Leagues(Id),
)
