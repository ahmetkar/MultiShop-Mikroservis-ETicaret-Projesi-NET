### MultiShop E-Commerce Website Project

This is a web project that includes an admin panel and e-commerce website interface, with a .NET Core MVC frontend and approximately 12 microservices as the backend. It features categorized product display, product reviews, cart additions, order details entry, and an animated payment card screen, coupon application, and a homepage portfolio.

This project was written using Murat Yücedağ's Multishop E-commerce training series, and I have made corrections and additions.

### My Additions

- Cargo microservice added.
- Cart structure updated and rewritten; exceptions for adding items via cookies and rediscounting were added for both logged-in and non-logged-in users.
- Payment frontend and backend corrections were made. - Order service was redesigned.
- Asynchronous queue messaging structure between Order-Payment-Cargo in Kafka was established using the choreography saga pattern.

# Messaging Structure Between Kafka and Microservices

- The user adds products to the cart, continues, selects an address, and creates an Order. When the Order is created, the OrderCreated event is published to Kafka. The Payment service listens to OrderCreated and creates a PaymentOrderSnapshot record in the database.
- The user continues, makes a payment, and creates a Payment. While creating the Payment, the information in the PaymentOrderSnapshot table is checked. The Payment is created, the payment process is simulated, and a PaymentCompleted or PaymentFailed event is published.
- Order listens to the PaymentCompleted or PaymentFailed event and changes the Status in the Ordering table accordingly.
- Cargo listens to the PaymentCompleted event, and when it occurs, completes the process of creating the cargo customer, cargo details, and cargo operation. It publishes a CargoCreated or CargoFailed event.
- Order listens to these events and changes the Status in its table accordingly.
- If the cargo is marked as delivered, the CargoDelivered event is published by the Cargo service.
- Order listens to this CargoDelivered event and changes the Status in the Ordering table to Completed.

Since each microservice doesn't have a specific orchestrator to listen to for its relevant event, this is called the choreography saga pattern. And I adapted the pattern to this application in this way.

# Microservices Included

Basket
Cargo
Catalog
Comment
Discount
Order
Payment
IdentityService

# Database Information
- Redis database running on Docker for Basket
- ​​MSSQL database running on Docker for Payment
- MSSQL database running on Docker for Identity
- MSSQL database running on Docker for Cargo
- MSSQL database running on Docker for Order
- MSSQL database running on Docker for Comment
- MongoDB database running on Docker for Catalog
- MSSQL database running on Docker for Discount

# Technologies Used
• Asp.Net Core 9.0 Web API and MVC • Entity Framework Core
• Dapper ORM
• Ocelot Gateway
• JSON Web Token / Identity Service
• Kafka
• Docker
• Saga apattern
• Onion Architecture
• N-tier Architecture
• Monolithic Architecture
• CQRS Design Pattern
• Generic Repository Design Pattern
• Mediator Design Pattern • SOLID and Clean Code Principles

# Database Technologies Used
• MSSQL
• MongoDB
• Redis

# Website Homepage Screenshots

![resim1](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20203429.png)

![resim2](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20203442.png)

![resim3](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20203507.png)

# Shopping Cart Screenshot

![resim4](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20203939.png)

![resim5](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20204113.png)

# Order Details Screen

![resim6](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20205552.png)

# Payment Screen

![resim7](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20210256.png)

# Product List Screen

![resim8](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20210431.png)

![resim81](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20213811.png)

# Product Details Screen

![resim8](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20213637.png)

![resim9](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20213647.png)

# Admin Panel Screen

![resim9](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20214411.png)


