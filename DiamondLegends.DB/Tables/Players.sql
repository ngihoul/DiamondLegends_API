CREATE TABLE [dbo].[Players]
(
	[Id] INT IDENTITY,
	Firstname NVARCHAR(120) NOT NULL,
	Lastname NVARCHAR(120) NOT NULL,
	Date_of_birth DATE NOT NULL,
	Nationality INT NOT NULL,
	[Throw] INT NOT NULL,
	Bat INT NOT NULL,
	Salary DECIMAL NOT NULL,
	Energy INT NOT NULL DEFAULT 100,
	Contact INT NOT NULL,
	Contact_potential INT NOT NULL,
	[Power] INT NOT NULL,
	Power_potential INT NOT NULL,
	Running INT NOT NULL,
	Running_potential INT NOT NULL,
	Defense INT NOT NULL,
	Defense_potential INT NOT NULL,
	Mental INT NOT NULL,
	Mental_potential INT NOT NULL,
	Stamina INT NOT NULL,
	Stamina_potential INT NOT NULL,
	[Control] INT NOT NULL,
	Control_potential INT NOT NULL,
	Velocity INT NOT NULL,
	Velocity_potential INT NOT NULL,
	Movement INT NOT NULL,
	Movement_potential INT NOT NULL

	CONSTRAINT PK_Players PRIMARY KEY (Id),
	CONSTRAINT FK_Players_Countries_Nationality FOREIGN KEY (Nationality) REFERENCES Countries (Id)
)
