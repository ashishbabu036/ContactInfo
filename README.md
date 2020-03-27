# ContactInfo
Evolent/ContactInformationManagement

There are four APIs Designed for the same and tested on Postman locally.

 APIs Info :
 
1. GetContacts
   Route : /Contacts
   Response : List<Contacts> all Active Records
2. AddContact
   Route : /Contacts/AddContact
   Request Body: Contact Object Type
   Response : 200 OK Status Code
3. UpdateContact
   Route : /Contacts/UpdateContact
   Request Body: Contact Object Type
   Response : 200 OK Status Code with Updated Contact Object
4. DeleteContact
   Route : /Contacts/DeleteContact
   Query Parameters : 
     isDelete : boolean (Removes Records)
     isInActive : boolean (Changes status to active)
   Request Body: Contact Object Type
   Response : 200 OK Status Code with boolean Value
   
   Brief Info about the Project Structure:
   
   1. Respective Xunit Test Cases have been written and all are passing.
   2. Used EF framework and code first approach to create the database.
   3. Used Autofac for Dependency Injection in Controllers.
   4. Used Repository Pattern in the solution.
