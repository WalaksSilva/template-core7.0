# Guia de Instalação e Configuração do Template BlueCyberAPI

Este guia fornece instruções passo a passo para instalar e configurar o template BlueCyberAPI, disponibilizado pelo repositório da BlueCyber. Siga os passos abaixo para começar a usar o template em seus projetos.

## Pré-requisitos

Antes de iniciar, certifique-se de que o .NET SDK está instalado em sua máquina. Se necessário, você pode baixar a versão mais recente do .NET SDK do [site oficial da Microsoft](https://dotnet.microsoft.com/download).

## 1. Clonar o Template do Repositório

Primeiro, clone o template do repositório da BlueCyber para sua máquina local. Você pode fazer isso usando o comando `git clone` seguido do URL do repositório. Substitua `[URL]` pelo link correto.

```bash
git clone [URL]
cd [nome-do-diretório-clonado]
```

## 2. Instalar o Template

Com o template clonado, você precisa instalar o template localmente para utilizá-lo em seus projetos. No diretório raiz do template clonado, execute o comando:

```bash
dotnet new --install .
```

## 3. Criar um Novo Projeto com o Template

Após a instalação, você pode iniciar um novo projeto utilizando o template BlueCyberAPI. Substitua [NomeProjeto] pelo nome desejado para seu projeto.

```bash
dotnet new blue-api -n [NomeProjeto]
```

## 4. Restaurar as Dependências do Projeto

Navegue até o diretório do projeto criado e execute o seguinte comando para restaurar as dependências necessárias:

```bash
cd [NomeProjeto]
dotnet restore
```

## 5. Compilar o Projeto
Para compilar o projeto e verificar se tudo está configurado corretamente, use o comando:

```bash
dotnet build
```

## 6. Configurar HTTPS
Para configurar a aplicação para usar HTTPS, execute o seguinte comando, que irá confiar no certificado de desenvolvimento HTTPS local:

```bash
dotnet dev-certs https --trust
```

---

## Listar Templates Disponíveis
Para visualizar todos os templates instalados no seu sistema, incluindo o BlueCyberAPI se instalado corretamente, utilize:

```bash
dotnet new -l
```

## Desinstalar o Template
Caso deseje remover o template BlueCyberAPI da sua lista de templates instalados, execute o comando:

```bash
dotnet new --uninstall BlueCyber - Web API .NET 8
```