#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/BlazorServerTemplate.App/BlazorServerTemplate.App.csproj", "src/BlazorServerTemplate.App/"]
RUN dotnet restore "src/BlazorServerTemplate.App/BlazorServerTemplate.App.csproj"
COPY . .
WORKDIR "/src/BlazorServerTemplate.App"
RUN dotnet build "BlazorServerTemplate.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorServerTemplate.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorServerTemplate.App.dll"]