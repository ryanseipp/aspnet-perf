FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.18 as build
WORKDIR /src
COPY AspnetPerf.MinimalApi.csproj ./

RUN dotnet restore "./AspnetPerf.MinimalApi.csproj" --use-current-runtime

COPY . .
RUN dotnet publish "AspnetPerf.MinimalApi.csproj" \
    -c Release \
    -o /app \
    --no-restore \
    --use-current-runtime \
    --self-contained true \
    /p:PublishTrimmed=true \
    /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine3.18
RUN adduser --disabled-password \
    --home /app \
    --gecos '' apiuser && chown -R apiuser /app

USER apiuser
WORKDIR /app
EXPOSE 5000
COPY --from=build /app .

HEALTHCHECK CMD wget -qO- -t1 http://localhost:5000/healthz || exit 1
ENTRYPOINT [ "./AspnetPerf.MinimalApi", "--urls", "http://*:5000" ]
