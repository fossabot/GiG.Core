#!/bin/sh
dotnet test -c Release --no-build --no-restore --logger:"console;verbosity=normal" --logger:"trx;" --filter "FullyQualifiedName!~Sample" -r $ARTIFACTS_DIR *.sln
