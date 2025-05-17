using Petri.Events;
using Petri.Infrostructure;
using UnityEngine;
using UnityEngine.UI;

namespace Petri.UI
{
    public class MainMenuButtonsListener : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(OnStartGameButtonPressed);
        }

        private static void OnStartGameButtonPressed()
        {
            ServiceLocator.Get<EventBus>().Raise(new StartGameEvent());
        }
    }
}