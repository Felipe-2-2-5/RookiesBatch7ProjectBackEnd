﻿// <auto-generated />
using System;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    [DbContext(typeof(AssetContext))]
    [Migration("20240626145439_addReturnRequest")]
    partial class addReturnRequest
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backend.Domain.Entities.Asset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AssetCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("AssetName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("InstalledDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Specification")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("Backend.Domain.Entities.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AssetId")
                        .HasColumnType("int");

                    b.Property<int>("AssignedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("AssignedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("AssignedToId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("AssignedById");

                    b.HasIndex("AssignedToId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("Backend.Domain.Entities.ReturnRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AcceptorId")
                        .HasColumnType("int");

                    b.Property<int>("AssignmentId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("RequestorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReturnedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AcceptorId");

                    b.HasIndex("AssignmentId")
                        .IsUnique();

                    b.HasIndex("RequestorId");

                    b.ToTable("ReturnRequests");
                });

            modelBuilder.Entity("Backend.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<bool>("FirstLogin")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("JoinedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("Location")
                        .HasMaxLength(30)
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("StaffCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("StaffCode")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstLogin = true,
                            FirstName = "John",
                            Gender = 1,
                            IsDeleted = false,
                            JoinedDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Doe",
                            Location = 1,
                            Password = "$2a$11$csxVZkakwYZrcnlnF1y9eOe4loscS1Tdqv2fGLZbauOwiblFVP0NO",
                            StaffCode = "SD0001",
                            Type = 1,
                            UserName = "johnd"
                        },
                        new
                        {
                            Id = 2,
                            DateOfBirth = new DateTime(1990, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstLogin = true,
                            FirstName = "Jane",
                            Gender = 2,
                            IsDeleted = false,
                            JoinedDate = new DateTime(2019, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Smith",
                            Location = 0,
                            Password = "$2a$11$wUIJ3WdA4cqSp0XlcdA7R.HSegabE/lk55QmjbpC8ZYokHiSUM3Am",
                            StaffCode = "SD0002",
                            Type = 0,
                            UserName = "janes"
                        },
                        new
                        {
                            Id = 3,
                            DateOfBirth = new DateTime(1975, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstLogin = true,
                            FirstName = "Michael",
                            Gender = 1,
                            IsDeleted = false,
                            JoinedDate = new DateTime(2018, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Brown",
                            Location = 1,
                            Password = "$2a$11$gleLrnRQ1KztAUdJJ6kNROfrk3uFlUcxhfazmoXGciti8idCPBuDi",
                            StaffCode = "SD0003",
                            Type = 1,
                            UserName = "michaelb"
                        },
                        new
                        {
                            Id = 4,
                            DateOfBirth = new DateTime(1988, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstLogin = true,
                            FirstName = "Emily",
                            Gender = 2,
                            IsDeleted = false,
                            JoinedDate = new DateTime(2021, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Jones",
                            Location = 1,
                            Password = "$2a$11$nJECDMe.XIbx4/x0jAgO..eNq4dzUL7IDNLGwUS4mz.57San7lLt6",
                            StaffCode = "SD0004",
                            Type = 0,
                            UserName = "emilyj"
                        },
                        new
                        {
                            Id = 5,
                            DateOfBirth = new DateTime(1995, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstLogin = true,
                            FirstName = "David",
                            Gender = 1,
                            IsDeleted = false,
                            JoinedDate = new DateTime(2017, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Williams",
                            Location = 0,
                            Password = "$2a$11$sqvXMW3J5w3RBKBs0nQ88u1pqA7PcrtEqnjl2zNLOzvGzf16cQvdS",
                            StaffCode = "SD0005",
                            Type = 0,
                            UserName = "davidw"
                        });
                });

            modelBuilder.Entity("Backend.Domain.Entity.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Backend.Domain.Entities.Asset", b =>
                {
                    b.HasOne("Backend.Domain.Entity.Category", "Category")
                        .WithMany("Assets")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Backend.Domain.Entities.Assignment", b =>
                {
                    b.HasOne("Backend.Domain.Entities.Asset", "Asset")
                        .WithMany("Assignments")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Domain.Entities.User", "AssignedBy")
                        .WithMany()
                        .HasForeignKey("AssignedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Backend.Domain.Entities.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("AssignedBy");

                    b.Navigation("AssignedTo");
                });

            modelBuilder.Entity("Backend.Domain.Entities.ReturnRequest", b =>
                {
                    b.HasOne("Backend.Domain.Entities.User", "Acceptor")
                        .WithMany()
                        .HasForeignKey("AcceptorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Domain.Entities.Assignment", "Assignment")
                        .WithOne("ReturnRequest")
                        .HasForeignKey("Backend.Domain.Entities.ReturnRequest", "AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Domain.Entities.User", "Requestor")
                        .WithMany()
                        .HasForeignKey("RequestorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Acceptor");

                    b.Navigation("Assignment");

                    b.Navigation("Requestor");
                });

            modelBuilder.Entity("Backend.Domain.Entities.Asset", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Backend.Domain.Entities.Assignment", b =>
                {
                    b.Navigation("ReturnRequest")
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Domain.Entity.Category", b =>
                {
                    b.Navigation("Assets");
                });
#pragma warning restore 612, 618
        }
    }
}
