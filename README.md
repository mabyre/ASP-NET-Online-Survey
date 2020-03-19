# ASP-NET-Online-Survey
You are searching for a truly professional solution for online survey. You need to do some customers relationship management. You want this solution to be free. You're at the right place.

Find all documentation and retreive us on dedicated blog : http://sodevlog-online-survey.blogspot.fr/

# Requirements
* Visual Studio 2013 +
* ASP.NET 4.5 +

2020 - The project works with Visual Studio Community 2019 Yes!
commit made to use SQL LocalDB see the Web.config file

# Get started
1. Clone repository
1. Open solution in Visual Studio 2013 or more
1. Builld and run solution to load website in the browser
1. Use **admin/admin** to log into website administration

# High level Features
Users can respond anonymously or authenticated.

Manager can share export/import surveys

Administrateur manage community of managers and users

Very great ability to interrogate

Very effective statistics tabulation form

#### Notes from 2020/03/17
I did can open, compile and run the solution under Visual Studio Community 2019.
Which is very nice.

When I update the connection strings to attach databases ASPNETDB and QUESTIONNAIREDB 
to SqlLocalDB server the message from the application was :

**The user instance login flag is not supported on this version of SQL Server. The connection will be closed.**

Which is less and less nice, but I find the solution and make a commit see the Web.config file.
