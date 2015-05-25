using System;
namespace @base.control
{
    public class RegionStatesController
    {

        public RegionManagerController Last;
        public RegionManagerController Curr;
        public RegionManagerController Next;

        public RegionStatesController(RegionManagerController last, RegionManagerController curr, RegionManagerController next)
        {
            Last = last;
            Curr = curr;
            Next = next;
        }

    }
}

