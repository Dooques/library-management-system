# Library Management System
Created by Dooques

## Overview
This project is a console-based Library Management System built in C# / .NET10. It features a menu-based system that allows you to:
- View available books
- Search for books using title keywords or by author name
- Add a book
- Delete a book
- Borrow a book
- Return a book

## How to run
To run this program: 
1. You need to have the .NET 10 SDK installed on your device.
2. Clone the git repository locally.
3. Use `dotnet run` from the command line within the project directory.
Note: If you are running the program for the first time the database will be seeded with 10 sample books from Resources/Books.json. These can be edited if you so wish.

## Approach
### Design
I approached this project by using a layered design model, with separate classes for Models, Services, Repositories and UI. This follows the separation of concerns principle and allows for more thorough testing. 

### Persistence
The data persistence model for this chat is SQLite implemented using Entity Framework's DBContext model. SQLite allows you to easily port the database to other systems since it exists within the devices file system. Entity Framework was chosen as it allows you to interact with database tables as if they were C# objects, making it very straightforward to perform complex SQL queries.

## Testing
Testing is a core focus of the development of this system and the main reason why it was written with layered design in mind. With separate classes for Library's UI, services, and repository, we can mock each of these dependencies and make sure that each stage is executing effectively and returning the expected results every timne. 

### Running the tests
Run the testing suite using `dotnet test`.

### Book Model
For the book class we are testing that books can be:
- Created correctly using the fields provided
- Borrow and Return functions change the `isBorrowed` field appropriately
- ToString correctly returns the book data in a readable format

### Library
For the library model we will be testing that UI functions are triggered and return the appropriate data allowing the function to process the data correctly.

#### Library Services
Testing the functionality of the service layer of the Library class. The repository layer is mocked so it can return test data and allow us to ensure these functions are running correctly. They are testing success cases, fail cases and edge cases.
