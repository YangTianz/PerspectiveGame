using UnityEngine;

public class LevelSceneManager : MonoBehaviour {

    public Save save;
    private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath + "/save.Json";
        save = SaveManager.ReadSave(filePath);
    }
}
