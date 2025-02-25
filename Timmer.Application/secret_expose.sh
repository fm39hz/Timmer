#!/usr/bin/bash
# shellcheck disable=SC2002
cat ./config.json | dotnet user-secrets set
