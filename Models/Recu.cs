using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImmobiliere.Models
{
    public class Recu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NumeroRecu { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateEmission { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontantTotal { get; set; }

        [Required]
        [StringLength(50)]
        public string Periode { get; set; } // Ex: "Janvier 2025"

        [StringLength(20)]
        public string FormatDocument { get; set; } = "PDF"; // PDF, Email

        // Clé étrangère
        [Required]
        public int PaiementId { get; set; }

        // Relation
        [ForeignKey("PaiementId")]
        public virtual Paiement Paiement { get; set; }

        // Propriété calculée
        [NotMapped]
        public string NomFichier => $"Recu_{NumeroRecu}_{Periode.Replace(" ", "_")}.pdf";
    }
}
