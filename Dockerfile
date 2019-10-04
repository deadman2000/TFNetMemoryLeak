# Build
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

COPY ./src/*.csproj ./
RUN dotnet restore

COPY ./src/ ./

RUN dotnet publish --no-restore -c Release -f netcoreapp2.2 -o ./out

#  Run
FROM mcr.microsoft.com/dotnet/core/runtime:2.2

WORKDIR /app

COPY --from=build-env /app/out ./

COPY model/ model

COPY run.sh ./

RUN chmod a+x run.sh

CMD ["./run.sh"]
