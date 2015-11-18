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
        private List<Tuple<int, int>> WorldObstacles;
        public RoverState CurrentState;

        public PlutoRover(RoverState initialState, int worldBoundX, int worldBoundY, List<Tuple<int, int>> worldObstacles)
        {
            WorldBoundX = worldBoundX;
            WorldBoundY = worldBoundY;
            CurrentState = initialState;
            WorldObstacles = worldObstacles;
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
                        CurrentState.Status = CommandStatus.Failure;

                        return new CommandResult { 
                            Message = "Unrecognized command received.", 
                            RoverState = CurrentState };
                }

                if(CurrentState.Status == CommandStatus.Failure)
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
                    CurrentState.Status = (MoveUp()) ? 
                        CurrentState.Status = CommandStatus.Success : 
                        CurrentState.Status = CommandStatus.Failure ;
                    break;
                case Facing.South: 
                    CurrentState.Status = (MoveDown()) ?
                         CurrentState.Status = CommandStatus.Success :
                         CurrentState.Status = CommandStatus.Failure;
                    break;
                case Facing.East:
                    CurrentState.Status = (MoveRight()) ?
                         CurrentState.Status = CommandStatus.Success :
                         CurrentState.Status = CommandStatus.Failure;
                    break;
                case Facing.West:
                    CurrentState.Status = (MoveLeft()) ?
                        CurrentState.Status = CommandStatus.Success :
                        CurrentState.Status = CommandStatus.Failure;
                    break;
            }

            string message;
            if(CurrentState.Status == CommandStatus.Success)
            {
                message = String.Format("Rover moved to [{0},{1}]", CurrentState.X, CurrentState.Y);
            }
            else
            {
                message = String.Format("Obstacle blocked rover movement to [{0},{1}]", CurrentState.X, CurrentState.Y);
            }

            return new CommandResult { 
                RoverState = CurrentState, 
                Message = message };
        }

        private CommandResult MoveBackward()
        {

            switch (CurrentState.Facing)
            {
                case Facing.North:
                    CurrentState.Status = (MoveDown()) ?
                        CurrentState.Status = CommandStatus.Success :
                        CurrentState.Status = CommandStatus.Failure;
                    break;
                case Facing.South:
                    CurrentState.Status = (MoveUp()) ?
                         CurrentState.Status = CommandStatus.Success :
                         CurrentState.Status = CommandStatus.Failure;
                    break;
                case Facing.East:
                    CurrentState.Status = (MoveLeft()) ?
                         CurrentState.Status = CommandStatus.Success :
                         CurrentState.Status = CommandStatus.Failure;
                    break;
                case Facing.West:
                    CurrentState.Status = (MoveRight()) ?
                        CurrentState.Status = CommandStatus.Success :
                        CurrentState.Status = CommandStatus.Failure;
                    break;
            }

            string message;
            if (CurrentState.Status == CommandStatus.Success)
            {
                message = String.Format("Rover moved to [{0},{1}]", CurrentState.X, CurrentState.Y);
            }
            else
            {
                message = String.Format("Obstacle blocked rover movement to [{0},{1}]", CurrentState.X, CurrentState.Y);
            }

            return new CommandResult
            {
                RoverState = CurrentState,
                Message = message
            };

        }

        private bool ObstacleExists(int X, int Y)
        {
            //I'm not particularly happy with this as the complexity is poor for large numbers of obstacles
            //should probably be 2d array or something else
            //but in the interests of time will leave as is
            
            foreach(var obstacle in WorldObstacles)
            {
                if(obstacle.Item1 == X && obstacle.Item2 == Y)
                {
                    return true;
                }
            }

            return false;
        }

        private bool MoveUp()
        {
            int targetY = CurrentState.Y + 1;

            if (targetY == WorldBoundY)
            {
                targetY = 0;
            }

            if (ObstacleExists(CurrentState.X, targetY))
            {
                return false;
            }
            else
            {
                CurrentState.Y = targetY;
                return true;
            }          
        }

        private bool MoveDown()
        {
            int targetY = CurrentState.Y - 1;

            if (targetY < 0)
            {
                targetY = WorldBoundY - 1;
            }

            if(ObstacleExists(CurrentState.X, targetY))
            {
                return false;
            }
            else
            {
                CurrentState.Y = targetY;
                return true;
            }
        }

        private bool MoveLeft()
        {
            int targetX = CurrentState.X - 1;
            if (targetX < 0)
            {
                targetX = WorldBoundX - 1;
            }

            if(ObstacleExists(targetX, CurrentState.Y))
            {
                return false;
            }
            else
            {
                CurrentState.X = targetX;
                return true;
            }
        }

        private bool MoveRight()
        {
            int targetX = CurrentState.X + 1;
            
            if (targetX == WorldBoundX)
            {
                targetX = 0;
            }

            if (ObstacleExists(targetX, CurrentState.Y))
            {
                return false;
            }
            else
            {
                CurrentState.X = targetX;
                return true;
            }
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

            CurrentState.Status = CommandStatus.Success;

            return new CommandResult
            {
                RoverState = CurrentState,
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

            CurrentState.Status = CommandStatus.Success;

            return new CommandResult
            {
                RoverState = CurrentState,
                Message = String.Format("Rover turned right and is now facing {0}", CurrentState.Facing)
            };
        }
    }

    public class CommandResult
    {
        public string Message { get; set; }
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
        public CommandStatus Status { get; set; }
    }
}
