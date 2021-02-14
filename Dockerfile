FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build_env
WORKDIR /app

COPY Hexarc.Pact.Demo.Api/Hexarc.Pact.Demo.Api.csproj ./Hexarc.Pact.Demo.Api/Hexarc.Pact.Demo.Api.csproj
COPY Hexarc.Pact.AspNetCore/Hexarc.Pact.AspNetCore.csproj ./Hexarc.Pact.AspNetCore/Hexarc.Pact.AspNetCore.csproj
COPY Hexarc.Pact.Protocol/Hexarc.Pact.Protocol.csproj ./Hexarc.Pact.Protocol/Hexarc.Pact.Protocol.csproj
RUN dotnet restore Hexarc.Pact.Demo.Api/Hexarc.Pact.Demo.Api.csproj

COPY Hexarc.Pact.Demo.Api ./Hexarc.Pact.Demo.Api
COPY Hexarc.Pact.AspNetCore ./Hexarc.Pact.AspNetCore
COPY Hexarc.Pact.Protocol ./Hexarc.Pact.Protocol
RUN dotnet publish Hexarc.Pact.Demo.Api/Hexarc.Pact.Demo.Api.csproj -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build_env /app/out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Hexarc.Pact.Demo.Api.dll
