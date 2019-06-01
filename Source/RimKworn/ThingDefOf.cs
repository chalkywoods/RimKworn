using System;
using RimWorld;
using Verse;

namespace RimKworn
{
    [DefOf]
    public static class ThingDefOf
    {
        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }

        public static ThingDef Mycoprotein;

        public static ThingDef KwornVat;

        public static ThingDef SugarRefined;
    }
}
