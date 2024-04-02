using System;
using System.Collections.Generic;
using System.Text;

namespace Boop.Pathogen.Locales
{
    public class BuffLocales
    {
        private static Dictionary<string, string> _locales = new Dictionary<string, string>()
        {
            { "ResilienceBuffBrokenLegDescription", "Increases walking speed by <color=#54c1ff>(+{0:0%})</color> while suffering from a leg fracture" }, // Implemented
            { "ResilienceBuffBrokenLegRunDescription", "Reduces damage taken while running with a leg fracture by <color=#54c1ff>(-{0:0%})</color>" },
            { "ResilienceBuffInfectionDescription", "Reduces the effects of an infection by <color=#54c1ff>(-{0:0%})</color>" }, // Will Be Unused
            { "ResilienceBuffBrokenLegRunEliteDescription", "Allows you to run with a leg fracture" },
            { "ResilienceBuffBrokenLegEliteDescription", "Chance to get the <color=#54c1ff>Resilient</color> buff when injured for a period of time" }
        };

        public static string GetLocaleById(string id)
        {
                if (_locales.ContainsKey(id))
                {
                    return _locales[id];
                }
                else
                {
                    return null;
                }
        }
    }
}
