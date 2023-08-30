#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
EXPOSE 80
WORKDIR /src
COPY ./src . 
ENTRYPOINT ["dotnet", "Blog.dll"]
