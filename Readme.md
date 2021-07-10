# Happy5 Chat API Challenge
Chat API built to fullfill Happy5 Junior Back end Engineer role.
## FEATURES
- User Registration.
- User can find all available username to send message to.
- User can send and reply message in a conversation
- User can list all active conversation with their unread message count and latest message.
- User can list all message in a specific active conversation.
## TECHNOLOGY
### Tech Stack
-  .Net 5 API
- MsSQL Express 2019
### Dependencies
- Entity Framework Core
- Entity Framework Core Tools
- Entity Framework Core SQL
- Swashbuckle.AspNetCore (Swagger)
### Authorization
- Basic access auhtentication
## INSTALLATION
**This installation is only for local development environment only**.
**Before cloning or downloading this Git repository make sure you have the followings:**

- Visual Studio 2019
- MsSQL Express 2019 running on local machine
- Postman (optional)
- .Net 5 SDK

To run this application follow the steps below:

1. Clone/Download or pull this Git repository
2. Open **Happy5ChatTest.csproj** with Visual Studio
3. Open **appsettings.json** in solution exproler
4. Change the Data Source to your SQLEXPRESS IP. (Optionally you can leave it as it is since ".\\SQLEXPRESS" will always points to you local SQLExpress instance)
``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=ChatApp;Integrated Security=True"
  }
```
5. Click **F5** to run the API

## HOW TO USE
### Users Features

***User Registration***

In order to use the application you must register a username and password to the API.

**POST Request Route**
 > https://localhost:5001/register
 
 **Request Body**
```json
{
"username":  "user1",
"password":  "password123"
}
``` 
**Success Body Response**
```json
User Registered
```

***Get Available Username***

To send a message you need to know your friend's username, similar to whatsapp adding contacts. To get all the available username do the following request.

**GET Request Route**
>https://localhost:5001/users

**Success Body Response**
```json
[
	"arthur",
	"sllash",
	"user1"
]
```
----
### Messaging Features
***Authorization***

All  messaging features required an authorization header bearer. To do this on 
Postman follow the following steps.

On Authorization tab change the Type to BasicAuth and insert your Username and Password
![Imgur](https://i.imgur.com/2yFePxg.png)

***Sending and Replying to a conversation***

To start a conversation simply do a POST request to the given route.
The API will know weather if you already had a conversation with the given username.
If there are no conversation, the API will make a new conversation, hence if there are an existing conversation the API will continue on that conversation.

**POST Request Route**
>https://localhost:5001/chat/send/{ReceiverUsername}

**Request Example**
>https://localhost:5001/chat/send/sllash

**Success Body Response**
```JSON
Message Sent
```

***Get Active Conversations***

To get all active convesation that the user have and its unread messages count with latest message shown.

**GET Request Route**
>https://localhost:5001/active/conversation/

**Successful Response Body**
```JSON
[
	{
		"groupId":  "5d0dad2e-5b4b-439b-7133-08d94388e281",
		"receiver":  "sllash",
		"unreadMessages":  0,
		"latestMessage":  {
			"messageSender":  "arthur",
			"message":  "hi bro",
			"timesent":  "Saturday, 10 July 2021 19:41"
		}
	},
	{
		"groupId":  "92f5060e-b616-462b-65d7-08d943c7fc2d",
		"receiver":  "user1",
		"unreadMessages":  0,
		"latestMessage":  {
			"messageSender":  "arthur",
			"message":  "Hi",
			"timesent":  "Sunday, 11 July 2021 00:27"
		}
	}
]
```

***Get Messages in a Conversation***

To get all messages in a specific conversation that the user have.

**GET Request Route**
>https://localhost:5001/active/conversation/{ReciverUsername}

**Request Example**
>https://localhost:5001/active/conversation/user1

**Success Response Body**
```JSON
{
	"conversationId":  "92f5060e-b616-462b-65d7-08d943c7fc2d",
	"reciever":  "arthur",
	"messages":  [
		{
		"messageSender":  "arthur",
		"message":  "Hi",
		"timesent":  "Sunday, 11 July 2021 00:27"
		},
		{
		"messageSender":  "user1",
		"message":  "Wassup?",
		"timesent":  "Sunday, 11 July 2021 00:36"
		}
	]
}
```