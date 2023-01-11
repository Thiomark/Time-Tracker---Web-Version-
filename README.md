# Time Tracker
An ASP.NET web application (ASP.NET Core.)

![thumbnail](https://res.cloudinary.com/thiomark/image/upload/v1673425605/portfolio/Time_Tracker.jpg)
![desktop-home](https://res.cloudinary.com/thiomark/image/upload/v1673425125/portfolio/My-Modules-Time-Tracker.png)
![desktop-home](https://res.cloudinary.com/thiomark/image/upload/v1673425125/portfolio/My-Modules-Time-Tracker2.png)
![add-module](https://res.cloudinary.com/thiomark/image/upload/v1673425125/portfolio/Create-Time-Tracker.png)
![hours](https://res.cloudinary.com/thiomark/image/upload/v1673425125/portfolio/My-Modules-Time-Trackere.png)

# The assignment requirements;
- [x] The user should be able to register and login
- [x] The registration information must be stored in a Microsoft SQL Server database
- [x] Users must fill in the name and student number fields when registering and then create a username and password
- [x] The user must be able to add the modules they will be working on
- [x] The user should also be add hours they spent on each module
- [x] The user should also be edit their profile
- [x] The User should be able to add a reminder to any moudle
- [x] All the reminders that are within 24 hours should be displayed on the home page of the user
- [x] The application must remember the logged in user
- [x] The user should be able to toggle between using cards or list to display added modules

# How to run the application

## Prerequisite

1) C#
2) Microsoft SQL Server
3) IDE (Recommended Visual Studio 2019 or higer)
4) .Net 3.1

To create your first migration or a new migration open your Package Manager Console or any other command line of your choice. Make sure that you are in the root folder of the application before creating the migration

## Opening Package Manager Console

1. Click on View => Other Windows => Package Manager Console
2. Package Manager Console will show up at the bottom left of your IDE
3. Click on it to open it.

## Enable Migrations (copy the following command)
```sh
enable-migrations
```

## Create your first migration (copy the following command)
= add-migration InitialMigration
EF Core will create a directory called Migrations in your project, and generate some files. More information visit [https://docs.microsoft.com/](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs)

## Create your database and schema (copy the following command)
```sh
update-database
```

* This will create and add all the tables needed for the web application
* Your application is ready to run on your new database, and you didn't need to write a single line of SQL. More information visit [https://docs.microsoft.com/](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs)

## Running the application

1. Click on Build => Rebuild Solution
2. Look at the Output tab to see if they were any errors if not:
3. Click on the green button to run the application
4. The programme will compile and run

More information on how to use the web application is on the User Manual
## Close the program when done
