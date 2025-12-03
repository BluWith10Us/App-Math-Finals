using UnityEngine;

public class FireballManager : MonoBehaviour
{
    public static FireballManager Instance;

    public Material fireballMaterial;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnFireball()
    {
        Vector3 spawnPos = EnhancedMeshGeneratorInstance().GetPlayerPosition();
        spawnPos.x += 1.5f; // spawn slightly in front

        GameObject go = new GameObject("Fireball");
        FireballProjectile fb = go.AddComponent<FireballProjectile>();
        fb.material = fireballMaterial;
        fb.Init(spawnPos);
    }

    EnhancedMeshGenerator EnhancedMeshGeneratorInstance()
    {
        return FindAnyObjectByType<EnhancedMeshGenerator>();
    }
}
