# Running Migrations

## Docs

https://www.milanjovanovic.tech/blog/using-multiple-ef-core-dbcontext-in-single-application

## Recipies

For this project you can run migrations for a specific DbContext using

```
dotnet ef migrations add NAME -c DBCONTEXT -o Migrations/FOLDER
```

For example:

```
dotnet ef migrations add BoardGamesInit -c GamesDb  -o Migrations/BoardGames
```

To update the database

```
dotnet ef database update -c DBCONTEXT
```

For example:

```
dotnet ef database update -c GamesDb
```

