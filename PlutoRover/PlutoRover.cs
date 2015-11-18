using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoRover
{
    public class PlutoRover
    {
        //Note: we'll probably eventually need a better world-state tracking to list all the obstacles for task #4

        private int WorldBoundX;    //Indicates the number of units available, starting from coordinate 0 - eg. when passed a worldbound of 100 and 100, the X wrapping goes    97, 98, 99, 0, 1 
        private int WorldBoundY;
        public RoverState CurrentState;

        public PlutoRover(RoverState initialState, int worldBoundX, int worldBoundY)
        {
            WorldBoundX = worldBoundX;
            WorldBoundY = worldBoundY;
            CurrentState = initialState;
        }

        public CommandResult ExecuteCommands(string commandString)
        {
            CommandResult commandResult = null;

            foreach(char command in commandString.ToArray())
            {
                switch(command)
                {
                    case 'F':
                        commandResult = MoveForward();
                        break;
                    case 'B':
                        commandResult = MoveBackward();
                        break;
                    case 'L':
                        commandResult = TurnLeft();
                        break;
                    case 'R':
                        commandResult = TurnRight();
                        break;
                    default:
                        return new CommandResult { 
                            Message = "Unrecognized command received.", 
                            RoverState = CurrentState, 
                            Status = CommandStatus.Failure };
                }

                if(commandResult.Status == CommandStatus.Failure)
                {
                    return commandResult;
                }
            }

            return null;
        }

        private CommandResult MoveForward()
        {
            switch(CurrentState.Facing)
            {
                case Facing.North:
                    CurrentState.Y++;
                    if(CurrentState.Y == WorldBoundY)
                    {
                        CurrentState.Y = 0;
                    }
                    break;
                case Facing.South:
                    CurrentState.Y--;
                    if(CurrentState.Y < 0)
                    {
                        CurrentState.Y = WorldBoundY - 1;
                    }
                    break;
                case Facing.East:
                    CurrentState.X++;
                    if(CurrentState.X == WorldBoundX)
                    {
                        CurrentState.X = 0;
                    }
                    break;
                case Facing.West:
                    CurrentState.X--;
                    if(CurrentState.X < 0)
                    {
                        CurrentState.X = WorldBoundX - 1;
                    }
                    break;
            }

            return new CommandResult { 
                RoverState = CurrentState, 
                Status = CommandStatus.Success, 
                Message = String.Format("Rover moved to [{0},{1}]", CurrentState.X, CurrentState.Y) };
        }

        private CommandResult MoveBackward()
        {
            switch (CurrentState.Facing)
            {
                case Facing.North:
                    CurrentState.Y--;
                    if(CurrentState.Y < 0)
                    {
                        CurrentState.Y = WorldBoundY - 1;
                    }
                    break;
                case Facing.South:
                    CurrentState.Y++;
                    if(CurrentState.Y == WorldBoundY)
                    {
                        CurrentState.Y = 0;
                    }
                    break;
                case Facing.East:
                    CurrentState.X--;
                    if(CurrentState.X < 0)
                    {
                        CurrentState.X = WorldBoundX - 1;
                    }
                    break;
                case Facing.West:
                    CurrentState.X++;
                    if(CurrentState.X == WorldBoundX)
                    {
                        CurrentState.X = 0;
                    }
                    break;
            }

            return new CommandResult
            {
                RoverState = CurrentState,
                Status = CommandStatus.Success,
                Message = String.Format("Rover moved to [{0},{1}]", CurrentState.X, CurrentState.Y)
            };
        }

        private CommandResult TurnLeft()
        {
            switch(CurrentState.Facing)
            {
                case Facing.North:
                    CurrentState.Facing = Facing.West;
                    break;
                case Facing.South:
                    CurrentState.Facing = Facing.East;
                    break;
                case Facing.East:
                    CurrentState.Facing = Facing.North;
                    break;
                case Facing.West:
                    CurrentState.Facing = Facing.South;
                    break;
            }

            return new CommandResult
            {
                RoverState = CurrentState,
                Status = CommandStatus.Success,
                Message = String.Format("Rover turned left and is now facing {0}", CurrentState.Facing)
            };
        }

        private CommandResult TurnRight()
        {
            switch (CurrentState.Facing)
            {
                case Facing.North:
                    CurrentState.Facing = Facing.East;
                    break;
                case Facing.South:
                    CurrentState.Facing = Facing.West;
                    break;
                case Facing.East:
                    CurrentState.Facing = Facing.South;
                    break;
                case Facing.West:
                    CurrentState.Facing = Facing.North;
                    break;
            }

            return new CommandResult
            {
                RoverState = CurrentState,
                Status = CommandStatus.Success,
                Message = String.Format("Rover turned right and is now facing {0}", CurrentState.Facing)
            };
        }
    }

    public class CommandResult
    {
        public string Message { get; set; }
        public CommandStatus Status { get; set;}
        public RoverState RoverState { get; set; }
    }

    public enum CommandStatus
    {
        Success,
        Failure
    }

    public enum Facing
    {
        North,
        South,
        East,
        West
    }

    public class RoverState
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Facing Facing { get; set; }
    }
}
