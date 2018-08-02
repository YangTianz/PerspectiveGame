using LitJson;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static Save ReadSave(string filePath)
    {
        Save save;
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            save = JsonMapper.ToObject<Save>(jsonStr);
        }
        else
        {
            save = InitSave();
            WriteSave(save, filePath);
        }
        return save;
    }

    public static Save InitSave()
    {
        Save save = new Save();
        save.unLockScenes.Add(true);
        for (int i = 0; i < 20; i++)
        {
            save.unLockScenes.Add(false);
        }
        return save;
    }

    public static void WriteSave(Save save, string filePath)
    {
        string saveJsonStr = JsonMapper.ToJson(save);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
}
