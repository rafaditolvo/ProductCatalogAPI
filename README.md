## Descrição

A `ProductCatalogAPI` é um CRUD (Create, Read, Update, Delete) desenvolvido especificamente para o cadastro de produtos. Utiliza a arquitetura MVC para uma organização eficiente do código, o Entity Framework Core para interação com o banco de dados SQL Server e RabbitMQ para mensageria.


## Tecnologias Utilizadas

- **C# e .NET 7:** Linguagem e framework principais para o desenvolvimento da aplicação.
- **Entity Framework Core:** Framework ORM para mapear objetos para um banco de dados relacional.
- **Docker:** Utilizado para criar e executar contêineres isolados para o SQL Server, simplificando o ambiente de desenvolvimento e implantação.
- **RabbitMQ:** Serviço de mensageria assíncrona para facilitar a comunicação entre diferentes partes da aplicação. Nesta aplicação, é utilizado para criar uma fila que gera uma mensagem a cada novo produto adicionado.
- **xUnit e Moq:** Frameworks de teste unitário para garantir a qualidade do código.

## Operações CRUD

A API suporta as seguintes operações CRUD:

[//]: # (AQUI so um toque tu pode ter isso com um arquivo de postman ai tu publica a api e o swagger ou o postman e o arquivo de postman vao disponibilizar essas informacoes) 
- **Create**: Adiciona um novo produto ao catálogo.
- **Read**: Recupera informações sobre produtos existentes.
- **Update**: Atualiza os detalhes de um produto existente.
- **Delete**: Remove um produto do catálogo.
-  **Get by ID**: Obtém as informações de um produto específico usando o ID.

## Configuração do Ambiente

Certifique-se de ter o Docker instalado em sua máquina. Para iniciar a aplicação e configurar o ambiente, siga os passos abaixo:


1. Execute o seguinte comando para subir os serviços necessários usando Docker Compose:

   ```bash
   docker-compose up -d
   
2. Após a conclusão da inicialização dos contêineres, execute os seguintes comandos para compilar e executar a aplicação:

   ```bash
   dotnet build
   dotnet run 

3. Execução de Testes Unitários

   ```bash
   dotnet test

# Informações Adicionais

Caso deseje parar e remover os contêineres após o uso, execute:

 ```bash
 docker-compose down
