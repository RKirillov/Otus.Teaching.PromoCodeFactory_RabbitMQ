FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
# EXPOSE 8080
# EXPOSE 5432
# copy csproj and restore as distinct layers
COPY *.sln .
# COPY */*.csproj ./
COPY Otus.Teaching.Pcf.GivingToCustomer.Core/*.csproj ./Otus.Teaching.Pcf.GivingToCustomer.Core/
COPY Otus.Teaching.Pcf.GivingToCustomer.DataAccess/*.csproj ./Otus.Teaching.Pcf.GivingToCustomer.DataAccess/
COPY Otus.Teaching.Pcf.GivingToCustomer.Integration/*.csproj ./Otus.Teaching.Pcf.GivingToCustomer.Integration/
COPY Otus.Teaching.Pcf.GivingToCustomer.WebHost/*.csproj ./Otus.Teaching.Pcf.GivingToCustomer.WebHost/
COPY Otus.Teaching.Pcf.IntegrationTests/*.csproj ./Otus.Teaching.Pcf.IntegrationTests/
RUN dotnet restore

# copy everything else and build app
COPY . ./aspnetapp
WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
ENTRYPOINT ["dotnet", "Otus.Teaching.Pcf.GivingToCustomer.WebHost.dll"]