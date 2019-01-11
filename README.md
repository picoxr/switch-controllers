<p align="right"><a href="https://github.com/PicoSupport/PicoSupport" target="_blank">Pico Support Home</a></p>

## Unity_Demo_HandleSwitch

## Unity Versions：
- 2017.1.0f3 and later

## Description：

-  PicoNeo Switch between main and secondary handles
-  Judge handle button response event

Main Code **ChangeHand**，Called on the first run of the application **Pvr_ControllerManager.SetControllerServiceBindStateEvent** delegate
later **Controller.UPvr_GetMainHandNess()** Judge the main and secondary of the current handle.

## Usage：
- Scene： Assets -> Scene -> PicoNeoHandleSwitch

- click the button "switch handle" to switch the main and secondary handles
- this demo is suitable for PicoNeo
- switch function requires two handles to be connected at the same time

