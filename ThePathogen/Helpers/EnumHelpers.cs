using EFT;
using System;

namespace Boop.Pathogen.Helpers
{
    public class EnumHelpers
    {
        public static EBuffId GetBuffEnum(string buffEnumName)
        {
            return (EBuffId)Enum.Parse(typeof(EBuffId), buffEnumName);
        }
    }
}
