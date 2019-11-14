FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["Wkhtmltopdf/Wkhtmltopdf.csproj", "src/Wkhtmltopdf/"]
RUN dotnet restore "src/Wkhtmltopdf/Wkhtmltopdf.csproj"
COPY . .
WORKDIR "/src/Wkhtmltopdf"
RUN dotnet build "Wkhtmltopdf.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Wkhtmltopdf.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN chmod 755 /app/Wkhtmltopdf/Linux/wkhtmltopdf
ENTRYPOINT ["dotnet", "Wkhtmltopdf.dll"]
