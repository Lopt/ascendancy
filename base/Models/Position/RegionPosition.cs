using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace @base.model
{
    public class RegionPosition : Object
    {
        [JsonConstructor]
        public RegionPosition(int regionX, int regionY)
        {
            RegionX = regionX;
            RegionY = regionY;
        }

        public RegionPosition(Position position)
        {
            RegionX = (int)(position.X / Constants.REGION_SIZE_X);
            RegionY = (int)(position.Y / Constants.REGION_SIZE_Y);
        }

        public RegionPosition(PositionI position)
        {
            RegionX = (int)(position.X / Constants.REGION_SIZE_X);
            RegionY = (int)(position.Y / Constants.REGION_SIZE_Y);
        }


        public RegionPosition(JContainer obj)
        {
            RegionX = (int)obj.SelectToken("RegionX");
            RegionY = (int)obj.SelectToken("RegionY");
        }

        public static RegionPosition operator +(RegionPosition first, RegionPosition second)
        {
            return new RegionPosition(first.RegionX + second.RegionX, first.RegionY + second.RegionY);
        }

        public int RegionX
        {
            get;
            private set;
        }

        public int RegionY
        {
            get;
            private set;
        }

        [JsonIgnore]
        public int MajorX
        {
            get
            {
                return RegionX / Constants.MAJOR_REGION_SIZE_X;
            }
        }

        [JsonIgnore]
        public int MajorY
        {
            get
            {
                return RegionY / Constants.MAJOR_REGION_SIZE_Y;
            }
        }


        public override bool Equals(Object obj)
        {
            var regionPosition = (RegionPosition)obj;
            return (regionPosition.RegionX == RegionX && regionPosition.RegionY == RegionY);
        }

        public override int GetHashCode()
        {
            return RegionX * 1000000 + RegionY;
        }

    }
}