using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace @base.model
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

        public PositionI(LatLon latLon)
        {
            var zoom = Constants.EARTH_CIRCUMFERENCE / Constants.CELL_SIZE;
            X = (int)((latLon.Lon + 180.0) / 360.0 * zoom);
            Y = (int)((1.0 - Math.Log(Math.Tan(latLon.Lat * Math.PI / 180.0) +
                1.0 / Math.Cos(latLon.Lat * Math.PI / 180.0)) / Math.PI) / 2.0 * zoom);
        }

        public PositionI(RegionPosition regionPosition, CellPosition cellPosition)
        {
            X = regionPosition.RegionX * Constants.REGION_SIZE_X + cellPosition.CellX;
            Y = regionPosition.RegionY * Constants.REGION_SIZE_Y + cellPosition.CellY;
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


        public override bool Equals(Object obj)
        {
            var pos = (PositionI)obj;
            return this == pos;
        }


        public override int GetHashCode()
        {
            return X * 1000000 + Y;
        }
    }
}

