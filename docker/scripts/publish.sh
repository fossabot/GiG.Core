#!/bin/sh
cd /app/artifacts/nugets/
dotnet nuget push **/*.nupkg --source $SOURCE --api-key $API_KEY 