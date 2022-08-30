using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Repositories.Entities;
using StudentEntity = Repositories.Entities.StudentEntity;

namespace Repositories
{
    public partial class DepartmentContext : DbContext
    {
        public DepartmentContext()
        {
        }

        public DepartmentContext(DbContextOptions<DepartmentContext> options)
            : base(options)
        {
            Database.EnsureCreated();

        }

        public virtual DbSet<GroupEntity> Groups { get; set; } = null!;
        public virtual DbSet<PointEntity> Points { get; set; } = null!;
        public virtual DbSet<ProfessorEntity> Professors { get; set; } = null!;
        public virtual DbSet<StudentEntity> Students { get; set; } = null!;
        public virtual DbSet<StudentEntity_PointEntity> StudentPoints { get; set; } = null!;
        public virtual DbSet<UserEntity> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(
                "Data Source=DESKTOP-JB2B996;Initial Catalog=Department;Integrated Security=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupEntity>(entity =>
            {
                entity.ToTable("Group");

                entity.HasIndex(e => e.ProfessorId, "IX_Group_ProfessorId");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ProfessorId).HasMaxLength(50);

                entity.HasOne(d => d.Professor).WithMany(p => p.Groups).HasForeignKey(d => d.ProfessorId)
                    .HasConstraintName("FK_Group_Professor");
            });

            modelBuilder.Entity<PointEntity>(entity =>
            {
                entity.ToTable("Points");
                
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Point).HasColumnName("Point");

                entity.Property(e => e.Subject).HasMaxLength(50);
            });

            modelBuilder.Entity<ProfessorEntity>(entity =>
            {
                entity.ToTable("Professor");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Subject).HasMaxLength(50);
            });

            modelBuilder.Entity<StudentEntity>(entity =>
            {
                entity.ToTable("Student");

                entity.HasIndex(e => e.GroupId, "IX_Student_GroupId");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.GroupId).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.HasOne(d => d.Group).WithMany(p => p.Students).HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Student_Group");
            });

            modelBuilder.Entity<StudentEntity_PointEntity>(entity =>
            {

                entity.ToTable("StudentPoint");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasMaxLength(50);
                
                entity.Property(e => e.PointId).HasMaxLength(50);

                entity.Property(e => e.StudentId).HasMaxLength(50);

                entity.HasOne(d => d.PointEntity).WithMany(p => p.StudentPoints).HasForeignKey(d => d.PointId)
                    .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_StudentPoint_Point");

                entity.HasOne(d => d.StudentEntity).WithMany(s => s.StudentPoints).HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_StudentPoint_Student");
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}