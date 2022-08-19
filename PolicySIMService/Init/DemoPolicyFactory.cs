using PolicySIMService.Model;
using System.Collections.Generic;


namespace PolicySIMService.Init
{
    internal static class DemoPolicyFactory
    {
        internal static Policy DemoPolicy()
        {
            var h = new PolicyHolder("Alex", "X", "P", Address.Of("CA","TXT0XZ","OW","123 <ain ST"));
            var p = new Dictionary<string, decimal>();
            p.Add("P1", 123);
            var o = Offer.ForPrice("TRI", System.DateTime.Today, System.DateTime.Today, h,new Price(p));
            var po = Policy.FromOffer(h,o);
            return po;
        }
    }
}
