///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_UnitySDKManager
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription:  Be fully careful of  Code modification!
/////////////////////////////////////////////////////////////////////////////// 
using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Collections.Generic;
using Pvr_UnitySDKAPI;
using UnityEngine.UI;

public class Pvr_UnitySDKManager : MonoBehaviour
{

    /************************************    Properties  *************************************/
    #region Properties
    public static PlatForm platform;
    
    //signtal                   
    private static Pvr_UnitySDKManager sdk = null;
    public static Pvr_UnitySDKManager SDK
    {
        get
        {
            if (sdk == null)
            {
                sdk = UnityEngine.Object.FindObjectOfType<Pvr_UnitySDKManager>();
            }
            if (sdk == null)
            {
                var go = new GameObject("Pvr_UnitySDKManager");
                sdk = go.AddComponent<Pvr_UnitySDKManager>();
                go.transform.localPosition = Vector3.zero;
            }
            return sdk;
        }
    }

    // Sensor
    [HideInInspector]
    public static Pvr_UnitySDKSensor pvr_UnitySDKSensor;
    [HideInInspector]
    public Pvr_UnitySDKPose HeadPose;
    [HideInInspector]
    public bool reStartHead = false;
    //render
    [HideInInspector]
    public static Pvr_UnitySDKRender pvr_UnitySDKRender;
    [SerializeField]
    private float eyeFov = 90.0f;
    [HideInInspector]
    public float EyeFov
    {
        get
        {
            return eyeFov;
        }
        set
        {
            if (value != eyeFov)
            {
                eyeFov = value;
            }
        }
    }
    [HideInInspector]
    public const int eyeTextureCount = 6;
    [HideInInspector]
    public RenderTexture[] eyeTextures = new RenderTexture[eyeTextureCount];
    [HideInInspector]
    public int[] eyeTextureIds = new int[eyeTextureCount];
    [HideInInspector]
    public int currEyeTextureIdx = 0;
    [HideInInspector]
    public int nextEyeTextureIdx = 1;
    [HideInInspector]
    public int resetRot = 0;
    [HideInInspector]
    public int resetPos = 0;
    [HideInInspector]
    public int posStatus = 0;
    [HideInInspector]
    public bool isPUI;
    [HideInInspector]
    public Vector3 resetBasePos = new Vector3();
    [HideInInspector]
    public Vector3 resetCol0Pos = new Vector3();
    [HideInInspector]
    public Vector3 resetCol1Pos = new Vector3();
    [HideInInspector]
    public int trackingmode = -1;
    [HideInInspector]
    public int platFormType = -1;
    [HideInInspector]
    public bool systemFPS = false;

    [HideInInspector]
    public float[] headData = new float[7] {0, 0, 0, 0, 0, 0, 0};

    [SerializeField]
    private HeadDofNum _headDofNum = HeadDofNum.SixDof;
    public HeadDofNum HeadDofNum
    {
        get
        {
            return _headDofNum;
        }
        set
        {
            if (value != _headDofNum)
            {
                _headDofNum = value;

            }
        }
    }
    [SerializeField]
    private HandDofNum _handDofNum = HandDofNum.SixDof;
    public HandDofNum HandDofNum
    {
        get
        {
            return _handDofNum;
        }
        set
        {
            if (value != _handDofNum)
            {
                _handDofNum = value;

            }
        }
    }
    [SerializeField]
    private HandNum handnum = HandNum.Two;
    public HandNum HandNum
    {
        get
        {
            return handnum;
        }
        set
        {
            if (value != handnum)
            {
                handnum = value;

            }
        }
    }
    
