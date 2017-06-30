using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// The actual protaganist of the game
    /// The Actor controlled by the player
    /// </summary>
    public class Player : Actor
    {
        /// <summary>
        /// Constructor with initial dungeon
        /// </summary>
        /// <param name="Dungeon"></param>
        public Player(Dungeon Dungeon) : base(Dungeon)
        {
        }

        public override string SpriteId
        {
            get
            {
                return "PLAYER";
            }
        }

        /// <summary>
        /// Act, per NextMove
        /// </summary>
        public override void DoTurn()
        {
            switch (NextMove)
            {
                case Instruction.MOVE_NW:
                    Move(new XY(-1, -1));
                    break;
                case Instruction.MOVE_N:
                    Move(new XY(0, -1));
                    break;
                case Instruction.MOVE_NE:
                    Move(new XY(1, -1));
                    break;
                case Instruction.MOVE_W:
                    Move(new XY(-1, 0));
                    break;
                case Instruction.MOVE_E:
                    Move(new XY(1, 0));
                    break;
                case Instruction.MOVE_SW:
                    Move(new XY(-1, 1));
                    break;
                case Instruction.MOVE_S:
                    Move(new XY(0, 1));
                    break;
                case Instruction.MOVE_SE:
                    Move(new XY(1, 1));
                    break;
                case Instruction.WAIT:
                default:
                    break;
            }
            NextMove = Instruction.NOT_SET;
        }

        /// <summary>
        /// Instructions for NextMove
        /// </summary>
        public enum Instruction
        {
            NOT_SET,
            MOVE_NW,
            MOVE_N,
            MOVE_NE,
            MOVE_W,
            MOVE_E,
            MOVE_SW,
            MOVE_SE,
            MOVE_S,
            WAIT
        }

        public Instruction NextMove;
    }
}
