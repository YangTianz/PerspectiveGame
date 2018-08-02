using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {
    //场景转换特效1
    public Animator UITransitionAnim;
    //场景转换特效2
    public Animator GameTransitionAnim;

    public void OnClickButton(string scene)
    {
        StartCoroutine(LoadScene(scene));
    }

    public void OnClickGame(string scene)
    {
        StartCoroutine(LoadGame(scene));
    }

    IEnumerator LoadGame(string sceneName)
    {
        GameTransitionAnim.SetTrigger("game");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadScene(string sceneName)
    {
        UITransitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