    [SerializeField]
    private RenderTextureAntiAliasing rtAntiAlising = RenderTextureAntiAliasing.X_2;
    public RenderTextureAntiAliasing RtAntiAlising
    {
        get
        {
            return rtAntiAlising;
        }
        set
        {
            if (value != rtAntiAlising)
            {
                rtAntiAlising = value;

            }
        }
    }
    [SerializeField]
    private RenderTextureDepth rtBitDepth = RenderTextureDepth.BD_24;
    public RenderTextureDepth RtBitDepth
    {
        get
        {
            return rtBitDepth;
        }
        set
        {
            if (value != rtBitDepth)
                rtBitDepth = value;

        }
    }
    [SerializeField]
    private RenderTextureFormat rtFormat = RenderTextureFormat.Default;
    public RenderTextureFormat RtFormat
    {
        get
        {
            return rtFormat;
        }
        set
        {
            if (value != rtFormat)
                rtFormat = value;

        }
    }


    [SerializeField]
     public float RtSizeWH = 1280.0f;

  

    [HideInInspector]
    public int RenderviewNumber = 0;
    public Vector3 EyeOffset(Eye eye)
    {
        return eye == Eye.LeftEye ? leftEyeOffset : rightEyeOffset;
    }
    [HideInInspector]
    public Vector3 leftEyeOffset;
    [HideInInspector]
    public Vector3 rightEyeOffset;
    public Rect EyeRect(Eye eye)
    {
        return eye == Eye.LeftEye ? leftEyeRect : rightEyeRect;
    }
    [HideInInspector]
    public Rect leftEyeRect;
    [HideInInspector]
    public Rect rightEyeRect;
    [HideInInspector]
    public Matrix4x4 leftEyeView;
    [HideInInspector]
    public Matrix4x4 rightEyeView;

    // unity editor
    [HideInInspector]
    public Pvr_UnitySDKEditor pvr_UnitySDKEditor;
    [SerializeField]
    private bool vrModeEnabled = true;
    [HideInInspector]
    public bool VRModeEnabled
    {

        get
        {
            return vrModeEnabled;
        }
        set
        {
            if (value != vrModeEnabled)
                vrModeEnabled = value;

        }
    }
    [HideInInspector]
    public Material Eyematerial;
    [HideInInspector]
    public Material Middlematerial;
    [HideInInspector]
    public bool picovrTriggered { get; set; }
    [HideInInspector]
    public bool newPicovrTriggered = false;

    // FPS
    [SerializeField]
    private bool showFPS = false;
    public bool ShowFPS
    {
        get
        {
            return showFPS;
        }
        set
        {
            if (value != showFPS)
            {
                showFPS = value;
            }
        }
    }
    //6dof recenter
    [SerializeField]
    private bool sixDofRecenter;
    public bool SixDofRecenter
    {
        get
        {
            return sixDofRecenter;
        }
        set
        {
            if (value != sixDofRecenter)
            {
                sixDofRecenter = value;
            }
        }
    }

    //show safe panel
    [SerializeField]
    private bool showSafePanel;
    public bool ShowSafePanel
    {
        get
        {
            return showSafePanel;
        }
        set
        {
            if (value != showSafePanel)
            {
                showSafePanel = value;
            }
        }
    }
    //use default range 0.8m
    private bool defaultRange = true;
    public bool DefaultRange
    {
        get
        {
            return defaultRange;
        }
        set
        {
            if (value != defaultRange)
            {
                defaultRange = value;
            }
        }
    }
    //custom range
    [SerializeField]
    private float customRange = 0.8f;
    public float CustomRange
    {
        get
        {
            return customRange;
        }
        set
        {
            if (value != customRange)
            {
                customRange = value;
            }
        }
    }

    //Moving Ratios
    [SerializeField]
    private float movingRatios;
    public float MovingRatios
    {
        get
        {
            return movingRatios;
        }
        set
        {
            if (value != movingRatios)
            {
                movingRatios = value;
            }
        }
    }
    // screenFade
    [SerializeField] 
    private bool screenFade = false;
    public bool ScreenFade
    {
        get
        {
            return screenFade;
        }
        set
        {
            if (value != screenFade)
            {
                screenFade = value;
            }
        }
    }
    //Neck model
    [HideInInspector]
    public Vector3 neckOffset = new Vector3(0, 0.075f, 0.0805f);
    
