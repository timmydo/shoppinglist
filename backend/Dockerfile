# build image
FROM microsoft/aspnetcore-build:2.0 as build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .
WORKDIR /app/webapp
RUN apt-get update
ADD https://deb.nodesource.com/setup_10.x ./setup-npm.sh
RUN bash ./setup-npm.sh
RUN apt-get install -y nodejs
RUN rm -f /usr/local/bin/nod* /usr/local/bin/npm
RUN npm install -g @angular/cli
RUN npm install
RUN ng build

WORKDIR /app
RUN dotnet publish --output /out/ --configuration Release

# runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
EXPOSE 80
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "backend.dll" ]