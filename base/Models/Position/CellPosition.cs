using System;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace Core.Models
{
    public class CellPosition : Object
    {
        public CellPosition(int cellX, int cellY)
        {
            CellX = ((int)cellX % Constants.REGION_SIZE_X);
            CellY = ((int)cellY % Constants.REGION_SIZE_Y);
        }

        public CellPosition(Position position)
        {
            CellX = ((int)position.X % Constants.REGION_SIZE_X);
            CellY = ((int)position.Y % Constants.REGION_SIZE_Y);
        }

        public CellPosition(PositionI position)
        {
            CellX = (position.X % Constants.REGION_SIZE_X);
            CellY = (position.Y % Constants.REGION_SIZE_Y);
        }

        public CellPosition(JContainer obj)
        {
            CellX = (int)obj.SelectToken("CellX");
            CellY = (int)obj.SelectToken("CellY");
        }

        public int CellX
        {
            get;
            private set;
        }

        public int CellY
        {
            get;
            private set;
        }

        public override bool Equals(Object obj)
        {
            var cellPosition = (CellPosition)obj;
            return (cellPosition.CellX == CellX && cellPosition.CellY == CellY);
        }

        public static bool operator ==(CellPosition obj, CellPosition obj2)
        {
            return obj.CellX == obj2.CellX && obj.CellY == obj2.CellY;        
        }

        public static bool operator !=(CellPosition obj, CellPosition obj2)
        {
            return obj.CellX != obj2.CellX || obj.CellY != obj2.CellY;        
        }

        public override int GetHashCode()
        {
            return CellX * Constants.REGION_SIZE_Y + CellY;
        }
    }
}

