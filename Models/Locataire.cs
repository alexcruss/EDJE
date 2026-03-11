using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionImmobiliere.Models
{
    public class Locataire
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

        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [StringLength(100)]
        public string Profession { get; set; }

        // Relations
        public virtual ICollection<Contrat> Contrats { get; set; } = new List<Contrat>();

        // Propriétés calculées
        public string NomComplet => $"{Prenom} {Nom}";

        public int Age => DateTime.Now.Year - DateNaissance.Year - 
                         (DateTime.Now.DayOfYear < DateNaissance.DayOfYear ? 1 : 0);
    }
}
