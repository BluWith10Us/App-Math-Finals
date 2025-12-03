using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class ObstacleInstanced : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    public Material material;

    private int colliderID;
    private Mesh mesh;
    private Matrix4x4 matrix;

    void Start()
    {
        mesh = PowerUpInstanced.CreateCubeMesh();

        Vector3 pos = transform.position;

        colliderID = CollisionManager.Instance.RegisterCollider(pos, size, false);
        CollisionManager.Instance.SetTrigger(colliderID, false);

        matrix = Matrix4x4.TRS(pos, Quaternion.identity, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
    }

    void Update()
    {
        // Obstacles do not move — only draw
        Graphics.DrawMesh(mesh, matrix, material, 0);
    }

    void OnDestroy()
    {
        CollisionManager.Instance.RemoveCollider(colliderID);
    }

    public int GetColliderID() => colliderID;
}
