<p align="right"><a href="https://github.com/PicoSupport/PicoSupport" target="_blank">Pico Support Home</a></p>

## Unity_Demo_HandleSwitch

## Unity Versions：
- 2017.1.0f3 and later

## Description：

-  PicoNeo Switch between main and secondary handles
-  Judge handle button response event

场景主要脚本是ChangeHand，在应用第一次运行时调用Pvr_ControllerManager.SetControllerServiceBindStateEvent委托
再用Controller.UPvr_GetMainHandNess()方法判断当前手柄主副。

## Usage：
- Scene： Assets -> Scene -> PicoNeoHandleSwitch

- 点击按钮 "切换手柄" 切换主副手柄
- 本demo适用于PicoNeo
- 切换功能需要同时连接两个手柄

