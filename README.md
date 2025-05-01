# UdemyReact
A NET Core Entity Framework API to support the Udemy React course, with data being updated to an Oracle database.
I developed this while on Module 15 of the Udemy React course that deals with RESTful calls.
The course had a node.js server bundled in that had text files as a datastore.

I sidetracked to see if I could teach myself Entity Framework based on the course requirement.
The result is a fully CRUD-functional API, that consists of :
* Two Controller-based APIs (Place and UserPlace)
* One Minimal API (ping)
* Full CRUD with multiple GET options
* Fully async

It shows examples of :
* Code-first 'Join' of two related objects (Place and UserPlace)
* a 'Where' construct
* 'Include' a nested object (Image)

For extra worth it also shows connecting to an Oracle Database, rather than SQL Server (which is well-documented)

It does not contain :
* Any authentication/authorisation

The original React code in the course (credit Max Shwarzmüller) needed to change a bit to accommodate the Entity Framework API, 
one reason being that the add or delete of UserPlace was done by PUTting the amended full array, whereas the new API works by POSTing a new UserPlace or DELETEing a removed UserPlace.
I've included the modified React components in a sub-folder.