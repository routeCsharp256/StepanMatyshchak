FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

WORKDIR /src
COPY ["src/OzonEdu.MerchandiseService/OzonEdu.MerchandiseService.csproj", "src/OzonEdu.MerchandiseService/"]
RUN dotnet restore "src/OzonEdu.MerchandiseService/OzonEdu.MerchandiseService.csproj"

COPY . .

WORKDIR "/src/src/OzonEdu.MerchandiseService"

RUN dotnet build "OzonEdu.MerchandiseService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OzonEdu.MerchandiseService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM runtime AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OzonEdu.MerchandiseService.dll"]