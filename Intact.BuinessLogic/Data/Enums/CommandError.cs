namespace Intact.BusinessLogic.Data.Enums;

public enum CommandError : short
{
    NoError = 0,
    InvalidCommand = 1,
    NotYourTurn = 2,
    PlayerOutOfRange = 3,
    InvalidRoomState = 4,
    InvalidValue = 5
}
