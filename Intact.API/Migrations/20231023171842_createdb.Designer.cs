﻿// <auto-generated />
using Intact.BusinessLogic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Intact.API.Migrations
{
    [DbContext(typeof(IntactDbContext))]
    [Migration("20231023171842_createdb")]
    partial class createdb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Intact.BusinessLogic.Data.Models.FactionDao", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("TermDescription")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasComputedColumnSql("[Id] + 'FactionDescription'");

                    b.Property<string>("TermName")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasComputedColumnSql("[Id] + 'FactionName'");

                    b.HasKey("Id");

                    b.ToTable("Factions");
                });

            modelBuilder.Entity("Intact.BusinessLogic.Data.Models.LocalizationDao", b =>
                {
                    b.Property<string>("TermId")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("LanguageCode")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(950)
                        .HasColumnType("nvarchar(950)");

                    b.HasKey("TermId", "LanguageCode");

                    b.ToTable("Localizations");
                });

            modelBuilder.Entity("Intact.BusinessLogic.Data.Models.MapBuildingDao", b =>
                {
                    b.Property<string>("MapId")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Proto")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("MapId", "Number");

                    b.ToTable("MapBuildings");
                });

            modelBuilder.Entity("Intact.BusinessLogic.Data.Models.MapDao", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.Property<string>("Factions")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("SceneBackground")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TermDescription")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasComputedColumnSql("[Id] + 'MapDescription'");

                    b.Property<string>("TermName")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasComputedColumnSql("[Id] + 'MapName'");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id", "Version");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("Intact.BusinessLogic.Data.Models.PlayerOptionsDao", b =>
                {
                    b.Property<string>("MapId")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("Bottom")
                        .HasColumnType("int");

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<int>("Left")
                        .HasColumnType("int");

                    b.Property<int>("Money")
                        .HasColumnType("int");

                    b.Property<int>("Right")
                        .HasColumnType("int");

                    b.Property<int>("Top")
                        .HasColumnType("int");

                    b.HasKey("MapId", "Number");

                    b.ToTable("PlayerOptions");
                });

            modelBuilder.Entity("Intact.BusinessLogic.Data.Models.ProtoBuildingDao", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("AssetId")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<int>("BuildingType")
                        .HasColumnType("int");

                    b.Property<byte>("InLife")
                        .HasColumnType("tinyint");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("TermDescription")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasComputedColumnSql("[Id] + 'BuildingDescription'");

                    b.Property<string>("TermName")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasComputedColumnSql("[Id] + 'BuildingName'");

                    b.HasKey("Id");

                    b.ToTable("ProtoBuildings");
                });

            modelBuilder.Entity("Intact.BusinessLogic.Data.Models.ProtoWarriorDao", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("AssetId")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<byte>("Cost")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)0);

                    b.Property<string>("FactionId")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Force")
                        .HasColumnType("int");

                    b.Property<byte>("InActs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<byte>("InLife")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<byte>("InMana")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)0);

                    b.Property<byte>("InMoves")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<byte>("InShots")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)0);

                    b.Property<bool>("IsBlockFree")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsHero")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsImmune")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsMelee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsRanged")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("TermDescription")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasComputedColumnSql("[Id] + 'WarriorDescription'");

                    b.Property<string>("TermName")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasComputedColumnSql("[Id] + 'WarriorName'");

                    b.HasKey("Id");

                    b.ToTable("ProtoWarriors");
                });
#pragma warning restore 612, 618
        }
    }
}