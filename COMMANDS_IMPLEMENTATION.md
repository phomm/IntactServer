# Issue #24: Command System Implementation

## Overview
Implemented a complete command system for gameplay, allowing two clients to connect to a server room and exchange game commands.

## Components Implemented

### 1. Enums
- **CommandType.cs** - All 26 command types from c_None to c_UnitSwap
- **CommandError.cs** - Error codes for command validation (NoError, InvalidCommand, NotYourTurn, etc.)

### 2. Entity Framework Model
- **CommandDao.cs** - Entity model with properties:
  - RoomId (required, foreign key)
  - ProfileId (required, foreign key)
  - PlayerIndex (ushort, required)
  - CommandId (CommandType enum, required)
  - QueueNumber (uint, required, part of composite key)
  - Value (string, nullable, JSON)
  - Error (CommandError enum, default NoError)
  - Composite Primary Key: (RoomId, QueueNumber)

### 3. Business Logic Models
- **Command.cs** - Model for GET endpoint responses (all fields visible)
- **PostCommand.cs** - Model for POST endpoint input (only PlayerIndex, CommandId, Value)

### 4. Mapper
- **CommandMapper.cs** - Bidirectional mapping between DAO and BL models
  - Map CommandDao → Command
  - Map PostCommand → CommandDao (with roomId, profileId, queueNumber injection)

### 5. Repository
- **ICommandsRepository.cs** / **CommandsRepository.cs**
  - GetCommandsAsync() - Retrieve commands from offset
  - GetNextQueueNumberAsync() - Get next available queue number
  - GetLastCommandAsync() - Get most recent command
  - AddCommandsAsync() - Insert new commands
  - GetRoomAsync() - Fetch room details
  - IsProfileInRoomAsync() - Validate player membership
  - GetPlayerRoomIdAsync() - Find player's current room

### 6. Service
- **CommandsService.cs** - Business logic with validation:
  - Profile state validation (banned/deleted check)
  - Room membership verification
  - Room state validation (must be InGame)
  - Player number validation (0-1 for 2 players)
  - Turn validation (EndTurn command transitions)
  - Queue number generation
  - Command persistence

### 7. Controller
- **CommandsController.cs** - RESTful API endpoints:
  
  **GET /api/commands?offset={int}**
  - Returns: IEnumerable<Command>
  - Status Codes:
    - 200 OK
    - 401 Unauthorized (no token)
    - 403 Forbidden (banned/deleted profile)
    - 404 Not Found (not in room)

  **POST /api/commands**
  - Body: IEnumerable<PostCommand>
  - Status Codes:
    - 200 OK
    - 400 Bad Request (not your turn, invalid player number)
    - 401 Unauthorized (no token)
    - 403 Forbidden (banned/deleted profile)
    - 404 Not Found (not in room)

### 8. Database
- **AppDbContext.cs** - Added Commands DbSet and composite key configuration
- **Migration: 20250109000000_AddCommandsTable** - Creates Commands table with:
  - Composite primary key (RoomId, QueueNumber)
  - Foreign keys to Rooms and Profiles
  - Index on ProfileId
  - Default value for Error field

### 9. Dependency Injection
- **InternalServicesExtensions.cs** - Registered:
  - ICommandsService → CommandsService
  - ICommandsRepository → CommandsRepository

## Validation Logic

### GetCommands:
1. Validate player token (Authorization middleware)
2. Check profile state (active/banned/deleted)
3. Verify player is in a room
4. Return commands starting from offset

### PostCommands:
1. Validate player token (Authorization middleware)
2. Check profile state (active/banned/deleted)
3. Verify player is in a room
4. Validate room state is InGame
5. Validate all player indices are in valid range (0-1)
6. Check turn validity:
   - If last command was EndTurn from other player → valid
   - If last command was from current player → can continue
   - Otherwise → not your turn error
7. Generate sequential queue numbers
8. Persist commands to database

## Command Flow Example
1. Player 1 joins room
2. Player 2 joins room
3. Room state changes to InGame
4. Player 1 posts commands (PlayerIndex=0)
5. Player 1 posts c_EndTurn
6. Player 2 can now post commands (PlayerIndex=1)
7. Both players can GET commands from any offset

## Error Handling
All exceptions are caught and converted to appropriate ProblemDetails responses:
- NotFoundException → 404
- ForbiddenException → 403
- BadRequestException → 400

## Database Schema
```sql
CREATE TABLE Commands (
    RoomId INT NOT NULL,
    QueueNumber BIGINT NOT NULL,
    ProfileId INT NOT NULL,
    PlayerIndex INT NOT NULL,
    CommandId INT NOT NULL,
    Value TEXT NULL,
    Error SMALLINT NOT NULL DEFAULT 0,
    PRIMARY KEY (RoomId, QueueNumber),
    FOREIGN KEY (RoomId) REFERENCES Rooms(Id),
    FOREIGN KEY (ProfileId) REFERENCES Profiles(Id)
);
```

## Next Steps
To use this implementation:
1. Run the EF migration to create the Commands table
2. Ensure rooms are in InGame state before posting commands
3. Use the GET endpoint with offset=0 to retrieve all commands
4. Use offset > 0 to get only new commands (polling pattern)

## Testing Endpoints

### Get Commands
```bash
GET /api/commands?offset=0
Authorization: Bearer {token}
```

### Post Commands
```bash
POST /api/commands
Authorization: Bearer {token}
Content-Type: application/json

[
  {
    "playerIndex": 0,
    "commandId": 7,
    "value": "{\"faction\": \"humans\"}"
  },
  {
    "playerIndex": 0,
    "commandId": 16,
    "value": null
  }
]
```
