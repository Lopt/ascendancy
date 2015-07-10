using System;
using Newtonsoft.Json;

namespace @base.model
{
    public class Position
    {
        [JsonConstructor]
        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Position(PositionI position)
        {
            X = position.X;
            Y = position.Y;
        }

        public Position(LatLon latLon)
        {
            var zoom = Constants.EARTH_CIRCUMFERENCE / Constants.CELL_SIZE;
            X = (float)((latLon.Lon + 180.0) / 360.0 * zoom);
            Y = (float)((1.0 - Math.Log(Math.Tan(latLon.Lat * Math.PI / 180.0) +
                1.0 / Math.Cos(latLon.Lat * Math.PI / 180.0)) / Math.PI) / 2.0 * zoom);
        }

        public Position(RegionPosition regionPosition)
        {
            X = regionPosition.RegionX * Constants.REGION_SIZE_X;
            Y = regionPosition.RegionY * Constants.REGION_SIZE_Y;
        }

        public Position(RegionPosition regionPosition, CellPosition cellPosition)
        {
            X = regionPosition.RegionX * Constants.REGION_SIZE_X + cellPosition.CellX;
            Y = regionPosition.RegionY * Constants.REGION_SIZE_Y + cellPosition.CellY;
        }

        public double X
        {
            get;
            private set;
        }

        public double Y
        {
            get;
            private set;
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




        public override bool Equals(Object obj)
        {
            var pos = (Position)obj;
            return this == pos;
        }

        public static Position operator +(Position first, Position second)
        {
            return new Position(first.X + second.X, first.Y + second.Y);
        }

        public static Position operator -(Position first, Position second)
        {
            return new Position(first.X - second.X, first.Y - second.Y);
        }

        public static bool operator ==(Position first, Position second)
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

        public static bool operator !=(Position first, Position second)
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



        public double Distance(Position position)
        {
            var xDistance = (position.X - X);
            var yDistance = (position.Y - Y);
            return xDistance * xDistance + yDistance * yDistance;
        }

        public double Distance(PositionI position)
        {
            var xDistance = (position.X - X);
            var yDistance = (position.Y - Y);
            return xDistance * xDistance + yDistance * yDistance;
        }
    }
}

