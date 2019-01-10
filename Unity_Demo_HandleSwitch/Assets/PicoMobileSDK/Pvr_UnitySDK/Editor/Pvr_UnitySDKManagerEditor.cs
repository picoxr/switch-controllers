using UnityEngine;
using UnityEditor;
using Pvr_UnitySDKAPI;

[CustomEditor(typeof(Pvr_UnitySDKManager))]
public class Pvr_UnitySDKManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        GUI.changed = false;

        //GUI style 设置
        GUIStyle firstLevelStyle = new GUIStyle(GUI.skin.label);
        firstLevelStyle.alignment = TextAnchor.UpperLeft;
        firstLevelStyle.fontStyle = FontStyle.Bold;
        firstLevelStyle.fontSize = 12;
        firstLevelStyle.wordWrap = true;

        //inspector 所在 target 
        Pvr_UnitySDKManager manager = (Pvr_UnitySDKManager)target;

        //一级分层标题 1
        GUILayout.Space(10);
        EditorGUILayout.LabelField("ConfigFile Setting", firstLevelStyle);
        EditorGUILayout.LabelField("Current Build Target ： " + EditorUserBuildSettings.activeBuildTarget.ToString() + "\n", firstLevelStyle);
        GUILayout.Space(10);

        //一级分层标题 2
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Render Texture Setting", firstLevelStyle);
        GUILayout.Space(10);

        manager.RtAntiAlising = (RenderTextureAntiAliasing)EditorGUILayout.EnumPopup("Render Texture Anti-Aliasing ", manager.RtAntiAlising);
        manager.RtBitDepth = (RenderTextureDepth)EditorGUILayout.EnumPopup("Render Texture Bit Depth   ", manager.RtBitDepth);
        manager.RtFormat = (RenderTextureFormat)EditorGUILayout.EnumPopup("Render Texture Format", manager.RtFormat);

        manager.RtSizeWH = EditorGUILayout.FloatField("Render Texture Size",manager.RtSizeWH);


        //一级分层标题 1
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Show FPS", firstLevelStyle);
        GUILayout.Space(10);
        manager.ShowFPS = EditorGUILayout.Toggle("Show FPS in Scene", manager.ShowFPS);
        GUILayout.Space(10);
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Show SafePanel", firstLevelStyle);
        GUILayout.Space(10);
        manager.ShowSafePanel = EditorGUILayout.Toggle("Show SafePanel", manager.ShowSafePanel);
        GUILayout.Space(10);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Screen Fade", firstLevelStyle);
        GUILayout.Space(10);
        manager.ScreenFade = EditorGUILayout.Toggle("Screen Fade", manager.ScreenFade);
        GUILayout.Space(10);


        GUILayout.Space(10);
        EditorGUILayout.LabelField("Head Pose", firstLevelStyle);
        manager.HeadDofNum = (HeadDofNum) EditorGUILayout.EnumPopup(manager.HeadDofNum);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Hand Pose", firstLevelStyle);
        manager.HandDofNum = (HandDofNum)EditorGUILayout.EnumPopup(manager.HandDofNum);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Controller Num", firstLevelStyle);
        manager.HandNum = (HandNum)EditorGUILayout.EnumPopup(manager.HandNum);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("6Dof Position Reset", firstLevelStyle);
        GUILayout.Space(10);
        manager.SixDofRecenter = EditorGUILayout.Toggle("Enable 6Dof Position Reset", manager.SixDofRecenter);
        GUILayout.Space(10);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Use Default Range", firstLevelStyle);
        GUILayout.Space(10);
        manager.DefaultRange = EditorGUILayout.Toggle("Use Default Range", manager.DefaultRange);
        GUILayout.Space(10);

        if (!manager.DefaultRange)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Custom Range", firstLevelStyle);
            GUILayout.Space(10);
            manager.CustomRange = EditorGUILayout.FloatField("Range", manager.CustomRange);
            GUILayout.Space(10);
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Moving Ratios", firstLevelStyle);
        GUILayout.Space(10);
        manager.MovingRatios = EditorGUILayout.FloatField("Ratios", manager.MovingRatios);
        GUILayout.Space(10);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
        }

        // 保存序列化数据，否则会出现设置数据丢失情况
        serializedObject.ApplyModifiedProperties();
    }

}
