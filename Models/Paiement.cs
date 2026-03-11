using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImmobiliere.Models
{
    public class Paiement
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La date de paiement est requise")]
        [DataType(DataType.Date)]
        public DateTime DatePaiement { get; set; }

        [Required(ErrorMessage = "La date d'échéance est requise")]
        [DataType(DataType.Date)]
        public DateTime DateDue { get; set; }

        [Required(ErrorMessage = "Le montant est requis")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Montant { get; set; }

        [Required]
        [StringLength(50)]
        public string TypePaiement { get; set; } = "Virement"; // Virement, Chèque, Espèces, Prélèvement

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "En attente"; // En attente, Payé, En retard, Annulé

        [StringLength(100)]
        public string Reference { get; set; }

        // Clé étrangère
        [Required]
        public int ContratId { get; set; }

        // Relations
        [ForeignKey("ContratId")]
        public virtual Contrat Contrat { get; set; }

        public virtual Recu Recu { get; set; }

        // Propriétés calculées
        [NotMapped]
        public bool EstEnRetard => Statut != "Payé" && DateDue < DateTime.Now;

        [NotMapped]
        public int JoursRetard => EstEnRetard ? (DateTime.Now - DateDue).Days : 0;

        [NotMapped]
        public string Periode => DateDue.ToString("MMMM yyyy");
    }
}
