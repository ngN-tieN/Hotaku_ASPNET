using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using Azure.Identity;
//using Azure.Security.KeyVault.Secrets;
using DotNetEnv;

namespace Hotaku.Persistence;

public partial class HotakuContext : DbContext
{
    public HotakuContext()
    {
    }

    public HotakuContext(DbContextOptions<HotakuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Manga> Mangas { get; set; }

    public virtual DbSet<MangaChapter> MangaChapters { get; set; }

    public virtual DbSet<MangaPage> MangaPages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserFavoriteManga> UserFavoriteMangas { get; set; }

    public virtual DbSet<UserMangaHistory> UserMangaHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //if (!optionsBuilder.IsConfigured)
        //{
        //    var keyVaultName = "hotaku";
        //    var kvUri = $"https://{keyVaultName}.vault.azure.net/";

        //    var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

        //    KeyVaultSecret secret = client.GetSecret("HotakuDbConnection");
        //    string connectionString = secret.Value;
        //    optionsBuilder.UseNpgsql(connectionString);
        //}
        DotNetEnv.Env.Load();
        var connectionString = System.Environment.GetEnvironmentVariable("HotakuDbConnection");

        System.Diagnostics.Debug.WriteLine(connectionString);

        if (connectionString != null)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("Authors_pkey");

            entity.Property(e => e.AuthorId).HasMaxLength(12);
            entity.Property(e => e.AuthorName).HasColumnType("character varying");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("Categories_pkey");

            entity.Property(e => e.CategoryId).HasMaxLength(12);
            entity.Property(e => e.CategoryName).HasColumnType("character varying");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("Groups_pkey");

            entity.Property(e => e.GroupId).HasMaxLength(12);
            entity.Property(e => e.GroupName).HasColumnType("character varying");
        });

        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(e => e.MangaId).HasName("Manga_pkey");

            entity.ToTable("Manga");

            entity.Property(e => e.MangaId).HasMaxLength(12);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.Status).HasColumnType("character varying");
            entity.Property(e => e.Title).HasColumnType("character varying");

            entity.HasMany(d => d.Authors).WithMany(p => p.Mangas)
                .UsingEntity<Dictionary<string, object>>(
                    "MangaAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MangaAuthors_AuthorId_fkey"),
                    l => l.HasOne<Manga>().WithMany()
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MangaAuthors_MangaId_fkey"),
                    j =>
                    {
                        j.HasKey("MangaId", "AuthorId").HasName("MangaAuthors_pkey");
                        j.ToTable("MangaAuthors");
                        j.IndexerProperty<string>("MangaId").HasMaxLength(12);
                        j.IndexerProperty<string>("AuthorId").HasMaxLength(12);
                    });

            entity.HasMany(d => d.Categories).WithMany(p => p.Mangas)
                .UsingEntity<Dictionary<string, object>>(
                    "MangaCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MangaCategories_CategoryId_fkey"),
                    l => l.HasOne<Manga>().WithMany()
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MangaCategories_MangaId_fkey"),
                    j =>
                    {
                        j.HasKey("MangaId", "CategoryId").HasName("MangaCategories_pkey");
                        j.ToTable("MangaCategories");
                        j.IndexerProperty<string>("MangaId").HasMaxLength(12);
                        j.IndexerProperty<string>("CategoryId").HasMaxLength(12);
                    });

            entity.HasMany(d => d.Groups).WithMany(p => p.Mangas)
                .UsingEntity<Dictionary<string, object>>(
                    "MangaGroup",
                    r => r.HasOne<Group>().WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MangaGroups_GroupId_fkey"),
                    l => l.HasOne<Manga>().WithMany()
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MangaGroups_MangaId_fkey"),
                    j =>
                    {
                        j.HasKey("MangaId", "GroupId").HasName("MangaGroups_pkey");
                        j.ToTable("MangaGroups");
                        j.IndexerProperty<string>("MangaId").HasMaxLength(12);
                        j.IndexerProperty<string>("GroupId").HasMaxLength(12);
                    });
        });

        modelBuilder.Entity<MangaChapter>(entity =>
        {
            entity.HasKey(e => e.ChapterId).HasName("MangaChapters_pkey");

            entity.Property(e => e.ChapterId).HasMaxLength(12);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.MangaId).HasMaxLength(12);
            entity.Property(e => e.Title).HasColumnType("character varying");

            entity.HasOne(d => d.Manga).WithMany(p => p.MangaChapters)
                .HasForeignKey(d => d.MangaId)
                .HasConstraintName("MangaChapters_MangaId_fkey");
        });

        modelBuilder.Entity<MangaPage>(entity =>
        {
            entity.HasKey(e => e.PageId).HasName("MangaPages_pkey");

            entity.Property(e => e.PageId).HasMaxLength(12);
            entity.Property(e => e.ChapterId).HasMaxLength(12);
            entity.Property(e => e.ImageUrl).HasColumnType("character varying");

            entity.HasOne(d => d.Chapter).WithMany(p => p.MangaPages)
                .HasForeignKey(d => d.ChapterId)
                .HasConstraintName("MangaPages_ChapterId_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("Notifications_pkey");

            entity.Property(e => e.NotificationId).HasMaxLength(12);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Message).HasColumnType("character varying");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pkey");

            entity.Property(e => e.UserId).HasMaxLength(12);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email).HasColumnType("character varying");

            entity.HasMany(d => d.Notifications).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserNotification",
                    r => r.HasOne<Notification>().WithMany()
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserNotifications_NotificationId_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserNotifications_UserId_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "NotificationId").HasName("UserNotifications_pkey");
                        j.ToTable("UserNotifications");
                        j.IndexerProperty<string>("UserId").HasMaxLength(12);
                        j.IndexerProperty<string>("NotificationId").HasMaxLength(12);
                    });
        });

        modelBuilder.Entity<UserFavoriteManga>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("UserFavoriteManga_pkey");

            entity.ToTable("UserFavoriteManga");

            entity.Property(e => e.FavoriteId).HasMaxLength(12);
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.MangaId).HasMaxLength(12);
            entity.Property(e => e.UserId).HasMaxLength(12);

            entity.HasOne(d => d.Manga).WithMany(p => p.UserFavoriteMangas)
                .HasForeignKey(d => d.MangaId)
                .HasConstraintName("UserFavoriteManga_MangaId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserFavoriteMangas)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserFavoriteManga_UserId_fkey");
        });

        modelBuilder.Entity<UserMangaHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("UserMangaHistory_pkey");

            entity.ToTable("UserMangaHistory");

            entity.Property(e => e.HistoryId).HasMaxLength(12);
            entity.Property(e => e.MangaId).HasMaxLength(12);
            entity.Property(e => e.ReadAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ReadChapterIds).HasColumnType("character varying(12)[]");
            entity.Property(e => e.UserId).HasMaxLength(12);

            entity.HasOne(d => d.Manga).WithMany(p => p.UserMangaHistories)
                .HasForeignKey(d => d.MangaId)
                .HasConstraintName("UserMangaHistory_MangaId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserMangaHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserMangaHistory_UserId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
