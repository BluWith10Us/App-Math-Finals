using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class PowerUp_Invincibility : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    public float duration = 5f;
    public Material material;

    private int colliderID;
    private bool collected = false;
    private Mesh mesh;
    private Matrix4x4 matrix;

    void Start()
    {
        mesh = PowerUpInstanced.CreateCubeMesh();

        Vector3 pos = transform.position;

        colliderID = CollisionManager.Instance.RegisterCollider(pos, size);
        CollisionManager.Instance.SetTrigger(colliderID, true);

        matrix = Matrix4x4.TRS(pos, Quaternion.identity, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
    }

    void Update()
    {
        if (collected) return;

        DetectPlayer();

        // DRAW POWER-UP
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
                    PlayerStats.Instance.EnableInvincibility(duration);
                    DestroySelf();
                    return;
                }
            }
        }
    }

    void DestroySelf()
    {
        collected = true;
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
