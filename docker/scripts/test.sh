#!/bin/sh
dotnet test -c Release --no-build --no-restore --logger:"console;verbosity=normal" --logger:"trx;" --filter "Category=Unit|Category=Integration" -r $ARTIFACTS_DIR *.sln