using Petri.Formula;
using UnityEngine;

namespace Petri.Configs
{
    [CreateAssetMenu(fileName = "ReagentGroupColorsConfig", menuName = "Petri/ReagentGroupColorsConfig")]
    public class ReagentGroupColorsConfig : ScriptableObject
    {
        public Color GroupA;
        public Color GroupB;
        public Color GroupC;
        public Color GroupD;
        public Color GroupE;
        public Color GroupF;
    }

    public static class ReagentGroupExtensions
    {
        private static ReagentGroupColorsConfig _instance;

        public static ReagentGroupColorsConfig Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.Load<ReagentGroupColorsConfig>("ReagentGroupColorsConfig");
                }
                return _instance;
            }
        }
                
        public static Color GetColor(this ReagentGroup reagentGroup)
        {
            return reagentGroup switch
            {
                ReagentGroup.GroupA => Instance.GroupA,
                ReagentGroup.GroupB => Instance.GroupB,
                ReagentGroup.GroupC => Instance.GroupC,
                ReagentGroup.GroupD => Instance.GroupD,
                ReagentGroup.GroupE => Instance.GroupE,
                ReagentGroup.GroupF => Instance.GroupD,
                ReagentGroup.None => Color.white,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}