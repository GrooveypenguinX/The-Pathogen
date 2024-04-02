using Aki.Reflection.Patching;
using EFT.UI;
using EFT;
using System;
using System.Reflection;
using UnityEngine;
using System.IO;

namespace Boop.Pathogen.Patches
{
    public class BuffIconPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(StaticIcons.BuffIdToSpriteDictionary).GetMethod(nameof(StaticIcons.BuffIdToSpriteDictionary.GetValueOrDefault));
        }

        [PatchPrefix]
        static bool Prefix(StaticIcons.BuffIdToSpriteDictionary __instance, EBuffId key, ref Sprite __result)
        {
            //Logger.LogInfo("StaticIcons BuffIdToSpriteDictionary Triggered");

            if (key == (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffBrokenLeg") || key == (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffBrokenLegRun") || key == (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffInfection") || key == (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffBrokenLegElite") || key == (EBuffId)Enum.Parse(typeof(EBuffId), "ResilienceBuffBrokenLegRunElite"))
            {
                //Logger.LogInfo("StaticIcons BuffIdToSpriteDictionary Key Checked");
                if (!__instance.TryGetValue(key, out var result))
                {
                    //Logger.LogInfo("StaticIcons BuffIdToSpriteDictionary Enum Valid");
                    var texture2D = new Texture2D(90, 90);

                    try
                    {
                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ThePathogen.Images.skill_resilience.png"))
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            var imageData = memoryStream.ToArray();
                            texture2D.LoadImage(imageData);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogInfo("Pathogen Buff Icon Patch Err: " + ex.Message);
                    }

                    __instance[key] = __result = Sprite.Create(texture2D, new Rect(0f, 0f, 90f, 90f), Vector2.zero);
                    //Logger.LogInfo("StaticIcons BuffIdToSpriteDictionary Patched");
                }
                else
                {
                    __result = result;
                }

                return false;
            }

            return true;
        }
    }
}
