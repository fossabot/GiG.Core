#!/bin/sh
dotnet nuget push --source $SOURCE --api-key $API_KEY artifacts/nugets/*.nupkg
