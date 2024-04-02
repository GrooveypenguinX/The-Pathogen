using Mono.Cecil;
using System.Collections.Generic;
using System;
using System.Linq;
using BepInEx.Logging;
using System.Diagnostics;
using System.Reflection;
using FieldAttributes = Mono.Cecil.FieldAttributes;

public static class Patcher
{
    public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };

    public static TypeDefinition skillManager;

    private static FieldDefinition CreateNewEnum(ref AssemblyDefinition assembly ,string AttributeName, string EnumName, TypeDefinition EnumClass, int CustomConstant)
    {
        TypeDefinition enumAttributeClass = assembly.MainModule.GetType("GAttribute20");
        MethodReference attributeConstructor = enumAttributeClass.Methods.First(m => m.IsConstructor);
        CustomAttributeArgument valueArgument = new CustomAttributeArgument(assembly.MainModule.TypeSystem.String, AttributeName);

        CustomAttribute attribute = new CustomAttribute(attributeConstructor);
        attribute.ConstructorArguments.Add(valueArgument);

        var newEnum = new FieldDefinition(EnumName, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault, EnumClass) { Constant = CustomConstant };
        newEnum.CustomAttributes.Add(attribute);

        return newEnum;
    }

    private static void PatchNewSkills(ref AssemblyDefinition assembly)
    {
        // Skill Patching
        Logger.CreateLogSource("The Pathogen Pre-Patcher").LogInfo("Patching Skills...");

        /// New Skill Enum
        Logger.CreateLogSource("The Pathogen Pre-Patcher").LogInfo("Patching Skills Enum...");
        TypeDefinition skillEnums = assembly.MainModule.GetType("EFT.ESkillId");

        FieldDefinition resilienceSkill = CreateNewEnum(ref assembly, "Resilience", "Resilience", skillEnums, 1000);

        skillEnums.Fields.Add(resilienceSkill);

        /// New Skill Var
        Logger.CreateLogSource("The Pathogen Pre-Patcher").LogInfo("Patching Skills Var...");
        TypeDefinition skillClass = assembly.MainModule.GetType("SkillClass");

        FieldDefinition skillsVar = new FieldDefinition("Resilience", FieldAttributes.Public, skillClass);
        skillManager.Fields.Add(skillsVar);
    }

    private static void PatchNewBuffs(ref AssemblyDefinition assembly)
    {
        /// New Buffs Enums
        Logger.CreateLogSource("The Pathogen Pre-Patcher").LogInfo("Patching Buffs Enum...");
        TypeDefinition buffEnums = assembly.MainModule.GetType("EFT.EBuffId");
        TypeDefinition skillEnums = assembly.MainModule.GetType("EFT.ESkillId");

        //// ResilienceBuffBrokenLeg
        FieldDefinition brokenLegEnum = CreateNewEnum(ref assembly, "ResilienceBuffBrokenLeg", "ResilienceBuffBrokenLeg", buffEnums, 1000);

        //// ResilienceBuffBrokenLegRun
        FieldDefinition brokenLegRunEnum = CreateNewEnum(ref assembly, "ResilienceBuffBrokenLegRun", "ResilienceBuffBrokenLegRun", buffEnums, 1001);

        //// ResilienceBuffInfection
        FieldDefinition infectionEnum = CreateNewEnum(ref assembly, "ResilienceBuffInfection", "ResilienceBuffInfection", buffEnums, 1002);

        //// ResilienceBuffBrokenLegElite
        FieldDefinition brokenLegEliteEnum = CreateNewEnum(ref assembly, "ResilienceBuffBrokenLegElite", "ResilienceBuffBrokenLegElite", buffEnums, 1003);

        //// ResilienceBuffBrokenLegRunElite
        FieldDefinition brokenLegRunEliteEnum = CreateNewEnum(ref assembly, "ResilienceBuffBrokenLegRunElite", "ResilienceBuffBrokenLegRunElite", buffEnums, 1004);

        buffEnums.Fields.Add(brokenLegEnum);
        buffEnums.Fields.Add(brokenLegRunEnum);
        buffEnums.Fields.Add(infectionEnum);
        buffEnums.Fields.Add(brokenLegEliteEnum);
        buffEnums.Fields.Add(brokenLegRunEliteEnum);

        /// New Buffs Vars
        Logger.CreateLogSource("The Pathogen Pre-Patcher").LogInfo("Patching Buffs Var...");
        TypeDefinition gClass1774 = skillManager.NestedTypes.FirstOrDefault(t => t.Name == "GClass1774");

        FieldDefinition brokenLegBuffVar = new FieldDefinition("ResilienceBuffBrokenLeg", FieldAttributes.Public, gClass1774);
        FieldDefinition brokenLegRunBuffVar = new FieldDefinition("ResilienceBuffBrokenLegRun", FieldAttributes.Public, gClass1774);
        FieldDefinition infectionBuffVar = new FieldDefinition("ResilienceBuffInfection", FieldAttributes.Public, gClass1774);
        FieldDefinition brokenLegEliteVar = new FieldDefinition("ResilienceBuffBrokenLegElite", FieldAttributes.Public, gClass1774);
        FieldDefinition brokenLegRunEliteVar = new FieldDefinition("ResilienceBuffBrokenLegRunElite", FieldAttributes.Public, gClass1774);

        skillManager.Fields.Add(brokenLegBuffVar);
        skillManager.Fields.Add(brokenLegRunBuffVar);
        skillManager.Fields.Add(infectionBuffVar);
        skillManager.Fields.Add(brokenLegEliteVar);
        skillManager.Fields.Add(brokenLegRunEliteVar);
    }

    private static void PatchLocalPlayer(ref AssemblyDefinition assembly)
    {
        // EFT.LocalPlayer Patching
        /// New Var for Local Player Defining Infection
        Logger.CreateLogSource("The Pathogen Pre-Patcher").LogInfo("Patching LocalPlayer Infection Var...");
        TypeDefinition localPlayerClass = assembly.MainModule.GetType("EFT.LocalPlayer");
        TypeReference boolean = assembly.MainModule.ImportReference(typeof(bool));

        FieldDefinition infectionVar = new FieldDefinition("IsInfected", FieldAttributes.Public | FieldAttributes.HasDefault, boolean) { Constant = false };
        localPlayerClass.Fields.Add(infectionVar);
    }

    public static void Patch(ref AssemblyDefinition assembly)
    {
        try
        {
            //Set Global Vars
            skillManager = assembly.MainModule.GetType("EFT.SkillManager");

            PatchNewSkills(ref assembly);
            PatchNewBuffs(ref assembly);
            PatchLocalPlayer(ref assembly);

            Logger.CreateLogSource("The Pathogen Pre-Patcher").LogInfo("Patching Complete!");
        } catch (Exception ex)
        {
            // Get stack trace for the exception with source file information
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();

            Logger.CreateLogSource("The Pathogen Pre-Patcher").LogInfo("Error When Patching: " + ex.Message + " - Line " + line);
        }

        // For some fucking reson requires this to be written in order to build the plugin... Reeeeeeeeeeeeeeee
        //assembly.Write("Assembly-CSharp-pathogen.dll");
    }

}