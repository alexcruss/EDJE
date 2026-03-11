using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImmobiliere.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du document est requis")]
        [StringLength(200)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } // PDF, Image, Word, Excel, etc.

        [Required]
        public string Chemin { get; set; } // URL ou chemin du fichier

        [DataType(DataType.Date)]
        public DateTime DateUpload { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Taille { get; set; } // En Mo

        [StringLength(100)]
        public string Categorie { get; set; } // Contrat, Justificatif, Facture, Quittance, etc.

        // Clés étrangères optionnelles (un document peut être lié à différentes entités)
        public int? ContratId { get; set; }
        public int? ChargeId { get; set; }

        // Relations
        [ForeignKey("ContratId")]
        public virtual Contrat Contrat { get; set; }

        [ForeignKey("ChargeId")]
        public virtual Charge Charge { get; set; }

        // Propriétés calculées
        [NotMapped]
        public string Extension => Type.StartsWith(".") ? Type : $".{Type}";

        [NotMapped]
        public string TailleFormatee => Taille < 1 
            ? $"{Taille * 1024:F2} Ko" 
            : $"{Taille:F2} Mo";
    }
}
