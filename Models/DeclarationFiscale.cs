using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImmobiliere.Models
{
    public class DeclarationFiscale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(2000, 2100, ErrorMessage = "Année invalide")]
        public int Annee { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal RevenuTotal { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal ChargesDeductibles { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal RevenuImposable { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateGeneration { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Statut { get; set; } = "Brouillon"; // Brouillon, Validée, Transmise

        // Clé étrangère
        [Required]
        public int ProprietaireId { get; set; }

        // Relations
        [ForeignKey("ProprietaireId")]
        public virtual Proprietaire Proprietaire { get; set; }

        // Collections pour le calcul (Many-to-Many via tables de jonction)
        public virtual ICollection<DeclarationPaiement> DeclarationPaiements { get; set; } = new List<DeclarationPaiement>();
        public virtual ICollection<DeclarationCharge> DeclarationCharges { get; set; } = new List<DeclarationCharge>();

        // Propriétés calculées
        [NotMapped]
        public decimal TauxImposition => RevenuTotal > 0 ? (RevenuImposable / RevenuTotal) * 100 : 0;

        [NotMapped]
        public bool EstComplete => Statut != "Brouillon";
    }

    // Tables de jonction pour Many-to-Many
    public class DeclarationPaiement
    {
        [Key]
        public int Id { get; set; }

        public int DeclarationFiscaleId { get; set; }
        public int PaiementId { get; set; }

        [ForeignKey("DeclarationFiscaleId")]
        public virtual DeclarationFiscale DeclarationFiscale { get; set; }

        [ForeignKey("PaiementId")]
        public virtual Paiement Paiement { get; set; }
    }

    public class DeclarationCharge
    {
        [Key]
        public int Id { get; set; }

        public int DeclarationFiscaleId { get; set; }
        public int ChargeId { get; set; }

        [ForeignKey("DeclarationFiscaleId")]
        public virtual DeclarationFiscale DeclarationFiscale { get; set; }

        [ForeignKey("ChargeId")]
        public virtual Charge Charge { get; set; }
    }
}
