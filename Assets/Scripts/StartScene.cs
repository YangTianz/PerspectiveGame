using System.Collections;
using UnityEngine;

//开始场景管理脚本
public class StartScene : MonoBehaviour {
    //场景特效
    public Animator transitionAnim;
    public int screenWidth = 1280;
    public int screenHeight = 720;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("isFull"))
        {
            if(PlayerPrefs.GetInt("isFull") == 0)
            {
                Screen.SetResolution(screenWidth, screenHeight, false);
            }
            else
            {
                Screen.SetResolution(screenWidth, screenHeight, true);
            }
        }
    }

    public void OnClickExit()
    {
        StartCoroutine(Exit());
    }

    IEnumerator Exit()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