    [SerializeField]
    private static bool pvrNeck = true;
    [HideInInspector]
    public static bool PVRNeck
    {
        get
        {
            return pvrNeck;
        }
        set
        {
            if (value != pvrNeck)
            {
                pvrNeck = value;
            }
        }
    }

    // life
    [HideInInspector]
    public bool onResume = false;

    private GameObject safeArea;
    private GameObject safeToast;
    private GameObject resetPanel;
    private GameObject safePanel1;
    private GameObject safePanel2;
    private bool isHasController = false;

    public Pvr_UnitySDKConfigProfile pvr_UnitySDKConfig;
    #endregion

    /************************************ Public Interfaces  *********************************/
    #region Public Interfaces

    #endregion

    /************************************ Private Interfaces  *********************************/
    #region Private Interfaces

    private void AddPrePostRenderStages()
    {
        var preRender = UnityEngine.Object.FindObjectOfType<Pvr_UnitySDKPreRender>();
        if (preRender == null)
        {
            var go = new GameObject("PreRender", typeof(Pvr_UnitySDKPreRender));
            go.SendMessage("Reset");
            go.transform.parent = transform;
        }

        var postRender = UnityEngine.Object.FindObjectOfType<Pvr_UnitySDKPostRender>();
        if (postRender == null)
        {
            var go = new GameObject("PostRender", typeof(Pvr_UnitySDKPostRender));
            go.SendMessage("Reset");
            go.transform.parent = transform;
        }
    }


    private bool SDKManagerInit()
    {
        if (SDKManagerInitConfigProfile())
        {
#if UNITY_EDITOR
            if (SDKManagerInitEditor())
                return true;
            else
                return false;
#else
                
                if (SDKManagerInitCoreAbility())
                   
                    return true;
                else
                    return false;
#endif
        }
        else
            return false;

    }

    private bool SDKManagerInitCoreAbility()
    {
        AddPrePostRenderStages();
        if (pvr_UnitySDKRender == null)
        {
            Debug.Log("pvr_UnitySDKRender  init");
           // pvr_UnitySDKRender = this.gameObject.AddComponent<Pvr_UnitySDKRender>();
            pvr_UnitySDKRender = new Pvr_UnitySDKRender();
          

        }else
            pvr_UnitySDKRender.Init();
        if (pvr_UnitySDKSensor == null)
        {
            Debug.Log("pvr_UnitySDKSensor init");
            HeadPose = new Pvr_UnitySDKPose(Vector3.forward, Quaternion.identity);
            // pvr_UnitySDKSensor = this.gameObject.AddComponent<Pvr_UnitySDKSensor>();
             pvr_UnitySDKSensor = new Pvr_UnitySDKSensor();
           // pvr_UnitySDKSensor.Init();
        }
        Pvr_UnitySDKAPI.System.UPvr_StartHomeKeyReceiver(this.gameObject.name);

        return true;
    }

    private bool SDKManagerInitFPS()
    {
        Transform[] father;
        father = GetComponentsInChildren<Transform>(true);
        GameObject FPS = null;
        foreach (Transform child in father)
        {
            if (child.gameObject.name == "FPS")
            {
                FPS = child.gameObject;
            }
        }
        if (FPS != null)
        {
            if (systemFPS)
            {
                FPS.SetActive(true);
                return true;
            }
            int fps = 0;
#if !UNITY_EDITOR
            int rate = (int)GlobalIntConfigs.iShowFPS;
            Render.UPvr_GetIntConfig(rate, ref fps);
#endif
            if (Convert.ToBoolean(fps))
            {
                FPS.SetActive(true);
                return true;
            }
            if (ShowFPS)
            {
                FPS.SetActive(true);
                return true;
            }
            return false;
        }
        return false;
    }

