dotnet ef database drop -c appdbcontext -f
dotnet ef database drop -c appidentitydbcontext -f
dotnet ef database drop -c datasetupdbcontext -f

for /r "Migrations" %%f in (*) do (
    set "filename=%%~nxf"
    if "!filename!"=="%%~nxf" (
        if "!filename:DataSetup=!"=="!filename!" (
            echo Remove: %%f
            del /f /q "%%f"
        ) else (
            echo Skip: %%f
        )
    )
)

dotnet ef migrations add InitialCreate -c appdbcontext
dotnet ef migrations add InitialCreate -c appidentitydbcontext

dotnet ef database update -c appdbcontext
dotnet ef database update -c appidentitydbcontext
dotnet ef database update -c datasetupdbcontext