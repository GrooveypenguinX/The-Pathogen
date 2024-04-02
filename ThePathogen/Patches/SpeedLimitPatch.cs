using Aki.Reflection.Patching;
using EFT;
using System.Reflection;
using Aki.Reflection.Utils;
using System;
using System.Linq;

namespace Boop.Pathogen.Patches
{
    public class SpeedLimitPatch : ModulePatch
    {   
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethod("UpdateSpeedLimitByHealth", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        }

        [PatchPostfix]
        static void PatchPostfix(ref Player __instance)
        {
            //Logger.LogInfo("UpdateSpeedLimitByHealth Triggered");

            try
            {

            SkillManager.GClass1774 speedBuff = __instance.Skills.ResilienceBuffBrokenLeg;
            SkillManager.GClass1774 eliteSprintBuff = __instance.Skills.ResilienceBuffBrokenLegRunElite;
            float calcValue = 0f;

            if (speedBuff.Value > 0)
            {
                //Logger.LogInfo("UpdateSpeedLimitByHealth L BOZO");
                if (__instance.MovementContext.PhysicalConditionIs(EPhysicalCondition.RightLegDamaged) || __instance.MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftLegDamaged))
                {
                    if (!__instance.MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers))
                    {
                        if (__instance.MovementContext.PhysicalConditionIs(EPhysicalCondition.RightLegDamaged) && __instance.MovementContext.PhysicalConditionIs(EPhysicalCondition.LeftLegDamaged))
                        {
                            calcValue = 0.65f - (0.45f * speedBuff.Value);
                            __instance.RemoveStateSpeedLimit(Player.ESpeedLimit.HealthCondition);
                            __instance.AddStateSpeedLimit(calcValue, Player.ESpeedLimit.HealthCondition);

                            if (eliteSprintBuff.Value > 0)
                            {
                                __instance.MovementContext.EnableSprint(true);
                            }
                        }
                        else
                        {
                            calcValue = 0.65f - (0.35f * speedBuff.Value);
                            __instance.RemoveStateSpeedLimit(Player.ESpeedLimit.HealthCondition);
                            __instance.AddStateSpeedLimit(calcValue, Player.ESpeedLimit.HealthCondition);

                            if (eliteSprintBuff.Value > 0)
                            {
                                __instance.MovementContext.EnableSprint(true);
                            }
                        }
                    }
                }
            

                    //Logger.LogInfo("UpdateSpeedLimitByHealth: " + speedBuff.Value + " - " + eliteSprintBuff.Value + " - " + calcValue);
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo("UpdateSpeedLimitByHealth Err: " + ex.Message);
            }
        }
    }
}