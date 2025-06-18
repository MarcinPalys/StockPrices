using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StockPrices.Core.Entities;
using StockPrices.Infrastructure.Entities;

namespace StockPrices.Infrastructure;

public partial class StockPricesDbContext : DbContext
{
    public StockPricesDbContext()
    {
    }

    public StockPricesDbContext(DbContextOptions<StockPricesDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<StockPrice> StockPrices { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<FavoriteStock> FavoriteStocks { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=StockPricesDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<StockPrice>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Close)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("close");
            entity.Property(e => e.Date)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("date");
            entity.Property(e => e.High)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("high");
            entity.Property(e => e.Low)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("low");
            entity.Property(e => e.Open)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("open");
            entity.Property(e => e.Symbol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("symbol");
            entity.Property(e => e.Volume)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("volume");
        });

        // Relacja User - Comment
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacja User - FavoriteStock
        modelBuilder.Entity<FavoriteStock>()
            .HasOne(f => f.User)
            .WithMany(u => u.FavoriteStocks)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
