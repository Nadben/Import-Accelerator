﻿// <auto-generated />
using Accelerator.Shared.Infrastructure.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Accelerator.Shared.Infrastructure.Migrations
{
    [DbContext(typeof(LandingImportContext))]
    [Migration("20250115145727_01_init")]
    partial class _01_init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Accelerator.Shared.Infrastructure.Entities.Landing.Generated.Category_Group_Subgroup_Partterm_mappingGenerated", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Groupid")
                        .HasColumnType("integer");

                    b.Property<string>("Groupname")
                        .HasColumnType("text");

                    b.Property<int>("Parttermid")
                        .HasColumnType("integer");

                    b.Property<string>("Parttermname")
                        .HasColumnType("text");

                    b.Property<int>("Subgroupid")
                        .HasColumnType("integer");

                    b.Property<string>("Subgroupname")
                        .HasColumnType("text");

                    b.Property<int>("USG")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Category_Group_Subgroup_Partterm_mappingGenerated");
                });

            modelBuilder.Entity("Accelerator.Shared.Infrastructure.Entities.Landing.Generated.Inventory_USIC_INV20240325152337Generated", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrentSubline")
                        .HasColumnType("integer");

                    b.Property<int>("LocationID")
                        .HasColumnType("integer");

                    b.Property<string>("Mfg")
                        .HasColumnType("text");

                    b.Property<string>("Part")
                        .HasColumnType("text");

                    b.Property<int>("QtyMinOrder")
                        .HasColumnType("integer");

                    b.Property<double>("StdUPC")
                        .HasColumnType("double precision");

                    b.Property<int>("StockQty")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Inventory_USIC_INV20240325152337Generated");
                });

            modelBuilder.Entity("Accelerator.Shared.Infrastructure.Entities.Landing.Generated.Price_USIC_PRICE20240325152337ABGenerated", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Core")
                        .HasColumnType("double precision");

                    b.Property<double>("Ehfee")
                        .HasColumnType("double precision");

                    b.Property<string>("Mfg")
                        .HasColumnType("text");

                    b.Property<string>("Part")
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("Regionid")
                        .HasColumnType("text");

                    b.Property<int>("Subline")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Price_USIC_PRICE20240325152337ABGenerated");
                });

            modelBuilder.Entity("Accelerator.Shared.Infrastructure.Entities.Landing.Generated.Price_USIC_PRICE20240325152337BCGenerated", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Core")
                        .HasColumnType("double precision");

                    b.Property<double>("Ehfee")
                        .HasColumnType("double precision");

                    b.Property<string>("Mfg")
                        .HasColumnType("text");

                    b.Property<string>("Part")
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("Regionid")
                        .HasColumnType("text");

                    b.Property<int>("Subline")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Price_USIC_PRICE20240325152337BCGenerated");
                });
#pragma warning restore 612, 618
        }
    }
}
