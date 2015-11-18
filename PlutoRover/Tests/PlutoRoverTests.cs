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
            rover = new PlutoRover(new RoverState { Facing = Facing.North, X = 0, Y = 0 }, 100, 100); 
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
            Assert.AreEqual(rover.CurrentState.Y, -1);                      //note: this will have to change when the world wraps
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
            RoverState initialState = rover.CurrentState;

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
        public void MoveRoverInASquare()
        {
            RoverState initialState = rover.CurrentState;

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

        /*
         * Implement obstacle detection before each move to a new square. If a given
         * sequence of commands encounters an obstacle, the rover moves up to the last
         * possible point and reports the obstacle.
         */



    }
}
