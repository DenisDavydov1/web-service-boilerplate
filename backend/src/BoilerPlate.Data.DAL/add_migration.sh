#!/bin/bash

set -e # Exit on error

# ARG1: migration name
if [ -z "$1" ]; then
    echo "Error: provide migration name." >&2
    exit 1
fi

# optional ARG2: environment
if [ -z "$2" ]; then
    environment="Local"
else
    environment="$2"
fi

ASPNETCORE_ENVIRONMENT=$environment dotnet ef migrations add $1 --startup-project ../BoilerPlate.App.API/BoilerPlate.App.API.csproj
