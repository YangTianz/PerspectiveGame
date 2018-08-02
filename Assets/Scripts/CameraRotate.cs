using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotate : MonoBehaviour
{

    public float speedX = 1f;
    public float speedY = 1f;
    public float xDelta = 5f;
    public float yDelta = 5f;
    public float CameraTime = 0.5f;
    public float aimFieldOfView = 47f;
    public float lightOffsetX = 0f;
    public float lightOffsetY = 10f;
    public float aimLightDelta = 5f;
    public bool movable;
    public bool aimAtCamera;

    private float x = 0f;
    private float y = 0f;
    private float initX;
    private float initY;
    private Quaternion initRotation;
    private Quaternion initLightRotation;
    private float initFieldOfView;
    private float initLightAngle;
    private Camera thisCamera;
    private bool aimAtLight;
    private Quaternion aimInitRotation;
    private Quaternion aimInitLightRotation;
    private float aimStartTime;
    private float aimEndTime;
    private float aimLightAngle;

    private void Awake()
    {
        initRotation = transform.rotation;
        aimInitRotation = initRotation;
        
        
        initX = transform.eulerAngles.x;
        initY = transform.eulerAngles.y;
        x = initX;
        y = initY;
        thisCamera = GetComponent<Camera>();
        initFieldOfView = thisCamera.fieldOfView;
    }

    private void Start()
    {
        initLightRotation = GameManager.Instance.spotLight.transform.rotation;
        initLightAngle = GameManager.Instance.initSpotLightSpotAngle;
        aimInitLightRotation = initLightRotation;
        aimLightAngle = initLightAngle + aimLightDelta;
    }

    private void FixedUpdate()
    {
        if (movable)
        {
            x -= Input.GetAxis("Mouse Y") * speedY;
            y += Input.GetAxis("Mouse X") * speedX;
            float tempDeltaX = x - initX;
            tempDeltaX = Mathf.Clamp(tempDeltaX, -xDelta, xDelta);
            float tempDeltaY = y - initY;
            tempDeltaY = Mathf.Clamp(tempDeltaY, -yDelta, yDelta);
            x = initX + tempDeltaX;
            y = initY + tempDeltaY;
            transform.eulerAngles = new Vector3(x, y);
            GameManager.Instance.spotLight.transform.eulerAngles = new Vector3(x, y + lightOffsetY);
        }
        if (aimAtCamera)
        {
            transform.rotation = Quaternion.Slerp(aimInitRotation, initRotation, (Time.time - aimStartTime) / CameraTime);
            thisCamera.fieldOfView = Mathf.Lerp(initFieldOfView, aimFieldOfView, (Time.time - aimStartTime) / CameraTime);
            x = initX;
            y = initY;
            if (aimAtLight)
            {
                GameManager.Instance.spotLight.transform.rotation = Quaternion.Slerp(aimInitLightRotation, initLightRotation, (Time.time - aimStartTime) / CameraTime);
                GameManager.Instance.spotLight.GetComponent<Light>().spotAngle = Mathf.Lerp(initLightAngle, aimLightAngle, (Time.time - aimStartTime) / CameraTime);
            }
        }
        else
        {
            if (thisCamera.fieldOfView != initFieldOfView)
            {
                thisCamera.fieldOfView = Mathf.Lerp(aimFieldOfView, initFieldOfView, (Time.time - aimEndTime) / CameraTime);
                if (!GameManager.Instance.win || !GameManager.Instance.fail)
                {
                    GameManager.Instance.spotLight.spotAngle = Mathf.Lerp(aimLightAngle, initLightAngle, (Time.time - aimEndTime) / CameraTime);
                }
            }
        }
    }

    public void aimEnabled()
    {
        aimAtLight = true;
        aimCameraEnabled();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        aimInitLightRotation = GameManager.Instance.spotLight.transform.rotation;
    }

    public void aimCameraEnabled()
    {
        aimAtCamera = true;
        aimStartTime = Time.time;
        aimInitRotation = transform.rotation;
        movable = false;
    }

    public void aimUnabled()
    {
        aimAtLight = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        foreach (GameObject Line in GameManager.Instance.lines)
        {
            Line.GetComponent<Animator>().SetTrigger("Up");
        }
        aimCameraUnabled();
    }

    public void aimCameraUnabled()
    {
        aimAtCamera = false;
        movable = true;
        aimEndTime = Time.time;
    }

}
