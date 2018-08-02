using UnityEngine;

//游戏场景中用于拖拽物体
public class ObjectRotate : MonoBehaviour
{
    public float speed = 5f;
    //用于指定鼠标拖拽时物体的边缘对象
    public bool movable = true;

    private void OnMouseDrag()
    {
        if (!GameManager.Instance.win && movable)
        {
            float rotationX = -Input.GetAxis("Mouse X");
            float rotationY = Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.up, rotationX, Space.World);
            transform.Rotate(Vector3.right, rotationY, Space.World);
        }
    }

    private void OnMouseDown()
    {
        if (movable)
        {
            if (!GameManager.Instance.win && !GameManager.Instance.fail)
            {
                GameManager.Instance.cameraRotate.aimEnabled();
                foreach (GameObject Line in GameManager.Instance.lines)
                {
                    Line.GetComponent<Animator>().SetTrigger("Down");
                }
            }
            if (GameManager.Instance.fail)
            {
                GameManager.Instance.cameraRotate.aimCameraEnabled();
            }
        }
    }

    private void OnMouseUp()
    {
        if (movable)
        {
            if (!GameManager.Instance.win && !GameManager.Instance.fail)
            {
                GameManager.Instance.cameraRotate.aimUnabled();
                foreach (GameObject Line in GameManager.Instance.lines)
                {
                    Line.GetComponent<Animator>().SetTrigger("Up");
                }
            }
            if (GameManager.Instance.fail)
            {
                GameManager.Instance.cameraRotate.aimCameraUnabled();
            }
        }
    }

}
