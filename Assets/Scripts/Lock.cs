using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//level场景中用于关卡图标脚本
public class Lock : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public LevelSceneManager levelSceneManager;

    private int scenNumber;
    private Button btn;
    private Animator anim;
    void Awake ()
    {
        anim = gameObject.GetComponent<Animator>();
        scenNumber = int.Parse(gameObject.name);
        btn = gameObject.GetComponent<Button>();
	}

    private void Start()
    {
        SetButton(levelSceneManager.save.unLockScenes[scenNumber - 1]);
    }

    public void SetButton(bool state)
    {
        transform.GetChild(1).gameObject.SetActive(state);
        transform.GetChild(0).gameObject.SetActive(!state);
        btn.enabled = state;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("IsActive", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("IsActive", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        anim.SetTrigger("Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        anim.SetTrigger("Up");
    }

    public void buttonEnabled()
    {
        btn.enabled = true;
    }

    public void buttonUnabled()
    {
        btn.enabled = false;
    }
}
