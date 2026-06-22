#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DevTool_SetupGameSystems
{
    [MenuItem("Zombie Shooter/Setup/Create Game Manager")]
    static void CreateGameManager()
    {
        if (Object.FindAnyObjectByType<GameManager>() != null)
        {
            Debug.LogWarning("A GameManager already exists in the scene.");
            return;
        }

        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();

        Undo.RegisterCreatedObjectUndo(gm, "Create Game Manager");
        Selection.activeGameObject = gm;
        Debug.Log("GameManager created. Wire up InputReader, Pause/GameOver/Victory UI panels, and WaveSpawner events.");
    }

    [MenuItem("Zombie Shooter/Setup/Create Audio Manager")]
    static void CreateAudioManager()
    {
        if (Object.FindAnyObjectByType<AudioManager>() != null)
        {
            Debug.LogWarning("An AudioManager already exists in the scene.");
            return;
        }

        GameObject am = new GameObject("AudioManager");
        AudioManager audioMgr = am.AddComponent<AudioManager>();

        GameObject musicObj = new GameObject("MusicSource");
        musicObj.transform.SetParent(am.transform);
        AudioSource musicSrc = musicObj.AddComponent<AudioSource>();
        musicSrc.playOnAwake = false;
        musicSrc.loop = true;
        musicSrc.spatialBlend = 0f;

        SerializedObject so = new SerializedObject(audioMgr);
        so.FindProperty("musicSource").objectReferenceValue = musicSrc;
        so.ApplyModifiedProperties();

        Undo.RegisterCreatedObjectUndo(am, "Create Audio Manager");
        Selection.activeGameObject = am;
        Debug.Log("AudioManager created (DontDestroyOnLoad). Assign menu/gameplay music clips in the inspector.");
    }

    [MenuItem("Zombie Shooter/Setup/Create Scene Transition Manager")]
    static void CreateSceneTransition()
    {
        if (Object.FindAnyObjectByType<SceneTransitionManager>() != null)
        {
            Debug.LogWarning("A SceneTransitionManager already exists in the scene.");
            return;
        }

        GameObject stm = new GameObject("SceneTransitionManager");
        stm.AddComponent<SceneTransitionManager>();

        Undo.RegisterCreatedObjectUndo(stm, "Create Scene Transition Manager");
        Selection.activeGameObject = stm;
        Debug.Log("SceneTransitionManager created (DontDestroyOnLoad). It auto-creates its own fade canvas.");
    }

    [MenuItem("Zombie Shooter/Setup/Create Camera Shake")]
    static void CreateCameraShake()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("No Main Camera found. Add a Camera tagged 'MainCamera' first.");
            return;
        }

        if (cam.GetComponent<CameraShake>() != null)
        {
            Debug.LogWarning("CameraShake already exists on the Main Camera.");
            return;
        }

        if (cam.GetComponent<Unity.Cinemachine.CinemachineImpulseSource>() == null)
            cam.gameObject.AddComponent<Unity.Cinemachine.CinemachineImpulseSource>();

        cam.gameObject.AddComponent<CameraShake>();

        Selection.activeGameObject = cam.gameObject;
        Debug.Log("CameraShake + CinemachineImpulseSource added to Main Camera. Also add CinemachineImpulseListener to your Virtual Camera.");
    }

    [MenuItem("Zombie Shooter/Setup/Create Time Scale Effect")]
    static void CreateTimeScale()
    {
        if (Object.FindAnyObjectByType<TimeScaleEffect>() != null)
        {
            Debug.LogWarning("A TimeScaleEffect already exists in the scene.");
            return;
        }

        GameObject tse = new GameObject("TimeScaleEffect");
        tse.AddComponent<TimeScaleEffect>();

        Undo.RegisterCreatedObjectUndo(tse, "Create Time Scale Effect");
        Selection.activeGameObject = tse;
        Debug.Log("TimeScaleEffect created. Call TimeScaleEffect.Instance.HitStop() from events or code.");
    }

    [MenuItem("Zombie Shooter/Setup/--- Full Scene Setup ---")]
    static void FullSetup()
    {
        Debug.Log("=== Full Scene Setup ===\nUse the individual setup items above in this order:\n" +
                  "1. Create Audio Manager (first - it's DontDestroyOnLoad)\n" +
                  "2. Create Scene Transition Manager\n" +
                  "3. Create Game Manager\n" +
                  "4. Create Player\n" +
                  "5. Create Bullet Pool\n" +
                  "6. Create HUD Canvas\n" +
                  "7. Create Camera Shake\n" +
                  "8. Create Time Scale Effect\n" +
                  "9. Create Enemy Templates + Spawn Points + Wave Spawner\n" +
                  "10. Wire events between components in the inspector");
    }
}
#endif
