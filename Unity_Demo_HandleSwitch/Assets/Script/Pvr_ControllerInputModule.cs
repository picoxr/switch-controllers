using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Pvr_UnitySDKAPI
{
    // To use:
    // 1. Drag onto your EventSystem game object.
    // 2. Disable any other Input Modules (eg: StandaloneInputModule & TouchInputModule) as they will fight over selections.
    // 3. Make sure your Canvas is in world space and has a GraphicRaycaster (should by default).
    public class Pvr_ControllerInputModule: PointerInputModule
    {
        Vector3 pos;
        public Camera camera;
        private GameObject currentLookAtHandler;
        private PointerEventData pointerEventData;
        public RaycastResult CurrentRaycast;
        // ----------------------  aili 20170706
        private PointerEventData controllerpointerEventData;
        public RaycastResult controllerCurrentRaycast;
        public GameObject controllerPointerOne;
        public GameObject controllerPointerSec;
        // ----------------------  aili 20170706
        public override void Process()
        {

            HandleLook();
            HandleSelection();
        }
        public override bool ShouldActivateModule()
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
        }

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
            if (!Pvr_UnitySDKManager.SDK.picovrTriggered
                && Time.unscaledTime - pointerEnter.clickTime < 0.1)
            {
                return;
            }
            // Send pointer up and click events.
            ExecuteEvents.Execute(pointerEnter.pointerPress, pointerEnter, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(pointerEnter.pointerPress, pointerEnter, ExecuteEvents.pointerClickHandler);

            // Clear the click state.
            pointerEnter.pointerPress = null;
            pointerEnter.rawPointerPress = null;
            pointerEnter.eligibleForClick = false;
            pointerEnter.clickCount = 0;
        }

        private void UpdateCurrentObject(PointerEventData pointerData)
        {
            // Send enter events and update the highlight.
            var go = pointerData.pointerCurrentRaycast.gameObject;

            HandlePointerExitAndEnter(pointerData, go);
            // Update the current selection, or clear if it is no longer the current object.
            var selected = ExecuteEvents.GetEventHandler<ISelectHandler>(go);
            if (selected == eventSystem.currentSelectedGameObject)
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, GetBaseEventData(), ExecuteEvents.updateSelectedHandler);
            }
            else
            {
                eventSystem.SetSelectedGameObject(null, pointerData);
            }
        }
        void HandleLook()
        {
            // ----------------------  aili 20170706
            if (controllerpointerEventData == null)
            {
                controllerpointerEventData = new PointerEventData(eventSystem);
            }
            controllerpointerEventData.Reset();
            if (Controller.UPvr_GetMainHandNess() == 0 ||controllerPointerOne.activeSelf)
            {
                pos = camera.WorldToScreenPoint(controllerPointerOne.transform.position);
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
            else if(Controller.UPvr_GetMainHandNess()==1&&controllerPointerSec.activeSelf)
            {
              pos = camera.WorldToScreenPoint(controllerPointerSec.transform.position);
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
            //List<RaycastResult> controllerraycastResults = new List<RaycastResult>();
            //eventSystem.RaycastAll(controllerpointerEventData, controllerraycastResults);
            //controllerCurrentRaycast = controllerpointerEventData.pointerCurrentRaycast = FindFirstRaycast(controllerraycastResults);
            //ProcessMove(controllerpointerEventData);
            //Vector3 pos = camera.WorldToScreenPoint(controllerPointer.transform.position);
            //if (pos.x > 0 && pos.x < Screen.width && pos.y > 0 && pos.y < Screen.height)
            //{
            //    controllerpointerEventData.position = new Vector2(pos.x, pos.y);
            //    controllerpointerEventData.delta = Vector2.zero;

            //    List<RaycastResult> controllerraycastResults = new List<RaycastResult>();
            //    eventSystem.RaycastAll(controllerpointerEventData, controllerraycastResults);
            //    controllerCurrentRaycast = controllerpointerEventData.pointerCurrentRaycast = FindFirstRaycast(controllerraycastResults);
            //    ProcessMove(controllerpointerEventData);

            //}
            // ----------------------  aili 20170706
        }

        public void HandleSelection()
        {
            currentLookAtHandler = null;

            if (controllerpointerEventData.pointerEnter != null)
            {

                GameObject handler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(controllerpointerEventData.pointerEnter);
                //  GameObject handler1=ExecuteEvents.GetEventHandler<IPointerClickHandler>(controllerpointerEventData.)
                if (currentLookAtHandler != handler && handler != null)
                {
                    UpdateCurrentObject(controllerpointerEventData);
                    HandlePendingClick(controllerpointerEventData);
                    currentLookAtHandler = handler;

                    if (!Pvr_UnitySDKManager.SDK.picovrTriggered)
                    {
                        return;
                    }
                    controllerpointerEventData.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentLookAtHandler);

                    controllerpointerEventData.pointerDrag = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentLookAtHandler);

                    controllerpointerEventData.pressPosition = controllerpointerEventData.position;
                    controllerpointerEventData.scrollDelta = controllerpointerEventData.position;

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
}