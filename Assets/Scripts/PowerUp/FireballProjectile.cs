using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class FireballProjectile : MonoBehaviour
{
    public Vector3 size = Vector3.one * 0.5f;
    public float speed = 15f;
    public float lifeTime = 3f;
    public Material material;

    private Vector3 position;
    private int colliderID;
    private float timer;

    public void Init(Vector3 start)
    {
        position = start;
        colliderID = CollisionManager.Instance.RegisterCollider(position, size, false);
        CollisionManager.Instance.SetTrigger(colliderID, true);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) DestroySelf();

        position.x += speed * Time.deltaTime;

        if (CollisionManager.Instance.CheckCollision(colliderID, position, out var hits))
        {
            foreach (int id in hits)
            {
                var b = GetBounds(id);
                if (b != null && !b.IsPlayer)
                {
                    if (b != null && !b.IsPlayer)
                    {
                        // remove collider from collision manager
                        CollisionManager.Instance.RemoveCollider(id);

                        // try to destroy the enemy GameObject
                        var enemy = FindEnemyByColliderID(id);
                        if (enemy != null)
                            Destroy(enemy.gameObject);
                    }
                }
            }
            DestroySelf();
        }

        Matrix4x4 m = Matrix4x4.TRS(position, Quaternion.identity, size);
        CollisionManager.Instance.UpdateCollider(colliderID, position, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, m);

        Graphics.DrawMesh(PowerUpInstanced.CreateCubeMesh(), m, material, 0);
    }

    void DestroySelf()
    {
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

    EnemyInstanced FindEnemyByColliderID(int id)
    {
        var enemies = FindObjectsByType<EnemyInstanced>(FindObjectsSortMode.None);

        foreach (var e in enemies)
        {
            if (e.GetColliderID() == id)
                return e;
        }
        return null;
    }
}
