#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevTool_SetupGameOverUI
{
    [MenuItem("Zombie Shooter/Setup/Create Pause Menu Panel")]
    static void CreatePauseMenu()
    {
        Canvas canvas = FindOrWarnCanvas();
        if (canvas == null) return;

        GameObject panel = CreateOverlayPanel(canvas.transform, "PauseMenu_Panel", new Color(0, 0, 0, 0.7f));
        panel.SetActive(false);

        CreateText(panel.transform, "PauseTitle", "PAUSED", 56, new Vector2(0, 100));

        Button resumeBtn = CreateButton(panel.transform, "ResumeButton", "RESUME", new Vector2(0, 0), new Color(0.2f, 0.7f, 0.2f));
        Button restartBtn = CreateButton(panel.transform, "RestartButton", "RESTART", new Vector2(0, -70), new Color(0.6f, 0.6f, 0.2f));
        Button menuBtn = CreateButton(panel.transform, "MainMenuButton", "MAIN MENU", new Vector2(0, -140), new Color(0.7f, 0.2f, 0.2f));

        GameManager gm = Object.FindAnyObjectByType<GameManager>();
        if (gm != null)
        {
            resumeBtn.onClick.AddListener(gm.Resume);
            restartBtn.onClick.AddListener(gm.RestartGame);
            menuBtn.onClick.AddListener(gm.LoadMainMenu);

            SerializedObject so = new SerializedObject(gm);
            so.FindProperty("pauseMenuUI").objectReferenceValue = panel;
            so.ApplyModifiedProperties();
        }

        Undo.RegisterCreatedObjectUndo(panel, "Create Pause Menu");
        Selection.activeGameObject = panel;
        Debug.Log("Pause Menu panel created" + (gm != null ? " and wired to GameManager." : ". Create a GameManager first to auto-wire."));
    }

    [MenuItem("Zombie Shooter/Setup/Create Game Over Panel")]
    static void CreateGameOver()
    {
        Canvas canvas = FindOrWarnCanvas();
        if (canvas == null) return;

        GameObject panel = CreateOverlayPanel(canvas.transform, "GameOver_Panel", new Color(0.3f, 0, 0, 0.8f));
        panel.SetActive(false);

        CreateText(panel.transform, "GameOverTitle", "GAME OVER", 64, new Vector2(0, 100));
        CreateText(panel.transform, "FinalScore", "Score: 0", 28, new Vector2(0, 30));

        Button restartBtn = CreateButton(panel.transform, "RestartButton", "TRY AGAIN", new Vector2(0, -50), new Color(0.6f, 0.6f, 0.2f));
        Button menuBtn = CreateButton(panel.transform, "MainMenuButton", "MAIN MENU", new Vector2(0, -120), new Color(0.4f, 0.4f, 0.4f));

        GameManager gm = Object.FindAnyObjectByType<GameManager>();
        if (gm != null)
        {
            restartBtn.onClick.AddListener(gm.RestartGame);
            menuBtn.onClick.AddListener(gm.LoadMainMenu);

            SerializedObject so = new SerializedObject(gm);
            so.FindProperty("gameOverUI").objectReferenceValue = panel;
            so.ApplyModifiedProperties();
        }

        Undo.RegisterCreatedObjectUndo(panel, "Create Game Over Panel");
        Selection.activeGameObject = panel;
    }

    [MenuItem("Zombie Shooter/Setup/Create Victory Panel")]
    static void CreateVictory()
    {
        Canvas canvas = FindOrWarnCanvas();
        if (canvas == null) return;

        GameObject panel = CreateOverlayPanel(canvas.transform, "Victory_Panel", new Color(0, 0.2f, 0, 0.8f));
        panel.SetActive(false);

        CreateText(panel.transform, "VictoryTitle", "YOU SURVIVED!", 64, new Vector2(0, 100));
        CreateText(panel.transform, "VictoryScore", "Final Score: 0", 28, new Vector2(0, 30));

        Button restartBtn = CreateButton(panel.transform, "PlayAgainButton", "PLAY AGAIN", new Vector2(0, -50), new Color(0.2f, 0.7f, 0.2f));
        Button menuBtn = CreateButton(panel.transform, "MainMenuButton", "MAIN MENU", new Vector2(0, -120), new Color(0.4f, 0.4f, 0.4f));

        GameManager gm = Object.FindAnyObjectByType<GameManager>();
        if (gm != null)
        {
            restartBtn.onClick.AddListener(gm.RestartGame);
            menuBtn.onClick.AddListener(gm.LoadMainMenu);

            SerializedObject so = new SerializedObject(gm);
            so.FindProperty("victoryUI").objectReferenceValue = panel;
            so.ApplyModifiedProperties();
        }

        Undo.RegisterCreatedObjectUndo(panel, "Create Victory Panel");
        Selection.activeGameObject = panel;
    }

    static Canvas FindOrWarnCanvas()
    {
        Canvas canvas = Object.FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No Canvas found in scene. Create an HUD Canvas first (Zombie Shooter > Setup > Create HUD Canvas).");
            return null;
        }
        return canvas;
    }

    static GameObject CreateOverlayPanel(Transform parent, string name, Color bgColor)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        Image img = panel.AddComponent<Image>();
        img.color = bgColor;
        return panel;
    }

    static void CreateText(Transform parent, string name, string content, int fontSize, Vector2 position)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = new Vector2(500, 80);
        TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
    }

    static Button CreateButton(Transform parent, string name, string label, Vector2 position, Color color)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = new Vector2(280, 55);
        Image img = obj.AddComponent<Image>();
        img.color = color;
        Button btn = obj.AddComponent<Button>();

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(obj.transform, false);
        RectTransform textRT = textObj.AddComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero; textRT.anchorMax = Vector2.one; textRT.sizeDelta = Vector2.zero;
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = label;
        text.fontSize = 22;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        return btn;
    }
}
#endif
