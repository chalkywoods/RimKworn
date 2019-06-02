using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimKworn
{
    public class WorkGiver_TakeFungusOutOFKwornVat : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForDef(RimKworn.ThingDefOf.KwornVat);
            }
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_KwornVat building_KwornVat = t as Building_KwornVat;
            if (building_KwornVat == null || !building_KwornVat.Fermented)
            {
                return false;
            }
            if (t.IsBurning())
            {
                return false;
            }
            if (!t.IsForbidden(pawn))
            {
                LocalTargetInfo target = t;
                if (pawn.CanReserve(target, 1, -1, null, forced))
                {
                    return true;
                }
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(RimKworn.JobDefOf.TakeFungusOutOfKwornVat, t);
        }
    }
}
