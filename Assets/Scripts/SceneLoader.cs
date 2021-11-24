using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using IO;
using LockSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject curtain;
        [SerializeField] private string lobbyName = "Intro";
        [SerializeField] private string gameName = "play ground";

        [SerializeField] private LockerProperties lockerProperties;
        private IEnumerator LoadAsync(string sceneName)
        {
            curtain.SetActive(true);
            // yield return LockRequest();
            // yield return new WaitWhile(() => lockerProperties.IsLocked);
            var op = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => op.isDone);
            curtain.SetActive(false);
        }

        [ContextMenu("rEQ")]
        private void SendReq()
        {
            StartCoroutine(LockRequest());
        }
        private IEnumerator LockRequest()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(lockerProperties.URL))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                Debug.Log("Req to: " + lockerProperties.URL);

                yield return request.SendWebRequest();
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
                if (request.responseCode == 201)
                {
                    var data = JsonUtility.FromJson<LockReqData>(request.downloadHandler.text);
                    lockerProperties.SetData(data);
                    BinarySerializer.Serialize(data, lockerProperties.Path);
                }
                else
                {
                    var des = BinarySerializer.Deserialize<LockReqData>(lockerProperties.Path, out var exists);
                    if (exists)
                    {
                        lockerProperties.SetData(des);
                    }
                    else
                    {
                        var newReqdata = new LockReqData
                        {
                            isLock = lockerProperties.IsLocked,
                            time = 1
                        };
                        
                        BinarySerializer.Serialize(newReqdata, lockerProperties.Path);
                    }
                }


            }

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