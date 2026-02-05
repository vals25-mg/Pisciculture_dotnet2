using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pisciculture_dotnet2.Models;

public partial class PiscicultureDbContext : DbContext
{
    public PiscicultureDbContext()
    {
    }

    public PiscicultureDbContext(DbContextOptions<PiscicultureDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aliment> Aliments { get; set; }

    public virtual DbSet<CroissancePoissonDobo> CroissancePoissonDobos { get; set; }

    public virtual DbSet<CroissanceRace> CroissanceRaces { get; set; }

    public virtual DbSet<Dobo> Dobos { get; set; }

    public virtual DbSet<EntreVague> EntreVagues { get; set; }

    public virtual DbSet<Nourrissage> Nourrissages { get; set; }

    public virtual DbSet<PoissonDobo> PoissonDobos { get; set; }
    public virtual DbSet<VNourrissage> VNourrissages { get; set; }
    public virtual DbSet<VNourissagePrixAliment> VNourissagePrixAliments { get; set; }

    public virtual DbSet<Race> Races { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aliment>(entity =>
        {
            entity.HasKey(e => e.IdAliment).HasName("aliment_pkey");

            entity.ToTable("aliment");

            entity.Property(e => e.IdAliment)
                .ValueGeneratedNever()
                .HasColumnName("id_aliment");
            entity.Property(e => e.NomAliment)
                .HasMaxLength(100)
                .HasColumnName("nom_aliment");
            entity.Property(e => e.PourcentageGlucide).HasColumnName("pourcentage_glucide");
            entity.Property(e => e.PourcentageProteine).HasColumnName("pourcentage_proteine");
            entity.Property(e => e.PrixAchatKg).HasColumnName("prix_achat_kg");
        });

        modelBuilder.Entity<CroissancePoissonDobo>(entity =>
        {
            entity.HasKey(e => e.IdCroissancePoissonDobo).HasName("croissance_poisson_dobo_pkey");

            entity.ToTable("croissance_poisson_dobo");

            entity.Property(e => e.IdCroissancePoissonDobo).HasColumnName("id_croissance_poisson_dobo");
            entity.Property(e => e.DateCroissance)
                .HasColumnName("date_croissance");
            entity.Property(e => e.IdPoissonDobo)
                .HasMaxLength(20)
                .HasColumnName("id_poisson_dobo");
            entity.Property(e => e.PoidsRecuKg).HasColumnName("poids_recu_kg");

            entity.HasOne(d => d.IdPoissonDoboNavigation).WithMany(p => p.CroissancePoissonDobos)
                .HasForeignKey(d => d.IdPoissonDobo)
                .HasConstraintName("croissance_poisson_dobo_id_poisson_dobo_fkey");
        });

        modelBuilder.Entity<CroissanceRace>(entity =>
        {
            entity.HasKey(e => e.IdCroissanceRace).HasName("croissance_race_pkey");

            entity.ToTable("croissance_race");

            entity.Property(e => e.IdCroissanceRace).HasColumnName("id_croissance_race");
            entity.Property(e => e.ApportGlucideG).HasColumnName("apport_glucide_g");
            entity.Property(e => e.ApportProteineG).HasColumnName("apport_proteine_g");
            entity.Property(e => e.IdRace).HasColumnName("id_race");
            entity.Property(e => e.PoidsObtenuG).HasColumnName("poids_obtenu_g");

            entity.HasOne(d => d.IdRaceNavigation).WithMany(p => p.CroissanceRaces)
                .HasForeignKey(d => d.IdRace)
                .HasConstraintName("croissance_race_id_race_fkey");
        });

        modelBuilder.Entity<Dobo>(entity =>
        {
            entity.HasKey(e => e.IdDobo).HasName("dobo_pkey");

            entity.ToTable("dobo");

            entity.Property(e => e.IdDobo)
                .HasMaxLength(30)
                .HasColumnName("id_dobo");
        });

        modelBuilder.Entity<EntreVague>(entity =>
        {
            entity.HasKey(e => e.IdEntreVague).HasName("entre_vague_pkey");

            entity.ToTable("entre_vague");

            entity.Property(e => e.IdEntreVague)
                .HasDefaultValueSql("nextval('seq_entree_vague'::regclass)")
                .HasColumnName("id_entre_vague");
            entity.Property(e => e.DateEntree).HasColumnName("date_entree");
            entity.Property(e => e.IdDobo)
                .HasMaxLength(30)
                .HasColumnName("id_dobo");
            entity.Property(e => e.IdRace).HasColumnName("id_race");
            entity.Property(e => e.NombrePoissons).HasColumnName("nombre_poissons");
            entity.Property(e => e.PoidsInitialePoisson).HasColumnName("poids_initiale_poisson");

            entity.HasOne(d => d.IdDoboNavigation).WithMany(p => p.EntreVagues)
                .HasForeignKey(d => d.IdDobo)
                .HasConstraintName("entre_vague_id_dobo_fkey");

            entity.HasOne(d => d.IdRaceNavigation).WithMany(p => p.EntreVagues)
                .HasForeignKey(d => d.IdRace)
                .HasConstraintName("entre_vague_id_race_fkey");
        });

        modelBuilder.Entity<Nourrissage>(entity =>
        {
            entity.HasKey(e => e.IdNourrissage).HasName("nourrissage_pkey");

            entity.ToTable("nourrissage");

            entity.Property(e => e.IdNourrissage)
                .HasDefaultValueSql("nextval('seq_nourrissage'::regclass)")
                .HasColumnName("id_nourrissage");
            entity.Property(e => e.DateNourrissage).HasColumnName("date_nourrissage");
            entity.Property(e => e.IdAliment).HasColumnName("id_aliment");
            entity.Property(e => e.IdDobo)
                .HasMaxLength(30)
                .HasColumnName("id_dobo");
            entity.Property(e => e.PoidsAliments).HasColumnName("poids_aliments");

            entity.HasOne(d => d.IdAlimentNavigation).WithMany(p => p.Nourrissages)
                .HasForeignKey(d => d.IdAliment)
                .HasConstraintName("nourrissage_id_aliment_fkey");

            entity.HasOne(d => d.IdDoboNavigation).WithMany(p => p.Nourrissages)
                .HasForeignKey(d => d.IdDobo)
                .HasConstraintName("nourrissage_id_dobo_fkey");
        });

        modelBuilder.Entity<PoissonDobo>(entity =>
        {
            entity.HasKey(e => e.IdPoissonDobo).HasName("poisson_dobo_pkey");

            entity.ToTable("poisson_dobo");

            entity.Property(e => e.IdPoissonDobo)
                .HasMaxLength(20)
                .HasDefaultValueSql("('POIS0'::text || nextval('seq_poisson_dobo'::regclass))")
                .HasColumnName("id_poisson_dobo");
            entity.Property(e => e.IdEntreVague).HasColumnName("id_entre_vague");
            entity.Property(e => e.IdRace).HasColumnName("id_race");
            entity.Property(e => e.PoidsInitialePoisson).HasColumnName("poids_initiale_poisson");

            entity.HasOne(d => d.IdEntreVagueNavigation).WithMany(p => p.PoissonDobos)
                .HasForeignKey(d => d.IdEntreVague)
                .HasConstraintName("poisson_dobo_id_entre_vague_fkey");

            entity.HasOne(d => d.IdRaceNavigation).WithMany(p => p.PoissonDobos)
                .HasForeignKey(d => d.IdRace)
                .HasConstraintName("poisson_dobo_id_race_fkey");
        });

        modelBuilder.Entity<Race>(entity =>
        {
            entity.HasKey(e => e.IdRace).HasName("race_pkey");

            entity.ToTable("race");

            entity.Property(e => e.IdRace)
                .ValueGeneratedNever()
                .HasColumnName("id_race");
            entity.Property(e => e.NomRace)
                .HasMaxLength(50)
                .HasColumnName("nom_race");
            entity.Property(e => e.PoidsMax).HasColumnName("poids_max");
            entity.Property(e => e.PrixAchatKg).HasColumnName("prix_achat_kg");
            entity.Property(e => e.PrixVenteKg).HasColumnName("prix_vente_kg");
        });
        
        modelBuilder.Entity<VNourrissage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_nourrissage");

            entity.Property(e => e.DateNourrissage).HasColumnName("date_nourrissage");
            entity.Property(e => e.IdAliment).HasColumnName("id_aliment");
            entity.Property(e => e.IdDobo)
                .HasMaxLength(30)
                .HasColumnName("id_dobo");
            entity.Property(e => e.IdNourrissage).HasColumnName("id_nourrissage");
            entity.Property(e => e.NomAliment)
                .HasMaxLength(100)
                .HasColumnName("nom_aliment");
            entity.Property(e => e.PoidsAliments).HasColumnName("poids_aliments");
            entity.Property(e => e.PourcentageGlucide).HasColumnName("pourcentage_glucide");
            entity.Property(e => e.PourcentageProteine).HasColumnName("pourcentage_proteine");
            entity.Property(e => e.PrixAchatKg).HasColumnName("prix_achat_kg");
        });
        
        modelBuilder.Entity<VNourissagePrixAliment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_nourissage_prix_aliment");

            entity.Property(e => e.DateNourrissage).HasColumnName("date_nourrissage");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAliment).HasColumnName("id_aliment");
            entity.Property(e => e.IdDobo)
                .HasMaxLength(30)
                .HasColumnName("id_dobo");
            entity.Property(e => e.NomAliment)
                .HasMaxLength(100)
                .HasColumnName("nom_aliment");
            entity.Property(e => e.PoidsAliments).HasColumnName("poids_aliments");
            entity.Property(e => e.PrixAchatKg).HasColumnName("prix_achat_kg");
            entity.Property(e => e.PrixTotal).HasColumnName("prix_total");
        });
        modelBuilder.HasSequence("seq_entree_vague");
        modelBuilder.HasSequence("seq_nourrissage");
        modelBuilder.HasSequence("seq_poisson_dobo");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
