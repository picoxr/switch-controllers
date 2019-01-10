using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecenterButton : MonoBehaviour {

    public void DemoResetTracking()
    {
        if (Pvr_UnitySDKManager.SDK != null)
        {
            if (Pvr_UnitySDKManager.pvr_UnitySDKSensor != null)
            {
                Pvr_UnitySDKManager.pvr_UnitySDKSensor.ResetUnitySDKSensor();
            }
            else
            {
                Pvr_UnitySDKManager.SDK.pvr_UnitySDKEditor.ResetUnitySDKSensor();
            }

        }

    }
}
