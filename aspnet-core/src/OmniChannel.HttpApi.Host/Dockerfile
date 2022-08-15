#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/OmniChannel.HttpApi.Host/OmniChannel.HttpApi.Host.csproj", "src/OmniChannel.HttpApi.Host/"]
COPY ["src/OmniChannel.HttpApi/OmniChannel.HttpApi.csproj", "src/OmniChannel.HttpApi/"]
COPY ["src/OmniChannel.Domain/OmniChannel.Domain.csproj", "src/OmniChannel.Domain/"]
COPY ["src/OmniChannel.Application.Contracts/OmniChannel.Application.Contracts.csproj", "src/OmniChannel.Application.Contracts/"]
COPY ["src/OmniChannel.Domain.Shared/OmniChannel.Domain.Shared.csproj", "src/OmniChannel.Domain.Shared/"]
COPY ["src/OmniChannel.Application/OmniChannel.Application.csproj", "src/OmniChannel.Application/"]
COPY ["src/OmniChannel.MongoDB/OmniChannel.MongoDB.csproj", "src/OmniChannel.MongoDB/"]
RUN dotnet restore "src/OmniChannel.HttpApi.Host/OmniChannel.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/OmniChannel.HttpApi.Host"
RUN dotnet build "OmniChannel.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OmniChannel.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
ARG environment=Development
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=$environment
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OmniChannel.HttpApi.Host.dll"]
