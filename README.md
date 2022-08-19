https://altkomsoftware.pl/en/blog/building-microservices-6/
# ASCLAB .NET Core PoC - LAB Insurance Sales Portal

[![Build Status](https://travis-ci.org/asc-lab/dotnetcore-microservices-poc.svg?branch=master)](https://travis-ci.org/asc-lab/dotnetcore-microservices-poc)

This is an example of a very simplified insurance sales system made in a microservice architecture using:

* .NET 5
* Entity Framework Core
* CQRS
* MediatR
* Marten
* Eureka
* Ocelot
* JWT Tokens
* RestEase
* RawRabbit
* NHibernate
* Polly
* NEST (ElasticSearch client)
* Dapper
* DynamicExpresso
* SignalR
* Hangfire

**Comprehensive guide describing exactly the architecture, applied design patterns and technologies can be found on our blog:**

* [Part 1 The Plan](https://altkomsoftware.pl/en/blog/building-microservices-net-core-part-1-plan/)
* [Part 2 Shaping microservice internal architecture with CQRS and MediatR](https://altkomsoftware.pl/en/blog/microservices-net-core-cqrs-mediatr/)
* [Part 3 Service Discovery with Eureka](https://altkomsoftware.pl/en/blog/service-discovery-eureka/)
* [Part 4 Building API Gateways With Ocelot](https://altkomsoftware.pl/en/blog/building-api-gateways-with-ocelot/)
* [Part 5 Marten An Ideal Repository For Your Domain Aggregates](https://altkomsoftware.pl/blog/building-microservices-net-core-part-5-marten-ideal-repository-domain-aggregates//)
* [Part 6 Real time server client communication with SignalR and RabbitMQ](https://altkomsoftware.pl/en/blog/building-microservices-6/)
* [Part 7 Transactional Outbox with RabbitMQ](https://altkomsoftware.pl/en/blog/microservices-outbox-rabbitmq/)

Other articles around microservices that could be interesting:

* [CQRS and Event Sourcing Intro For Developers](https://altkomsoftware.pl/en/blog/cqrs-event-sourcing/)
* [From monolith to microservices – to migrate or not to migrate?](https://altkomsoftware.pl/en/blog/monolith-microservices/)
* [Event Storming — innovation in IT projects](https://altkomsoftware.pl/en/blog/event-storming/)

## Business Case

We are going to build very simplified system for insurance agents to sell various kind of insurance products.
Insurance agents will have to log in and system will present them with list of products they can sell. Agents will be able to view products and find a product appropriate for their customers. Then they can create an offer and system will calculate a price based on provided parameters. \
Finally agent will be able to confirm the sale by converting offer to policy and printing pdf certificate. \
Portal will also give them ability to search and view offer and policies. \
Portal will also have some basic social network features like chat for agents. \
Latest feature is a business dashboard that displays sales stats using ElasticSearch Aggregations and ChartJS.

## Architecture overview

<p align="center">
    <img alt="NET Microservices Architecture" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-microservices-poc/master/readme-images/dotnetcore-microservices-architecture.png" />
</p>

* **Web** - a VueJS Single Page Application that provides insurance agents ability to select appropriate product for their customers, calculate price, create an offer and conclude the sales process by converting offer to policy. This application also provides search and view functions for policies and offers. Frontend talks to backend services via `agent-portal-gateway`.

* **Agent Portal API Gateway** - is a special microservice whose main purpose it to hide complexity of the underlying back office services structure from client application. Usually we create a dedicated API gateway for each client app. In case in the future we add a Xamarin mobile app to our system, we will need to build a dedicated API gateway for it. API gateway provides also security barrier and does not allow unauthenticated request to be passed to backend services. Another popular usage of API gateways is content aggregation from multiple services.

* **Auth Service** - a service responsible for users authentication. Our security system will be based on JWT tokens. Once user identifies himself correctly, auth service issues a token that is further use to check user permission and available products.

* **Chat Service** - a service that uses SignalR to give agents ability to chat with each other.

* **Payment Service** - main responsibilities: create Policy Account, show Policy Account list, register in payments from bank statement file. \
This module is taking care of a managing policy accounts. Once the policy is created, an account is created in this service with expected money income.  PaymentService also has an implementation of a scheduled process where CSV file with payments is imported and payments are assigned to policy accounts. This component shows asynchronous communication between services using RabbitMQ and ability to create background jobs. It also features accessing database using Dapper.

* **Policy Service** - creates offers, converts offers to insurance policies. \
In this service we demonstrated usage of CQRS pattern for better read/write operation isolation. This service demonstrates two ways of communication between services: synchronous REST based calls to `PricingService` through HTTP Client to get the price, and asynchronous event based using RabbitMQ to publish information about newly created policies. In this service we also access RDBMS using NHibernate.

* **Policy Search Service** - provides insurance policy search. \
This module listens for events from RabbitMQ, converts received DTOs to “read model” and indexes given model in ElasticSearch to provide advanced search capabilities.

* **Pricing Service** - a service responsible for calculation of price for given insurance product based on its parametrization. \
For each product a tariff should be defined. The tariff is a set of rules on the basis of which the price is calculated. DynamicExpresso was used to parse the rules. During the policy purchase process, the `PolicyService` connects with this service to calculate a price. Price is calculated based on user’s answers for defined questions.

* **Product Service** - simple insurance product catalog. \
It provides basic information about each insurance product and its parameters that can be customized while creating an offer for a customer.

* **Document Service** - this service uses JS Report to generate pdf certificates.

* **Dashboard Service** - Dashboard that presents sales statistics. \
Business dashboards that presents our agents sales results. Dashboard service subscribes to events of selling policies and index sales data in ElasticSearch. Then ElasticSearch aggregation framework is used to calculate sales stats like: total sales and number of policies per product per time period, sales per agent in given time period and sales timeline. Sales stats are nicely visualized using ChartJS.

Each business microservice has also **.Api project** (`PaymentService.Api`, `PolicyService.Api` etc.), where we defined commands, events, queries and operations and **.Test project** (`PaymentService.Test`, `PolicyService.Test`) with unit and integration tests.

## Running with Docker

You must install Docker & Docker Compose before. \
Scripts have been divided into two parts:

* [`infra.yml`](scripts/infra.yml) runs the necessary infrastructure.
* [`app.yml`](scripts/app.yml) is used to run the application.

You can use scripts to build/run/stop/down all containers.

To run the whole solution:

```bash
./infra-run.sh
./app-run.sh
```

>If ElasticSearch fails to start, try running `sudo sysctl -w vm.max_map_count=262144` first

## Manual running

### Prerequisites

Install [PostgreSQL](https://www.postgresql.org/) version >= 10.0.

Install [RabbitMQ](https://www.rabbitmq.com/).

Install [Elasticsearch](https://www.elastic.co/guide/en/elasticsearch/reference/current/install-elasticsearch.html) version >= 6.

### Init databases

#### Windows

```bash
cd postgres
"PATH_TO_PSQL.EXE" --host "localhost" --port EXAMPLE_PORT --username "EXAMPLE_USER" --file "createdatabases.sql"
```

In my case this command looks like:

```bash
cd postgres
"C:\Program Files\PostgreSQL\9.6\bin\psql.exe" --host "localhost" --port 5432 --username "postgres" --file "createdatabases.sql"
```

#### Linux

```bash
sudo -i -u postgres
psql --host "localhost" --port 5432 --username "postgres" --file "PATH_TO_FILE/createdatabases.sql"
```

This script should create `lab_user` user and the following databases: `lab_netmicro_payments`, `lab_netmicro_jobs`, `lab_netmicro_policy` and `lab_netmicro_pricing`.

### Run Eureka

Service registry and discovery tool for our project is Eureka. It is included in the project.
In order to start it open terminal / command prompt.

```bash
cd eureka-server
./gradlew.[bat] bootRun
```

This should start Eureka and you should be able to go to http://localhost:8761/ and see Eureka management panel.

### Build

Build all projects from command line without test:

#### Windows

```bash
cd scripts
build-without-tests.bat
```

#### Linux

```bash
cd scripts
./build-without-tests.sh
```

Build all projects from command with test:

#### Windows

```bash
cd scripts
build.bat
```

#### Linux

```bash
cd scripts
./build.sh
```

## Run specific service

Go to folder with specific service (`PolicyService`, `ProductService` etc) and use `dotnet run` command.

Let's build Uber 2.0 with REACT NATIVE! (Navigation, Redux, Tailwind CSS & Google Autocomplete)
https://www.youtube.com/watch?v=bvn_HYpix6s




How to install Docker, Kubernetes, Kubernetes Dashboard UI
https://github.com/kubernetes/dashboard/blob/master/docs/user/access-control/creating-sample-user.md
https://www.youtube.com/watch?v=cgYOpw5XLtk 
.NET Microservices – Full Course
https://www.youtube.com/watch?v=DgVjEo3OGBI(InGress 04:58:59)
1.apply dashboard :
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.3.1/aio/deploy/recommended.yaml
2.Proxy

kubectl proxy
3.http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/.
4.kubectl apply -f service_account.yaml
5.kubectl apply -f cluster_role_binding.yaml
6.Powershell:
kubectl -n kubernetes-dashboard get secret $(kubectl -n kubernetes-dashboard get sa/admin-user -o jsonpath="{.secrets[0].name}") -o go-template="{{.data.token | base64decode}}"
7.eyJhbGciOiJSUzI1NiIsImtpZCI6Ik13dlV1YWlZQ1JHOEdWY3UwMjFtN3ppb1dNVWwzQmVfdmhCbjJtck1HdTQifQ.eyJpc3MiOiJrdWJlcm5ldGVzL3NlcnZpY2VhY2NvdW50Iiwia3ViZXJuZXRlcy5pby9zZXJ2aWNlYWNjb3VudC9uYW1lc3BhY2UiOiJrdWJlcm5ldGVzLWRhc2hib2FyZCIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VjcmV0Lm5hbWUiOiJhZG1pbi11c2VyLXRva2VuLW1uOHo3Iiwia3ViZXJuZXRlcy5pby9zZXJ2aWNlYWNjb3VudC9zZXJ2aWNlLWFjY291bnQubmFtZSI6ImFkbWluLXVzZXIiLCJrdWJlcm5ldGVzLmlvL3NlcnZpY2VhY2NvdW50L3NlcnZpY2UtYWNjb3VudC51aWQiOiJhZDllOWI1Yy00OTIxLTQxOWYtODIyOC04MmFjYjRiYWQ4MDMiLCJzdWIiOiJzeXN0ZW06c2VydmljZWFjY291bnQ6a3ViZXJuZXRlcy1kYXNoYm9hcmQ6YWRtaW4tdXNlciJ9.n3FCcn6E3e6XHowH9WYzapxdAWqkxHRTbb0h1X28argVZxsGLaLcrbKlSvPfTC-myBU-Ms0i7amxFaqzzxm7aIhQYCr9kWDgZVCg9k5vwI0ULMmy6UloZX6AE5D_vTNgYetkUBX68lQKj7WJ2ExSWQou_OJDf3xPG4Bt6TwYaY7mP38tS9tOuhQ5J_YK3sTELyBgro44-Y47I_xgydpGYOK8w4atwY1tBRblEBRWOMftnKKxdu2FB8h6eNwdISbWRh4WBjAFF0JKH1QJN-bmYd1tNChovO-K3N32MfUH92O_pLVmyTzn2C6UdMuYa8gQzmUXd3cjsai2vo8zBUV5Pw

I.ProductSIMService
1.Add Ref
2.applicationUrl:7000
3.Razor Pages with Entity Framework Core in ASP.NET Core - Tutorial 1 of 8
https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-5.0&tabs=visual-studio
4.Add connection string(ProductConn)
5.Program Ensure DB Created
6.Add db context & initializer
7.Add Models
8.Add dbcontext to startup
9.uuid-ossp is a contrib module, so it isn't loaded into the server by default. You must load it into your database to use it.
For modern PostgreSQL versions (9.1 and newer) that's easy:
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
10.Init
11.Add Product Controller
12.Add ProductRepo
13.Add Dtos
14.Add mapper profile
15.Add JSON convertor for Command
16.Add JSON convertor for Quereies


*******************************Development**********************************************
II.PricingSIMService
1.ASP.NET Core Web API project template
      <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="8.1.1" />
	  <PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="5.0.11" />
	  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="5.0.10" />
	  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="5.0.8"/>
	  <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="5.0.10" />
      <PackageReference Include="Swashbuckle.AspNetCore" Version="5.6.3" />
2.DB
  appsetting/Conn
  models add key required..
  add dbContext
  UseInitializer
  1->* public Tariff Tariff { get; set; }
       modelBuilder.Entity<BasePremiumCalculationRule>().HasOne(t => t.Tariff).WithMany(p => p.basePremiumRules);
  add repository
  add controller
  add dtos
3.JSON Converter
.AddNewtonsoftJson(JsonOptions)

III.PolicySIMService

1.RestClient
      <PackageReference Include="Polly" Version="7.2.1" />
	  <PackageReference Include="RestEase" Version="1.5.2" />
add appsetting->PricingService
services.AddPricingRestClient();
2.RabbitMQ
<PackageReference Include="RabbitMQ.Client" Version="6.2.2" />
add RabbitMQHost RabbitMQPort
IMessageBusClient
MessageBusClient
services.AddSingleton<IMessageBusClient, MessageBusClient>();

IV.PolicySearchSIMService
1. Serilog https://github.com/serilog/serilog-aspnetcore
2. Elastic Search
docker pull docker.elastic.co/elasticsearch/elasticsearch:7.15.1
docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:7.15.1
localhost:9200/_aliases
<PackageReference Include="NEST" Version="6.4.0" />
services.AddElasticSearch(Configuration.GetConnectionString("ElasticSearchConnection"));
localhost:9200/policy/_search

V.PaymentSIMService
1.HangFire
https://dev.to/gmarokov/getting-started-with-hangfire-on-asp-net-core-and-postgresql-on-docker-56ak

"BackgroundJobs": {
    "HangfireConnectionStringName": "Host=192.168.0.10;Port=5416;Database=hangfireJobs;Username=postgres;Password=Abc123!",
    "InPaymentFileFolder": "/mnt/bigdisk/dotnetpoc/testcases"
  }
  ''
  app.UseHangfireDashboard(); //Will be available under http://localhost:5000/hangfire
VI. DashboardSIMService
Automapper:Converting JSON to list of objects
https://stackoverflow.com/questions/38107415/automapperconverting-json-to-list-of-objects
Query API. Using the Query DSL (language based on JSON to build complex query)
1.Add <PackageReference Include="NEST" Version="6.4.0" />
2."ConnectionStrings": {
    "ElasticSearchConnection": "http://localhost:9200"
  },
3.services.AddElasticSearch(Configuration.GetConnectionString("ElasticSearchConnection")); services.AddSingleton<IPolicyRepository, ElasticPolicyRepository>();services.AddInitialSalesData();
4.NestInstaller.cs
5.ElasticPolicyRepository.cs
6.Domain
7.Init
8.Dtos
9.docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:7.15.1
10.localhost:9200/policy/_search
11.http://localhost:9200/policy_lab_stats/_search
----12. kubectl apply -f elastic-depl.yaml
----13. kubectl delete deployment elastic-depl
----14. https://raphaeldelio.medium.com/deploy-the-elastic-stack-in-kubernetes-with-the-elastic-cloud-on-kubernetes-eck-b51f667828f9
----kubectl create -f https://download.elastic.co/downloads/eck/1.8.0/crds.yaml
----kubectl apply -f https://download.elastic.co/downloads/eck/1.8.0/operator.yaml
----kubectl get service quickstart-es-http  : Name: quickstart-es-http


*************************************Production*******************************************
I.ProductSIMService
docker build -t ax9587/productsimservice .
docker run -p 7000:80 -d ax9587/productsimservice  (http://localhost:7000/api/products)
docker push ax9587/productsimservice
docker stop 0df5
kubectl apply -f product-depl.yaml
--kubectl delete deployment product-depl
--kubectl delete svc product-clusterip-srv
--kubectl delete svc productnpservice-srv
--kubectl apply -f product-nodeport-srv.yaml  (http://localhost:30551/api/products)
kubectl get deployments
kubectl get services
kubectl get pods

kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.0.4/deploy/static/provider/cloud/deploy.yaml
kubectl create namespace ingress-nginx
kubectl get namespace
kubectl apply -f ingress-srv.yaml --namespace=ingress-nginx
kubectl get pods --namespace=ingress-nginx
kubectl get services --namespace=ingress-nginx 

1.kubectl apply -f product-depl.yaml
2.kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.0.4/deploy/static/provider/cloud/deploy.yaml
3.kubectl apply -f ingress-srv.yaml 

II.AuthService
https://kubernetes.io/docs/concepts/services-networking/ingress/
docker build -t ax9587/authsimservice .
docker run -p 6060:80 -d ax9587/authsimservice
docker push ax9587/authsimservice
1.kubectl apply -f auth-depl.yaml
--2.kubectl delete namespace ingress-nginx
--3.kubectl delete -A ValidatingWebhookConfiguration ingress-nginx-admission
--3.kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.0.4/deploy/static/provider/cloud/deploy.yaml
2.kubectl delete ingress ingress-srv
3.kubectl apply -f ingress-srv.yaml

III.RabbitMQ
kubectl apply -f rabbitmq-depl.yaml(http://localhost:15672/)

IV.ChatService
docker build -t ax9587/chatsimservice .
docker run -p 4099:80 -d ax9587/chatsimservice
docker push ax9587/chatsimservice
1.kubectl apply -f chat-depl.yaml
--2.kubectl delete ingress ingress-srv
--3.kubectl apply -f ingress-srv.yaml

V.PricingService

docker build -t ax9587/pricingsimservice .
docker run -p 4099:80 -d ax9587/pricingsimservice
docker push ax9587/pricingsimservice

1.kubectl apply -f pricing-depl.yaml
--kubectl delete deployment pricing-depl
--2.kubectl delete ingress ingress-srv
--3.kubectl apply -f ingress-srv.yaml

V.PolicyService
docker build -t ax9587/policysimservice .
docker run -p 4099:80 -d ax9587/policysimservice
docker push ax9587/policysimservice
1.kubectl apply -f policy-depl.yaml
--kubectl delete deployment policy-depl
--2.kubectl delete ingress ingress-srv
--3.kubectl apply -f ingress-srv.yaml

VI.ElasticSearch
Setting of Kubernete: 5GB memory

1.kubectl create -f https://download.elastic.co/downloads/eck/1.8.0/crds.yaml
2.kubectl apply -f https://download.elastic.co/downloads/eck/1.8.0/operator.yaml
3.kubectl apply -f elastic-depl.yaml
--kubectl delete Elasticsearch quickstart
--https://www.elastic.co/guide/en/cloud-on-k8s/1.8/k8s-deploy-elasticsearch.html
4.kubectl get elasticsearch
5.kubectl get service quickstart-es-http
--6.http://localhost:9200/_aliases  https://localhost:9200/_aliases
--7.https://www.youtube.com/watch?v=9tkrDqMbFMg(Elastic Search)
--8.https://www.youtube.com/watch?v=qjnT0pU0IRo(Elastic Cloud on Kubernetes - Part 1)
--9.kubectl edit service quickstart-es-http  (Type: ClusterIP->LoadBalancer)
--kubectl version --short
--kubectl -n elastic-system get all
--kubectl get crds (customer resource definitions)
--kubectl get kibana
--kubectl get elasticsearch
--kubectl get all
--kubectl get secrets
--kubectl describe secret quickstart-es-elastic-user
--linux car etc/hosts
6.kubectl port-forward svc/quickstart-es-http 9200
6.kubectl edit service quickstart-es-http (Type: ClusterIP->LoadBalancer)
7.Linux: PASSWORD=$(kubectl get secret quickstart-es-elastic-user -o go-template='{{.data.elastic | base64decode}}')
Windows:
kubectl get secret quickstart-es-elastic-user -o=jsonpath="{.data.elastic}"
certutil -decode a.txt data.b64
286Mh96KlSxxHJ2PZ8tr694z

https://localhost:9200/_aliases
https://localhost:9200/policy_lab_stats/_search

How to Install Ubuntu on Windows 10 (WSL)
https://www.youtube.com/watch?v=X-DHaQLrBi8
bash
8.Kibana
kubectl apply -f kibana-depl.yaml
kubectl port-forward service/quickstart-kb-http 5601
https://localhost:5601/
9.delete
kubectl delete kibana quickstart
kubectl delete Elasticsearc quickstart

VII.PolicySearchSIMService
docker build -t ax9587/policysearchsimservice .
docker push ax9587/policysearchsimservice
--kubectl delete deployment policy-search-depl


1.kubectl apply -f policy-search-depl.yaml
--2.kubectl delete ingress ingress-srv
--3.kubectl apply -f ingress-srv.yaml

VIII.DashboardSIMService
docker build -t ax9587/dashboardsimservice .
docker push ax9587/dashboardsimservice
1.kubectl apply -f dashboard-depl.yaml
--kubectl delete deployment dashboard-depl
--2.kubectl delete ingress ingress-srv
--3.kubectl apply -f ingress-srv.yaml

IX.PaymentSIMService
docker build -t ax9587/paymentsimservice .
docker push ax9587/paymentsimservice
1.kubectl apply -f payment-depl.yaml
--2.kubectl delete ingress ingress-srv
--3.kubectl apply -f ingress-srv.yaml

X.Postgres
https://severalnines.com/blog/using-kubernetes-deploy-postgresql/
kubectl create -f postgres-configmap.yaml
kubectl create -f postgres-storage.yaml
kubectl create -f postgres-deployment.yaml
kubectl create -f postgres-service.yaml
*For connecting PostgreSQL, we need to get the Node port from the service deployment.32427
$ kubectl get svc postgres

Delete PostgreSQL Deployments
For deletion of PostgreSQL resources, we need to use below commands.

# kubectl delete service postgres 
# kubectl delete deployment postgres
# kubectl delete configmap postgres-config
# kubectl delete persistentvolumeclaim postgres-pv-claim
# kubectl delete persistentvolume postgres-pv-volume