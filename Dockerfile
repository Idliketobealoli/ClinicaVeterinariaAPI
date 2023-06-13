#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClinicaVeterinaria.API/ClinicaVeterinaria.API.csproj", "ClinicaVeterinaria.API/"]
RUN dotnet restore "ClinicaVeterinaria.API/ClinicaVeterinaria.API.csproj"
COPY . .
WORKDIR "/src/ClinicaVeterinaria.API"
RUN dotnet build "ClinicaVeterinaria.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClinicaVeterinaria.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicaVeterinaria.API.dll"]
