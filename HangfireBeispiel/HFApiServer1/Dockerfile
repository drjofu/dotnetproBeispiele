#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["HFApiServer1/HFApiServer1.csproj", "HFApiServer1/"]
RUN dotnet restore "HFApiServer1/HFApiServer1.csproj"
COPY . .
WORKDIR "/src/HFApiServer1"
RUN dotnet build "HFApiServer1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HFApiServer1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HFApiServer1.dll"]