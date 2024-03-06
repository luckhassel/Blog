# FIAP - TechChallenge 02

## Visão Geral
Projeto consiste em uma API desenvolvida utilizando .NET 7 que possui automação de seu processo de CI/CD para deploy em um cluster kubernetes na Azure (AKS).
A persistência de dados ocorre em um banco de dados SQL hospedado na Azure, bem como os seus segredos e conexões também estão, utilizando o serviço de Key Vault.

## Requisitos
- Docker OU .NET 7 SDK
- Cadastro na plataforma Azure
- Cadastro na plataforma Azure DevOps
- Git

## Configuração
### Criação da infraestrutura
- Para o banco de dados SQL, é necessário criar uma instância de SQL Server bem como de um banco de dados, liberando as pemissões de acesso
- O armazenamento de segredos utilizados na pipeline de CD é feito através do recurso Azure Key Vault. Lá, estarão as configurações de appsettings, dentre outras
- Uma vez que o projeto é executado em contâiner, é necessário que o mesmo seja armazenado em um repositório. Para isso, é preciso criar um Azure Container Registry
- O deploy do contâiner foi feito em um serviço kubernetes (Azure Kubernetes Service). Para tal, foi criar uma instância do mesmo

## CI/CD
### Processo de CI
Toda vez que é feito um commit na branch main, a pipeline configurada através do arquivos azure-pipelines.yml é executada na plataforma Azure DevOps.
Quando inicada, essa pipeline realiza configura o NuGet como a fonte dos pacotes utilizados no projeto. Feito isso, o projeto é restaurado e publicado.
Logo depois, os arquivos de configuração Docker e deployment.yml (responsável pela configuração do deployment do kubernetes) são copiados para a pasta do artefato.
Então, na sequência, é feito o build do projeto e seu artefato é publicado.

### Processo de CD
Após a publicação do artefato, é possível fazer o deploy da aplicação no cluster kubernetes através da sua pipeline de CD. Nela, primeiramente é feita a conexão com o Azure Key Vault.
Feito isso, o appsettings.json tem suas variáveis alteradas de acordo com o ambiente de deploy (development no caso) utilizando as variáveis retiradas da key vault.
Agora, o container está pronto para ser publicado no Azure Container Registry. Para isso, é criada uma tarefa de Build e Push do container para o recurso da nuvem. Ao final, a imagem do container deve estar publicada
com a versão da imagem relacionada ao Id do build. A última etapa então consiste em fazer a publicação dessa imagem no cluster. Isso é feito através de uma tarefa que utilizada o arquivo deployment.yml. No caso desse projeto,
esse arquivo cria um serviço de Load Balancer que serve de porta de entrada do mundo exterior ao contâiner, e o deployment do contâiner em si. Vale ressaltar que deve ser criado um segredo para que seja permitido
que o serviço do kubernetes possa fazer o push dessa imagem.
