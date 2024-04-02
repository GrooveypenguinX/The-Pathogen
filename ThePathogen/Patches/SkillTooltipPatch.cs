using Aki.Reflection.Patching;
using Aki.Reflection.Utils;
using System;
using System.Reflection;

namespace Boop.Pathogen.Patches
{
    public class SkillTooltipPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(SkillTooltipClass).GetMethod("method_4", PatchConstants.PublicFlags);
        }

        [PatchPrefix]
        static bool PatchPrefix(ref object __instance, string id, ref string __result)
        {
            // Add SkillIconPatch Functionality to Prevent Checking Multiple Times
            //Logger.LogInfo("SkillTooltipClass Localized Id Triggered: " + id);
            try
            {
                if (id == "ResilienceDescription")
                {
                    //Logger.LogInfo("SkillTooltipClass Localized Id Checked and Confirmed");

                    __result = "Resilience affects the ability to persevere through injuries and withstand disease.";
                    return false;
                }
                else
                {
                    return true;
                }
            } catch (Exception ex)
            {
                Logger.LogInfo("SkillTooltipPatch Err: " + ex.Message);
                return true;
            }
        }
    }
}
