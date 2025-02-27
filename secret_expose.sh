#!/usr/bin/bash
# shellcheck disable=SC2002
cat ./config.json | dotnet user-secrets set --project ./src/Timmer.Api/Timmer.Api.csproj
