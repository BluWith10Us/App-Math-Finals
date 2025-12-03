using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class EnemyInstanced : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    public float speed = 2f;
    public int damage = 1;
    public float lifeTime = 30f; // auto-destroy if stuck

    private int colliderID;
    private Vector3 position;
    private bool movingRight;
    private float timer;
    private Mesh mesh;
    private Matrix4x4 matrix;

    void Start()
    {
        mesh = PowerUpInstanced.CreateCubeMesh();

        position = transform.position;
        movingRight = Random.value > 0.5f;

        colliderID = CollisionManager.Instance.RegisterCollider(position, size, false);
        matrix = Matrix4x4.TRS(position, Quaternion.identity, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            DestroySelf();
            return;
        }

        Move();
        HandleCollision();
        Draw();
    }

    void Move()
    {
        float dir = movingRight ? 1 : -1;
        position.x += dir * speed * Time.deltaTime;

        // Direction flip if hits a wall
        if (CollisionManager.Instance.CheckCollision(colliderID, position, out _))
        {
            movingRight = !movingRight;
        }

        UpdateCollider();
    }

    void HandleCollision()
    {
        if (CollisionManager.Instance.CheckCollision(colliderID, position, out var hits))
        {
            foreach (int id in hits)
            {
                var b = GetBounds(id);

                // HIT PLAYER
                if (b != null && b.IsPlayer)
                {
                    PlayerStats.Instance.TakeDamage(damage);
                    DestroySelf();
                    return;
                }
            }
        }
    }

    void Draw()
    {
        matrix = Matrix4x4.TRS(position, Quaternion.identity, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
        Graphics.DrawMesh(mesh, matrix, FindAnyObjectByType<EnhancedMeshGenerator>().material, 0);
    }

    void UpdateCollider()
    {
        CollisionManager.Instance.UpdateCollider(colliderID, position, size);
    }

    void DestroySelf()
    {
        CollisionManager.Instance.RemoveCollider(colliderID);
        Destroy(gameObject);
    }

    // Get AABB from CollisionManager (reflection method used by your powerups)
    AABBBounds GetBounds(int id)
    {
        var dict = (System.Collections.Generic.Dictionary<int, AABBBounds>)
            typeof(CollisionManager)
            .GetField("_colliders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(CollisionManager.Instance);

        dict.TryGetValue(id, out var bounds);
        return bounds;
    }

    public int GetColliderID() => colliderID;
}
