﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RPFBE.Auth;

namespace RPFBE.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220511082335_Exit card n Form")]
    partial class ExitcardnForm
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RPFBE.Auth.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<string>("Rank")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.AppliedJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Deadline")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobAppplicationNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobReqNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Viewed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("AppliedJobs");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.ExitInterviewCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InterviewDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Interviewer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtherReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reemploy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SeparationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SeparationGround")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExitInterviewCard");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.ExitInterviewForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminPolicy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AnyOtherSuggetionQ")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AppropriateChallengingAssignments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Careerdevops")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClearlyCommExpectation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoarchedTrainedDev")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommitmentCustServ")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Communicationmgtemp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcernedQualityExcellence")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cooperationinoffice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Decisionaffected")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employeemorale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employeeorientation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EncouragedTeamworkCoop")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExitCardRef")
                        .HasColumnType("int");

                    b.Property<string>("Fairnessofworkload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fairtreatment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Interestinvemp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KeptTeamInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ListeningToSuggetions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manager")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NowDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Payment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Performancedevplan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvidedLeadership")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecognitionAccomp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Recognitionofwelldone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Recruitmentprocess")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResolvedConcerns")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rxtioncoworker")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Supervisonreceived")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Suportofworklifebal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupportedWorkLifeBal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TheJobLeaving")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TheOrgoverla")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Toolsprovided")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Trainingopportunity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Trainingreceived")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TreatedFairly")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Typeofwork")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Typeworkperformed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Whatulldosummarydous")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Workingcondition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkingconditionOne")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("YourSupervisorMgr")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExitCardRef")
                        .IsUnique();

                    b.ToTable("ExitInterviewForm");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.JobSpecFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TagName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SpecFiles");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.JustificationFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReqNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TagName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("JustificationFiles");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.MonitoringDoc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Filepath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MonitoringID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MonitoringDoc");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.MonitoringDocView", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MonitoringDocId")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Viewed")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MonitoringDocId");

                    b.ToTable("MonitoringDocView");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.MonitoringSupportingDoc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MonitorId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TagName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MonitoringSupportingDoc");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.PerformanceMonitoring", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApprovalStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HODId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HRId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManagerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PerformanceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Progresscode")
                        .HasColumnType("int");

                    b.Property<string>("StaffName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PerformanceMonitoring");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Age")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankBranchCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankBranchName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BirthCertificateNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Citizenship")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentSalary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DOB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DriverLincenceNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ethnicgroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpectedSalary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Experience")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HighestEducation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HudumaNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaritalStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobilePhoneNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobilePhoneNoAlt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NHIFNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NSSFNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalIDNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassPortNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonWithDisability")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PinNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Religion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResidentialAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubCounty")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SurName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WillingtoRelocate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.RequisitionProgress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClosingDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobGrade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProgressStatus")
                        .HasColumnType("int");

                    b.Property<string>("ReqID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestedEmployees")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UIDFour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UIDThree")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UIDTwo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RequisitionProgress");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExperienceYears")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.UserCV", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TagName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserCVs");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RPFBE.Auth.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RPFBE.Auth.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RPFBE.Auth.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RPFBE.Auth.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.ExitInterviewForm", b =>
                {
                    b.HasOne("RPFBE.Model.DBEntity.ExitInterviewCard", "ExitInterviewCard")
                        .WithOne("ExitInterviewForm")
                        .HasForeignKey("RPFBE.Model.DBEntity.ExitInterviewForm", "ExitCardRef")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RPFBE.Model.DBEntity.MonitoringDocView", b =>
                {
                    b.HasOne("RPFBE.Model.DBEntity.MonitoringDoc", "MonitoringDoc")
                        .WithMany("MonitoringDocView")
                        .HasForeignKey("MonitoringDocId");
                });
#pragma warning restore 612, 618
        }
    }
}
