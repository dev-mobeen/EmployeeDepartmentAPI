# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY EmployeeDepartmentAPI.csproj ./
RUN dotnet restore EmployeeDepartmentAPI.csproj
COPY . .
RUN dotnet publish EmployeeDepartmentAPI.csproj -c Release -o /app/publish

# ---------- runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet","EmployeeDepartmentAPI.dll"]