    private bool SDKManagerInitConfigProfile()
    {
        pvr_UnitySDKConfig = Pvr_UnitySDKConfigProfile.Default;
        return true;
    }

    private bool SDKManagerInitEditor()
    {
        if (pvr_UnitySDKEditor == null)
        {
            HeadPose = new Pvr_UnitySDKPose(Vector3.forward, Quaternion.identity);
            pvr_UnitySDKEditor = this.gameObject.AddComponent<Pvr_UnitySDKEditor>();
        }
        return true;
    }
    
    private bool SDKManagerInitPara()
    {
        return true;
    }

    public void SDKManagerLongHomeKey()
    {
        //closepanel
        resetPanel.SetActive(false);
        if (pvr_UnitySDKSensor != null)
        {
            if (isHasController)
            {
                if (Controller.UPvr_GetControllerState(0) == ControllerState.Connected ||
                    Controller.UPvr_GetControllerState(1) == ControllerState.Connected)
                {
                    pvr_UnitySDKSensor.OptionalResetUnitySDKSensor(0, 1);
                }
                else
                {
                    pvr_UnitySDKSensor.OptionalResetUnitySDKSensor(1,1);
                }
            }
            else
            {
                pvr_UnitySDKSensor.OptionalResetUnitySDKSensor(1,1);
            }
            
        }
    }

    private void setLongHomeKey()
    {
        if (SDK.HeadDofNum == HeadDofNum.ThreeDof)
        {
            if (pvr_UnitySDKSensor != null)
            {
                Debug.Log(pvr_UnitySDKSensor.ResetUnitySDKSensor()
                    ? "Long Home Key to Reset Sensor Success!"
                    : "Long Home Key to Reset Sensor Failed!");
            }
        }
        else
        {
            if (SDK.sixDofRecenter)
            {
                //showtoast
                resetPanel.SetActive(true);
            }
        }
    }
#endregion

    /*************************************  Unity API ****************************************/
#region Unity API

