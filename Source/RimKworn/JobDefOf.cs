using System;
using RimWorld;
using Verse;

namespace RimKworn
{
    [DefOf]
    public static class JobDefOf
    {
        static JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
        }

        public static JobDef FillKwornVat;

        public static JobDef TakeFungusOutOfKwornVat;
    }
}