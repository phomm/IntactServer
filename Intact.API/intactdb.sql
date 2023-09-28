IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Factions] (
    [Id] nvarchar(16) NOT NULL,
    [Number] int NOT NULL,
    [TermName] AS [Id] + 'FactionName',
    [TermDescription] AS [Id] + 'FactionDescription',
    CONSTRAINT [PK_Factions] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Localizations] (
    [TermId] nvarchar(32) NOT NULL,
    [LanguageCode] nvarchar(16) NOT NULL,
    [Value] nvarchar(950) NOT NULL,
    CONSTRAINT [PK_Localizations] PRIMARY KEY ([TermId], [LanguageCode])
);
GO

CREATE TABLE [ProtoBuildings] (
    [Id] nvarchar(16) NOT NULL,
    [Number] int NOT NULL,
    [TermName] AS [Id] + 'BuildingName',
    [TermDescription] AS [Id] + 'BuildingDescription',
    [AssetId] nvarchar(32) NOT NULL,
    [InLife] tinyint NOT NULL,
    [BuildingType] int NOT NULL,
    CONSTRAINT [PK_ProtoBuildings] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProtoWarriors] (
    [Id] nvarchar(16) NOT NULL,
    [Number] int NOT NULL,
    [TermName] AS [Id] + 'WarriorName',
    [TermDescription] AS [Id] + 'WarriorDescription',
    [FactionId] nvarchar(16) NOT NULL,
    [Force] int NOT NULL,
    [AssetId] nvarchar(32) NOT NULL,
    [IsHero] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsRanged] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsMelee] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsBlockFree] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsImmune] bit NOT NULL DEFAULT CAST(0 AS bit),
    [InLife] tinyint NOT NULL DEFAULT CAST(1 AS tinyint),
    [InMana] tinyint NOT NULL DEFAULT CAST(0 AS tinyint),
    [InMoves] tinyint NOT NULL DEFAULT CAST(1 AS tinyint),
    [InActs] tinyint NOT NULL DEFAULT CAST(1 AS tinyint),
    [InShots] tinyint NOT NULL DEFAULT CAST(0 AS tinyint),
    [Cost] tinyint NOT NULL DEFAULT CAST(0 AS tinyint),
    CONSTRAINT [PK_ProtoWarriors] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230928154546_createdb', N'6.0.22');
GO

COMMIT;
GO

