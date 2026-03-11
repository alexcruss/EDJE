using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImmobiliere.Models
{
    public class Charge
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le type de charge est requis")]
        [StringLength(100)]
        public string Type { get; set; } // Travaux, Entretien, Assurance, Taxe foncière, etc.

        [Required(ErrorMessage = "Le montant est requis")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Montant { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Categorie { get; set; } // Réparation, Amélioration, Gestion, Fiscale

        public bool Deductible { get; set; } = true;

        // Clé étrangère
        [Required]
        public int BatimentId { get; set; }

        // Relations
        [ForeignKey("BatimentId")]
        public virtual Batiment Batiment { get; set; }

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

        // Propriétés calculées
        [NotMapped]
        public string Annee => Date.Year.ToString();
    }
}
