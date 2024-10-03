set fallback
set windows-shell := ['nu', '-n', '-c']
set dotenv-load

default *args:
    just publish {{args}}
    just run

restore:
    dotnet restore

optimize:
    dotnet ef dbcontext optimize --project CnR.Server -n CnR.Server.SharedKernel.Persistence.CompiledModels -o ../CnR.Server.SharedKernel/Persistence/CompiledModels

mig +arg:
    dotnet ef migrations {{arg}} --project CnR.Server

db +arg:
    dotnet ef database {{arg}} --project CnR.Server

zzz:
    just db drop -f
    just mig remove
    just mig add InitialCreate
    just db update
