using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionImmobiliere.Models
{
    public class Proprietaire
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(100)]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Format de téléphone invalide")]
        public string Telephone { get; set; }

        public string Adresse { get; set; }

        public DateTime DateInscription { get; set; } = DateTime.Now;

        // Relations
        public virtual ICollection<Batiment> Batiments { get; set; } = new List<Batiment>();
        public virtual ICollection<DeclarationFiscale> DeclarationsFiscales { get; set; } = new List<DeclarationFiscale>();

        // Propriété calculée
        public string NomComplet => $"{Prenom} {Nom}";
    }
}
