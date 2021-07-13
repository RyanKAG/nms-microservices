﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetworkAPI.Repository;

namespace NetworkAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210713070120_DeviceIntegration.2")]
    partial class DeviceIntegration2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetworkAPI.Models.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("NetworkId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("NetworkId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("NetworkAPI.Models.Network", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MacAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Networks");
                });

            modelBuilder.Entity("NetworkAPI.Models.Device", b =>
                {
                    b.HasOne("NetworkAPI.Models.Network", "Network")
                        .WithMany("ConnectedDevices")
                        .HasForeignKey("NetworkId");

                    b.Navigation("Network");
                });

            modelBuilder.Entity("NetworkAPI.Models.Network", b =>
                {
                    b.Navigation("ConnectedDevices");
                });
#pragma warning restore 612, 618
        }
    }
}