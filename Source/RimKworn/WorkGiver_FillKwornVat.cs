using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimKworn
{
    public class WorkGiver_FillKwornVat : WorkGiver_Scanner
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

        public static void ResetStaticData()
        {
            WorkGiver_FillKwornVat.TemperatureTrans = "BadTemperature".Translate().ToLower();
            WorkGiver_FillKwornVat.NoSugarTrans = "NoSugar".Translate();
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_KwornVat building_KwornVat = t as Building_KwornVat;
            if (building_KwornVat == null || building_KwornVat.Fermented || building_KwornVat.SpaceLeftForSugar <= 0)
            {
                return false;
            }
            float ambientTemperature = building_KwornVat.AmbientTemperature;
            CompProperties_TemperatureRuinable compProperties = building_KwornVat.def.GetCompProperties<CompProperties_TemperatureRuinable>();
            if (ambientTemperature < compProperties.minSafeTemperature + 2f || ambientTemperature > compProperties.maxSafeTemperature - 2f)
            {
                JobFailReason.Is(WorkGiver_FillKwornVat.TemperatureTrans, null);
                return false;
            }
            if (!t.IsForbidden(pawn))
            {
                LocalTargetInfo target = t;
                if (pawn.CanReserve(target, 1, -1, null, forced))
                {
                    if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
                    {
                        return false;
                    }
                    if (this.FindSugar(pawn, building_KwornVat) == null)
                    {
                        JobFailReason.Is(WorkGiver_FillKwornVat.NoSugarTrans, null);
                        return false;
                    }
                    return !t.IsBurning();
                }
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_KwornVat vat = (Building_KwornVat)t;
            Thing t2 = this.FindSugar(pawn, vat);
            return new Job(JobDefOf.FillKwornVat, t, t2);
        }

        private Thing FindSugar(Pawn pawn, Building_KwornVat vat)
        {
            Predicate<Thing> predicate = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false);
            IntVec3 position = pawn.Position;
            Map map = pawn.Map;
            ThingRequest thingReq = ThingRequest.ForDef(RimKworn.ThingDefOf.SugarRefined);
            PathEndMode peMode = PathEndMode.ClosestTouch;
            TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
            Predicate<Thing> validator = predicate;
            return GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
        }

        private static string TemperatureTrans;

        private static string NoSugarTrans;
    }
}