FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["NannyServicesApi.sln", "./"]
COPY src ./src
COPY tests ./tests
RUN dotnet restore
RUN dotnet publish "src/NannyServices.Api/NannyServices.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NannyServices.Api.dll"] 