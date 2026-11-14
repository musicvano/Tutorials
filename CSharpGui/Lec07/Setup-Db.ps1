# One-time setup of the demo databases of topics 7–8 (run it yourself; psql asks for the postgres password).
#   pwsh -File "CSharpGui\Lec07\Setup-Db.ps1"
# Creates role library_app (the lecture's demo password Change-Me-2026), databases library, blog_ef, shop_ef,
# loads library.sql, stores the connection strings in the user secrets of the Lec07/Lec08 projects and adds
# pgpass.conf entries, so DataGrip (Authentication: pgpass) and scripts connect without typing a password.
$ErrorActionPreference = 'Stop'
$psql = 'C:\Program Files\PostgreSQL\18\bin\psql.exe'
$pw = 'Change-Me-2026'
$env:PGCLIENTENCODING = 'UTF8'   # the scripts are UTF-8 (Cyrillic data); the console default is WIN1252
& $psql -h localhost -U postgres -v ON_ERROR_STOP=1 -c @"
DO `$`$ BEGIN
  IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'library_app') THEN
    CREATE ROLE library_app LOGIN PASSWORD '$pw' CREATEDB;
  END IF;
END `$`$;
"@
foreach ($db in 'library', 'blog_ef', 'shop_ef') {
  $exists = & $psql -h localhost -U postgres -At -c "SELECT 1 FROM pg_database WHERE datname = '$db'"
  if (-not $exists) { & $psql -h localhost -U postgres -c "CREATE DATABASE $db OWNER library_app" }
  # A database left from earlier runs: give it and its public tables/sequences to library_app
  & $psql -h localhost -U postgres -d $db -v ON_ERROR_STOP=1 -q -c @"
ALTER DATABASE $db OWNER TO library_app;
ALTER SCHEMA public OWNER TO library_app;
DO `$`$ DECLARE r record; BEGIN
  FOR r IN SELECT c.relname, c.relkind FROM pg_class c JOIN pg_namespace n ON n.oid = c.relnamespace
           WHERE n.nspname = 'public' AND c.relkind IN ('r', 'p', 'v', 'm', 'S')
             AND NOT EXISTS (SELECT FROM pg_depend d WHERE d.objid = c.oid AND d.deptype IN ('a', 'i')) LOOP
    EXECUTE format('ALTER %s public.%I OWNER TO library_app',
      CASE r.relkind WHEN 'S' THEN 'SEQUENCE' WHEN 'v' THEN 'VIEW' WHEN 'm' THEN 'MATERIALIZED VIEW' ELSE 'TABLE' END, r.relname);
  END LOOP;
END `$`$;
"@
}
$pass = Join-Path $env:APPDATA 'postgresql\pgpass.conf'
New-Item -ItemType Directory -Force (Split-Path $pass) | Out-Null
$line = "localhost:5432:*:library_app:$pw"
if (-not (Test-Path $pass) -or -not (Select-String -Path $pass -SimpleMatch $line -Quiet)) { Add-Content $pass $line }
$env:PGPASSFILE = $pass
& $psql -h localhost -U library_app -d library -v ON_ERROR_STOP=1 -q -f "$PSScriptRoot\library.sql"
$base = 'Host=localhost;Port=5432;Username=library_app'
foreach ($p in @(
    @("$PSScriptRoot\Library", 'Library', 'library'), @("$PSScriptRoot\Injection", 'Library', 'library'),
    @("$PSScriptRoot\BookBrowser", 'Library', 'library'),
    @("$PSScriptRoot\..\Lec08\Blog", 'Blog', 'blog_ef'), @("$PSScriptRoot\..\Lec08\Shop", 'Shop', 'shop_ef'),
    @("$PSScriptRoot\..\Lec08\States", 'Shop', 'shop_ef'), @("$PSScriptRoot\..\Lec08\Stock", 'Shop', 'shop_ef'))) {
  if (Select-String -Path "$($p[0])\*.csproj" -Pattern UserSecretsId -Quiet) {
    dotnet user-secrets set "ConnectionStrings:$($p[1])" "$base;Database=$($p[2]);Password=$pw" --project $p[0] | Out-Null
  }
}
'Done: role library_app, databases library, blog_ef, shop_ef; user secrets and pgpass.conf set.'
