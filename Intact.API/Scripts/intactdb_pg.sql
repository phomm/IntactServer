CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Factions" (
    "Id" character varying(16) NOT NULL,
    "Number" integer NOT NULL,
    "TermName" character varying(32) GENERATED ALWAYS AS ("Id" || 'FactionName') STORED,
    "TermDescription" character varying(32) GENERATED ALWAYS AS ("Id" || 'FactionDescription') STORED,
    CONSTRAINT "PK_Factions" PRIMARY KEY ("Id")
);

CREATE TABLE "Localizations" (
    "TermId" character varying(32) NOT NULL,
    "LanguageCode" character varying(16) NOT NULL,
    "Value" character varying(950) NOT NULL,
    CONSTRAINT "PK_Localizations" PRIMARY KEY ("TermId", "LanguageCode")
);

CREATE TABLE "MapBuildings" (
    "MapId" character varying(16) NOT NULL,
    "Number" integer NOT NULL,
    "Proto" character varying(16) NOT NULL,
    "X" integer NOT NULL,
    "Y" integer NOT NULL,
    CONSTRAINT "PK_MapBuildings" PRIMARY KEY ("MapId", "Number")
);

CREATE TABLE "Maps" (
    "Id" character varying(16) NOT NULL,
    "Version" integer NOT NULL,
    "Width" integer NOT NULL,
    "Height" integer NOT NULL,
    "Factions" character varying(128) NOT NULL,
    "SceneBackground" text NOT NULL,
    "TermName" character varying(32) GENERATED ALWAYS AS ("Id" || 'MapName') STORED,
    "TermDescription" character varying(32) GENERATED ALWAYS AS ("Id" || 'MapDescription') STORED,
    CONSTRAINT "PK_Maps" PRIMARY KEY ("Id", "Version")
);

CREATE TABLE "PlayerOptions" (
    "MapId" character varying(16) NOT NULL,
    "Number" integer NOT NULL,
    "Money" integer NOT NULL,
    "Color" integer NOT NULL,
    "Left" integer NOT NULL,
    "Right" integer NOT NULL,
    "Top" integer NOT NULL,
    "Bottom" integer NOT NULL,
    CONSTRAINT "PK_PlayerOptions" PRIMARY KEY ("MapId", "Number")
);

CREATE TABLE "ProtoBuildings" (
    "Id" character varying(16) NOT NULL,
    "Number" integer NOT NULL,
    "AssetId" character varying(32) NOT NULL,
    "InLife" smallint NOT NULL,
    "BuildingType" integer NOT NULL,
    "TermName" character varying(32) GENERATED ALWAYS AS ("Id" || 'BuildingName') STORED,
    "TermDescription" character varying(32) GENERATED ALWAYS AS ("Id" || 'BuildingDescription') STORED,
    CONSTRAINT "PK_ProtoBuildings" PRIMARY KEY ("Id")
);

CREATE TABLE "ProtoWarriors" (
    "Id" character varying(16) NOT NULL,
    "Number" integer NOT NULL,
    "FactionId" character varying(16) NOT NULL,
    "Force" integer NOT NULL,
    "AssetId" character varying(32) NOT NULL,
    "IsHero" boolean NOT NULL DEFAULT FALSE,
    "IsRanged" boolean NOT NULL DEFAULT FALSE,
    "IsMelee" boolean NOT NULL DEFAULT FALSE,
    "IsBlockFree" boolean NOT NULL DEFAULT FALSE,
    "IsImmune" boolean NOT NULL DEFAULT FALSE,
    "InLife" smallint NOT NULL DEFAULT 1,
    "InMana" smallint NOT NULL DEFAULT 0,
    "InMoves" smallint NOT NULL DEFAULT 1,
    "InActs" smallint NOT NULL DEFAULT 1,
    "InShots" smallint NOT NULL DEFAULT 0,
    "Cost" smallint NOT NULL DEFAULT 0,
    "TermName" character varying(32) GENERATED ALWAYS AS ("Id" || 'WarriorName') STORED,
    "TermDescription" character varying(32) GENERATED ALWAYS AS ("Id" || 'WarriorDescription') STORED,
    CONSTRAINT "PK_ProtoWarriors" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231108191036_createdb', '6.0.22');

COMMIT;


