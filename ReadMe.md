## Create data base

Ouvrir console depuis Visual Studio ou CMD

### Installer dotnet-ef CLI si...

dotnet tool install --global dotnet-ef

### Aller au dossier du projet exécutable

cd PostsVerify.Poc.Api

### Supprimer la base si nécessaire

dotnet ef -v database drop

### Générer les scripts de création de base de données et le modèle (les tables)

dotnet ef -v migrations add CreateModel --output-dir "Infrastructure/Storage.Relational/EntityFrameworkCore/Migrations" --namespace "PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore.Migrations"

### Exécuter

dotnet ef -v database update
