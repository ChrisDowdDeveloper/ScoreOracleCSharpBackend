﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScoreOracleCSharp;

#nullable disable

namespace ScoreOracleCSharp.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20240503234040_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ScoreOracleCSharp.Models.Friendship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateEstablished")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int?>("RequesterId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("RequesterId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AwayTeamId")
                        .HasColumnType("int");

                    b.Property<int>("AwayTeamScore")
                        .HasColumnType("int");

                    b.Property<DateOnly>("GameDate")
                        .HasColumnType("date");

                    b.Property<int>("GameStatus")
                        .HasColumnType("int");

                    b.Property<int?>("HomeTeamId")
                        .HasColumnType("int");

                    b.Property<int>("HomeTeamScore")
                        .HasColumnType("int");

                    b.Property<int?>("SportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("SportId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.GroupMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("JoinedAt")
                        .HasColumnType("date");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Injury", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("Injuries");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Leaderboard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SportId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.ToTable("Leaderboards");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SportId")
                        .HasColumnType("int");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Prediction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("PredictedAwayTeamScore")
                        .HasColumnType("int");

                    b.Property<int>("PredictedHomeTeamScore")
                        .HasColumnType("int");

                    b.Property<int?>("PredictedTeamId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("PredictionDate")
                        .HasColumnType("date");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("Predictions");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Sport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("League")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LeaderboardId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.UserScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("LeaderboardId")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedLast")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeaderboardId");

                    b.HasIndex("UserId");

                    b.ToTable("UserScores");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Friendship", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.User", "Receiver")
                        .WithMany("ReceivedFriendships")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ScoreOracleCSharp.Models.User", "Requester")
                        .WithMany("RequestedFriendships")
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Receiver");

                    b.Navigation("Requester");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Game", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Team", "AwayTeam")
                        .WithMany("AwayGames")
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ScoreOracleCSharp.Models.Team", "HomeTeam")
                        .WithMany("HomeGames")
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ScoreOracleCSharp.Models.Sport", "Sport")
                        .WithMany("Games")
                        .HasForeignKey("SportId");

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Group", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.GroupMember", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Group", "Group")
                        .WithMany("Members")
                        .HasForeignKey("GroupId");

                    b.HasOne("ScoreOracleCSharp.Models.User", "User")
                        .WithMany("GroupsJoined")
                        .HasForeignKey("UserId");

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Injury", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Player", "Player")
                        .WithMany("PlayerInjury")
                        .HasForeignKey("PlayerId");

                    b.HasOne("ScoreOracleCSharp.Models.Team", "Team")
                        .WithMany("InjuriesOnTeam")
                        .HasForeignKey("TeamId");

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Leaderboard", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Sport", "Sport")
                        .WithMany("LeaderboardsBySport")
                        .HasForeignKey("SportId");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Notification", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Player", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Sport", null)
                        .WithMany("PlayersInSport")
                        .HasForeignKey("SportId");

                    b.HasOne("ScoreOracleCSharp.Models.Team", "Team")
                        .WithMany("PlayersOnTeam")
                        .HasForeignKey("TeamId");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Prediction", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Game", "Game")
                        .WithMany("GamePrediction")
                        .HasForeignKey("GameId");

                    b.HasOne("ScoreOracleCSharp.Models.Team", "Team")
                        .WithMany("TeamPredicted")
                        .HasForeignKey("TeamId");

                    b.HasOne("ScoreOracleCSharp.Models.User", "User")
                        .WithMany("PredictionsByPlayer")
                        .HasForeignKey("UserId");

                    b.Navigation("Game");

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Team", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Sport", "Sport")
                        .WithMany("Teams")
                        .HasForeignKey("SportId");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.User", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Leaderboard", null)
                        .WithMany("Users")
                        .HasForeignKey("LeaderboardId");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.UserScore", b =>
                {
                    b.HasOne("ScoreOracleCSharp.Models.Leaderboard", "Leaderboard")
                        .WithMany("ScoreByUser")
                        .HasForeignKey("LeaderboardId");

                    b.HasOne("ScoreOracleCSharp.Models.User", "User")
                        .WithMany("UserScores")
                        .HasForeignKey("UserId");

                    b.Navigation("Leaderboard");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Game", b =>
                {
                    b.Navigation("GamePrediction");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Group", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Leaderboard", b =>
                {
                    b.Navigation("ScoreByUser");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Player", b =>
                {
                    b.Navigation("PlayerInjury");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Sport", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("LeaderboardsBySport");

                    b.Navigation("PlayersInSport");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.Team", b =>
                {
                    b.Navigation("AwayGames");

                    b.Navigation("HomeGames");

                    b.Navigation("InjuriesOnTeam");

                    b.Navigation("PlayersOnTeam");

                    b.Navigation("TeamPredicted");
                });

            modelBuilder.Entity("ScoreOracleCSharp.Models.User", b =>
                {
                    b.Navigation("GroupsJoined");

                    b.Navigation("Notifications");

                    b.Navigation("PredictionsByPlayer");

                    b.Navigation("ReceivedFriendships");

                    b.Navigation("RequestedFriendships");

                    b.Navigation("UserScores");
                });
#pragma warning restore 612, 618
        }
    }
}
