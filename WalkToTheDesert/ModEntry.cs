using StardewModdingAPI;
using HarmonyLib;
using StardewValley;

namespace WalkToTheDesert {
    internal class ModEntry : Mod {

        public static ModConfig Config;
        public static IMonitor SMonitor;

        public override void Entry(IModHelper helper) {
            Config = helper.ReadConfig<ModConfig>();
            SMonitor = Monitor;

            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(Game1), nameof(Game1.ShouldDismountOnWarp)),
                postfix: new HarmonyMethod(typeof(ModEntry), nameof(Game1_ShouldDismountOnWarp_Postfix))
            );
        }

        static void Game1_ShouldDismountOnWarp_Postfix(GameLocation __1, ref bool __result) { // __1 is the index equivelant of new_location from original method
            bool dismount = __1.IsOutdoors || __1.treatAsOutdoors.Value; // Check if new_location is outdoors or treated as outdoors
            __result = !dismount; // Set the result that's passed back to the original call
        }                         // True causes dismount and we don't want to dismount in places treated as outdoors
    }
}