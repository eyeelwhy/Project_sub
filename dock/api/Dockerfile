FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["dockers_api.csproj", "."]
RUN dotnet restore "dockers_api.csproj"
COPY . .
RUN dotnet build "dockers_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dockers_api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dockers_api.dll"]