#!/usr/bin/env bash

declare -r SCRIPT_DIR=$(realpath "$(dirname "$0")")

dotnet run --project "$SCRIPT_DIR\src\CodeGen\CodeGen.csproj"
