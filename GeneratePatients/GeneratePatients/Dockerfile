FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /App
EXPOSE 80
EXPOSE 443
COPY --from=build-env /App/out .
COPY *.json .
ENTRYPOINT ["dotnet", "GeneratePatients.dll"]