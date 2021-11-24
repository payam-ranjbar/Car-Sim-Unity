using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace IO
{
    

    [Serializable]
    public class LockData
    {
        public bool isLocked;
        public int time;
    }

    public class BinarySerializer
    {
        public static void Serialize(object data, string path)
        {
            if (File.Exists(path)) File.Delete(path);
            var address = Application.persistentDataPath + "/" + path;
            FileStream fileStream = File.Create(address);
            Console.WriteLine($"data at: {address}");

            BinaryFormatter binarySerializer = new BinaryFormatter();
            binarySerializer.Serialize(fileStream, data);
            fileStream.Close();
        }


        public static T Deserialize<T>(string path, out bool exists)
        {
            var address = Application.persistentDataPath + "/" + path;
            exists = File.Exists(address);
            if (exists)
            {
                FileStream fileStream = File.OpenRead(address);

                BinaryFormatter formatter = new BinaryFormatter();

                var data = formatter.Deserialize(fileStream);

                if (data is T castedData)
                {
                    return castedData;
                }
            }

            return default;

        }

    }
}