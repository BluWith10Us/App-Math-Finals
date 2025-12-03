using System.Collections.Generic;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> AllEnemies = new List<Enemy>();

    [Header("Enemy Settings")]
    public Vector3 size = Vector3.one;
    public Material material;
    public float moveDistance = 5f;
    public float speed = 2f;
    public int damage = 1;

    private Vector3 startPos;
    private Vector3 currentPos;
    private float direction = 1;

    private int colliderID;
    private Mesh mesh;
    private Matrix4x4 matrix;

    void Awake()
    {
        AllEnemies.Add(this);
    }

    void OnDestroy()
    {
        AllEnemies.Remove(this);
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.RemoveCollider(colliderID);
    }

    void Start()
    {
        startPos = transform.position;
        currentPos = startPos;

        mesh = PowerUpInstanced.CreateCubeMesh();

        colliderID = CollisionManager.Instance.RegisterCollider(currentPos, size);
        CollisionManager.Instance.SetTrigger(colliderID, true);

        matrix = Matrix4x4.TRS(currentPos, Quaternion.identity, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
    }

    void Update()
    {
        Patrol();
        UpdateColliderAndRender();
    }

    void Patrol()
    {
        currentPos.x += speed * direction * Time.deltaTime;

        if (Mathf.Abs(currentPos.x - startPos.x) >= moveDistance)
            direction *= -1;
    }

    void UpdateColliderAndRender()
    {
        matrix = Matrix4x4.TRS(currentPos, Quaternion.identity, size);
        CollisionManager.Instance.UpdateCollider(colliderID, currentPos, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
        Graphics.DrawMesh(mesh, matrix, material, 0);
    }

    public int GetColliderID()
    {
        return colliderID;
    }

    public void Die()
    {
        CollisionManager.Instance.RemoveCollider(colliderID);
        Destroy(gameObject);
    }
}
