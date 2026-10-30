# EDJE — Gestion immobilière

Application web **Blazor (.NET)** pour gérer un patrimoine immobilier locatif de bout en bout.

## Fonctionnalités

- Propriétaires, bâtiments et logements
- Locataires et contrats de bail
- Paiements de loyer et génération automatique des quittances
- Suivi des charges, déductibles ou non
- Préparation de la déclaration annuelle des revenus fonciers
- Documents attachés (contrats PDF, factures…)

## Modèle de données

`Proprietaire` → `Batiment` → `Logement` → `Contrat` ← `Locataire`, avec `Paiement`, `Recu`, `Charge`, `DeclarationFiscale` et `Document`. Le détail est dans [Models/README.md](Models/README.md).

## Lancer le projet

Prérequis : le [SDK .NET](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/alexcruss/EDJE.git
cd EDJE
dotnet run
```

## Stack

C# · .NET · Blazor · Entity Framework Core

## Auteur

[Pierre Kossi DEGBOEVI](https://pierredegboevi.com/)
