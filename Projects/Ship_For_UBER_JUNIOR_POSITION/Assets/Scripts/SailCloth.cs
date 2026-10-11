using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SailCloth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShipController shipController;
    [SerializeField] private ShipMovement shipMovement;

    [Header("Shape")]
    [SerializeField] private float width = 7f;
    [SerializeField] private float height = 5f;
    [SerializeField] private float boomHeight = 0.8f;
    [SerializeField] private int columns = 20;
    [SerializeField] private int rows = 18;
    [SerializeField] private float minVisibleHeight = 0.1f;

    [Header("Behavior")]
    [SerializeField] private float maxCamber = 0.18f;
    [SerializeField] private float billowSmoothTime = 0.35f;
    [SerializeField] private float flutterAmplitude = 0.15f;
    [SerializeField] private float flutterSpeed = 3f;

    private Mesh mesh;
    private Vector3[] vertices;
    private int pointsPerSide;
    private float billow;
    private float billowVelocity;

    private void Awake()
    {
        EnsureUpright();
        BuildMesh();
    }

    private void LateUpdate()
    {
        UpdateVertices();
    }

    private void OnDestroy()
    {
        if (mesh != null)
        {
            Destroy(mesh);
        }
    }

    private void EnsureUpright()
    {
        if (transform.localScale != Vector3.one)
        {
            Debug.LogWarning("SailCloth: resetting the pivot scale to (1,1,1).", this);
            transform.localScale = Vector3.one;
        }
    }

    private void BuildMesh()
    {
        pointsPerSide = (columns + 1) * (rows + 1);

        vertices = new Vector3[pointsPerSide * 2];
        var uvs = new Vector2[pointsPerSide * 2];
        var triangles = new int[columns * rows * 12];

        int triangleIndex = 0;

        for (int row = 0; row <= rows; row++)
        {
            for (int column = 0; column <= columns; column++)
            {
                int index = row * (columns + 1) + column;

                var uv = new Vector2(
                    (float)column / columns,
                    1f - (float)row / rows);

                uvs[index] = uv;
                uvs[index + pointsPerSide] = uv;

                if (row == rows || column == columns)
                {
                    continue;
                }

                int i0 = index;
                int i1 = index + 1;
                int i2 = index + columns + 1;
                int i3 = i2 + 1;

                triangles[triangleIndex++] = i0;
                triangles[triangleIndex++] = i2;
                triangles[triangleIndex++] = i1;
                triangles[triangleIndex++] = i1;
                triangles[triangleIndex++] = i2;
                triangles[triangleIndex++] = i3;

                int offset = pointsPerSide;

                triangles[triangleIndex++] = i0 + offset;
                triangles[triangleIndex++] = i1 + offset;
                triangles[triangleIndex++] = i2 + offset;
                triangles[triangleIndex++] = i1 + offset;
                triangles[triangleIndex++] = i3 + offset;
                triangles[triangleIndex++] = i2 + offset;
            }
        }

        mesh = new Mesh();
        mesh.name = "SailMesh";
        mesh.MarkDynamic();

        UpdateVertices();

        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void UpdateVertices()
    {
        float opening = shipController.SailOpeningNormalized;
        float across = shipMovement.SignedWindAcrossSail;

        float visibleHeight = height * Mathf.Lerp(minVisibleHeight, 1f, opening);

        float targetBillow = Mathf.Sign(across)
            * Mathf.Clamp01(Mathf.Abs(across) * 2f)
            * opening;

        billow = Mathf.SmoothDamp(
            billow,
            targetBillow,
            ref billowVelocity,
            billowSmoothTime);

        float luff = (1f - Mathf.Clamp01(Mathf.Abs(across) * 4f)) * opening;
        float windFactor = Mathf.Clamp01(shipMovement.ApparentWindSpeed / 10f);
        float flutterAmount = flutterAmplitude * luff * windFactor;
        float noiseTime = Time.time * flutterSpeed;

        float camberDepth = maxCamber * width;
        float top = boomHeight + height;

        for (int row = 0; row <= rows; row++)
        {
            float v = (float)row / rows;
            float y = top - v * visibleHeight;
            float heightProfile = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * v);

            for (int column = 0; column <= columns; column++)
            {
                float u = (float)column / columns;
                float chordProfile = Mathf.Sin(Mathf.PI * u);

                float belly = billow
                    * camberDepth
                    * chordProfile
                    * heightProfile;

                float noise = Mathf.PerlinNoise(
                    u * 3f + noiseTime,
                    v * 3f) - 0.5f;

                float flutter = noise
                    * 2f
                    * flutterAmount
                    * u
                    * (0.3f + 0.7f * v);

                Vector3 position = new Vector3(
                    belly + flutter,
                    y,
                    (u - 0.5f) * width);

                int index = row * (columns + 1) + column;

                vertices[index] = position;
                vertices[index + pointsPerSide] = position;
            }
        }

        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}