using System;
using System.Collections.Generic;
using Kursovaya_var6.Models;
using Microsoft.EntityFrameworkCore;

namespace Kursovaya_var6.Data;

public partial class MBContext : DbContext
{
    public MBContext()
    {
    }

    public MBContext(DbContextOptions<MBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Band> Bands { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<GroupName> GroupNames { get; set; }

    public virtual DbSet<Leader> Leaders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Band>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bands__3214EC0715BEF2CE");

            entity.HasOne(d => d.Genre).WithMany(p => p.Bands)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_Bands_Genres");

            entity.HasOne(d => d.GroupName).WithMany(p => p.Bands)
                .HasForeignKey(d => d.GroupNameId)
                .HasConstraintName("FK_Bands_GroupNames");

            entity.HasOne(d => d.Leader).WithMany(p => p.Bands)
                .HasForeignKey(d => d.LeaderId)
                .HasConstraintName("FK_Bands_Leaders");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC0753E81BFD");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<GroupName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupNam__3214EC07D0F2B32F");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Leader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leaders__3214EC079F5CCFD3");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
