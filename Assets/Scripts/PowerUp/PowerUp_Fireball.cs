using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class PowerUp_Fireball : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    public Material material;

    private int colliderID;
    private Matrix4x4 matrix;
    private bool collected = false;
    private Mesh mesh;

    void Start()
    {
        mesh = PowerUpInstanced.CreateCubeMesh();

        Vector3 pos = transform.position;

        colliderID = CollisionManager.Instance.RegisterCollider(pos, size, false);
        CollisionManager.Instance.SetTrigger(colliderID, true);

        matrix = Matrix4x4.TRS(pos, Quaternion.identity, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
    }

    void Update()
    {
        if (collected) return;

        DetectPlayer();
        Graphics.DrawMesh(mesh, matrix, material, 0);
    }

    void DetectPlayer()
    {
        if (CollisionManager.Instance.CheckCollision(colliderID, transform.position, out var hits))
        {
            foreach (int id in hits)
            {
                var b = GetBounds(id);
                if (b != null && b.IsPlayer)
                {
                    Collect();
                    return;
                }
            }
        }
    }

    void Collect()
    {
        collected = true;

        PlayerStats.Instance.canShootFireball = true;
        Debug.Log("Fireball ability unlocked!");

        CollisionManager.Instance.RemoveCollider(colliderID);
        Destroy(gameObject);
    }

    AABBBounds GetBounds(int id)
    {
        var dict = (System.Collections.Generic.Dictionary<int, AABBBounds>)
            typeof(CollisionManager)
            .GetField("_colliders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(CollisionManager.Instance);

        dict.TryGetValue(id, out var bounds);
        return bounds;
    }
}
