#!bin/bash

require() {
    local BINARY="$1"
    local NAME="$2"
    [[ -n `which ${BINARY}` ]] || echo "Could not find '${BINARY}' on the path. Please install ${NAME}."
}

require dotnet '.NET CLI'

dotnet restore
dotnet build -c Release Semantics3
dotnet pack -c Release Semantics3