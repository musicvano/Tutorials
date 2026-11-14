# Базовий образ фінального етапу можна змінити:
# docker build --build-arg BASE=aspnet:10.0-noble-chiseled ...
ARG BASE=aspnet:10.0

# Етап 1: збирання в образі з повним SDK.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Primes.Api.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app --no-restore

# Етап 2: лише середовище виконання й опублікований застосунок.
FROM mcr.microsoft.com/dotnet/${BASE} AS final
ARG VERSION=1.0
ENV APP_VERSION=$VERSION
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "Primes.Api.dll"]
