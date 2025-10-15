FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HospitalManagementSystem/HospitalManagementSystem.csproj", "HospitalManagementSystem/"]
COPY ["HospitalManagementSystem.Repository/HospitalManagementSystem.Repository.csproj", "HospitalManagementSystem.Repository/"]
COPY ["HospitalManagementSystem.Core/HospitalManagementSystem.Core.csproj", "HospitalManagementSystem.Core/"]
COPY ["HospitalManagementSystem.Services/HospitalManagementSystem.Services.csproj", "HospitalManagementSystem.Services/"]
RUN dotnet restore "HospitalManagementSystem/HospitalManagementSystem.csproj"
COPY . .
WORKDIR "/src/HospitalManagementSystem"
RUN dotnet build "HospitalManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HospitalManagementSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HospitalManagementSystem.dll", "--urls", "http://0.0.0.0:8080"]
