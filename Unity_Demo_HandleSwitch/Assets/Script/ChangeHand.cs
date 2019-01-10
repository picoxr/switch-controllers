using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pvr_UnitySDKAPI;

    public class ChangeHand : MonoBehaviour
    {
        public GameObject changHand;
        public GameObject maindot;
        public GameObject maindirection;
        public GameObject mainray_alpha;
        public GameObject secdot;
        public GameObject secdirection;
        public GameObject secray_alpha;
    // Use this for initialization
    private void Start()
    {
        Debug.Log(Pvr_UnitySDKAPI.System.UPvr_GetUnitySDKVersion());
        Pvr_ControllerManager.SetControllerServiceBindStateEvent += new Pvr_ControllerManager.SetControllerServiceBindState(ShowButton);//当serviceBind成功，初始化手柄
    }
    public void ShowButton()
    {
        if(Controller.UPvr_GetMainHandNess()==0||Controller.UPvr_GetMainHandNess()==1)
        {
            changHand.SetActive(true);
            if (Controller.UPvr_GetMainHandNess() == 0)
            {
                maindot.SetActive(true);
                maindirection.SetActive(true);
                mainray_alpha.SetActive(true);
            }
            else if (Controller.UPvr_GetMainHandNess() == 1)
            {
                secdirection.SetActive(true);
                secdot.SetActive(true);
                secray_alpha.SetActive(true);
            }
        }
    }
    public void ChangeHands()
    {
        if (Controller.UPvr_GetMainHandNess() == 0)
        {
            maindot.SetActive(false);
            maindirection.SetActive(false);
            mainray_alpha.SetActive(false);
            secdirection.SetActive(true);
            secdot.SetActive(true);
            secray_alpha.SetActive(true);
            Controller.UPvr_SetMainHandNess(1);

        }
        else 
        {
            secdirection.SetActive(false);
            secdot.SetActive(false);
            secray_alpha.SetActive(false);
            maindot.SetActive(true);
            maindirection.SetActive(true);
            mainray_alpha.SetActive(true);
            Controller.UPvr_SetMainHandNess(0);
        }
    }
}

