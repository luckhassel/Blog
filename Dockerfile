#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
ENV ASPNETCORE_URLS=http://*:80
EXPOSE 80
WORKDIR /src
COPY ./app . 
ENTRYPOINT ["dotnet", "Blog.dll"]
