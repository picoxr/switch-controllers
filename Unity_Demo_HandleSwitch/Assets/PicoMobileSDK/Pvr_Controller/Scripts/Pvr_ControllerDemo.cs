using System;
using UnityEngine;
using System.Collections;

public class Pvr_ControllerDemo : MonoBehaviour
{

    public GameObject controller0dot;
    public GameObject controller0direction;
    public GameObject controller0ray;
    public GameObject controller1dot;
    public GameObject controller1direction;
    public GameObject controller1ray;


	// Use this for initialization
	void Start () {
        Pvr_ControllerManager.SetControllerServiceBindStateEvent += BindSuccessToGetMainHand;
        Pvr_ControllerManager.ChangeMainControllerCallBackEvent += MainHandChanged;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnDestroy()
    {
        Pvr_ControllerManager.SetControllerServiceBindStateEvent -= BindSuccessToGetMainHand;
    }
    private void ShowController0()
    {
        controller0dot.SetActive(true);
        controller0direction.SetActive(true);
        controller0ray.SetActive(true);
        controller1dot.SetActive(false);
        controller1direction.SetActive(false);
        controller1ray.SetActive(false);
    }

    private void ShowController1()
    {
        controller0dot.SetActive(false);
        controller0direction.SetActive(false);
        controller0ray.SetActive(false);
        controller1dot.SetActive(true);
        controller1direction.SetActive(true);
        controller1ray.SetActive(true);
    }

    private void BindSuccessToGetMainHand()
    {
        if (Pvr_UnitySDKAPI.Controller.UPvr_GetMainHandNess() != -1)
        {
            if (Pvr_UnitySDKAPI.Controller.UPvr_GetMainHandNess() == 0)
            {
                ShowController0();
            }
            else
            {
                ShowController1();
            }
        }
    }

    private void MainHandChanged(string index)
    {
        if (Convert.ToInt16(index) == 0)
        {
            ShowController0();
        }
        else
        {
            ShowController1();
        }
    }
}
