using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Models
{
    public class PositionI
    {
        [JsonConstructor]
        public PositionI(int x, int y)
        {
            X = x;
            Y = y;
        }

        public PositionI(JContainer obj)
        {
            X = (int)obj.SelectToken("X");
            Y = (int)obj.SelectToken("Y");
        }
            
        public PositionI(RegionPosition regionPosition, CellPosition cellPosition)
        {
            X = regionPosition.RegionX * Constants.REGION_SIZE_X + cellPosition.CellX;
            Y = regionPosition.RegionY * Constants.REGION_SIZE_Y + cellPosition.CellY;
        }

        public PositionI(Position position)
        {
            X = (int) position.X;
            Y = (int) position.Y;
        }

        [JsonIgnore]
        public RegionPosition RegionPosition
        {
            get
            {
                return new RegionPosition(this);
            }
        }

        [JsonIgnore]
        public CellPosition CellPosition
        {
            get
            {
                return new CellPosition(this);
            }
        }

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }




        public override int GetHashCode()
        {
            return X * 1000000 + Y;
        }


        public override bool Equals(Object obj)
        {
            var pos = (PositionI)obj;
            return this == pos;
        }

        public static PositionI operator +(PositionI first, PositionI second)
        {
            return new PositionI(first.X + second.X, first.Y + second.Y);
        }

        public static PositionI operator -(PositionI first, PositionI second)
        {
            return new PositionI(first.X - second.X, first.Y - second.Y);
        }

        public static bool operator ==(PositionI first, PositionI second)
        {
            if (System.Object.ReferenceEquals(first, second))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)first == null) || ((object)second == null))
            {
                return false;
            }

            return (first.X == second.X && first.Y == second.Y);
        }

        public static bool operator !=(PositionI first, PositionI second)
        {
            if (System.Object.ReferenceEquals(first, second))
            {
                return false;
            }

            // If one is null, but not both, return false.
            if (((object)first == null) || ((object)second == null))
            {
                return true;
            }


            return (first.X != second.X || first.Y != second.Y);
        }


        public double Distance(PositionI position)
        {
            var xDistance = (position.X - X);
            var yDistance = (position.Y - Y);
            return xDistance * xDistance + yDistance * yDistance;
        }

        public double Distance(Position position)
        {
            var xDistance = (position.X - X);
            var yDistance = (position.Y - Y);
            return xDistance * xDistance + yDistance * yDistance;
        }

    }
}

