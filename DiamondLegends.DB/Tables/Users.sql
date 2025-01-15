CREATE TABLE [dbo].[Users]
(
	Id INT IDENTITY,
	Username NVARCHAR(120) NOT NULL,
	Email NVARCHAR(250),
	[Password] NVARCHAR(250),
	Salt NVARCHAR(250),
	Nationality INT NOT NULL,

	CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT FK_Users_Nationality FOREIGN KEY (Nationality) REFERENCES Countries(Id),
	CONSTRAINT UK_Users_Username UNIQUE (Username),
	CONSTRAINT UK_Users_Email UNIQUE (Email)
)
