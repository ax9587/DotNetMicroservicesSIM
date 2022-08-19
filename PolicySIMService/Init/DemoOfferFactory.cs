using PolicySIMService.Model;
using System.Collections.Generic;


namespace PolicySIMService.Init
{
    internal static class DemoOfferFactory
    {
        internal static Offer DemoOffer()
        {
            var h = new PolicyHolder("Alex", "X", "P", Address.Of("CA","TXT0XZ","OW","123 <ain ST"));
            var p = new Dictionary<string, decimal>();
            p.Add("P1", 123);
            var o = Offer.ForPrice("TRI", System.DateTime.Today, System.DateTime.Today, h,new Price(p));
            return o;
        }


    }
}
