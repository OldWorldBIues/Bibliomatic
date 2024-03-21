﻿// <auto-generated />
using System;
using BibliomaticData.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BibliomaticData.Migrations
{
    [DbContext(typeof(BibliomaticAppContext))]
    [Migration("20231117110952_Infos")]
    partial class Infos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BibliomaticData.Models.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AnswerBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("BaseQuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsAnswer")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BaseQuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("BibliomaticData.Models.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ArticleDocumentSource")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArticleImageSource")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.AnswerFormulaInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AnswerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FormulaFilename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FormulaLatex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.ToTable("AnswerFormula");
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.AnswerHyperlinkInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AnswerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Hyperlink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HyperlinkDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.ToTable("AnswerHyperlinks");
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.AnswerImageInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AnswerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageFilename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.ToTable("AnswerImages");
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.QuestionFormulaInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FormulaFilename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FormulaLatex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionFormula");
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.QuestionHyperlinkInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Hyperlink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HyperlinkDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionHyperlinks");
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.QuestionImageInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageFilename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionImages");
                });

            modelBuilder.Entity("BibliomaticData.Models.BaseQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSolved")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("BaseQuestions");
                });

            modelBuilder.Entity("BibliomaticData.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BaseQuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QuestionBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BaseQuestionId")
                        .IsUnique()
                        .HasFilter("[BaseQuestionId] IS NOT NULL");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("BibliomaticData.Models.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("BibliomaticData.Models.TestAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCorrectAnswer")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TestQuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Variant")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TestQuestionId");

                    b.ToTable("TestAnswers");
                });

            modelBuilder.Entity("BibliomaticData.Models.TestQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("PointsPerAnswer")
                        .HasColumnType("float");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TestQuestionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("TestQuestions");
                });

            modelBuilder.Entity("BibliomaticData.Models.Answer", b =>
                {
                    b.HasOne("BibliomaticData.Models.BaseQuestion", "BaseQuestion")
                        .WithMany("Answers")
                        .HasForeignKey("BaseQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("BaseQuestion");
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.AnswerFormulaInfo", b =>
                {
                    b.HasOne("BibliomaticData.Models.Answer", null)
                        .WithMany("Formulas")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.AnswerHyperlinkInfo", b =>
                {
                    b.HasOne("BibliomaticData.Models.Answer", null)
                        .WithMany("Hyperlinks")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.AnswerImageInfo", b =>
                {
                    b.HasOne("BibliomaticData.Models.Answer", null)
                        .WithMany("Images")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.QuestionFormulaInfo", b =>
                {
                    b.HasOne("BibliomaticData.Models.Question", null)
                        .WithMany("Formulas")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.QuestionHyperlinkInfo", b =>
                {
                    b.HasOne("BibliomaticData.Models.Question", null)
                        .WithMany("Hyperlinks")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BibliomaticData.Models.AttachmentInfo.QuestionImageInfo", b =>
                {
                    b.HasOne("BibliomaticData.Models.Question", null)
                        .WithMany("Images")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BibliomaticData.Models.Question", b =>
                {
                    b.HasOne("BibliomaticData.Models.BaseQuestion", "BaseQuestion")
                        .WithOne("Question")
                        .HasForeignKey("BibliomaticData.Models.Question", "BaseQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("BaseQuestion");
                });

            modelBuilder.Entity("BibliomaticData.Models.TestAnswer", b =>
                {
                    b.HasOne("BibliomaticData.Models.TestQuestion", "TestQuestion")
                        .WithMany("TestAnswers")
                        .HasForeignKey("TestQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestQuestion");
                });

            modelBuilder.Entity("BibliomaticData.Models.TestQuestion", b =>
                {
                    b.HasOne("BibliomaticData.Models.Test", "Test")
                        .WithMany("TestQuestions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
                });

            modelBuilder.Entity("BibliomaticData.Models.Answer", b =>
                {
                    b.Navigation("Formulas");

                    b.Navigation("Hyperlinks");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("BibliomaticData.Models.BaseQuestion", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("BibliomaticData.Models.Question", b =>
                {
                    b.Navigation("Formulas");

                    b.Navigation("Hyperlinks");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("BibliomaticData.Models.Test", b =>
                {
                    b.Navigation("TestQuestions");
                });

            modelBuilder.Entity("BibliomaticData.Models.TestQuestion", b =>
                {
                    b.Navigation("TestAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}
