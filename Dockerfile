FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY . .

RUN dotnet restore /source/src/src.sln

COPY ./ ./
WORKDIR /source
RUN dotnet publish /source/src/External/Presentation/BookShop.API/BookShop.API.csproj -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8079

ENTRYPOINT ["dotnet", "BookShop.API.dll"]