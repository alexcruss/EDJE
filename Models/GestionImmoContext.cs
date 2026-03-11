using GestionImmobiliere.Models;
using Microsoft.EntityFrameworkCore;
namespace GestionImmobiliere.Data
{
    public class GestionImmoContext : DbContext
    {
        public GestionImmoContext(DbContextOptions<GestionImmoContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Proprietaire> Proprietaires { get; set; }
        public DbSet<Batiment> Batiments { get; set; }
        public DbSet<Logement> Logements { get; set; }
        public DbSet<Locataire> Locataires { get; set; }
        public DbSet<Contrat> Contrats { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<Recu> Recus { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<DeclarationFiscale> DeclarationsFiscales { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DeclarationPaiement> DeclarationPaiements { get; set; }
        public DbSet<DeclarationCharge> DeclarationCharges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des relations

            // Proprietaire -> Batiment (One-to-Many)
            modelBuilder.Entity<Batiment>()
                .HasOne(b => b.Proprietaire)
                .WithMany(p => p.Batiments)
                .HasForeignKey(b => b.ProprietaireId)
                .OnDelete(DeleteBehavior.Restrict);

            // Batiment -> Logement (One-to-Many)
            modelBuilder.Entity<Logement>()
                .HasOne(l => l.Batiment)
                .WithMany(b => b.Logements)
                .HasForeignKey(l => l.BatimentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Logement -> Contrat (One-to-Many)
            modelBuilder.Entity<Contrat>()
                .HasOne(c => c.Logement)
                .WithMany(l => l.Contrats)
                .HasForeignKey(c => c.LogementId)
                .OnDelete(DeleteBehavior.Restrict);

            // Locataire -> Contrat (One-to-Many)
            modelBuilder.Entity<Contrat>()
                .HasOne(c => c.Locataire)
                .WithMany(l => l.Contrats)
                .HasForeignKey(c => c.LocataireId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contrat -> Paiement (One-to-Many)
            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.Contrat)
                .WithMany(c => c.Paiements)
                .HasForeignKey(p => p.ContratId)
                .OnDelete(DeleteBehavior.Cascade);

            // Paiement -> Recu (One-to-One)
            modelBuilder.Entity<Recu>()
                .HasOne(r => r.Paiement)
                .WithOne(p => p.Recu)
                .HasForeignKey<Recu>(r => r.PaiementId)
                .OnDelete(DeleteBehavior.Cascade);

            // Batiment -> Charge (One-to-Many)
            modelBuilder.Entity<Charge>()
                .HasOne(c => c.Batiment)
                .WithMany(b => b.Charges)
                .HasForeignKey(c => c.BatimentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Proprietaire -> DeclarationFiscale (One-to-Many)
            modelBuilder.Entity<DeclarationFiscale>()
                .HasOne(d => d.Proprietaire)
                .WithMany(p => p.DeclarationsFiscales)
                .HasForeignKey(d => d.ProprietaireId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration des index pour améliorer les performances
            modelBuilder.Entity<Proprietaire>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Locataire>()
                .HasIndex(l => l.Email)
                .IsUnique();

            modelBuilder.Entity<Recu>()
                .HasIndex(r => r.NumeroRecu)
                .IsUnique();

            modelBuilder.Entity<DeclarationFiscale>()
                .HasIndex(d => new { d.ProprietaireId, d.Annee })
                .IsUnique();

            // Configuration des valeurs par défaut
            modelBuilder.Entity<Proprietaire>()
                .Property(p => p.DateInscription)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Paiement>()
                .Property(p => p.Statut)
                .HasDefaultValue("En attente");

            modelBuilder.Entity<Logement>()
                .Property(l => l.Statut)
                .HasDefaultValue("Disponible");

            modelBuilder.Entity<Contrat>()
                .Property(c => c.Statut)
                .HasDefaultValue("Actif");
        }
    }
}
