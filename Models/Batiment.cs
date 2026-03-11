using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImmobiliere.Models
{
    public class Batiment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'adresse est requise")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "La ville est requise")]
        [StringLength(100)]
        public string Ville { get; set; }

        [Required(ErrorMessage = "Le code postal est requis")]
        [StringLength(10)]
        public string CodePostal { get; set; }

        [StringLength(50)]
        public string Type { get; set; } // Immeuble, Maison, Résidence, etc.

        public int NombreLogements { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal SurfaceTotale { get; set; }

        public DateTime DateAcquisition { get; set; }

        // Clé étrangère
        [Required]
        public int ProprietaireId { get; set; }

        // Relations
        [ForeignKey("ProprietaireId")]
        public virtual Proprietaire Proprietaire { get; set; }
        
        public virtual ICollection<Logement> Logements { get; set; } = new List<Logement>();
        public virtual ICollection<Charge> Charges { get; set; } = new List<Charge>();

        // Propriété calculée
        public string AdresseComplete => $"{Adresse}, {CodePostal} {Ville}";
    }
}
