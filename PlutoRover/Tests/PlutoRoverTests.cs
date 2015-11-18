using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlutoRover.Tests
{
    [TestFixture]
    public class PlutoRoverTests
    {

        PlutoRover rover;

        [SetUp]
        public void SetUp() 
        {
            rover = new PlutoRover(
                        new RoverState { Facing = Facing.North, X = 0, Y = 0 },
                        100, 
                        100,
                        new List<Tuple<int,int>> { new Tuple<int, int> (5,5), 
                                                   new Tuple<int, int> (5,7) }); 
        }

        /*
         * Implement commands that move the rover forward/backward (‘F’,’B’). The rover may
         * only move forward/backward by one grid point, and must maintain the same heading.
         */
        [Test]
        public void move_rover_fowards()
        {
            rover.ExecuteCommands("F");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.North);
            Assert.AreEqual(rover.CurrentState.X, 0);
            Assert.AreEqual(rover.CurrentState.Y, 1);
        }

        [Test]
        public void move_rover_backwards()
        {
            rover.ExecuteCommands("B");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.North);
            Assert.AreEqual(rover.CurrentState.X, 0);
            Assert.AreEqual(rover.CurrentState.Y, 99);
        }
        
        [Test]
        public void move_rover_forwards_three_times()
        {
            rover.ExecuteCommands("FFF");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.North);
            Assert.AreEqual(rover.CurrentState.X, 0);
            Assert.AreEqual(rover.CurrentState.Y, 3);
        }

        [Test]
        public void move_rover_forwards_and_backwards()
        {
            RoverState initialState = new RoverState { Facing = rover.CurrentState.Facing, X = rover.CurrentState.X, Y = rover.CurrentState.Y };

            rover.ExecuteCommands("FBFBFB");

            Assert.AreEqual(rover.CurrentState.Facing, initialState.Facing);
            Assert.AreEqual(rover.CurrentState.X, initialState.X);
            Assert.AreEqual(rover.CurrentState.Y, initialState.Y);
        }

        /*
         * Implement commands that turn the rover left/right (‘L’,’R’). These commands make
         * the rover spin 90 degrees left or right respectively, without moving from its current
         * spot         
         */

        [Test]
        public void turn_rover_left()
        {
            rover.ExecuteCommands("L");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.West);
            Assert.AreEqual(rover.CurrentState.X, 0);
            Assert.AreEqual(rover.CurrentState.Y, 0);
        }

        [Test]
        public void turn_rover_right()
        {
            rover.ExecuteCommands("R");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.East);
            Assert.AreEqual(rover.CurrentState.X, 0);
            Assert.AreEqual(rover.CurrentState.Y, 0);
        }

        [Test]
        public void turn_rover_right_twice()
        {
            rover.ExecuteCommands("RR");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.South);
            Assert.AreEqual(rover.CurrentState.X, 0);
            Assert.AreEqual(rover.CurrentState.Y, 0);
        }

        [Test]
        public void turn_rover_360()
        {
            rover.ExecuteCommands("LLLL");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.North);
            Assert.AreEqual(rover.CurrentState.X, 0);
            Assert.AreEqual(rover.CurrentState.Y, 0);
        }

        [Test]
        public void move_rover_in_a_square()
        {
            RoverState initialState = new RoverState { Facing = rover.CurrentState.Facing, X = rover.CurrentState.X, Y = rover.CurrentState.Y };

            rover.ExecuteCommands("FFRFF");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.East);
            Assert.AreEqual(rover.CurrentState.X, 2);
            Assert.AreEqual(rover.CurrentState.Y, 2);

            rover.ExecuteCommands("RFFRFFR");

            Assert.AreEqual(rover.CurrentState.Facing, initialState.Facing);
            Assert.AreEqual(rover.CurrentState.X, initialState.X);
            Assert.AreEqual(rover.CurrentState.Y, initialState.Y);
        }

        /*
         * Implement wrapping from one edge of the grid to another. (Pluto is a sphere after all)
         */

        [Test]
        public void move_rover_wrapping_x_grid_edge()
        {
            rover.ExecuteCommands("FFLFF");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.West);
            Assert.AreEqual(rover.CurrentState.X, 98);
            Assert.AreEqual(rover.CurrentState.Y, 2);
        }

        [Test]
        public void move_rover_wrapping_y_grid_edge()
        {
            rover.ExecuteCommands("BBRFFFF");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.East);
            Assert.AreEqual(rover.CurrentState.X, 4);
            Assert.AreEqual(rover.CurrentState.Y, 98);
        }

        [Test]
        public void move_rover_wrapping_diagonally()
        {
            rover.ExecuteCommands("BRB");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.East);
            Assert.AreEqual(rover.CurrentState.X, 99);
            Assert.AreEqual(rover.CurrentState.Y, 99);
        }

        /*
         * Implement obstacle detection before each move to a new square. If a given
         * sequence of commands encounters an obstacle, the rover moves up to the last
         * possible point and reports the obstacle.
         */

        //5,5   // 5,7
        [Test]
        public void move_rover_with_obstacle()
        {
            rover.ExecuteCommands("FFFFFRFFFFF");

            Assert.AreEqual(rover.CurrentState.Facing, Facing.East);
            Assert.AreEqual(rover.CurrentState.X, 4);
            Assert.AreEqual(rover.CurrentState.Y, 5);
            Assert.AreEqual(rover.CurrentState.Status, CommandStatus.Failure);
        }
        
        //[Test]
        //public void move_rover_with_obstacle_approaching_from_different_facing()
        //{

        //}

        //[Test]
        //public void move_rover_between_two_obstacles()
        //{

        //}

        //[Test]
        //public void move_rover_to_obstacle_on_wrap_boundary()
        //{
                //todo: add a new obstacle
        //}

        [Test]
        public void move_rover_with_some_unrecognized_commands()
        {
            RoverState initialState = new RoverState { Facing = rover.CurrentState.Facing, X = rover.CurrentState.X, Y = rover.CurrentState.Y };

            rover.ExecuteCommands("FFQBB");                             //I'm not 100% sure about this terminating behavior; but let's roll with it for now.

            Assert.AreEqual(rover.CurrentState.X, initialState.X);
            Assert.AreNotEqual(rover.CurrentState.Y, initialState.Y);
        }
    }
}
