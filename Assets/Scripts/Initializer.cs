using System;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private SceneLoader loader;
        private  string Format = "txt";
        private  string Path => Application.persistentDataPath + "/" + "GAME_INIT" + "." + Format;

        private void Awake()
        {
            LoadGame();
        }

        public bool LoadGame()
        {
            
            if (!File.Exists(Path))
            {
                CreateFile();
                LoadLobby();
                return true;
            }

  
            FileStream fs = File.OpenRead(Path);
            var sr = new StreamReader(fs);
            string serialized = sr.ReadLine();
            sr.Close();
            var valid = int.TryParse(serialized, out var code);
            if (valid && code != 0)
            {
                LoadLobby();
                return true;
            }
            else
            {
                LoadMainGame();
                return false;
            }

        }

        private void CreateFile()
        {
            var serialized = "1";

            FileStream fs = new FileStream(Path, FileMode.Create);
            var sr = new StreamWriter(fs);
            sr.WriteLine(serialized);
            sr.Flush();
            sr.Close();
            
        }

        private void LoadLobby()
        {
            loader.LoadLobby();
        }

        private void LoadMainGame()
        { 
            loader.LoadMainGame();
        }
        
    }
}