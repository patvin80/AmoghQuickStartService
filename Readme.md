# Skylight Hello World Startup - Dockerized

The Dockerized version of the Hello World integration of the [Skylight SDK implementation](https://docs.upskill.io/development/getting-started/part-5-reading-and-writing-from-external-systems). The benefit is to run the dotnet core application as a docker container using docker compose to ensure containerized development. 

## Prerequisites:
1. Install and configure [Docker Desktop](https://www.docker.com/products/docker-desktop) for Windows / MacOSx. 
1. Configure the [Visual Studio 2019 Docker Support](https://docs.microsoft.com/en-us/dotnet/architecture/containerized-lifecycle/design-develop-containerized-apps/visual-studio-tools-for-docker). 
1. Request Skylight Developer Edition [here](https://upskill.io/landing/dev-edition-signup/)
1. Complete steps 0 to 4 of the [Skylight Hello World Getting started guide](https://docs.upskill.io/development/getting-started)

## Instructions:
1. Check out the repository or download the zip file from the [Github](https://github.com/patvin80/AmoghQuickStartService)
1. Rename the sample.env.dev file in the root folder as .env.dev
1. Replace the placeholders for USERNAME, PASSWORD and DOMAIN with credentials obtained.
1. Navigate to the folder containing the docker-compose file.
    ```
    docker-compose up --build
    ```

2. Note: To run in background, include the -d flag.
    ```
    docker-compose up --build -d
    ```

3. To tear down run the docker-compose down command as 
    ```
    docker-compose down
    ```

## Containerization:
1. Use the [Visual Studio 2019 Docker Support](https://docs.microsoft.com/en-us/dotnet/architecture/containerized-lifecycle/design-develop-containerized-apps/visual-studio-tools-for-docker) to generate the docker file. 
1. Note the Visual Studio generated docker image does not have capabilities to pull the Skylight SDK from the Azure Source. To resolve the issue include the command below in the build phase.
    ```
    RUN dotnet nuget add source https://pkgs.dev.azure.com/UpskillSDK/skylight-sdk/_packaging/release/nuget/v3/index.json -n skylightSdkRelease
    ```
1. Since the Skylight SDK needs credentials to connect, instead of using the credentials from the code, we have moved the credentials to environment variables.
1. Environment variables are passed to the docker image using the -env_file parameter in the docker compose file for the quickstartservice.
    ```
    version: '3.4'

    services:
      quickstartservice:
        image: ${DOCKER_REGISTRY-}quickstartservice
        build:
          context: .
          dockerfile: QuickStartService/Dockerfile
        env_file: .env.dev
    ```
1. The sample environment variables are made available in the sample.env.dev file and the .env files are excluded from the docker image and the source code repository for obvious security reasons.
