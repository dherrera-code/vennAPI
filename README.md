
# Venn Backend API - Project Overview:

## Project Goal

Create a backend for our Venn FullStack application!
- All Users can create an account, login, remove account, update account (username and password)!
- Users can create a room, and invite friends to join the room!

## Controllers Needed!:

### UserController

### RoomController

getAllROom
getRoomsByIsActive
getRoomsByCategory
getRooms

## Models Needed!

UserModel (For users to login)
RoomInfoModel ()
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

<!-- ### UserIdDTO
```csharp
    int UserId;
    List<int> RoomListIds;
``` -->