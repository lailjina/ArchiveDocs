﻿// <auto-generated />
using System;
using ArchiveDocs.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArchiveDocs.Migrations
{
    [DbContext(typeof(ADDbContext))]
    [Migration("20240114103550_AddDateArchiveObject")]
    partial class AddDateArchiveObject
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TCategoryDocument", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TDocument", b =>
                {
                    b.Property<int>("Id_Doc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_Doc"));

                    b.Property<DateTime>("BusinessDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DateSendToArchive")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DocumentNumber")
                        .HasColumnType("text");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HashSum")
                        .HasColumnType("text");

                    b.Property<DateTime?>("HranitDo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("ObjectId")
                        .HasColumnType("integer");

                    b.Property<int>("StatusDoc")
                        .HasColumnType("integer");

                    b.HasKey("Id_Doc");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DocumentTypeId");

                    b.HasIndex("ObjectId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("TNomenclature", b =>
                {
                    b.Property<int>("DocumentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DocumentTypeId"));

                    b.Property<string>("DocumentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StorageYears")
                        .HasColumnType("integer");

                    b.HasKey("DocumentTypeId");

                    b.ToTable("Nomenclatures");
                });

            modelBuilder.Entity("TObject", b =>
                {
                    b.Property<int>("Id_Object")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_Object"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateArchived")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateSendToArchive")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StatusObject")
                        .HasColumnType("integer");

                    b.HasKey("Id_Object");

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("TDocument", b =>
                {
                    b.HasOne("TCategoryDocument", "Category")
                        .WithMany("Documents")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TNomenclature", "DocumentType")
                        .WithMany("Documents")
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TObject", "Object")
                        .WithMany("Documents")
                        .HasForeignKey("ObjectId");

                    b.Navigation("Category");

                    b.Navigation("DocumentType");

                    b.Navigation("Object");
                });

            modelBuilder.Entity("TCategoryDocument", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("TNomenclature", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("TObject", b =>
                {
                    b.Navigation("Documents");
                });
#pragma warning restore 612, 618
        }
    }
}
