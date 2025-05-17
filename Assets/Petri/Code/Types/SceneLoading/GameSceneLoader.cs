using System.Threading.Tasks;
using Petri.Events;
using Petri.Infrostructure;
using Petri.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Petri.SceneLoading
{
    public class GameSceneLoader : IService, IEventReceiver<StartGameEvent>
    {
        public UniqueId Id { get; } = new UniqueId();

        private const int MenuSceneId = 0;
        private const int GameSceneId = 1;

        public GameSceneLoader()
        {
            ServiceLocator.Get<EventBus>().Register(this);
        }

        private void LoadScene(int sceneIndex)
        {
            var loadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex);
            Task.Run(() => LoadSceneTask(loadSceneOperation));
        }

        private async Task LoadSceneTask(AsyncOperation loadSceneOperation)
        {
            while (!loadSceneOperation.isDone)
            {
                Debug.Log(loadSceneOperation.progress);
                await Task.Yield();
            }
        }

        public void OnEvent(StartGameEvent startGameEvent)
        {
            LoadScene(GameSceneId);
        }
    }
}