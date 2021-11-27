using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarDemo
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject curtain;
        [SerializeField] private string lobbyName = "Intro";
        [SerializeField] private string gameName = "play ground";

        private IEnumerator LoadAsync(string sceneName)
        {
            curtain.SetActive(true);
            var op = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => op.isDone);
            curtain.SetActive(false);
        }



        public void LoadLobby()
        {
            StartCoroutine(LoadAsync(lobbyName));
        }

        public void LoadMainGame()
        {
            StartCoroutine(LoadAsync(gameName));
        }
    }
}