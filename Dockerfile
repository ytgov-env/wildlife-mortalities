FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WildlifeMortalities.App/WildlifeMortalities.App.csproj", "src/WildlifeMortalities.App/"]
RUN dotnet restore "src/WildlifeMortalities.App/WildlifeMortalities.App.csproj"
COPY . .
WORKDIR "/src/WildlifeMortalities.App"
RUN dotnet build "WildlifeMortalities.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WildlifeMortalities.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WildlifeMortalities.App.dll"]