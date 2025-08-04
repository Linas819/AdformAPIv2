# AdformAPIv2
An updated version of the original [AdForm API task](https://github.com/Linas819/AdForm-API). Exercise: [Order Management API Exercise](https://github.com/erinev/order-management-api-exercise)

# Problem statement

A retailer company is looking to develop an order management system to streamline their order processing and product management. 

Your task is to build an API that handles customer orders, processes them, and stores the information in the system. 
Additionally, the system should provide capabilities to generate reports about ordered products.

# Requirements

**Hint**: Some requirements are open-ended, so you can implement them in a way that you think would be most convenient for the system user. 

## Functional
* As a retailer, I can **create** new **products**. Each product should have a `name` and `price`;
* As a retailer, I can retrieve a **list** of **products**. It should be possible to perform **search** by product `name`;
* As a retailer, I can **apply discount** for a **product**. Discount has two settings: `percentage` value of discount and `minimum quantity of products` that must be purchased for the discount to apply;
* As a retailer, I can **create** new **orders** with a list of products. Each product should have a `quantity` of items ordered;
* As a retailer, I can retrieve a **list** of **orders**;
* As a retailer, I can retrieve an **order invoice**. The invoice should contain a list of products where each product has a `name`, `quantity`, `discount (%)` (if applicable), `amount` ($), and additionally show the `total amount` ($) to pay for all products;
* As a retailer, I can retrieve **report** for **discounted product**. The report should show the discounted product `name`, `discount (%)`, `number of orders` that include this discounted product, and `total amount` ($) ordered of this discounted product;

## Non-Functional
* The solution code must be in a **Git repository**;
* Include prerequisites and steps to launch in the **README**;
* The system should use a **persistence layer** (`Mongo` or `Postgres`);
  * **Note**: If you will be using `Entity Framework` please make sure that business layer is not refferencing it.
* The API should not accept **invalid requests**;
* The solution should be implemented using **.NET** (ideally `LTS`);

## Bonus points stuff
* **Automated tests**;
* **RESTful** API;
* Think about **performance considerations** (e.g., the system over time will have a large number of orders);
* **Pagination support**
* **API Documentation** generated from code (hint - `Swagger`);
* **Containerization/deployment** (hint - `Docker Compose`);
* Code is structured using some known **architecture** (e.g., NTier, Onion, etc.);
* **Continuous integration** (CI. hint - `GitHub Actions`);
* **Progress of your work** (hint - `commits strategy`);
* **Comments/thoughts** on the decisions you made;
* **Logging** (hint: `Serilog`);
* **Create GraphQL endpoint** which allows to create new product;
* **Monitoring / Metrics** (hint: `Prometheus`);

# Time for solution

Take as long as you need on the solution, but we suggest limiting yourself to 8 hours. Do let us know how much time it took you!

The task is not made to be completed in the period of 8 hours, and no one expects you to! However, knowing how much time you spent and seeing the solution you came up with allows us to see what you prioritize and where you would consider cutting corners on a sharp deadline.

## Technology used
1. PostgreSQL 17.
2. DBeaver: a database management system.
3. Docker.
4. Visual Studio 2022.
5. .NET 8.0
6. Swagger API testing tool.
7. Serilog.
8. GraphQL (HotChocolate).

## Tasks and time duration
Database set up: 30 minutes.</br>
.NET Web API Project set up: 1 hour.</br>
Product and discount queries: 2 hours.
GraphQL integration: 1 hour.</br>
Order queries: 3 hours.</br>
ErrorMiddleware: 1 hour.</br>
Swagger documentation: 2 hours.</br>
Docker setup: 1 hour.

### DB Scafold
dotnet ef dbcontext scaffold "Server=localhost;Port=5432;UserId=postgres;Password=admin;Database=AdformDatabase;" Npgsql.EntityFrameworkCore.PostgreSQL -o AdformDB