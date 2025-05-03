FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DiemEcommerce.Web/DiemEcommerce.Web.csproj", "DiemEcommerce.Web/"]
RUN dotnet restore "DiemEcommerce.Web/DiemEcommerce.Web.csproj"
COPY . .
WORKDIR "/src/DiemEcommerce.Web"
RUN dotnet build "DiemEcommerce.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DiemEcommerce.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiemEcommerce.Web.dll"]
