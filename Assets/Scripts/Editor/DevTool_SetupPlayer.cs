#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DevTool_SetupPlayer
{
    [MenuItem("Zombie Shooter/Setup/Create Player (Empty Shell)")]
    static void CreatePlayer()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.layer = LayerMask.NameToLayer("Default");

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        CapsuleCollider col = player.AddComponent<CapsuleCollider>();
        col.height = 2f;
        col.center = new Vector3(0, 1f, 0);
        col.radius = 0.4f;

        InputReader inputReader = player.AddComponent<InputReader>();
        HealthComponent health = player.AddComponent<HealthComponent>();
        PlayerController controller = player.AddComponent<PlayerController>();
        PlayerDash dash = player.AddComponent<PlayerDash>();
        WeaponController weapon = player.AddComponent<WeaponController>();
        AbilityController abilities = player.AddComponent<AbilityController>();

        // Muzzle point
        GameObject muzzle = new GameObject("MuzzlePoint");
        muzzle.transform.SetParent(player.transform);
        muzzle.transform.localPosition = new Vector3(0.3f, 1.2f, 1f);

        // Wire references
        SerializedObject controllerSO = new SerializedObject(controller);
        controllerSO.FindProperty("input").objectReferenceValue = inputReader;
        controllerSO.FindProperty("dash").objectReferenceValue = dash;
        controllerSO.FindProperty("weaponController").objectReferenceValue = weapon;
        controllerSO.FindProperty("aimPivot").objectReferenceValue = player.transform;
        controllerSO.ApplyModifiedProperties();

        SerializedObject dashSO = new SerializedObject(dash);
        dashSO.FindProperty("input").objectReferenceValue = inputReader;
        dashSO.FindProperty("healthComponent").objectReferenceValue = health;
        dashSO.FindProperty("playerController").objectReferenceValue = controller;
        dashSO.ApplyModifiedProperties();

        SerializedObject weaponSO = new SerializedObject(weapon);
        weaponSO.FindProperty("input").objectReferenceValue = inputReader;
        weaponSO.FindProperty("playerController").objectReferenceValue = controller;
        weaponSO.FindProperty("muzzlePoint").objectReferenceValue = muzzle.transform;
        weaponSO.ApplyModifiedProperties();

        SerializedObject abilitySO = new SerializedObject(abilities);
        abilitySO.FindProperty("input").objectReferenceValue = inputReader;
        abilitySO.FindProperty("playerController").objectReferenceValue = controller;
        abilitySO.ApplyModifiedProperties();

        Undo.RegisterCreatedObjectUndo(player, "Create Player");
        Selection.activeGameObject = player;
        Debug.Log("Player created. Next steps:\n" +
                  "1. Add your character model as a child\n" +
                  "2. Add an Animator component and assign the Animator Controller\n" +
                  "3. Assign the GameInputActions asset to the InputReader\n" +
                  "4. Assign WeaponData ScriptableObjects to the WeaponController\n" +
                  "5. Set the Ground Layer on the PlayerController for aiming\n" +
                  "6. Create and assign a Bullet ObjectPool");
    }

    [MenuItem("Zombie Shooter/Setup/Create Bullet Pool")]
    static void CreateBulletPool()
    {
        GameObject pool = new GameObject("BulletPool");
        ObjectPool op = pool.AddComponent<ObjectPool>();

        SerializedObject so = new SerializedObject(op);
        so.FindProperty("initialSize").intValue = 30;
        so.FindProperty("expandable").boolValue = true;
        so.ApplyModifiedProperties();

        Undo.RegisterCreatedObjectUndo(pool, "Create Bullet Pool");
        Selection.activeGameObject = pool;
        Debug.Log("Bullet Pool created. Assign a Bullet prefab to the 'Prefab' field.");
    }
}
#endif
