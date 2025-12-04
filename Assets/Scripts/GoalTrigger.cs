using UnityEngine;
using UnityEngine.SceneManagement;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class GoalTrigger : MonoBehaviour
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
        CollisionManager.Instance.SetTrigger(colliderID, true); // trigger only (no blocking)

        matrix = Matrix4x4.TRS(pos, Quaternion.identity, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
    }

    void Update()
    {
        Draw();
        DetectPlayer();
    }

    void DetectPlayer()
    {
        Vector3 center = GetCenter();

        if (CollisionManager.Instance.CheckCollision(colliderID, center, out var hits))
        {
            foreach (int id in hits)
            {
                var b = GetBounds(id);
                if (b != null && b.IsPlayer)
                {
                    FinishLevel();
                    return;
                }
            }
        }
    }

    void FinishLevel()
    {
        Debug.Log("GOAL REACHED!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Draw()
    {
        Graphics.DrawMesh(mesh, matrix, material, 0);
    }

    Vector3 GetCenter()
    {
        return CollisionManager.Instance.GetMatrix(colliderID).GetPosition();
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
