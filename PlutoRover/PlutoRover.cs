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

        private int WorldBoundX;
        private int WorldBoundY;
        public RoverState CurrentState;

        public PlutoRover(RoverState initialState, int worldBoundX, int worldBoundY)
        {
            WorldBoundX = worldBoundX;
            WorldBoundY = worldBoundY;
            CurrentState = initialState;
        }

        public CommandResult MoveForward()
        {
            switch(CurrentState.Facing)
            {
                case Facing.North:
                    CurrentState.Y++;
                    break;
                case Facing.South:
                    CurrentState.Y--;
                    break;
                case Facing.East:
                    CurrentState.X++;
                    break;
                case Facing.West:
                    CurrentState.X--;
                    break;
            }

            return new CommandResult { 
                RoverState = CurrentState, 
                Status = CommandStatus.Success, 
                Message = String.Format("Rover moved to [{0},{1}]", CurrentState.X, CurrentState.Y) };
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
