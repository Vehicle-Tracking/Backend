
FROM microsoft/dotnet:2.1-runtime-deps-alpine3.7 AS base
WORKDIR /app

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.400

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \ 
    # Skip extraction of XML docs - generally not useful within an image/container - helps perfomance
    NUGET_XMLDOC_MODE=skip

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='b4e07dc0700c0c663410b8ec22b70731f0a49a5a5a5e52068d68bfafe7f7e77d496dbfedb48429a542b0e82dc3356ac9f4cdb2f4768de45f1c3757bbaa97f9f8' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    && apk del .build-deps


# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \ 
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help

RUN apk add --update \
    python \
    python-dev \
    py-pip \
    build-base \
  && pip install virtualenv  \
  && pip install --upgrade pip \
  && rm -rf /var/cache/apk/* \
  && pip install --upgrade awscli  \
  && pip install --upgrade awsebcli 
  
  
RUN apk update && \
    apk add nginx
COPY nginx/nginx.conf /etc/nginx/sites-available
COPY nginx/nginx.conf /etc/nginx/sites-enabled

CMD ["nginx", "-g", "daemon off;"]

FROM base AS build
WORKDIR /app
COPY . .
COPY Avt.Web.Backend/*.csproj ./Avt.Web.Backend/
WORKDIR /app/Avt.Web.Backend
RUN dotnet restore

RUN dotnet build Avt.Web.Backend.csproj -c Release -o /out

FROM build AS publish
WORKDIR /app/Avt.Web.Backend
RUN dotnet publish -c Release -o /out

FROM base AS final
COPY --from=publish /out .

EXPOSE 18085

ENTRYPOINT ["dotnet", "Avt.Web.Backend.dll"]

















#FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
#WORKDIR /app
#EXPOSE 17929
#EXPOSE 44335
#
#FROM microsoft/dotnet:2.1-sdk AS build
#WORKDIR /src
#COPY Avt.Web.Backend/Avt.Web.Backend.csproj Avt.Web.Backend/
#RUN dotnet restore Avt.Web.Backend/Avt.Web.Backend.csproj
#COPY . .
#WORKDIR /src/Avt.Web.Backend
#RUN dotnet build Avt.Web.Backend.csproj -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish Avt.Web.Backend.csproj -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "Avt.Web.Backend.dll"]
