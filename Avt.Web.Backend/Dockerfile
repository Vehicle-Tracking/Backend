FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 17929
EXPOSE 44335

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Avt.Web.Backend/Avt.Web.Backend.csproj Avt.Web.Backend/
RUN dotnet restore Avt.Web.Backend/Avt.Web.Backend.csproj
COPY . .
WORKDIR /src/Avt.Web.Backend
RUN dotnet build Avt.Web.Backend.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Avt.Web.Backend.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Avt.Web.Backend.dll"]
