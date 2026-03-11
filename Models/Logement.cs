using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImmobiliere.Models
{
    public class Logement
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le numéro est requis")]
        [StringLength(20)]
        public string Numero { get; set; } // Ex: "Apt 101", "Maison A"

        [StringLength(50)]
        public string Type { get; set; } // Studio, T1, T2, T3, etc.

        [Column(TypeName = "decimal(10,2)")]
        public decimal Surface { get; set; }

        public int NombrePieces { get; set; }

        [Required(ErrorMessage = "Le loyer mensuel est requis")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal LoyerMensuel { get; set; }

        [StringLength(20)]
        public string Statut { get; set; } = "Disponible"; // Disponible, Loué, En travaux

        // Clé étrangère
        [Required]
        public int BatimentId { get; set; }

        // Relations
        [ForeignKey("BatimentId")]
        public virtual Batiment Batiment { get; set; }
        
        public virtual ICollection<Contrat> Contrats { get; set; } = new List<Contrat>();

        // Propriétés calculées
        [NotMapped]
        public bool EstDisponible => Statut == "Disponible";

        [NotMapped]
        public string Description => $"{Type} - {Numero} ({Surface} m²)";
    }
}
