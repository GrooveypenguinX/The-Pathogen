using System;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using Aki.Reflection.Utils;
using EFT;

namespace Boop.Pathogen.Patches
{
    public class SkillPatches
    {
        public class NewSkillPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                var desiredMethod = typeof(SkillManager).GetMethod("method_3", PatchConstants.PublicFlags);

                return desiredMethod;
            }
            
            [PatchPostfix]
            private static void PatchPostfix(SkillManager __instance, ref SkillClass ___Resilience, ref SkillClass ___Immunity, ref SkillManager.GClass1774 ___ResilienceBuffBrokenLeg, ref SkillManager.GClass1774 ___ResilienceBuffBrokenLegRun, ref SkillManager.GClass1774 ___ResilienceBuffInfection, ref SkillManager.GClass1774 ___ResilienceBuffBrokenLegElite, ref SkillManager.GClass1774 ___ResilienceBuffBrokenLegRunElite, ref SkillManager.GClass1780 ___LowHPDuration)
            {
                //Logger.LogInfo("Patching (Setting var Value)");

                try
                {
                    ESkillId skill = (ESkillId)Enum.Parse(typeof(ESkillId), "Resilience");
                    EBuffId brokenLegBuff = (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffBrokenLeg");
                    EBuffId brokenLegRunBuff = (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffBrokenLegRun");
                    EBuffId infectionBuff = (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffInfection");
                    EBuffId brokenLegEliteBuff = (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffBrokenLegElite");
                    EBuffId brokenLegRunEliteBuff = (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffBrokenLegRunElite");

                    ___ResilienceBuffBrokenLeg = new SkillManager.GClass1774 { Id = brokenLegBuff, BuffType = SkillManager.EBuffType.Switching };
                    ___ResilienceBuffBrokenLegRun = new SkillManager.GClass1774 { Id = brokenLegRunBuff };
                    ___ResilienceBuffInfection = new SkillManager.GClass1774 { Id = infectionBuff, BuffType = SkillManager.EBuffType.Switching };
                    ___ResilienceBuffBrokenLegElite = new SkillManager.GClass1774 { Id = brokenLegEliteBuff, BuffType = SkillManager.EBuffType.Elite };
                    ___ResilienceBuffBrokenLegRunElite = new SkillManager.GClass1774 { Id = brokenLegRunEliteBuff, BuffType = SkillManager.EBuffType.Elite };

                    ___Resilience = new SkillClass(__instance, skill, ESkillClass.Mental, new SkillManager.GClass1780[]
                    {
                    ___LowHPDuration.Factor(0.1f, true)
                    }, new SkillManager.GClass1773[]
                    {
                    ___ResilienceBuffBrokenLeg.PerLevel(0.005f).Elite(0.5f),
                    ___ResilienceBuffBrokenLegRun.PerLevel(0.01f),
                    ___ResilienceBuffInfection.PerLevel(0.01f).Elite(1f),
                    ___ResilienceBuffBrokenLegElite.PerLevel(0f).Elite(1f),
                    ___ResilienceBuffBrokenLegRunElite.PerLevel(0f).Elite(1f)
                    });

                    //Logger.LogInfo("Patched (Set var value)");
                    //Logger.LogInfo(___Resilience.ToString());
                } catch (Exception ex)
                {
                    Logger.LogInfo("Resilience Skill Patches Err: " + ex.Message);
                }
            }
        }
    }
}
