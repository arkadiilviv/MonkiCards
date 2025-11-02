## Getting Started

```bash
# Here is my docker run for db
docker run --name manki-db -e POSTGRES_PASSWORD=Arkasha123! -e POSTGRES_USER=arkasha -p 5432:5432 -d postgres
docker run --name manki-db-gui -p 5050:80 -e PGADMIN_DEFAULT_EMAIL=arkasha@arkasha.com -e PGADMIN_DEFAULT_PASSWORD=Arkasha123 -d dpage/pgadmin4
# and then EF add and update
dotnet ef migrations add InitIdentity -p ../Monki.DAL -s ../Monki.API
dotnet ef database update -p ../Monki.DAL -s ../Monki.API
```

<img src="https://github-readme-stats.vercel.app/api/top-langs?username=arkadiilviv&show_icons=true&locale=en&layout=compact&theme=chartreuse-dark" alt="ovi" />
