using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Vector3 = UnityEngine.Vector3;

public class PowerUpInstanced : MonoBehaviour
{
    public static PowerUpInstanced Instance;

    public Material material;
    public Mesh mesh;
    public Vector3 size = Vector3.one;

    private Matrix4x4 matrix;
    private int colliderID = -1;
    private bool collected = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreatePowerUp();
        if (mesh == null)
            mesh = CreateCubeMesh();
    }

    void CreatePowerUp()
    {
        Vector3 pos = transform.position;

        colliderID = CollisionManager.Instance.RegisterCollider(pos, size, false);
        CollisionManager.Instance.SetTrigger(colliderID, true);

        matrix = Matrix4x4.TRS(pos, Quaternion.identity, size);
        CollisionManager.Instance.UpdateMatrix(colliderID, matrix);
    }

    public static Mesh CreateCubeMesh()
    {
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Mesh m = temp.GetComponent<MeshFilter>().sharedMesh;
        Destroy(temp);
        return m;
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
                AABBBounds other = GetBounds(id);
                if (other != null && other.IsPlayer)
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
        Debug.Log("POWER-UP COLLECTED!");

        ApplyEffect();

        CollisionManager.Instance.RemoveCollider(colliderID);
        Destroy(gameObject);
    }

    void ApplyEffect()
    {
        // Example effect:
        Debug.Log("Speed Boost Applied!");
    }

    AABBBounds GetBounds(int id)
    {
        var field = typeof(CollisionManager)
            .GetField("_colliders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var dict = (System.Collections.Generic.Dictionary<int, AABBBounds>)field.GetValue(CollisionManager.Instance);
        dict.TryGetValue(id, out AABBBounds bounds);
        return bounds;
    }
}
