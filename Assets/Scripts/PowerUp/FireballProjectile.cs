using System.Collections.Generic;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class FireballProjectile : MonoBehaviour
{
    public Vector3 size = Vector3.one * 0.5f;
    public float speed = 15f;
    public float lifeTime = 3f;
    public Material material;

    private Vector3 position;
    private int colliderID = -1;
    private float timer = 0f;
    private Mesh mesh;
    private bool initialized = false;

    // -------------------------
    // Initialize fireball
    // -------------------------
    public void Init(Vector3 start)
    {
        position = start;
        initialized = true;

        mesh = PowerUpInstanced.CreateCubeMesh();

        colliderID = CollisionManager.Instance.RegisterCollider(position, size);
        CollisionManager.Instance.SetTrigger(colliderID, true);
        CollisionManager.Instance.UpdateMatrix(colliderID, Matrix4x4.TRS(position, Quaternion.identity, size));
    }

    void Update()
    {
        if (!initialized) return; // wait until Init() is called

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            DestroySelf();
            return;
        }

        // Move forward on X
        position.x += speed * Time.deltaTime;

        // Check collisions
        if (CollisionManager.Instance.CheckCollision(colliderID, position, out var hits))
        {
            foreach (int hitID in hits)
            {
                Enemy enemy = Enemy.AllEnemies.Find(e => e.GetColliderID() == hitID);
                if (enemy != null)
                {
                    enemy.Die();
                }
            }

            DestroySelf();
            return;
        }

        // Update collider and matrix
        Matrix4x4 m = Matrix4x4.TRS(position, Quaternion.identity, size);
        CollisionManager.Instance.UpdateCollider(colliderID, position, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, m);

        // Render
        Graphics.DrawMesh(mesh, m, material, 0);
    }

    void DestroySelf()
    {
        if (colliderID != -1)
        {
            CollisionManager.Instance.RemoveCollider(colliderID);
            colliderID = -1;
        }

        Destroy(gameObject);
    }
}