    void Awake()
    {
        PLOG.getConfigTraceLevel();
#if !UNITY_EDITOR && UNITY_ANDROID
        int enumindex = (int)GlobalIntConfigs.TRACKING_MODE;
        Render.UPvr_GetIntConfig(enumindex, ref trackingmode);
        if (isPUI)
        {
            if (trackingmode == 1 || trackingmode == 0)
            {
                HeadDofNum = HeadDofNum.ThreeDof;
                HandDofNum = HandDofNum.ThreeDof;
                HandNum = HandNum.One;
            } 
        }
#endif
        Application.targetFrameRate = 61;
#if !UNITY_EDITOR && UNITY_ANDROID
        int fps = -1;
        int rate = (int) GlobalIntConfigs.TARGET_FRAME_RATE;
        Render.UPvr_GetIntConfig(rate, ref fps);
        float ffps = 0.0f;
        int frame = (int) GlobalFloatConfigs.DISPLAY_REFRESH_RATE;
        Render.UPvr_GetFloatConfig(frame, ref ffps);
        Application.targetFrameRate = fps > 0 ? fps : (int)ffps;
#endif

#if !UNITY_EDITOR && UNITY_ANDROID
        float neckx = 0.0f;
        float necky = 0.0f;
        float neckz = 0.0f;
        int modelx = (int) GlobalFloatConfigs.NECK_MODEL_X;
        int modely = (int) GlobalFloatConfigs.NECK_MODEL_Y;
        int modelz = (int) GlobalFloatConfigs.NECK_MODEL_Z;
        Render.UPvr_GetFloatConfig(modelx, ref neckx);
        Render.UPvr_GetFloatConfig(modely, ref necky);
        Render.UPvr_GetFloatConfig(modelz, ref neckz);
        if (neckx != 0.0f || necky != 0.0f || neckz != 0.0f)
        {
            neckOffset = new Vector3(neckx, necky, neckz);
        }
#endif
        var controllermanager = FindObjectOfType<Pvr_ControllerManager>();
        isHasController = controllermanager != null;
        if (sdk == null)
        {
            sdk = this;
        }
        if (sdk != this)
        {
            Debug.LogError("SDK object should be a singleton.");
            enabled = false;
            return;
        }
        if (SDKManagerInit())
        {
            Debug.Log("SDK Init success.");
        }
        else
        {
            Debug.LogError("SDK Init Failed.");
            Application.Quit();
        }
        
        SDKManagerInitFPS();

        safeArea = transform.Find("SafeArea2").gameObject;
        safeToast = transform.Find("Head/SafeToast").gameObject;
        resetPanel = transform.Find("ResetPanel").gameObject;
        safePanel1 = transform.Find("SafePanel1").gameObject;
        safePanel2 = transform.Find("SafePanel2").gameObject;

        if (Application.systemLanguage != SystemLanguage.Chinese && Application.systemLanguage != SystemLanguage.ChineseSimplified)
        {
            safeToast.transform.Find("title").GetComponent<Text>().text = "Safety Warning";
            safeToast.transform.Find("Text").GetComponent<Text>().text =
                "You have left the safe zone. Please take off your headset and return to the safe zone,or hold down the Home key of headset to rebuild a safe zone.";
            resetPanel.transform.Find("Panel/resetbtn/Text").GetComponent<Text>().text = "Continue";
            safePanel1.transform.Find("Panel/startgameBtn/Text").GetComponent<Text>().text = "Continue";
            safePanel1.transform.Find("Panel/toast2").GetComponent<Text>().text = "I have read and agree to the ";
            safePanel1.transform.Find("Panel/toast3").GetComponent<Text>().text = "safety policy.";
            safePanel1.transform.Find("Panel/Toggle").localPosition = new Vector3(-37, -136, 0);
            safePanel2.transform.Find("Panel/Title").GetComponent<Text>().text = "Warning";
            safePanel2.transform.Find("Panel/toast2").GetComponent<Text>().text = "This device does not support this applications";
            safePanel2.transform.Find("Panel/forcequitBtn/Text").GetComponent<Text>().text = "Quit";
            if (DefaultRange)
            {
                resetPanel.transform.Find("Panel/toast").GetComponent<Text>().text =
                    "Please take off your headset and ensure that there are no obstacles within a radius of 0.8 meters,then press Continue";
                safePanel1.transform.Find("Panel/toast1").GetComponent<Text>().text =
                    "Please take off your headset and ensure that there are no obstacles within a radius of 0.8 meters,then press Continue";
            }
            else
            {
                resetPanel.transform.Find("Panel/toast").GetComponent<Text>().text =
                    "Please take off your headset and ensure that there are no obstacles within a radius of" + CustomRange + "meters,then press Continue";
                safePanel1.transform.Find("Panel/toast1").GetComponent<Text>().text =
                    "Please take off your headset and ensure that there are no obstacles within a radius of" + CustomRange + "meters,then press Continue";
            }
            
        }
#if !UNITY_EDITOR && UNITY_ANDROID
        if (HeadDofNum == HeadDofNum.SixDof || HandDofNum == HandDofNum.SixDof )
        {
            if (trackingmode == 1 || trackingmode == 0)
            {
                safePanel2.SetActive(true);
            }
        }
        if (ShowSafePanel && HeadDofNum == HeadDofNum.SixDof && Sensor.UPvr_Pvr_Get6DofSafePanelFlag())
        {
            safePanel1.SetActive(true);
            var toggle =  safePanel1.transform.Find("Panel/Toggle").GetComponent<Toggle>();
            toggle.isOn = false;
            toggle.onValueChanged.AddListener(ToggleValueChange);
            safePanel1.transform.Find("Panel/startgameBtn").GetComponent<Button>().interactable = false;
        }
#endif
    }

