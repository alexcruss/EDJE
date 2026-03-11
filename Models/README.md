# Modèles de Données - Plateforme de Gestion Immobilière

## 📋 Vue d'ensemble

Cette structure de modèles est conçue pour une application **Blazor WebAssembly** permettant la gestion complète d'un patrimoine immobilier avec :
- Gestion des propriétés et logements
- Suivi des contrats de location
- Gestion des paiements et génération de reçus
- Suivi des charges déductibles
- Préparation automatisée des déclarations fiscales

## 🏗️ Architecture des Modèles

### Modèles Principaux

1. **Proprietaire** - Propriétaire de bâtiments
2. **Batiment** - Immeuble ou ensemble immobilier
3. **Logement** - Appartement ou maison individuelle dans un bâtiment
4. **Locataire** - Personne louant un logement
5. **Contrat** - Bail de location entre propriétaire et locataire
6. **Paiement** - Loyer mensuel ou ponctuel
7. **Recu** - Quittance de loyer générée automatiquement
8. **Charge** - Dépense liée à un bâtiment (déductible ou non)
9. **DeclarationFiscale** - Déclaration annuelle des revenus fonciers
10. **Document** - Fichier attaché (contrat PDF, facture, etc.)

## 📊 Relations Clés

```
Proprietaire (1) ──→ (N) Batiment
Batiment (1) ──→ (N) Logement
Logement (1) ──→ (N) Contrat
Locataire (1) ──→ (N) Contrat
Contrat (1) ──→ (N) Paiement
Paiement (1) ──→ (1) Recu
Batiment (1) ──→ (N) Charge
Proprietaire (1) ──→ (N) DeclarationFiscale
DeclarationFiscale (N) ←→ (N) Paiement
DeclarationFiscale (N) ←→ (N) Charge
```

## 🚀 Installation et Configuration

### 1. Packages NuGet requis

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### 2. Configuration dans `Program.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using GestionImmobiliere.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configuration de la connexion à la base de données (côté serveur)
builder.Services.AddDbContext<GestionImmoContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

await builder.Build().RunAsync();
```

### 3. Connection String dans `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GestionImmo;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Migrations Entity Framework

```bash
# Créer la première migration
dotnet ef migrations add InitialCreate

# Appliquer la migration à la base de données
dotnet ef database update
```

## 💡 Exemples d'Utilisation

### Créer un nouveau contrat avec paiement automatique

```csharp
var nouveauContrat = new Contrat
{
    LogementId = 1,
    LocataireId = 5,
    DateDebut = DateTime.Now,
    DateFin = DateTime.Now.AddYears(1),
    LoyerMensuel = 850m,
    Charges = 100m,
    Caution = 1700m,
    TypeContrat = "Location nue",
    Statut = "Actif"
};

await context.Contrats.AddAsync(nouveauContrat);
await context.SaveChangesAsync();
```

### Générer un paiement et son reçu

```csharp
var paiement = new Paiement
{
    ContratId = contratId,
    DatePaiement = DateTime.Now,
    DateDue = new DateTime(2025, 2, 5),
    Montant = 950m,
    TypePaiement = "Virement",
    Statut = "Payé",
    Reference = "VIR-2025-02-001"
};

var recu = new Recu
{
    NumeroRecu = $"REC-{DateTime.Now:yyyyMMdd}-{paiement.Id}",
    DateEmission = DateTime.Now,
    MontantTotal = paiement.Montant,
    Periode = paiement.Periode,
    Paiement = paiement
};

await context.Paiements.AddAsync(paiement);
await context.Recus.AddAsync(recu);
await context.SaveChangesAsync();
```

### Calculer les revenus pour la déclaration fiscale

