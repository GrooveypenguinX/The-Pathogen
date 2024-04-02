using Aki.Reflection.Patching;
using EFT;
using System.Reflection;
using Aki.Reflection.Utils;
using System;

namespace Boop.Pathogen.Patches
{
    public class CanSprintPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(MovementContext).GetProperty("CanSprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).GetGetMethod();
        }

        [PatchPrefix]
        static bool PatchPrefix(ref MovementContext __instance, ref bool __result, ref Player ____player)
        {
            //Logger.LogInfo("CanSprint Triggered");

            try
            {
                if (!__instance.PhysicalConditionIs(EPhysicalCondition.SprintDisabled) && !__instance.PhysicalConditionIs(EPhysicalCondition.UsingMeds) && !__instance.PhysicalConditionIs(EPhysicalCondition.HealingLegs))
                {
                    if (____player.Skills.ResilienceBuffBrokenLegRunElite > 0)
                    {
                        __result = true;
                    }
                    else
                    {
                        if (__instance.PhysicalConditionIs(EPhysicalCondition.OnPainkillers) || (!__instance.PhysicalConditionIs(EPhysicalCondition.RightLegDamaged) && !__instance.PhysicalConditionIs(EPhysicalCondition.LeftLegDamaged)))
                        {
                            __result = true;
                        }
                        else
                        {
                            __result = false;
                        }
                    }
                }
                else
                {
                    __result = false;
                }

                if (__result == true) __instance.EnableSprint(true);

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("CanSprint Err: " + ex.Message);
                return true;
            }
        }
    }
}
