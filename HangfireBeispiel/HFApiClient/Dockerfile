#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["HFApiClient/HFApiClient.csproj", "HFApiClient/"]
RUN dotnet restore "HFApiClient/HFApiClient.csproj"
COPY . .
WORKDIR "/src/HFApiClient"
RUN dotnet build "HFApiClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HFApiClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HFApiClient.dll"]