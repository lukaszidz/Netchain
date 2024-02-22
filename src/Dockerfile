FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY Netchain.Core/*.csproj ./Netchain.Core/
COPY Netchain.Server/*.csproj ./Netchain.Server/

RUN dotnet restore Netchain.Server/Netchain.Server.csproj

COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Netchain.Server.dll"]
