namespace Intact.BusinessLogic.Data.Enums;

public enum CommandError : short
{
    NoError = 0,
    InvalidPlayer = 1,
    NotYourTurn = 2,
    InvalidCommand = 3,
    InvalidValue = 4,
    RoomNotFound = 5,
    PlayerNotInRoom = 6,
    GameNotStarted = 7
}
