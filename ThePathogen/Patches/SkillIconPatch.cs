using Aki.Reflection.Patching;
using EFT.UI;
using EFT;
using System;
using System.Reflection;
using UnityEngine;
using System.IO;

namespace Boop.Pathogen.Patches
{
    public class SkillIconPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(StaticIcons.SkillIdToSpriteDictionary).GetMethod(nameof(StaticIcons.SkillIdToSpriteDictionary.GetValueOrDefault));
        }

        [PatchPrefix]
        static bool Prefix(StaticIcons.SkillIdToSpriteDictionary __instance, ESkillId key, ref Sprite __result)
        {
            //Logger.LogInfo("StaticIcons SkillIdToSpriteDictionary Triggered");

            if (key == (ESkillId)Enum.Parse(typeof(ESkillId), "Resilience"))
            {
                //Logger.LogInfo("StaticIcons SkillIdToSpriteDictionary Key Checked");
                if (!__instance.TryGetValue(key, out var result))
                {
                    //Logger.LogInfo("StaticIcons SkillIdToSpriteDictionary Enum Valid");
                    var texture2D = new Texture2D(90, 90);

                    try {
                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ThePathogen.Images.skill_resilience.png"))
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            var imageData = memoryStream.ToArray();
                            texture2D.LoadImage(imageData);
                        }
                    } catch(Exception ex)
                    {
                        Logger.LogInfo("Pathogen Skill Icon Patch Err: " + ex.Message);
                    }

                    __instance[key] = __result = Sprite.Create(texture2D, new Rect(0f, 0f, 90f, 90f), Vector2.zero);
                    //Logger.LogInfo("StaticIcons SkillIdToSpriteDictionary Patched");
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
