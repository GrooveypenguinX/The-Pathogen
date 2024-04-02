using BepInEx;
using System;

namespace Boop.Pathogen
{
    [BepInPlugin("com.Boop.Pathogen", "The Pathogen", "0.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            //Logger.LogInfo($"Loading Patches for THINGY");
            try
            {
               new Patches.SkillClassPatches.SkillClassPatch().Enable();
                new Patches.SkillPatches.NewSkillPatch().Enable();
                new Patches.SkillIconPatch().Enable();
                new Patches.SkillTooltipPatch().Enable();
                new Patches.BuffIconPatch().Enable();
                new Patches.BuffDescriptionPatch().Enable();
                new Patches.SpeedLimitPatch().Enable();
                new Patches.CanSprintPatch().Enable();
            } catch (Exception ex)
            {
                Logger.LogInfo("Err: " + ex.Message);
            }

            Logger.LogInfo($"Plugin Pathogen is loaded!");
        }
    }
}
