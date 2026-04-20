FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/TimemanagementPlatform.Web/TimemanagementPlatform.Web.csproj", "src/TimemanagementPlatform.Web/"]
COPY ["src/TimemanagementPlatform.Application/TimemanagementPlatform.Application.csproj", "src/TimemanagementPlatform.Application/"]
COPY ["src/TimemanagementPlatform.Domain/TimemanagementPlatform.Domain.csproj", "src/TimemanagementPlatform.Domain/"]
COPY ["src/TimemanagementPlatform.Infrastructure/TimemanagementPlatform.Infrastructure.csproj", "src/TimemanagementPlatform.Infrastructure/"]
RUN dotnet restore "src/TimemanagementPlatform.Web/TimemanagementPlatform.Web.csproj"
COPY . .
RUN dotnet publish "src/TimemanagementPlatform.Web/TimemanagementPlatform.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TimemanagementPlatform.Web.dll"]

