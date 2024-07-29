FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
#https://habr.com/ru/companies/infobox/articles/240623/
#EXPOSE 8080
# copy csproj and restore as distinct layers
COPY *.sln .
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*} && mv $file ${file%.*}; done
RUN dotnet restore

# copy everything else and build app
COPY . ./aspnetapp
WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
ENTRYPOINT ["dotnet", "Otus.Teaching.Pcf.Administration.WebHost.dll"]