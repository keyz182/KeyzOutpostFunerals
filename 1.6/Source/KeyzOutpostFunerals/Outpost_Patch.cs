using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Outposts;
using RimWorld;
using Verse;

namespace KeyzOutpostFunerals;

[HarmonyPatch(typeof(Outpost))]
public static class Outpost_Patch
{
    public static Lazy<MethodInfo> Deliver_PackAnimalInfo = new Lazy<MethodInfo>(()=>AccessTools.Method(typeof(Outpost), "Deliver_PackAnimal"));

    [HarmonyPatch(nameof(Outpost.Tick))]
    [HarmonyPostfix]
    public static void OutpostTick(Outpost __instance)
    {
        if (Find.TickManager.TicksGame % (GenDate.TicksPerHour * 12) != 0 || !__instance.Things.Any(p => p is Corpse)) return;

        Corpse deadPawn = __instance.Things.OfType<Corpse>().RandomElement();
        if (deadPawn == null) return;

        __instance.TakeItem(deadPawn);
        if (!DeliverDeadPawn(__instance, deadPawn))
        {
            __instance.AddItem(deadPawn);
        }
    }

    public static bool DeliverDeadPawn(Outpost __instance, Corpse pawn)
    {
        Map map = __instance.deliveryMap ?? Find.Maps.Where(m => m.IsPlayerHome).OrderBy(m => Find.WorldGrid.ApproxDistanceInTiles(m.Parent.Tile, __instance.Tile)).FirstOrDefault();
        if (map == null)
        {
            ModLog.Warn("KeyzOutpostFunerals Tried to deliver to a null map, storing instead");
            return false;
        }

        TaggedString text = "KOF_DeadPawnFromOutpost".Translate(pawn.InnerPawn.NameFullColored,  __instance.Name) + "\n";
        List<Thing> lookAt = [];
        Rot4 rotFromTo = Find.WorldGrid.GetRotFromTo(map.Parent.Tile, __instance.Tile);
        List<Thing> list = [pawn];

        Deliver_PackAnimalInfo.Value.Invoke(__instance, [list, map, rotFromTo, lookAt]);

        Find.LetterStack.ReceiveLetter("KOF_DeadPawnFromOutpostLabel".Translate(pawn.InnerPawn.NameShortColored), text, LetterDefOf.NeutralEvent, new LookTargets(lookAt));
        return true;
    }
}
