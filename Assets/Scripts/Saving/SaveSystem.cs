using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

namespace Assets.Scripts.Saving
{
    public static class SaveSystem
    {
        private static string path = Path.Combine(Application.persistentDataPath, "gameSaveData");

        public static void Save(GameData data)
        {
            try
            {
                var bf = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Create);

                bf.Serialize(stream, data);
                stream.Close();
            }
            catch(Exception ex) { Debug.LogError(ex.Message); }
        }
        public static GameData Load()
        {
            GameData lastSave = null;
            if (File.Exists(path))
            {
                try
                {
                    var bf = new BinaryFormatter();
                    var stream = new FileStream(path, FileMode.Open);
                    lastSave = bf.Deserialize(stream) as GameData;
                    stream.Close();
                }
                catch(Exception ex) { Debug.LogError(ex.Message); }
            }
            return lastSave;
        }
        public static void DeleteSaveFile()
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex) { Debug.LogError(ex.Message); }
            }
        }

    }
}
