## Création de la base de données et son modèle

Ouvrir console depuis Visual Studio ou CMD

### Installer dotnet-ef CLI si...

dotnet tool install --global dotnet-ef

### Aller au dossier du projet exécutable

cd PostsVerify.Poc.Api

### Option 1 : Générer directement la base via le script de migration déjà existant

dotnet ef -v database update

### Option 2 : Recréer la migration et générer la base

#### (si nécessaire) Supprimer la base 

manuellement ou 

dotnet ef -v database drop

#### Supprimer la migration 

manuellement ou 

dotnet ef migrations remove

#### Générer le script de migration

dotnet ef -v migrations add CreateModel --output-dir "Infrastructure/Storage.Relational/EntityFrameworkCore/Migrations" --namespace "PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore.Migrations"

#### Générer directement la base

dotnet ef -v database update

## Lancer l'application

 Depuis Visual Studio, lancer l'application, cela ouvre une page swagger qui liste les différents endpoints

 Utiliser le bouton 'Try it out'
