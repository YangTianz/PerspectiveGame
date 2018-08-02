using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//作为每个按钮的组件检测按钮状态
public class ButtomCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler , IPointerDownHandler, IPointerUpHandler{

    private Animator anim;
    private Button button;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        button = GetComponent<Button>();
        button.enabled = false;
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
        button.enabled = true;
    }

    public void buttonUnabled()
    {
        button.enabled = false;
    }
}
