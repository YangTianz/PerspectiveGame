using UnityEngine;

//setting场景管理脚本
public class SettingScene : MonoBehaviour {

    //一级菜单中的按钮
    public GameObject[] buttons1;
    //二级菜单中的按钮
    public GameObject[] buttons2;


    public int screenWidth = 1280;
    public int screenHeight = 720;

    private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath + "/save.Json";
    }

    public void OnClickFullScreen()
    {
        Screen.SetResolution(screenWidth, screenHeight, true);
        PlayerPrefs.SetInt("isFull", 1);
        PlayerPrefs.Save();
    }

    public void OnClickWindow()
    {
        Screen.SetResolution(screenWidth, screenHeight, false);
        PlayerPrefs.SetInt("isFull", 0);
        PlayerPrefs.Save();
    }

    public void OnClickClearData()
    {
        foreach (GameObject button in buttons1)
        {
            button.GetComponent<Animator>().SetTrigger("Out");
        }
        foreach(GameObject button in buttons2)
        {
            button.GetComponent<Animator>().SetTrigger("In");
        }
    }

    public void OnClickNo()
    {
        foreach (GameObject button in buttons1)
        {
            button.GetComponent<Animator>().SetTrigger("In");
        }
        foreach (GameObject button in buttons2)
        {
            button.GetComponent<Animator>().SetTrigger("Out");
        }
    }

    public void OnClickYes()
    {
        Save save = SaveManager.InitSave();
        SaveManager.WriteSave(save, filePath);
        OnClickNo();
    }

    
}
