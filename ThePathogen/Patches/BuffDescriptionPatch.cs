using Aki.Reflection.Patching;
using EFT;
using System.Reflection;
using Boop.Pathogen.Helpers;
using Boop.Pathogen.Locales;
using Aki.Reflection.Utils;

namespace Boop.Pathogen.Patches
{
    public class BuffDescriptionPatch : ModulePatch
    {
        private static EBuffId ResilienceBuffBrokenLeg = EnumHelpers.GetBuffEnum("ResilienceBuffBrokenLeg");

        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.UI.BuffIcon).GetMethod("GetBuffDescription", PatchConstants.PublicFlags | BindingFlags.Static);
        }
        
        private static string GetFormatedBuffDescription(string text, SkillManager.GClass1773 buff)
        {
            if (text.Contains("{0"))
            {
                return string.Format(text, buff.ValueObj);
            }
            else
            {
                return text;
            }
        }
        
        [PatchPrefix]
        static bool PatchPrefix(EFT.UI.BuffIcon __instance, SkillManager.GClass1773 buff, ref string __result)
        {
            //Logger.LogInfo("GetBuffDescription Triggered: ");

            switch(buff.Id)
            {
                case (EBuffId)1000:
                    __result = GetFormatedBuffDescription(BuffLocales.GetLocaleById("ResilienceBuffBrokenLegDescription"), buff);
                    return false;
                case (EBuffId)1001:
                    __result = GetFormatedBuffDescription(BuffLocales.GetLocaleById("ResilienceBuffBrokenLegRunDescription"), buff);
                    return false;
                case (EBuffId)1002:
                    __result = GetFormatedBuffDescription(BuffLocales.GetLocaleById("ResilienceBuffInfectionDescription"), buff);
                    return false;
                case (EBuffId)1003:
                    __result = GetFormatedBuffDescription(BuffLocales.GetLocaleById("ResilienceBuffBrokenLegEliteDescription"), buff);
                    return false;
                case (EBuffId)1004:
                    __result = GetFormatedBuffDescription(BuffLocales.GetLocaleById("ResilienceBuffBrokenLegRunEliteDescription"), buff);
                    return false;
                case 0:
                    return true;
            }

            return true;
        }
    }
}
