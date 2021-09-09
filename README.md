# Posy v2
Rede social para teste de tecnologias, versão 2.


##  Hi, I'm Fernando Boáis

:computer: Analista de Sistemas Sênior

:blue_heart: Amante da Tecnologia! 


## About me

[![Github Badge](https://img.shields.io/badge/-Github-000?style=flat-square&logo=Github&logoColor=white&link=https://github.com/poseydonfba)](https://github.com/poseydonfba)
[![Linkedin Badge](https://img.shields.io/badge/-LinkedIn-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/fernandoboais/)](https://www.linkedin.com/in/fernandoboais/)


## Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- ASPNET WEBFORMS
- ASPNET MVC
- ASPNET WEBAPI
- ASPNET Identity
- ASPNET EntityFramework
- ASPNET SignalR
- Injeção de Dependencia
- NHibernate
- Swagger
- Elmah
- Redis

- SQL Server
- MySQL
- PostgreSQL
- Oracle

![alt text](https://raw.githubusercontent.com/poseydonfba/posy-v2/master/img/tela-login.png)


## PosyV2.Api

// CRIADO O PROJETO EMPTY, COM CHECK EM WEBAPI, OS SEGUINTES PACOTES VIERAM JA INSTALADOS
```
  <package id="Microsoft.AspNet.WebApi" version="5.2.4" targetFramework="net452" />
  <package id="Microsoft.AspNet.WebApi.Client" version="5.2.4" targetFramework="net452" />
  <package id="Microsoft.AspNet.WebApi.Core" version="5.2.4" targetFramework="net452" />
  <package id="Microsoft.AspNet.WebApi.WebHost" version="5.2.4" targetFramework="net452" />
  <package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="2.0.0" targetFramework="net452" />
  <package id="Newtonsoft.Json" version="11.0.1" targetFramework="net452" />

Install-Package Microsoft.AspNet.WebApi.Owin -Version 5.2.4
Install-Package Microsoft.AspNet.WebApi.Cors -Version 5.2.4

Install-Package Microsoft.AspNet.Web.Optimization -Version 1.1.3
Install-Package Microsoft.AspNet.WebApi.MessageHandlers.Compression -Version 1.3.0

Install-Package Microsoft.AspNet.Identity.Owin -Version 2.2.1
Install-Package Microsoft.AspNet.Identity.EntityFramework -Version 2.2.1

Install-Package Microsoft.Owin -Version 3.1.0
Install-Package Microsoft.Owin.Security.Facebook -Version 3.1.0
Install-Package Microsoft.Owin.Security.Google -Version 3.1.0
Install-Package Microsoft.Owin.Security.Twitter -Version 3.1.0
Install-Package Microsoft.Owin.Security.MicrosoftAccount -Version 3.1.0
Install-Package Microsoft.Owin.Security.Cookies -Version 3.1.0
Install-Package Microsoft.Owin.Host.SystemWeb -Version 3.1.0
Install-Package Microsoft.Owin.Security.OAuth -Version 3.1.0
Install-Package Microsoft.Owin.Cors -Version 3.1.0

Install-Package SimpleInjector -Version 4.0.0
Install-Package SimpleInjector.Integration.Web -Version 4.0.0
Install-Package SimpleInjector.Integration.WebApi -Version 4.0.0

Install-Package WebActivator -Version 1.4.4

Install-Package Microsoft.AspNet.SignalR -Version 2.2.1

Install-Package Swashbuckle -Version 5.5.3

Install-Package DotNetZip -Version 1.10.1
```


## PosyV2.MVC

```
Install-Package EntityFramework -Version 6.2.0
Install-Package SimpleInjector.MVC3 -Version 3.0.5
Install-Package Microsoft.Owin.Host.SystemWeb -Version 3.1.0
Install-Package Microsoft.Owin.Security -Version 3.1.0
Install-Package Microsoft.AspNet.Identity.Owin -Version 2.2.1
Install-Package Microsoft.AspNet.Identity.EntityFramework -Version 2.2.1
Install-Package Microsoft.Owin.Security.Facebook -Version 3.1.0
Install-Package Microsoft.Owin.Security.Google -Version 3.1.0
Install-Package Microsoft.Owin.Security.Twitter -Version 3.1.0
Install-Package Microsoft.Owin.Security.MicrosoftAccount -Version 3.1.0
Install-Package Owin.Security.Providers.LinkedIn -Version 2.25.0
Install-Package Microsoft.AspNet.Web.Optimization -Version 1.1.3
--Install-Package Microsoft.AspNet.Authentication.OAuth -Version 1.0.0-rc1-final
Install-Package DotNetZip -Version 1.10.1
Install-Package Microsoft.AspNet.SignalR -Version 2.2.1

// PARA USAR COM REPOSITORIO MYSQL
Install-Package MySql.Data -Version 6.9.9
Install-Package MySql.Data.Entity -Version 6.9.9

// PARA USAR COM REPOSITORIO POSTGRESQL
Install-Package Npgsql -Version 2.2.7
Install-Package Npgsql.EntityFramework -Version 2.2.7

// PARA USAR COM REPOSITORIO ORACLE
Install-Package Oracle.ManagedDataAccess.EntityFramework -Version 12.1.2400

Install-Package elmah -Version 1.2.2

Install-Package Glimpse.MVC5
Install-Package Glimpse.EF6

// PARA USAR COM REPOSITORIO NHIBERNATE
Install-Package Nhibernate -Version 5.2.3
```


## PosyV2.WF

```
Install-Package SimpleInjector -Version 4.0.0
Install-Package SimpleInjector.Integration.Web -Version 4.0.0
Install-Package SimpleInjector.Extensions.ExecutionContextScoping -Version 4.0.0
Install-Package JsonNet.PrivateSettersContractResolvers.Source
Install-Package Microsoft.AspNet.SignalR -Version 2.2.1
```


## PosyV2.Infra.Data

```
// Posy.V2.Infra.Data
Install-Package EntityFramework -Version 6.2.0

// Posy.V2.Infra.Data.MySql
Install-Package EntityFramework -Version 6.2.0
Install-Package MySql.Data -Version 6.9.9
Install-Package MySql.Data.Entity -Version 6.9.9

// Posy.V2.Infra.Data.PostgreSql
Install-Package EntityFramework -Version 6.2.0
Install-Package Npgsql -Version 2.2.7
Install-Package Npgsql.EntityFramework -Version 2.2.7

// Posy.V2.Infra.Data.Oracle 
//Install-Package EntityFramework -Version 6.2.0
Install-Package Oracle.ManagedDataAccess.EntityFramework -Version 12.1.2400

// Posy.V2.Infra.Data.NHibernate
Install-Package Nhibernate -Version 5.2.3
Install-Package FluentNhibernate -Version 2.1.2
```


## PosyV2.Infra.CrossCutting

```
Install-Package EntityFramework -Version 6.2.0
Install-Package Microsoft.AspNet.Identity.EntityFramework -Version 2.2.1
Install-Package Twilio -Version 4.0.51
Install-Package Microsoft.AspNet.Identity.Owin -Version 2.2.1
Install-Package Microsoft.AspNet.Mvc -Version 5.2.2
Install-Package SendGrid -Version 9.10.0

// PARA USAR COM REPOSITORIO MYSQL
Install-Package MySql.Data -Version 6.9.9
Install-Package MySql.Data.Entity -Version 6.9.9

// PARA USAR COM REPOSITORIO POSTGRESQL
Install-Package Npgsql -Version 2.2.7
Install-Package Npgsql.EntityFramework -Version 2.2.7

// PARA USAR COM REPOSITORIO ORACLE
Install-Package Oracle.ManagedDataAccess.EntityFramework -Version 12.1.2400
```


## PosyV2.Infra.CrossCutting.Common

```
Install-Package HtmlAgilityPack -Version 1.7.0
Install-Package ServiceStack.Redis -Version 5.4.0
```

OBS: A forma de instalação do Redis foi criar um projeto de console e instalar o pacote Install-Package Redis-64
       Ao abrir o caminho informado no console, ex: C:\Users\[User]\source\repos\ConsoleApp1\packages, entrar na pasta 
	   redis-VERSION\tools e copiar todo o conteudo para C:\Redis. 
	   Após isso, executar o comando abaixo para criar um serviço para o Redis, apenas cria, não inicia:
	
	   redis-server --service-install redis.windows-service.conf --loglevel verbose

	   Crie uma pasta C:\Redis\Logs.

	   Execute o comando abaixo para iniciar o serviço

	   redis-server --service-start

	   Após isso adicionar o caminho C:\Redis nas variaveis de ambiente


## PosyV2.Infra.CrossCutting.IoC

```
Install-Package EntityFramework -Version 6.2.0
Install-Package Microsoft.AspNet.Identity.EntityFramework -Version 2.2.1
Install-Package SimpleInjector.Integration.Web -Version 3.0.5
```


## Migrations

// Configurando o Migrations para DbContexts em projetos diferentes

// PARA O IDENTITY
//migrating ApplicationDbContext

	enable-migrations -ProjectName:Posy.V2.Infra.CrossCutting.Identity -MigrationsDirectory:Migrations
	add-migration InitialCreate -ProjectName:Posy.V2.Infra.CrossCutting.Identity
	update-database -ProjectName:Posy.V2.Infra.CrossCutting.Identity -verbose

// PARA O PROJETO
//migrating DatabaseContext

	enable-migrations -ProjectName:Posy.V2.Infra.Data -MigrationsDirectory:Migrations
	add-migration InitialCreate -ProjectName:Posy.V2.Infra.Data
	update-database -ProjectName:Posy.V2.Infra.Data -verbose
  
  
  
## Até breve...
