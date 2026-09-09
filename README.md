# Library Management System
Created by Dooques

## Overview
This project is a console-based Library Management System built in C# / .NET 10. It features a menu-based system that allows you to:
- View available books
- Search for books by title keyword or author name
- Sort search or view results by title or author, ascending or descending
- Add a book
- Delete a book
- Borrow a book
- Return a book

## How to run
To run this program:
1. You need to have the .NET 10 SDK installed on your device.
2. Clone the git repository locally.
3. Use `dotnet run` from the command line within the project directory.

Note: If you are running the program for the first time, the database will be seeded with 10 sample books from `Resources/Books.json`. These can be edited if you wish.

## Approach
### Design
I approached this project using a layered design, with separate classes for Models, Services, Repositories, and UI. This follows the separation of concerns principle and allows for more thorough, isolated testing at each layer.

### Persistence
The data persistence for this project uses SQLite, implemented via Entity Framework's DbContext model. SQLite makes the database easy to port between systems, since it exists as a single file on disk. Entity Framework was chosen because it allows database tables to be interacted with as C# objects, making it straightforward to perform more complex queries.

## Testing
Testing was a core focus of development, and a major reason the project was written with a layered design in mind. With separate classes for the Library's UI, services, and repository, each layer's dependencies can be mocked, so every stage can be tested in isolation and verified to return the expected results consistently.

### Running the tests
Run the test suite using `dotnet test`.

### Book Model
For the `Book` class, tests cover that:
- Books are created correctly from the fields provided
- `Borrow` and `Return` correctly update the `IsBorrowed` field
- `ToString` returns the book's data in a readable format

### Library
For the `Library` class, tests cover that the correct handler is triggered for each menu command, that user input flows through to the service layer correctly, and that unrecognised input is handled gracefully rather than causing a crash.

#### Library Services
These tests cover the service layer in isolation, with the repository layer mocked so it can return controlled test data. This includes success cases, failure cases (e.g. borrowing an already-borrowed book, adding a duplicate), and edge cases such as invalid sort or search arguments.