```csharp
var annee = 2024;
var proprietaireId = 1;

// Récupérer tous les paiements de l'année
var paiementsAnnee = await context.Paiements
    .Include(p => p.Contrat)
        .ThenInclude(c => c.Logement)
            .ThenInclude(l => l.Batiment)
    .Where(p => p.DatePaiement.Year == annee 
             && p.Contrat.Logement.Batiment.ProprietaireId == proprietaireId
             && p.Statut == "Payé")
    .ToListAsync();

var revenuTotal = paiementsAnnee.Sum(p => p.Montant);

// Récupérer les charges déductibles
var chargesDeductibles = await context.Charges
    .Include(c => c.Batiment)
    .Where(c => c.Date.Year == annee 
             && c.Batiment.ProprietaireId == proprietaireId
             && c.Deductible)
    .SumAsync(c => c.Montant);

var declaration = new DeclarationFiscale
{
    ProprietaireId = proprietaireId,
    Annee = annee,
    RevenuTotal = revenuTotal,
    ChargesDeductibles = chargesDeductibles,
    RevenuImposable = revenuTotal - chargesDeductibles,
    Statut = "Brouillon"
};

await context.DeclarationsFiscales.AddAsync(declaration);
await context.SaveChangesAsync();
```

## 🔍 Requêtes Utiles

### Dashboard propriétaire - Vue d'ensemble

```csharp
var dashboard = await context.Proprietaires
    .Where(p => p.Id == proprietaireId)
    .Select(p => new
    {
        NombreLogements = p.Batiments.SelectMany(b => b.Logements).Count(),
        LogementsLoues = p.Batiments
            .SelectMany(b => b.Logements)
            .Count(l => l.Statut == "Loué"),
        RevenuMensuel = p.Batiments
            .SelectMany(b => b.Logements)
            .SelectMany(l => l.Contrats)
            .Where(c => c.Statut == "Actif")
            .Sum(c => c.MontantTotal),
        PaiementsEnRetard = p.Batiments
            .SelectMany(b => b.Logements)
            .SelectMany(l => l.Contrats)
            .SelectMany(c => c.Paiements)
            .Count(pa => pa.EstEnRetard)
    })
    .FirstOrDefaultAsync();
```

### Liste des loyers impayés

```csharp
var impayés = await context.Paiements
    .Include(p => p.Contrat)
        .ThenInclude(c => c.Locataire)
    .Include(p => p.Contrat)
        .ThenInclude(c => c.Logement)
    .Where(p => p.EstEnRetard)
    .OrderBy(p => p.DateDue)
    .ToListAsync();
```

## 📝 Fonctionnalités Recommandées

### Services à implémenter

1. **PaiementService** - Gestion automatique des paiements récurrents
2. **RecuService** - Génération PDF des quittances
3. **DeclarationFiscaleService** - Calcul automatisé des revenus imposables
4. **NotificationService** - Rappels automatiques (loyers, échéances)
5. **DocumentService** - Gestion du stockage des fichiers (Azure Blob, etc.)

### API Controllers recommandés

- `ProprietairesController`
- `BatimentsController`
- `LogementsController`
- `ContratsController`
- `PaiementsController`
- `DeclarationsFiscalesController`

## ⚠️ Points d'attention

1. **Sécurité** : Implémenter l'authentification et l'autorisation (chaque propriétaire ne voit que ses données)
2. **Validation** : Utiliser les DataAnnotations déjà présentes dans les modèles
3. **Performance** : Utiliser `.AsNoTracking()` pour les requêtes en lecture seule
4. **Soft Delete** : Considérer l'ajout d'un champ `IsDeleted` plutôt que de supprimer les données
5. **Audit** : Ajouter des champs `CreatedAt`, `UpdatedAt`, `CreatedBy` pour le suivi

## 📚 Prochaines étapes

1. Créer les DTOs pour séparer les modèles de domaine de l'API
2. Implémenter les services métier
3. Créer les composants Blazor pour l'interface utilisateur
4. Ajouter la génération PDF des reçus et déclarations
5. Implémenter le système de notifications (email/SMS)
6. Créer un tableau de bord avec graphiques (revenus, taux d'occupation, etc.)

---

**Bon développement ! 🚀**
