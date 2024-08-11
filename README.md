# SmartShopAPI
SmartShopAPI is a straightforward application operating as an online shop. It allows users to sign up, log in, explore products, add them to the cart, and place orders. The application is built using ASP.NET Core according to REST architecture principles.

# Stack

* ASP.NET Core  
* Entity Framework Core  
* Microsoft SQL Server  
* FluentValidation  
* JWT Authentication (JWTBearer)  
* Data Transfer Object (DTO)  
* AutoMapper  
* Swagger  

# Database Diagram  
![scheme](https://github.com/user-attachments/assets/417d8875-6f48-48fb-8e73-4a2af92df886)

# Endpoints

## AccountController:  
**POST** /api/account/registration: Registers a new user.  
**POST** /api/account/login: Logs in a user.  

## Cart:  
**GET** /api/cart/{userId}: Retrieves the current cart for a specific user.  
**POST** /api/cart: Adds a new item to the cart.  
**DELETE** /api/cart/{cartItemId}: Removes a specific item from the cart.  
**PUT** /api/cart/{cartItemId}: Updates the information of a specific cart item.  

## Category:  
**GET** /api/category: Retrieves all available categories.  
**POST** /api/category: Creates a new product category.  
**GET** /api/category/{categoryId}: Retrieves details of a specific category.  
**DELETE** /api/category/{categoryId}: Deletes a specific product category.  
**PUT** /api/category/{categoryId}: Updates the information of a specific category.  

## Order:  
**POST** /api/order: Places a new order.  
**GET** /api/order/{orderId}: Retrieves the details of a specific order.  

## Product:  
**GET** /api/category/{categoryId}/product: Retrieves products from a specific category.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Support for filtering, sorting, and pagination to help you find exactly what you need.   
**POST** /api/category/{categoryId}/product: Adds a new product to a specific category.  
**GET** /api/category/{categoryId}/product/{productId}: Retrieves detailed information about a specific product.  
**DELETE** /api/category/{categoryId}/product/{productId}: Removes a specific product from the category.  
**PUT** /api/category/{categoryId}/product/{productId}: Updates the information of a specific product.  


# Swagger
![swagger](https://github.com/user-attachments/assets/478fcb48-a802-4cea-b58e-4c3bbc4a54c5)
