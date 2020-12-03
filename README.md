# HallsOrganization

A WPF (Windows Presentation Foundation) application that schedules the usage of university classrooms/halls

# General info

University evaluation project 

# Description

The application has MS SQL Server DB that keeps information about the university halls, teachers and the halls schedule. 
Entity Framework 6 ORM (Object-relational mapping) technology is used to design the DB. The application provides very
simple (not very elegant) user interface that allows add/delete/update/view/check-schedule operations, export to pdf 
(queries/full DB), backup and restore procedures. Only teachers in the DB could add/delete schedules and confirmation
e-mail is sent to them to confirm the reservation/cancellation of a classroom.

# Build 

Using Microsoft Visual Studio

# Run

```
\EfDbHallsOrg\EfDbHallsOrg\bin\Debug\EfDbHallsOrg.exe
```

 

