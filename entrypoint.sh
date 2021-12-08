#!/bin/bash

set -e
run_cmd="dotnet WebApi.dll --no-build -v d"

dotnet Migrator.dll --no-build -v d -- --dryrun

dotnet Migrator.dll --no-build -v d

>&2 echo "WebApi DB Migrations complete, starting app."
>&2 echo "Run WebApi: $run_cmd"
exec $run_cmd