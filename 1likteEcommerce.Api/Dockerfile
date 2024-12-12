# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_URLS="http://*:80"
ENV ASPNETCORE_ENVIRONMENT Development
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["1likteEcommerce.Api/1likteEcommerce.Api.csproj", "1likteEcommerce.Api/"]
COPY ["1likteEcommerce.Business/1likteEcommerce.Business.csproj", "1likteEcommerce.Business/"]
COPY ["1likteEcommerce.Core/1likteEcommerce.Core.csproj", "1likteEcommerce.Core/"]
COPY ["1likteEcommerce.Data/1likteEcommerce.Data.csproj", "1likteEcommerce.Data/"]
RUN dotnet restore "./1likteEcommerce.Api/1likteEcommerce.Api.csproj"
COPY . .
WORKDIR "/src/1likteEcommerce.Api"
RUN dotnet build "./1likteEcommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./1likteEcommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "1likteEcommerce.Api.dll"]

USER root
RUN mkdir -p /app/wwwroot/uploads && chmod -R 777 /app/wwwroot/uploads
USER 1000:1000

