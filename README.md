###  [ `Return | 首页` ](https://github.com/PicoSupport/PicoSupport)
* [AndroidDemo | 安卓](https://github.com/PicoSupport/PicoSupport/blob/master/android.md)
* [UnityDemo | Unity3d](https://github.com/PicoSupport/PicoSupport/blob/master/unity.md)

## Unity_Demo_HandleSwitch

## Unity_Versions：
- 2017.1.0f3 Or UP

## Explain 说明：

- 包含 PicoNeo主副手柄的切换
- 包含 判断手柄按键响应事件

场景主要脚本是ChangeHand，在应用第一次运行时调用Pvr_ControllerManager.SetControllerServiceBindStateEvent委托
再用Controller.UPvr_GetMainHandNess()方法判断当前手柄主副。

## Use 使用：
- 场景位置： Assets -> Scene -> PicoNeoHandleSwitch
- 点击按钮 "切换手柄" 切换主副手柄

## Announcements 注意事项：
- 本demo适用于PicoNeo
- 切换按钮需要同时连接两个手柄

## Pico技术支持
欢迎更多地了解我们，如果您有任何问题，请联系我们。
Learn about us, and contact us if you have any questions. 

- Email:  support@picovr.com
- Web:  [https://www.picovr.com/](https://www.picovr.com/)
