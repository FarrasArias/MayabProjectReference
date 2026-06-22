#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevTool_SetupHUD
{
    [MenuItem("Zombie Shooter/Setup/Create HUD Canvas")]
    static void CreateHUD()
    {
        GameObject canvasObj = new GameObject("HUD_Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();
        HUDManager hud = canvasObj.AddComponent<HUDManager>();

        // --- Health Bar (bottom-left) ---
        GameObject healthGroup = CreatePanel(canvasObj.transform, "HealthGroup", TextAnchor.LowerLeft, new Vector2(300, 40), new Vector2(20, 20));
        GameObject healthBG = CreateImage(healthGroup.transform, "HealthBar_BG", new Color(0.15f, 0.15f, 0.15f, 0.8f), Vector2.zero, new Vector2(300, 30));
        GameObject healthFill = CreateImage(healthBG.transform, "HealthBar_Fill", Color.green, Vector2.zero, new Vector2(300, 30));
        Image fillImage = healthFill.GetComponent<Image>();
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        RectTransform fillRT = healthFill.GetComponent<RectTransform>();
        fillRT.anchorMin = Vector2.zero;
        fillRT.anchorMax = Vector2.one;
        fillRT.sizeDelta = Vector2.zero;
        TMP_Text healthText = CreateText(healthGroup.transform, "HealthText", "100/100", 16, TextAlignmentOptions.Center);
        RectTransform htRT = healthText.GetComponent<RectTransform>();
        htRT.anchorMin = Vector2.zero; htRT.anchorMax = Vector2.one; htRT.sizeDelta = Vector2.zero;

        // --- Ammo (bottom-right) ---
        GameObject ammoGroup = CreatePanel(canvasObj.transform, "AmmoGroup", TextAnchor.LowerRight, new Vector2(200, 60), new Vector2(-20, 20));
        TMP_Text ammoText = CreateText(ammoGroup.transform, "AmmoText", "30 / 30", 28, TextAlignmentOptions.Right);
        TMP_Text weaponName = CreateText(ammoGroup.transform, "WeaponName", "Pistol", 16, TextAlignmentOptions.Right);
        RectTransform wnRT = weaponName.GetComponent<RectTransform>();
        wnRT.anchoredPosition = new Vector2(0, -30);

        // --- Wave Info (top-center) ---
        GameObject waveGroup = CreatePanel(canvasObj.transform, "WaveGroup", TextAnchor.UpperCenter, new Vector2(300, 60), new Vector2(0, -10));
        TMP_Text waveText = CreateText(waveGroup.transform, "WaveText", "Wave 1/5", 24, TextAlignmentOptions.Center);
        TMP_Text enemyText = CreateText(waveGroup.transform, "EnemyCountText", "Enemies: 0", 16, TextAlignmentOptions.Center);
        RectTransform etRT = enemyText.GetComponent<RectTransform>();
        etRT.anchoredPosition = new Vector2(0, -30);

        // --- Score (top-right) ---
        TMP_Text scoreText = CreateText(canvasObj.transform, "ScoreText", "Score: 0", 22, TextAlignmentOptions.Right);
        RectTransform sRT = scoreText.GetComponent<RectTransform>();
        sRT.anchorMin = new Vector2(1, 1); sRT.anchorMax = new Vector2(1, 1);
        sRT.pivot = new Vector2(1, 1);
        sRT.anchoredPosition = new Vector2(-20, -15);
        sRT.sizeDelta = new Vector2(200, 40);

        // --- Grenade Counter (bottom-left, above health) ---
        TMP_Text grenadeText = CreateText(canvasObj.transform, "GrenadeText", "G: 3/3", 18, TextAlignmentOptions.Left);
        RectTransform gRT = grenadeText.GetComponent<RectTransform>();
        gRT.anchorMin = Vector2.zero; gRT.anchorMax = Vector2.zero;
        gRT.pivot = Vector2.zero;
        gRT.anchoredPosition = new Vector2(20, 65);
        gRT.sizeDelta = new Vector2(150, 30);

        // --- Crosshair (center) ---
        GameObject crosshair = CreateImage(canvasObj.transform, "Crosshair", Color.white, Vector2.zero, new Vector2(32, 32));
        RectTransform chRT = crosshair.GetComponent<RectTransform>();
        chRT.anchorMin = new Vector2(0.5f, 0.5f); chRT.anchorMax = new Vector2(0.5f, 0.5f);

        // --- Damage Vignette (fullscreen) ---
        GameObject vignetteObj = CreateImage(canvasObj.transform, "DamageVignette", new Color(1, 0, 0, 0), Vector2.zero, Vector2.zero);
        RectTransform vRT = vignetteObj.GetComponent<RectTransform>();
        vRT.anchorMin = Vector2.zero; vRT.anchorMax = Vector2.one; vRT.sizeDelta = Vector2.zero;
        DamageVignette vignette = vignetteObj.AddComponent<DamageVignette>();

        // Wire up serialized fields via SerializedObject
        SerializedObject hudSO = new SerializedObject(hud);
        hudSO.FindProperty("healthBarFill").objectReferenceValue = fillImage;
        hudSO.FindProperty("healthText").objectReferenceValue = healthText;
        hudSO.FindProperty("ammoText").objectReferenceValue = ammoText;
        hudSO.FindProperty("weaponNameText").objectReferenceValue = weaponName;
        hudSO.FindProperty("waveText").objectReferenceValue = waveText;
        hudSO.FindProperty("enemyCountText").objectReferenceValue = enemyText;
        hudSO.FindProperty("scoreText").objectReferenceValue = scoreText;
        hudSO.FindProperty("grenadeText").objectReferenceValue = grenadeText;
        hudSO.FindProperty("crosshair").objectReferenceValue = chRT;
        hudSO.FindProperty("parentCanvas").objectReferenceValue = canvas;
        hudSO.ApplyModifiedProperties();

        SerializedObject vigSO = new SerializedObject(vignette);
        vigSO.FindProperty("vignetteImage").objectReferenceValue = vignetteObj.GetComponent<Image>();
        vigSO.ApplyModifiedProperties();

        Undo.RegisterCreatedObjectUndo(canvasObj, "Create HUD Canvas");
        Selection.activeGameObject = canvasObj;
        Debug.Log("HUD Canvas created. Assign a crosshair sprite to the Crosshair image and wire events from HealthComponent/WeaponController/WaveSpawner.");
    }

    static GameObject CreatePanel(Transform parent, string name, TextAnchor anchor, Vector2 size, Vector2 offset)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.AddComponent<RectTransform>();

        switch (anchor)
        {
            case TextAnchor.LowerLeft:
                rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.zero; rt.pivot = Vector2.zero; break;
            case TextAnchor.LowerRight:
                rt.anchorMin = Vector2.right; rt.anchorMax = Vector2.right; rt.pivot = Vector2.right; break;
            case TextAnchor.UpperCenter:
                rt.anchorMin = new Vector2(0.5f, 1); rt.anchorMax = new Vector2(0.5f, 1); rt.pivot = new Vector2(0.5f, 1); break;
            default:
                rt.anchorMin = new Vector2(0.5f, 0.5f); rt.anchorMax = new Vector2(0.5f, 0.5f); break;
        }

        rt.sizeDelta = size;
        rt.anchoredPosition = offset;
        return obj;
    }

    static GameObject CreateImage(Transform parent, string name, Color color, Vector2 position, Vector2 size)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchoredPosition = position;
        rt.sizeDelta = size;
        Image img = obj.AddComponent<Image>();
        img.color = color;
        return obj;
    }

    static TMP_Text CreateText(Transform parent, string name, string defaultText, int fontSize, TextAlignmentOptions alignment)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 40);
        TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();
        text.text = defaultText;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.enableAutoSizing = false;
        return text;
    }
}
#endif
