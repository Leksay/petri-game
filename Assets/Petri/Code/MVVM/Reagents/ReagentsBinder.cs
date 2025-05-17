using System.Collections.Generic;
using JfranMora.Inspector;
using Petri.Formula;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Petri.UI
{
    public class ReagentsBinder : MonoBehaviour
    {
        [field: SerializeField] public List<Reagent> AvailableReagents;

        [SerializeField] private ReagentView _reagentPrefab;
        [SerializeField] private RectTransform _reagentsParent;
        [SerializeField] private ScrollRect _scrollView;

        private void Start()
        {
            UpdateReagents();
        }

        public void UpdateReagents(List<Reagent> reagents)
        {
            AvailableReagents = reagents;
            UpdateReagents();
        }
        
        [Button]
        public void UpdateReagents()
        {
            while (_reagentsParent.childCount > 0)
            {
                DestroyImmediate(_reagentsParent.GetChild(0).gameObject);
            }

            for (int i = 0; i < AvailableReagents.Count; i++)
            {
                var newReagent = Instantiate(_reagentPrefab, _reagentsParent);
#if UNITY_EDITOR
                PrefabUtility.ConvertToPrefabInstance(newReagent.gameObject, _reagentPrefab.gameObject, new ConvertToPrefabInstanceSettings(), InteractionMode.AutomatedAction);
#endif
                newReagent.Initialize(AvailableReagents[i]);
                _scrollView.Rebuild(CanvasUpdate.Layout);
            }
        }
    }
}