using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class event_controller : PointerInputModule
{
    private PointerEventData controllerpointerEventData;
    public RaycastResult controllerCurrentRaycast;
    public GameObject controllerPointer;
    public Camera camera;
    private GameObject currentLookAtHandler;

    public override void Process()
    {
        HandlePendingClick(controllerpointerEventData);
        HandleLook();
        HandleSelection();
        
    }
   /* public override bool ShouldActivateModule()
    {
        if (!base.ShouldActivateModule())
        {
            return false;
        }
        return Pvr_UnitySDKManager.SDK.VRModeEnabled;
    }

    public override void DeactivateModule()
    {
        base.DeactivateModule();

        if (controllerpointerEventData != null)
        {
            HandlePendingClick(controllerpointerEventData);
            HandlePointerExitAndEnter(controllerpointerEventData, null);
            controllerpointerEventData = null;
        }
        eventSystem.SetSelectedGameObject(null, GetBaseEventData());
    }*/


    private void HandlePendingClick(PointerEventData pointerEnter)
    {
        if (pointerEnter == null)
        {
            return;
        }
        if (!pointerEnter.eligibleForClick)
        {
            
            return;
        }
       
        /* if (!Pvr_UnitySDKManager.SDK.picovrTriggered
            && Time.unscaledTime - pointerEnter.clickTime < 0.1)
        {
            return;
        }*/
        
        // Send pointer up and click events.
        ExecuteEvents.Execute(pointerEnter.pointerPress, pointerEnter, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(pointerEnter.pointerPress, pointerEnter, ExecuteEvents.pointerClickHandler);
        //ExecuteEvents.Execute(pointerEnter.pointerPress, pointerEnter, ExecuteEvents.scrollHandler);
        //ExecuteEvents.Execute(pointerEnter.pointerPress, pointerEnter, ExecuteEvents.pointerClickHandler);
        // Clear the click state.
        pointerEnter.pointerPress = null;
        pointerEnter.rawPointerPress = null;
        pointerEnter.eligibleForClick = false;
        pointerEnter.clickCount = 0;

    }
    void HandleLook()
    {
        if (controllerpointerEventData == null)
        {
            controllerpointerEventData = new PointerEventData(eventSystem);
        }       
        // fake a pointer always being at the center of the screen
        controllerpointerEventData.Reset();
        Vector3 pos = camera.WorldToScreenPoint(controllerPointer.transform.position);
        if (pos.x > 0 && pos.x < Screen.width && pos.y > 0 && pos.y < Screen.height)
        {
            controllerpointerEventData.position = new Vector2(pos.x, pos.y);
            controllerpointerEventData.delta = Vector2.zero;
            List<RaycastResult> controllerraycastResults = new List<RaycastResult>();
            eventSystem.RaycastAll(controllerpointerEventData, controllerraycastResults);
            controllerCurrentRaycast = controllerpointerEventData.pointerCurrentRaycast = FindFirstRaycast(controllerraycastResults);
            ProcessMove(controllerpointerEventData);    
        }
    }
    void HandleSelection()
    {
        currentLookAtHandler = null;

        if (controllerpointerEventData.pointerEnter != null)
        {

            GameObject handler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(controllerpointerEventData.pointerEnter);

            if (currentLookAtHandler != handler && handler != null)
            {
               // UpdateCurrentObject(controllerpointerEventData);
                HandlePendingClick(controllerpointerEventData);
                currentLookAtHandler = handler;

                if (!Pvr_UnitySDKManager.SDK.picovrTriggered)
                {
                    return;
                }
                controllerpointerEventData.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentLookAtHandler);

                controllerpointerEventData.pressPosition = controllerpointerEventData.position;
                controllerpointerEventData.pointerPressRaycast = controllerpointerEventData.pointerCurrentRaycast;
                controllerpointerEventData.pointerPress =
                    ExecuteEvents.ExecuteHierarchy(currentLookAtHandler, controllerpointerEventData, ExecuteEvents.pointerDownHandler)
                    ?? ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentLookAtHandler);

                controllerpointerEventData.rawPointerPress = currentLookAtHandler;
                controllerpointerEventData.eligibleForClick = true;
                controllerpointerEventData.clickCount = 1;
                controllerpointerEventData.clickTime = Time.unscaledTime;
            }
        }

    }
}
