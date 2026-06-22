#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Unity.Cinemachine;

public class DevTool_SetupCamera
{
    [MenuItem("Zombie Shooter/Setup/Create Camera Rig (Top-Down 45°)")]
    static void CreateCameraRig()
    {
        Camera mainCam = Camera.main;

        if (mainCam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            mainCam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
        }

        if (mainCam.GetComponent<CinemachineBrain>() == null)
            mainCam.gameObject.AddComponent<CinemachineBrain>();

        GameObject vcamObj = new GameObject("CM_TopDown");
        CinemachineCamera vcam = vcamObj.AddComponent<CinemachineCamera>();

        vcamObj.transform.position = new Vector3(0f, 14f, -10f);
        vcamObj.transform.rotation = Quaternion.Euler(50f, 0f, 0f);

        CinemachineFollow follow = vcamObj.AddComponent<CinemachineFollow>();
        follow.FollowOffset = new Vector3(0f, 14f, -10f);
        follow.TrackerSettings.PositionDamping = new Vector3(0.5f, 0.5f, 0.5f);

        vcamObj.AddComponent<CinemachineImpulseListener>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
        }

        if (mainCam.GetComponent<CameraShake>() == null)
        {
            if (mainCam.GetComponent<CinemachineImpulseSource>() == null)
                mainCam.gameObject.AddComponent<CinemachineImpulseSource>();
            mainCam.gameObject.AddComponent<CameraShake>();
        }

        Undo.RegisterCreatedObjectUndo(vcamObj, "Create Camera Rig");
        Selection.activeGameObject = vcamObj;
        Debug.Log("Camera rig created:\n" +
                  "- Main Camera has CinemachineBrain\n" +
                  "- CM_TopDown has CinemachineFollow + ImpulseListener\n" +
                  "- CameraShake on Main Camera\n" +
                  (player != null ? "- Auto-targeted Player" : "- Create the Player first, then assign Follow/LookAt targets on CM_TopDown"));
    }
}
#endif
