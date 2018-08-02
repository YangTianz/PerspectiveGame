using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

//游戏主要脚本
public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    
    //判定胜利的表面
    public GameObject[] surfaces;
    public float timeSet = 20f;
    public float initFadeTime = 0.8f;
    public float warningTextTime = 3f;
    public float warningBackTime = 1.5f;

    //初始背景
    public GameObject initBack;
    //新手提示
    public GameObject guideText;
    public GameObject guide1;
    public GameObject guide2;
    public float guideRemainTime = 2f;
    //关卡提示
    public GameObject levelText;
    public float initFadeOutTime = 0.33f;
    //需要旋转的目标
    public GameObject GO;
    //物体自转速度
    public float objectRotateSpeed = 1.5f;
    //所有HUD
    public GameObject[] Huds;
    //时间Text
    public Text timeText;
    public GameObject warningBackGround;
    //游戏结束时的背景
    public GameObject endBackGround;
    //胜利后的图标
    public GameObject[] winButton;
    //失败后的图标
    public GameObject[] failButton;
    //旋转的摄像机
    public CameraRotate cameraRotate;
    public ObjectRotate objectRotate;
    public GameObject[] lines;
    public Light spotLight;
    public Light pointLight;
    public SceneChange sceneChange;

    public bool win = false;
    public bool fail = false;
    public float initSpotLightIntensity = 2.2f;
    public float initSpotLightSpotAngle = 46f;
    public float initPointLightIntensity = 1f;
    public float guideSpotLightIntensity = 1f;
    public float guideSpotLightSpotAngle = 120f;
    public float guidePointLightIntensity = 2f;
    public float winSpotLightIntensity = 1.5f;
    public float failSpotLightSpotAngle = 0f;
    public float failSpotLightIntensity = 0f;
    public float winPointLightIntensity = 0.9f;
    public float failPointLightIntensity = 1.8f;
    private float time;
    private Save save;
    private string filePath;
    private int sceneNumber;
    private bool inGuide;
    private bool inLevelText;
    private bool initFadeOut;
    private float fadeOutStartTime;
    private float fadeTime = 0.5f;
    private float endFadeTime = 1f;

    private float goalSpotLightSpotAngle;
    private float goalSpotLightIntensity;
    private float goalPointLightIntensity;
    private bool isTextWarning;
    private bool isBackWarning;
    private Quaternion initObjectRotation;
    private Quaternion tempObjectRotation;
    private bool autoRotate;

    private void Awake()
    {
        _instance = this;
        time = timeSet;
        filePath = Application.dataPath + "/save.Json";
        save = SaveManager.ReadSave(filePath);
        sceneNumber = int.Parse(SceneManager.GetActiveScene().name);
        cameraRotate.movable = true;
        objectRotate.movable = true;
        spotLight.spotAngle = guideSpotLightSpotAngle;
        spotLight.intensity = guideSpotLightIntensity;
        pointLight.intensity = guidePointLightIntensity;
        inGuide = save.isFirst;
        isTextWarning = false;
        isBackWarning = false;
        inLevelText = true;
        foreach (GameObject Hud in Huds)
        {
            Hud.SetActive(false);
        }
        initBack.SetActive(true);
        guideText.SetActive(false);
        guide1.SetActive(false);
        guide2.SetActive(false);
        if (inGuide)
        {
            save.isFirst = false;
            SaveManager.WriteSave(save, filePath);
        }
        warningBackGround.SetActive(false);
        levelText.SetActive(true);
        initObjectRotation = GO.transform.rotation;
        autoRotate = true;
    }

    private void Start()
    {
        objectRotate.movable = false;
        StartCoroutine(AutoLevelText());
        foreach (var line in lines)
        {
            line.GetComponent<Animator>().SetTrigger("Down");
        }
    }

    private void FixedUpdate()
    {
        if (!win && !fail && !inGuide && !inLevelText)
        {
            if(initFadeTime >= 0)
            {
                initFadeTime -= Time.deltaTime;
            }
            else
            {
                time -= Time.deltaTime;
                if(time <= warningTextTime && !isTextWarning)
                {
                    isTextWarning = true;
                    timeText.gameObject.GetComponent<Animator>().SetBool("IsActive", true);
                }
                if(time <= warningBackTime && !isBackWarning)
                {
                    warningBackGround.SetActive(true);
                }
            }
            timeText.text = string.Format("time : {0:F1}s", time);
            if(time <= 0)
            {
                FailThisGame();
            }
            Judge();
        }
        if (initFadeOut)
        {
            spotLight.intensity = Mathf.Lerp(guideSpotLightIntensity, initSpotLightIntensity, (Time.time - fadeOutStartTime) / (initFadeOutTime + fadeTime));
            spotLight.spotAngle = Mathf.Lerp(guideSpotLightSpotAngle, initSpotLightSpotAngle, (Time.time - fadeOutStartTime) / (initFadeOutTime + fadeTime));
            pointLight.intensity = Mathf.Lerp(guidePointLightIntensity, initPointLightIntensity, (Time.time - fadeOutStartTime) / (initFadeOutTime + fadeTime));
            GO.transform.rotation = Quaternion.Slerp(tempObjectRotation, initObjectRotation, (Time.time - fadeOutStartTime) / (initFadeOutTime + fadeTime));
            if (spotLight.spotAngle == initSpotLightSpotAngle)
            {
                initFadeOut = false;
                objectRotate.movable = true;
            }
        }
        if(win || fail)
        {
            spotLight.intensity = Mathf.Lerp(initSpotLightIntensity, goalSpotLightIntensity, (Time.time - fadeOutStartTime) / endFadeTime);
            spotLight.spotAngle = Mathf.Lerp(initSpotLightSpotAngle, goalSpotLightSpotAngle, (Time.time - fadeOutStartTime) / endFadeTime);
            pointLight.intensity = Mathf.Lerp(initPointLightIntensity, goalPointLightIntensity, (Time.time - fadeOutStartTime) / endFadeTime);
        }
        if (win && !autoRotate)
        {
            GO.transform.rotation = Quaternion.Slerp(tempObjectRotation, initObjectRotation, (Time.time - fadeOutStartTime) / (endFadeTime * 0.3f));
            if(Time.time - fadeOutStartTime >= endFadeTime * 0.3f)
            {
                autoRotate = true;
                foreach (var line in lines)
                {
                    line.SetActive(true);
                    line.GetComponent<Animator>().SetTrigger("Down");
                }
            }
        }
        if(autoRotate)
        {
            GO.transform.Rotate(Vector3.up, objectRotateSpeed, Space.World);
        }
    }

    public void Judge()
    {
        foreach (var surface in surfaces)
        {
            if(!Physics.Linecast(cameraRotate.transform.position, surface.transform.position))
            {
                WinThisGame();
                break;
            }
        }
    }

    private bool JudgeRange(float objectAxis, float goalAxis, float axisError)
    {
        return ((goalAxis - axisError)  <= objectAxis && (goalAxis + axisError)  >= objectAxis) || axisError == 360;
    }

    private void WinThisGame()
    {
        if (!fail)
        {
            win = true;
            tempObjectRotation = GO.transform.rotation;
            GameFinish();
            foreach (GameObject button in winButton)
            {
                button.GetComponent<Animator>().SetTrigger("In");
            }
            if (sceneNumber != 21)
            {
                save.unLockScenes[sceneNumber] = true;
                SaveManager.WriteSave(save, filePath);
            }
        }
    }

    private void FailThisGame()
    {
        if (!win)
        {
            fail = true;
            GameFinish();
            foreach (GameObject button in failButton)
            {
                button.GetComponent<Animator>().SetTrigger("In");
            }
        }
    }

    private void GameFinish()
    {
        fadeOutStartTime = Time.time;
        cameraRotate.aimUnabled();
        goalSpotLightSpotAngle = guideSpotLightSpotAngle;
        goalSpotLightIntensity = winSpotLightIntensity;
        goalPointLightIntensity = winPointLightIntensity;
        initSpotLightSpotAngle = spotLight.spotAngle;
        warningBackGround.SetActive(false);
        foreach (GameObject hud in Huds)
        {
            hud.GetComponent<Animator>().SetTrigger("Out");
        }
        foreach (var line in lines)
        {
            line.SetActive(false);
        }
        endBackGround.SetActive(true);
    }

    public void OnClickReset()
    {
        sceneChange.OnClickButton(sceneNumber.ToString());
    }

    public void OnClickBack()
    {
        sceneChange.OnClickButton("Level");
    }

    public void OnClickContinue()
    {
        sceneChange.OnClickGame((sceneNumber + 1).ToString());
    }

    public void Guide1Click()
    {
        StopAllCoroutines();
        GuideFirstStep();
    }

    IEnumerator Guide1Auto()
    {
        yield return new WaitForSeconds(guideRemainTime);
        GuideFirstStep();
    }

    private void GuideFirstStep()
    {
        guide1.GetComponent<Animator>().SetTrigger("Out");
        StartCoroutine(Guide2In());
    }

    IEnumerator Guide2In()
    {
        yield return new WaitForSeconds(initFadeOutTime);
        guide2.SetActive(true);
        guide1.SetActive(false);
        StartCoroutine(Guide2Auto());
    }

    IEnumerator Guide2Auto()
    {
        yield return new WaitForSeconds(guideRemainTime);
        GuideSecondStep();
    }

    private void GuideSecondStep()
    {
        guide2.GetComponent<Animator>().SetTrigger("Out");
        initBack.GetComponent<Animator>().SetTrigger("Out");
        guideText.GetComponent<Animator>().SetTrigger("Out");
        StartCoroutine(Guide2Out());
    }

    public void Guide2Click()
    {
        StopAllCoroutines();
        GuideSecondStep();
    }

    IEnumerator Guide2Out()
    {
        SceneFade();
        yield return new WaitForSeconds(initFadeOutTime);
        guideText.SetActive(false);
        guide2.SetActive(false);
        inGuide = false;
        ShowHud();
    }

    public void LevelTextClick()
    {
        StopAllCoroutines();
        LevelTextStep();
    }

    IEnumerator AutoLevelText()
    {
        yield return new WaitForSeconds(guideRemainTime);
        LevelTextStep();
    }

    private void LevelTextStep()
    {
        levelText.GetComponent<Animator>().SetTrigger("Out");
        if (!inGuide)
        {
            initBack.GetComponent<Animator>().SetTrigger("Out");
        }
        StartCoroutine(LevelTextOut());
    }

    IEnumerator LevelTextOut()
    {
        if (!inGuide)
        {
            SceneFade();
        }
        yield return new WaitForSeconds(initFadeOutTime);
        inLevelText = false;
        if (inGuide)
        {
            StartCoroutine(guideFadeIn());
        }
        else
        {
            ShowHud();
        }
        levelText.SetActive(false);
    }

    IEnumerator guideFadeIn()
    {
        yield return new WaitForSeconds(initFadeOutTime);
        guideText.SetActive(true);
        guide1.SetActive(true);
        initFadeOut = false;
        StartCoroutine(Guide1Auto());
    }

    private void ShowHud()
    {
        initBack.SetActive(false);
        foreach (GameObject hud in Huds)
        {
            hud.SetActive(true);
        }
        StopAllCoroutines();
        
    }

    private void SceneFade()
    {
        initFadeOut = true;
        tempObjectRotation = GO.transform.rotation;
        autoRotate = false;
        fadeOutStartTime = Time.time;
        foreach (var line in lines)
        {
            line.GetComponent<Animator>().SetTrigger("Up");
        }
    }
}
