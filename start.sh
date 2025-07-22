#!/usr/bin/env bash
# publish and run
dotnet publish EmployeeDepartmentAPI.csproj -c Release -o ./out
exec dotnet ./out/EmployeeDepartmentAPI.dll