    void Update()
    {
        if (Input.touchCount == 1)//一个手指触摸屏幕
        {
            if (Input.touches[0].phase == TouchPhase.Began)//开始触屏
            {
                newPicovrTriggered = true;
            }
        }
        else
         if (Input.GetKeyDown(KeyCode.JoystickButton0) || Controller.UPvr_GetKeyDown(0, Pvr_KeyCode.TOUCHPAD) || Controller.UPvr_GetKeyDown(1, Pvr_KeyCode.TOUCHPAD))
        {
            newPicovrTriggered = true;
        }

        if (pvr_UnitySDKSensor != null)
        {
            pvr_UnitySDKSensor.SensorUpdate();
        }
        
        picovrTriggered = newPicovrTriggered;
        newPicovrTriggered = false;

#if !UNITY_EDITOR && UNITY_ANDROID

        //reset panel 跟随
        if (resetPanel.activeSelf)
        {
            resetPanel.transform.localPosition = SDK.HeadPose.Position;
            resetPanel.transform.localRotation = Quaternion.Euler(0, SDK.HeadPose.Orientation.eulerAngles.y, 0);
        }
        //safe panel2 跟随
        if (safePanel2.activeSelf)
        {
            safePanel2.transform.localPosition = SDK.HeadPose.Position;
            safePanel2.transform.localRotation = Quaternion.Euler(0, SDK.HeadPose.Orientation.eulerAngles.y, 0);
        }
        //安全围栏
        if (HeadDofNum == HeadDofNum.SixDof)
        {
            //default 0.8m
            if (DefaultRange)
            {
                if (isHasController)
                {
                    if (Math.Abs(HeadPose.Position.x) > 0.64f || Math.Abs(HeadPose.Position.z) > 0.64f || Math.Abs(Controller.UPvr_GetControllerPOS(0).x) > 0.8f || Math.Abs(Controller.UPvr_GetControllerPOS(0).z) > 0.8f || Math.Abs(Controller.UPvr_GetControllerPOS(1).x) > 0.8f || Math.Abs(Controller.UPvr_GetControllerPOS(1).z) > 0.8f)
                    {
                        safeArea.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
                        safeArea.SetActive(true);
                    }
                    else
                    {
                        safeArea.SetActive(false);
                    }
                }
                else
                {
                    if (Math.Abs(HeadPose.Position.x) > 0.64f || Math.Abs(HeadPose.Position.z) > 0.64f)
                    {
                        safeArea.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
                        safeArea.SetActive(true);
                    }
                    else
                    {
                        safeArea.SetActive(false);
                    }
                }
                
                if (Math.Abs(HeadPose.Position.x) > 0.8f || Math.Abs(HeadPose.Position.z) > 0.8f)
                {
                    safeToast.SetActive(true);

                }
                else
                {
                    safeToast.SetActive(false);
                }
            }
            else
            {
                if (isHasController)
                {
                    if (Math.Abs(HeadPose.Position.x) > (0.8f * CustomRange) || Math.Abs(HeadPose.Position.z) > (0.8f * CustomRange) || Math.Abs(Controller.UPvr_GetControllerPOS(0).x) > CustomRange || Math.Abs(Controller.UPvr_GetControllerPOS(0).z) > CustomRange || Math.Abs(Controller.UPvr_GetControllerPOS(1).x) > CustomRange || Math.Abs(Controller.UPvr_GetControllerPOS(1).z) > CustomRange)
                    {
                        safeArea.transform.localScale = new Vector3(CustomRange / 0.5f, CustomRange / 0.5f, CustomRange / 0.5f);
                        safeArea.SetActive(true);
                    }
                    else
                    {
                        safeArea.SetActive(false);
                    }
                }
                else
                {
                    if (Math.Abs(HeadPose.Position.x) > (0.8f * CustomRange) || Math.Abs(HeadPose.Position.z) > (0.8f * CustomRange))
                    {
                        safeArea.transform.localScale =
                            new Vector3(CustomRange / 0.5f, CustomRange / 0.5f, CustomRange / 0.5f);
                        safeArea.SetActive(true);
                    }
                    else
                    {
                        safeArea.SetActive(false);
                    }
                }
                if (Math.Abs(HeadPose.Position.x) > CustomRange || Math.Abs(HeadPose.Position.z) > CustomRange)
                {
                    safeToast.SetActive(true);
                }
                else
                {
                    safeToast.SetActive(false);
                }
            }
        }
#endif
    }

