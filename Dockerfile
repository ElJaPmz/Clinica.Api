# ===============================
# BUILD
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["Clinica.Api.slnx", "./"]
COPY ["Clinica.Api/Clinica.Api.csproj", "Clinica.Api/"]
COPY ["Clinica.Application/Clinica.Application.csproj", "Clinica.Application/"]
COPY ["Clinica.Domain/Clinica.Domain.csproj", "Clinica.Domain/"]
COPY ["Clinica.Infrastructure/Clinica.Infrastructure.csproj", "Clinica.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "Clinica.Api.slnx"

# Copiar todo el código y publicar
COPY . .

# Publicar la API
WORKDIR "/src/Clinica.Api"
RUN dotnet publish "Clinica.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===============================
# RUNTIME
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Librerías necesarias para la conexión a base de datos
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

# Copiar archivos publicados
COPY --from=build /app/publish .

# Render usa variable PORT automáticamente
ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE 8080

ENTRYPOINT ["dotnet", "Clinica.Api.dll"]