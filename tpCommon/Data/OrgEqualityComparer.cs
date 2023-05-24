using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class OrgEqualityComparer : IEqualityComparer<Org>
    {
        public int GetHashCode(Org org) { return org.Id.GetHashCode(); }
        public bool Equals(Org org1, Org org2) { return org1.Id == org1.Id; }
    }
}