    void OnDestroy()
    {
        if (sdk == this)
        {
            sdk = null;
        }
        RenderTexture.active = null;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public void ToggleValueChange(bool ison)
    {
        safePanel1.transform.Find("Panel/startgameBtn").GetComponent<Button>().interactable = ison;
    }
    public void OnApplicationQuit()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        /*
               if (pvr_UnitySDKSensor != null)
                 {
                pvr_UnitySDKSensor.StopUnitySDKSensor();
                  }
                try{
                    Debug.Log("OnApplicationQuit  1  -------------------------");
                    Pvr_UnitySDKPluginEvent.Issue( RenderEventType.ShutdownRenderThread );
                }
                catch (Exception e)
                {
                    Debug.Log("ShutdownRenderThread Error" + e.Message);
                }
        */
#endif
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnPause()
    {
        Pvr_UnitySDKAPI.System.UPvr_StopHomeKeyReceiver();
        LeaveVRMode();
		if (pvr_UnitySDKSensor != null)
        {
            pvr_UnitySDKSensor.StopUnitySDKSensor();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("OnApplicationPause-------------------------" + (pause ? "true" : "false"));
#if UNITY_ANDROID && !UNITY_EDITOR
        if (pause)
        {
            OnPause();
        }
        else
        {
            onResume = true;    
            GL.InvalidateState();
            StartCoroutine(OnResume());
        }
#endif
    }

    void OnApplicationFocus(bool focus)
    {
        Debug.Log("OnApplicationFocus-------------------------" + (focus ? "true" : "false"));
    }

    public static void EnterVRMode()
    {
        Pvr_UnitySDKPluginEvent.Issue(RenderEventType.Resume);
    }

    public static void LeaveVRMode()
    {
        Pvr_UnitySDKPluginEvent.Issue(RenderEventType.Pause);
    }
    public void SixDofStartGame()
    {
        safePanel1.SetActive(false);
    }
    
    public void SixDofForceQuit()
    {
        Application.Quit();
    }

    public void OpenSafeURL()
    {
        if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            Application.OpenURL("http://pui.picovr.com/api/statement?country_code=CN");
        }
        else
        {
            var code = "http://pui.picovr.com/api/statement?country_code=" + Pvr_UnitySDKAPI.System.UPvr_GetCountryCode();
            Application.OpenURL(code);
        }
    }
#endregion

    /************************************    IEnumerator  *************************************/
    private IEnumerator OnResume()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return null;
        }
        EnterVRMode();
        Pvr_UnitySDKAPI.System.UPvr_StartHomeKeyReceiver(this.gameObject.name);
        if (pvr_UnitySDKSensor != null)
        {
            pvr_UnitySDKSensor.StartUnitySDKSensor();
            int iEnable6Dof = -1;
#if !UNITY_EDITOR && UNITY_ANDROID
            int iEnable6DofGlobalTracking = (int) GlobalIntConfigs.ENBLE_6DOF_GLOBAL_TRACKING;
            Render.UPvr_GetIntConfig(iEnable6DofGlobalTracking, ref iEnable6Dof);
#endif
            if (iEnable6Dof == 1)
            {
                pvr_UnitySDKSensor.OptionalResetUnitySDKSensor(0,1);
            }
            else
            {
                pvr_UnitySDKSensor.ResetUnitySDKSensor();
            }
            
        }
    }

}
