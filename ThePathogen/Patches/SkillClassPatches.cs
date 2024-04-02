using System;
using BepInEx.Logging;
using System.Reflection;
using Aki.Reflection.Patching;
using HarmonyLib;
using EFT;

namespace Boop.Pathogen.Patches
{
    public class SkillClassPatches
    {
        public class SkillClassPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                var desiredMethod = typeof(SkillManager).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Public, null, new Type[] { typeof(EPlayerSide) }, null);

                Logger.LogDebug($"{this.GetType().Name} Method: {desiredMethod?.Name}");

                return desiredMethod;
            }
           
            [PatchPostfix]
            private static void PatchPostfix(ref object __instance, ref SkillClass ___Resilience, ref SkillClass[] ___DisplayList, ref SkillClass[] ___Skills)
            {
                //Logger.LogInfo("Patching (Adding to Skills and DisplayList)");
                int insertIndex = 12;

                try
                {
                    // Create a new array with an extra element
                    SkillClass[] newDisplayList = new SkillClass[___DisplayList.Length + 1];

                    // Copy the elements before the insert index to the new array
                    Array.Copy(___DisplayList, newDisplayList, insertIndex);

                    // Insert the new element at the desired index
                    newDisplayList[insertIndex] = ___Resilience;

                    // Copy the remaining elements to the new array
                    Array.Copy(___DisplayList, insertIndex, newDisplayList, insertIndex + 1, ___DisplayList.Length - insertIndex);

                    // Assign the new array to the ___DisplayList field
                    ___DisplayList = newDisplayList;

                    // Add the item to the end of the Skills array
                    Array.Resize(ref ___Skills, ___Skills.Length + 1);
                    ___Skills[___Skills.Length - 1] = ___Resilience;
                } catch(Exception ex)
                {
                    Logger.LogInfo("FUCKING ERROR BITCH" + ex.Message);
                }

                //Logger.LogInfo("Patched (Added to Skills and DisplayList)");
                //Logger.LogInfo(___Resilience.ToString());
            }
        }
    }
}