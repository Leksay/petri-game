using Petri.Events;
using Petri.SceneLoading;
using Petri.UI;
using UnityEngine;

namespace Petri.Infrostructure
{
    public class GameInitializer : MonoBehaviour
    {
        private static GameInitializer _instance;
        
        [SerializeField] private MainMenuButtonsListener _mainMenuButtonsListener;

        private void Awake()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                ServiceLocator.Register(new EventBus());
                ServiceLocator.Register(new GameSceneLoader());

                _instance = this;
                
                return;
            }

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                ServiceLocator.Clear();
            }
        }
    }
}