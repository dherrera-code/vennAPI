
# Venn Backend API - Project Overview:
### Link to hosted API
https://venngroupapi-emashqggf5gphwax.westus3-01.azurewebsites.net/

### Link to hosted Site

https://venn-iota.vercel.app/


The Following endpoints have been implemented:

UserController:
    - Login, CreateUser
## Project Goal

Create a backend for our Venn FullStack application!
- All Users can create an account, login, remove account, update account (username and password)!
- Users can create a room, and invite friends to join the room!

## Next Goals:

Add Navigation Props to Users to include user's availability!
Add prop to User to save account creation!

### UserController

- createAccount
- login
- removeAccount
- updateUsername
- GetUserByUsername
- GetUserByUserId

### RoomController

getAllRoom
getRoomsByIsActive
getRoomsByCategory
getRooms

## Models Needed!

UserModel (For users to login)
RoomInfoModel ()
RoomMembersModel (used to list members based on Room Id)
ProfileModel
FriendsModel
DTO models

### UserModel
``` csharp
    int Id;
    string Username;
    string Salt;
    string Hash;
    string Email;
```
### RoomInfoModel
``` csharp
    int Id;
    int CreatorId;
    string Title;
    string? Category;
    DateTime EventDate;
    List<string> UsersInRoomList;
    bool isActive;

```

## Models Saved into our DB

### LoginModelDTO
```csharp
    string Username
    string Password
```
### CreateAccountModelDTO
```csharp
    int Id = 0;
    string Username
    string Password
```

### PasswordModelDTO
```csharp
    string Salt;
    string Hash;
```
