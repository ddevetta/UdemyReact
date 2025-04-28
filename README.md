# UdemyReact
A NET Core Entity Framework API to support the Udemy React course, with data being updated to an Oracle database.
I developed this while on Module 13 of the Udemy React course that deals with RESTful calls.
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