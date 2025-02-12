using Verse;
using UnityEngine;
using HarmonyLib;

namespace KeyzOutpostFunerals;

public class KeyzOutpostFuneralsMod : Mod
{

    public KeyzOutpostFuneralsMod(ModContentPack content) : base(content)
    {
        Log.Message("Hello world from KeyzOutpostFunerals");
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("keyz182.rimworld.KeyzOutpostFunerals.main");	
        harmony.PatchAll();
    }
}
