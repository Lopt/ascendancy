using System;
namespace @base.model
{
    public class RegionStates
    {


        public RegionManager Last;
        public RegionManager Curr;
        public RegionManager Next;

        public RegionStates(RegionManager last, RegionManager curr, RegionManager next)
        {
            Last = last;
            Curr = curr;
            Next = next;
        }
    }
}

