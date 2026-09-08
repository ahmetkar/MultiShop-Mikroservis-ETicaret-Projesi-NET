### MultiShop E-Commerce Website Project

This is a web project built with .NET Core, featuring an admin panel and an e-commerce site interface; it utilizes .NET Core MVC for the frontend and comprises eight microservices for the backend. It includes features such as a shopping cart and payment system, coupon application, campaign screens, and management of order and shipping processes.

This project was developed based on Murat Yücedağ's "Multishop E-commerce" training series, with subsequent corrections and additions made by me.

### My Additions

- Cargo microservice added.
- Cart structure rewritten; added logic to handle cart additions via cookies for unauthenticated users and Redis for authenticated users. Ensured that products are added to the cart and order details along with their specific filter attributes.
- Payment frontend and backend fixes implemented.
- Order service refactored.
- Asynchronous queue-based messaging established between Order, Payment, and Cargo services using the choreography-based Saga pattern.
- Added features for product filtering, adding filters via the admin panel, and associating filters with products and categories.
- Added product campaign pages; implemented functionality to associate products with campaigns via the admin panel and display selected products on these campaign pages.
- Search functionality added.
- Pagination functionality added.
- User profile page added, including information management and order/cargo tracking.
- Admin panel features added: statistics page, order management, cash register/financial data, and cargo management.
- Admin panel features added: discount and coupon management, with updates reflected across the system.
- Admin panel features added: management of shipping companies and rates, with updates reflected across the system.
- Stock information added for each product filter variant; implemented automatic updates upon payment and order cancellation, as well as manual stock updates via the admin panel.

# Messaging Structure Between Kafka and Microservices

- The user adds items to the cart, proceeds to select an address, and creates an order. Once the order is created, an `OrderCreated` event is published to Kafka. The Payment service listens for `OrderCreated` and creates a `PaymentOrderSnapshot` record in the database.
- The user proceeds to make a payment, triggering the creation of a payment record. During payment creation, the information in the `PaymentOrderSnapshot` table is verified. The payment is created, the payment process is simulated, and either a `PaymentCompleted` or `PaymentFailed` event is published.
- The Order service listens for the `PaymentCompleted` or `PaymentFailed` event and updates the status in the `Ordering` table accordingly.
- The Cargo service listens for the `PaymentCompleted` event; upon receiving it, it completes the process of creating the cargo customer, cargo details, and cargo operation, then publishes a `CargoCreated` or `CargoFailed` event.
- The Catalog service listens for the `PaymentCompleted` event; upon receiving it, it retrieves the product IDs from the order and uses Product service functions to decrement the stock levels for those products.
- The Order service listens for these events and updates the status in its table accordingly.
- If the cargo is marked as delivered, the Cargo service publishes a `CargoDelivered` event.
- The Order service listens for the `CargoDelivered` event and updates the status in the `Ordering` table to "Completed."
- If the order is cancelled, an `OrderCancelled` event is published.
- The Catalog service listens for the `OrderCancelled` event; upon receiving it, it retrieves the product IDs from the order and uses Product service functions to increment the stock levels for those products again.

Since there is no central orchestrator and each microservice listens for the events relevant to it, this approach is known as the Choreography Saga pattern; I have adapted this pattern to the application in this manner.

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


