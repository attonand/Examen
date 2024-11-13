
rm -fR /c/Projects/Examen/API/Data/Migrations

rm -f /c/Projects/Examen/API/main.db
rm -f /c/Projects/Examen/API/main.db-shm
rm -f /c/Projects/Examen/API/main.db-wal

cd /c/Projects/Examen/API

dotnet ef migrations add InitialCreate -o Data/Migrations

*.db-shm
*.db-wal

rm -fR /c/Projects/Examen/client/node_modules/
rm -fR /c/Projects/Examen/client/.angular/
rm -f /c/Projects/Examen/client/package-lock.json
