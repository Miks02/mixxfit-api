FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

USER root
RUN apt-get update && apt-get install -y \
    libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MixxFit.VSA/MixxFit.VSA.csproj", "MixxFit.VSA/"]
RUN dotnet restore "MixxFit.VSA/MixxFit.VSA.csproj"

COPY MixxFit.VSA/ ./MixxFit.VSA/

RUN dotnet build "MixxFit.VSA/MixxFit.VSA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MixxFit.VSA/MixxFit.VSA.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "MixxFit.VSA.dll"]
