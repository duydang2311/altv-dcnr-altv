set fallback
set windows-shell := ['nu', '-n', '-c']
set dotenv-load

default *args:
    just publish {{args}}
    just run

restore:
    dotnet restore

add-mig +arg:
    dotnet ef migrations add {{arg}} --project CnR.Server -o Infrastructure/Persistence/Migrations

mig +arg:
    dotnet ef migrations {{arg}} --project CnR.Server

db +arg:
    dotnet ef database {{arg}} --project CnR.Server

zzz:
    just db drop -f
    just mig remove
    just mig add InitialCreate
    just db update
