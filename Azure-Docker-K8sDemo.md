# Lab: Build, Push, and Deploy a Dockerized ASP.NET Core REST API to Azure Kubernetes Service (AKS)

## Lab Objective

In this lab, you will:

* Build a Docker image for a simple ASP.NET Core REST API.
* Authenticate with Azure Container Registry (ACR).
* Push the Docker image to ACR.
* Connect to an Azure Kubernetes Service (AKS) cluster.
* Deploy the application to AKS.
* Verify that the application is running successfully.
* Scale the deployment and observe Kubernetes orchestration.

---

# Prerequisites

Ensure you have the following installed on your machine:

* Azure CLI
* Docker Desktop (Running)
* kubectl
* .NET 8 SDK
* Visual Studio Code or Visual Studio

Verify the installations.

```bash
az --version
docker --version
kubectl version --client
dotnet --version
```

---

# Step 1: Login to Azure

Open a terminal and sign in to your Azure account.

```bash
az login
```

A browser window opens. Sign in using your Azure credentials.

Verify the selected subscription.

```bash
az account show
```

If required, switch to the correct subscription.

```bash
az account set --subscription "<Subscription Name or ID>"
```

---

# Step 2: Create a Simple ASP.NET Core REST API

Create a new Web API project.

```bash
dotnet new webapi -n ProductApi
```

Navigate into the project.

```bash
cd ProductApi
```

Open the project in Visual Studio Code.

```bash
code .
```

Replace the default WeatherForecast endpoint with a simple Products endpoint that returns an in-memory collection.

Example output:

```json
[
    {
        "id":1,
        "name":"Laptop",
        "price":65000
    },
    {
        "id":2,
        "name":"Mouse",
        "price":1200
    }
]
```

Run the application locally.

```bash
dotnet run
```

Browse to

```
http://localhost:5000/products
```

or

```
https://localhost:5001/products
```

Verify that the API returns the list of products.

Stop the application.

---

# Step 3: Build a Docker Image

Ensure the project contains a Dockerfile.

A sample Dockerfile is shown below.

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet","ProductApi.dll"]
```

Build the Docker image.

```bash
docker build -t productapi:v1 .
```

Verify the image.

```bash
docker images
```

Run the container locally.

```bash
docker run -d -p 8080:8080 productapi:v1
```

Open the browser.

```
http://localhost:8080/products
```

Verify that the API works from the Docker container.

Stop the container.

```bash
docker ps
docker stop <container-id>
```

---

# Step 4: Authenticate with Azure Container Registry

List the available container registries.

```bash
az acr list --output table
```

Login to the registry.

```bash
az acr login --name <ACR_NAME>
```

Example:

```bash
az acr login --name contosoregistry
```

Verify successful login.

---

# Step 5: Tag the Docker Image

Before pushing, tag the image with the ACR login server.

Retrieve the login server.

```bash
az acr show \
    --name <ACR_NAME> \
    --query loginServer \
    --output tsv
```

Example output:

```
contosoregistry.azurecr.io
```

Tag the image.

```bash
docker tag productapi:v1 contosoregistry.azurecr.io/productapi:v1
```

Verify the tagged image.

```bash
docker images
```

---

# Step 6: Push the Image to Azure Container Registry

Push the image.

```bash
docker push contosoregistry.azurecr.io/productapi:v1
```

Wait until all layers are uploaded.

Verify that the repository exists.

```bash
az acr repository list \
    --name <ACR_NAME> \
    --output table
```

You should see

```
productapi
```

---

# Step 7: Connect to the AKS Cluster

List available AKS clusters.

```bash
az aks list --output table
```

Download cluster credentials.

```bash
az aks get-credentials \
    --resource-group <RESOURCE_GROUP> \
    --name <AKS_CLUSTER_NAME>
```

Verify connectivity.

```bash
kubectl get nodes
```

Expected output should display one or more nodes with **Ready** status.

---

# Step 8: Create a Kubernetes Deployment

Create a deployment.

```bash
kubectl create deployment productapi \
--image=contosoregistry.azurecr.io/productapi:v1
```

Verify deployment creation.

```bash
kubectl get deployments
```

View the pods.

```bash
kubectl get pods
```

Wait until the pod status becomes

```
Running
```

---

# Step 9: Expose the Deployment

Expose the deployment as a LoadBalancer service.

```bash
kubectl expose deployment productapi \
--port=80 \
--target-port=8080 \
--type=LoadBalancer
```

View the service.

```bash
kubectl get service
```

Initially, the External-IP may appear as:

```
<pending>
```

Wait for a few minutes and execute the command again.

Once assigned, note the External-IP.

Example:

```
20.204.xxx.xxx
```

---

# Step 10: Verify the Application

Open a browser.

```
http://<External-IP>/products
```

or

```bash
curl http://<External-IP>/products
```

Expected response:

```json
[
    {
        "id":1,
        "name":"Laptop",
        "price":65000
    },
    {
        "id":2,
        "name":"Mouse",
        "price":1200
    }
]
```

The application is now successfully running on Azure Kubernetes Service.

---

# Step 11: Observe Kubernetes Resources

View deployments.

```bash
kubectl get deployments
```

View ReplicaSets.

```bash
kubectl get replicasets
```

View Pods.

```bash
kubectl get pods
```

Notice that:

* Deployment manages the application.
* ReplicaSet maintains the desired number of pods.
* Pods host the running application containers.

---

# Step 12: Scale the Deployment

Increase the number of replicas to three.

```bash
kubectl scale deployment productapi --replicas=3
```

Verify.

```bash
kubectl get deployments
```

View the running pods.

```bash
kubectl get pods
```

Expected output shows three running pods.

```
productapi-xxxxx
productapi-yyyyy
productapi-zzzzz
```

---

# Step 13: Observe Automatic Orchestration

Delete one of the pods.

```bash
kubectl delete pod <pod-name>
```

Immediately monitor the pods.

```bash
kubectl get pods -w
```

Observe the following:

* One pod is terminated.
* Kubernetes automatically creates a new pod.
* The total number of running pods remains three.

This demonstrates Kubernetes' self-healing capability.

Press **Ctrl + C** to stop watching.

---

# Step 14: Clean Up (Optional)

Delete the service.

```bash
kubectl delete service productapi
```

Delete the deployment.

```bash
kubectl delete deployment productapi
```

Verify.

```bash
kubectl get all
```

---

# Summary

In this lab, you successfully:

* Created a simple ASP.NET Core REST API.
* Containerized the application using Docker.
* Authenticated with Azure Container Registry.
* Pushed the Docker image to ACR.
* Connected to an Azure Kubernetes Service cluster.
* Deployed the application to Kubernetes.
* Verified the deployment using an external endpoint.
* Scaled the application to multiple replicas.
* Observed Kubernetes automatic orchestration and self-healing capabilities.

You now have a complete understanding of the end-to-end workflow for deploying containerized applications to Azure Kubernetes Service using Azure Container Registry.
