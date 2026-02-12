FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solução e projetos
COPY MinhaBibliotecaAPI.slnx .
COPY Application/ Application/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/
COPY MinhaBibliotecaAPI/ MinhaBibliotecaAPI/

# Restaurar
RUN dotnet restore MinhaBibliotecaAPI.slnx

# Publicar
RUN dotnet publish MinhaBibliotecaAPI/MinhaBibliotecaAPI.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MinhaBibliotecaAPI.dll"]