#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine-amd64 AS build-env
EXPOSE 80
EXPOSE 443
WORKDIR /src
COPY . .
ENTRYPOINT ["dotnet", "Blog.dll"]