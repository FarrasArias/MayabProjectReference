#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class DevTool_SetupEnemy
{
    [MenuItem("Zombie Shooter/Setup/Create Enemy Template")]
    static void CreateEnemy()
    {
        GameObject enemy = new GameObject("Enemy_Template");
        enemy.tag = "Enemy";

        Rigidbody rb = enemy.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        CapsuleCollider col = enemy.AddComponent<CapsuleCollider>();
        col.height = 2f;
        col.center = new Vector3(0, 1f, 0);
        col.radius = 0.4f;

        NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
        agent.speed = 3.5f;
        agent.angularSpeed = 240f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 1.5f;
        agent.radius = 0.4f;
        agent.height = 2f;

        HealthComponent health = enemy.AddComponent<HealthComponent>();
        EnemyAI ai = enemy.AddComponent<EnemyAI>();
        HitFlash hitFlash = enemy.AddComponent<HitFlash>();
        LootDrop lootDrop = enemy.AddComponent<LootDrop>();

        // Wire references
        SerializedObject aiSO = new SerializedObject(ai);
        aiSO.FindProperty("health").objectReferenceValue = health;
        aiSO.FindProperty("hitFlash").objectReferenceValue = hitFlash;
        aiSO.ApplyModifiedProperties();

        // Wire health death event to loot drop
        SerializedObject healthSO = new SerializedObject(health);
        healthSO.FindProperty("destroyOnDeath").boolValue = false;
        healthSO.ApplyModifiedProperties();

        Undo.RegisterCreatedObjectUndo(enemy, "Create Enemy Template");
        Selection.activeGameObject = enemy;
        Debug.Log("Enemy Template created. Next steps:\n" +
                  "1. Add your zombie model as a child\n" +
                  "2. Add an Animator and assign controller\n" +
                  "3. Create an EnemyData ScriptableObject and assign it\n" +
                  "4. Set up LootDrop entries (optional)\n" +
                  "5. Save as prefab");
    }

    [MenuItem("Zombie Shooter/Setup/Create Wave Spawner")]
    static void CreateWaveSpawner()
    {
        GameObject spawner = new GameObject("WaveSpawner");
        spawner.AddComponent<WaveSpawner>();

        Undo.RegisterCreatedObjectUndo(spawner, "Create Wave Spawner");
        Selection.activeGameObject = spawner;
        Debug.Log("Wave Spawner created. Add Waves in the inspector and place SpawnPoint objects in your scene.");
    }

    [MenuItem("Zombie Shooter/Setup/Create Spawn Point")]
    static void CreateSpawnPoint()
    {
        GameObject sp = new GameObject("SpawnPoint");
        sp.AddComponent<SpawnPoint>();

        if (Selection.activeGameObject != null)
            sp.transform.position = Selection.activeGameObject.transform.position + Vector3.forward * 5f;

        Undo.RegisterCreatedObjectUndo(sp, "Create Spawn Point");
        Selection.activeGameObject = sp;
    }
}
#endif
