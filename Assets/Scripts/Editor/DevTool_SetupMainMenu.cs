#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevTool_SetupMainMenu
{
    [MenuItem("Zombie Shooter/Setup/Create Main Menu Canvas")]
    static void CreateMainMenu()
    {
        GameObject canvasObj = new GameObject("MainMenu_Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();
        MainMenuController menu = canvasObj.AddComponent<MainMenuController>();

        // --- Main Panel ---
        GameObject mainPanel = CreatePanel(canvasObj.transform, "MainPanel");

        CreateText(mainPanel.transform, "TitleText", "ZOMBIE SHOOTER", 64,
            new Vector2(0, 150), new Vector2(600, 80), TextAlignmentOptions.Center);

        Button playBtn = CreateButton(mainPanel.transform, "PlayButton", "PLAY",
            new Vector2(0, 20), new Vector2(300, 60), new Color(0.2f, 0.7f, 0.2f));

        Button optionsBtn = CreateButton(mainPanel.transform, "OptionsButton", "OPTIONS",
            new Vector2(0, -60), new Vector2(300, 60), new Color(0.3f, 0.3f, 0.3f));

        Button creditsBtn = CreateButton(mainPanel.transform, "CreditsButton", "CREDITS",
            new Vector2(0, -140), new Vector2(300, 60), new Color(0.3f, 0.3f, 0.3f));

        Button quitBtn = CreateButton(mainPanel.transform, "QuitButton", "QUIT",
            new Vector2(0, -220), new Vector2(300, 60), new Color(0.7f, 0.2f, 0.2f));

        // --- Options Panel ---
        GameObject optionsPanel = CreatePanel(canvasObj.transform, "OptionsPanel");
        optionsPanel.SetActive(false);

        CreateText(optionsPanel.transform, "OptionsTitle", "OPTIONS", 48,
            new Vector2(0, 180), new Vector2(400, 60), TextAlignmentOptions.Center);

        Slider masterSlider = CreateSlider(optionsPanel.transform, "MasterVolume", "Master Volume",
            new Vector2(0, 80));
        Slider musicSlider = CreateSlider(optionsPanel.transform, "MusicVolume", "Music",
            new Vector2(0, 10));
        Slider sfxSlider = CreateSlider(optionsPanel.transform, "SFXVolume", "SFX",
            new Vector2(0, -60));

        Button optBackBtn = CreateButton(optionsPanel.transform, "BackButton", "BACK",
            new Vector2(0, -160), new Vector2(200, 50), new Color(0.4f, 0.4f, 0.4f));

        // --- Credits Panel ---
        GameObject creditsPanel = CreatePanel(canvasObj.transform, "CreditsPanel");
        creditsPanel.SetActive(false);

        CreateText(creditsPanel.transform, "CreditsTitle", "CREDITS", 48,
            new Vector2(0, 180), new Vector2(400, 60), TextAlignmentOptions.Center);

        CreateText(creditsPanel.transform, "CreditsBody",
            "Made by Team Name\n\nInstructor: Rafael\nCo-Instructors: Miguel & Cristobal\n\nMade with Unity\nAssets: POLYGON Apocalypse Pack",
            20, new Vector2(0, 20), new Vector2(500, 300), TextAlignmentOptions.Center);

        Button credBackBtn = CreateButton(creditsPanel.transform, "BackButton", "BACK",
            new Vector2(0, -160), new Vector2(200, 50), new Color(0.4f, 0.4f, 0.4f));

        // Wire up MainMenuController
        SerializedObject so = new SerializedObject(menu);
        so.FindProperty("playButton").objectReferenceValue = playBtn;
        so.FindProperty("optionsButton").objectReferenceValue = optionsBtn;
        so.FindProperty("creditsButton").objectReferenceValue = creditsBtn;
        so.FindProperty("quitButton").objectReferenceValue = quitBtn;
        so.FindProperty("mainPanel").objectReferenceValue = mainPanel;
        so.FindProperty("optionsPanel").objectReferenceValue = optionsPanel;
        so.FindProperty("creditsPanel").objectReferenceValue = creditsPanel;
        so.FindProperty("masterVolumeSlider").objectReferenceValue = masterSlider;
        so.FindProperty("musicVolumeSlider").objectReferenceValue = musicSlider;
        so.FindProperty("sfxVolumeSlider").objectReferenceValue = sfxSlider;
        so.FindProperty("optionsBackButton").objectReferenceValue = optBackBtn;
        so.FindProperty("creditsBackButton").objectReferenceValue = credBackBtn;
        so.ApplyModifiedProperties();

        Undo.RegisterCreatedObjectUndo(canvasObj, "Create Main Menu Canvas");
        Selection.activeGameObject = canvasObj;
        Debug.Log("Main Menu created. Set your game scene name/index in the MainMenuController inspector.");
    }

    static GameObject CreatePanel(Transform parent, string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        return obj;
    }

    static Button CreateButton(Transform parent, string name, string label, Vector2 position, Vector2 size, Color color)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        Image img = obj.AddComponent<Image>();
        img.color = color;

        Button btn = obj.AddComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.highlightedColor = new Color(color.r + 0.15f, color.g + 0.15f, color.b + 0.15f);
        cb.pressedColor = new Color(color.r - 0.1f, color.g - 0.1f, color.b - 0.1f);
        btn.colors = cb;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(obj.transform, false);
        RectTransform textRT = textObj.AddComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.sizeDelta = Vector2.zero;

        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = label;
        text.fontSize = 24;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        return btn;
    }

    static void CreateText(Transform parent, string name, string content, int fontSize, Vector2 position, Vector2 size, TextAlignmentOptions alignment)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
    }

    static Slider CreateSlider(Transform parent, string name, string label, Vector2 position)
    {
        CreateText(parent, $"{name}_Label", label, 18,
            position + new Vector2(-180, 0), new Vector2(150, 30), TextAlignmentOptions.Right);

        GameObject sliderObj = new GameObject(name);
        sliderObj.transform.SetParent(parent, false);
        RectTransform rt = sliderObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position + new Vector2(50, 0);
        rt.sizeDelta = new Vector2(300, 20);

        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(sliderObj.transform, false);
        RectTransform bgRT = bg.AddComponent<RectTransform>();
        bgRT.anchorMin = Vector2.zero; bgRT.anchorMax = Vector2.one; bgRT.sizeDelta = Vector2.zero;
        Image bgImg = bg.AddComponent<Image>();
        bgImg.color = new Color(0.2f, 0.2f, 0.2f);

        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderObj.transform, false);
        RectTransform faRT = fillArea.AddComponent<RectTransform>();
        faRT.anchorMin = Vector2.zero; faRT.anchorMax = Vector2.one;
        faRT.offsetMin = new Vector2(5, 0); faRT.offsetMax = new Vector2(-5, 0);

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        RectTransform fRT = fill.AddComponent<RectTransform>();
        fRT.anchorMin = Vector2.zero; fRT.anchorMax = Vector2.one; fRT.sizeDelta = Vector2.zero;
        Image fillImg = fill.AddComponent<Image>();
        fillImg.color = new Color(0.3f, 0.8f, 0.3f);

        Slider slider = sliderObj.AddComponent<Slider>();
        slider.fillRect = fRT;
        slider.targetGraphic = bgImg;
        slider.value = 0.8f;
        slider.minValue = 0f;
        slider.maxValue = 1f;

        return slider;
    }
}
#endif
