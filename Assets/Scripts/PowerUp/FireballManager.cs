using UnityEngine;

public class FireballManager : MonoBehaviour
{
    public static FireballManager Instance;

    [Header("Fireball Settings")]
    public Material fireballMaterial;
    public float spawnOffsetX = 1.5f;  // in front of player
    public float spawnOffsetY = 0.5f;  // slightly above player

    void Awake()
    {
        // Singleton instance
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnFireball()
    {
        EnhancedMeshGenerator generator = FindAnyObjectByType<EnhancedMeshGenerator>();
        if (generator == null) return;

        Vector3 spawnPos = generator.GetPlayerPosition();
        spawnPos.x += 1.5f;
        spawnPos.y += 0.5f;

        GameObject go = new GameObject("Fireball");
        FireballProjectile fb = go.AddComponent<FireballProjectile>();
        fb.material = fireballMaterial;
        fb.Init(spawnPos); // now the fireball starts at the correct position
    }
}
