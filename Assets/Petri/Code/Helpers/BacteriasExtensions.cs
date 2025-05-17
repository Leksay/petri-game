using Petri.Configs;
using Petri.ECS;

namespace Petri.Helpers
{
    public static class BacteriasExtensions
    {
        public static void Set(this ref Bacteria bacteria, BacteriaData data)
        {
            bacteria.Hp = data.BaseHealth;
            bacteria.Type = data.Type;
        }
    }
}