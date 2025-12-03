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
                CollisionManager.Instance.RemoveCollider(id); // enemy dies
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
}
