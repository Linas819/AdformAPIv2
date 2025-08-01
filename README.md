# AdformAPIv2
An updated version of the original AdForm API task

### DB Scafold
dotnet ef dbcontext scaffold "Server=localhost;Port=5432;UserId=postgres;Password=admin;Database=AdformDatabase;" Npgsql.EntityFrameworkCore.PostgreSQL -o AdformDB