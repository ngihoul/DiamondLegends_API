﻿CREATE TABLE [dbo].[Countries]
(
	Id INT IDENTITY,
	Alpha2 NVARCHAR(2) NOT NULL,
	[Name] NVARCHAR(250) NOT NULL,

	CONSTRAINT PK_Countries PRIMARY KEY ([Id]),
	CONSTRAINT UK_Countries_Alpha2 UNIQUE (Alpha2)
)
