# official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Calculator/Calculator.csproj", "Calculator/"]
RUN dotnet restore "Calculator/Calculator.csproj"
COPY . .
WORKDIR "/src/Calculator"
RUN dotnet build "Calculator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calculator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calculator.dll"]
