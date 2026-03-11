using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImmobiliere.Models
{
    public class Contrat
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La date de début est requise")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage = "Le loyer mensuel est requis")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal LoyerMensuel { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Charges { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Caution { get; set; }

        [StringLength(50)]
        public string TypeContrat { get; set; } = "Location nue"; // Location nue, Meublée, etc.

        [StringLength(20)]
        public string Statut { get; set; } = "Actif"; // Actif, Résilié, Expiré

        // Clés étrangères
        [Required]
        public int LogementId { get; set; }

        [Required]
        public int LocataireId { get; set; }

        // Relations
        [ForeignKey("LogementId")]
        public virtual Logement Logement { get; set; }

        [ForeignKey("LocataireId")]
        public virtual Locataire Locataire { get; set; }

        public virtual ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

        // Propriétés calculées
        [NotMapped]
        public decimal MontantTotal => LoyerMensuel + Charges;

        [NotMapped]
        public bool EstActif => Statut == "Actif" && 
                               (DateFin == null || DateFin > DateTime.Now);

        [NotMapped]
        public int DureeEnMois
        {
            get
            {
                var dateFin = DateFin ?? DateTime.Now;
                return ((dateFin.Year - DateDebut.Year) * 12) + dateFin.Month - DateDebut.Month;
            }
        }
    }
}
