using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

public class ArrowBatchCombiner : MonoBehaviour
{
    public Mesh shaftMesh; // Mesh for the arrow shaft (cylinder)
    public Mesh arrowheadMesh; // Mesh for the arrowhead (cone)

    // Materials for energy ranges
    public Material blueMaterial;
    public Material lightBlueMaterial; 
    public Material greenMaterial;
    public Material lightGreenMaterial;
    public Material yellowMaterial;
    public Material orangeMaterial;
    public Material redMaterial;

    public TextAsset csvFile;
    public int batchSize = 500;

    public float lengthMultiplier = 1.0f; // Controls length scaling
    public float thicknessMultiplier = 0.1f; // Controls thickness scaling
    public float arrowheadScale = 1.0f;

    void Start()
    {
        float transparency = 0.75f; // Adjust this value for desired transparency
        SetMaterialTransparency(blueMaterial, transparency);
        SetMaterialTransparency(lightBlueMaterial, transparency);
        SetMaterialTransparency(greenMaterial, transparency);
        SetMaterialTransparency(lightGreenMaterial, transparency);
        SetMaterialTransparency(yellowMaterial, transparency);
        SetMaterialTransparency(orangeMaterial, transparency);
        SetMaterialTransparency(redMaterial, transparency);

        CombineArrowsInBatches(); // Start visualization process
    }

    void CombineArrowsInBatches()
    {
        var arrowDataList = ParseCSV(csvFile.text);

        float shaftBaseLength = shaftMesh.bounds.size.z;

        //Create a gameobject called arrowsParent
        GameObject arrowsParent = new GameObject("ArrowsParent");
        
        // Dictionaries for batching arrows based on material
        Dictionary<Material, List<CombineInstance>> shaftBatches = new Dictionary<Material, List<CombineInstance>>();
        Dictionary<Material, List<CombineInstance>> arrowheadBatches = new Dictionary<Material, List<CombineInstance>>();

        foreach (var arrowData in arrowDataList)
        {
            if (arrowData.length <= 0f) continue;

            // Adjust scaling based on density to reduce overlaps
            float densityScale = 0.5f; // Adjust this to control the size reduction
            float scaledLength = arrowData.length * lengthMultiplier * densityScale;
            float thickness = arrowData.length * thicknessMultiplier * densityScale;

            // Shaft transform
            Matrix4x4 shaftMatrix = Matrix4x4.TRS(
                arrowData.position,
                arrowData.rotation,
                new Vector3(thickness, thickness, scaledLength)
            );

            // Arrowhead position
            Vector3 arrowheadPosition = arrowData.position + arrowData.rotation * new Vector3(0, 0, scaledLength * shaftBaseLength);
            Matrix4x4 arrowheadMatrix = Matrix4x4.TRS(
                arrowheadPosition,
                arrowData.rotation,
                Vector3.one * thickness * arrowheadScale * densityScale
            );

            // Determine material based on temperature
            Material arrowMaterial = GetMaterialForTemperature(arrowData.temperature);

            // Combine instances for shaft and arrowhead
            CombineInstance shaftCombineInstance = new CombineInstance
            {
                mesh = shaftMesh,
                transform = shaftMatrix
            };

            CombineInstance arrowheadCombineInstance = new CombineInstance
            {
                mesh = arrowheadMesh,
                transform = arrowheadMatrix
            };

            if (!shaftBatches.ContainsKey(arrowMaterial))
            {
                shaftBatches[arrowMaterial] = new List<CombineInstance>();
                arrowheadBatches[arrowMaterial] = new List<CombineInstance>();
            }

            shaftBatches[arrowMaterial].Add(shaftCombineInstance);
            arrowheadBatches[arrowMaterial].Add(arrowheadCombineInstance);
        }

        // Create and render batched GameObjects
        foreach (var kvp in shaftBatches)
        {
            Material material = kvp.Key;

            // Combine shaft meshes
            Mesh combinedShaftMesh = new Mesh();
            combinedShaftMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            combinedShaftMesh.CombineMeshes(shaftBatches[material].ToArray(), true, true);

            // Create a new GameObject for the shaft batch
            GameObject shaftBatchObject = new GameObject($"ArrowShaftBatch_{material.name}");
            MeshFilter shaftMF = shaftBatchObject.AddComponent<MeshFilter>();
            shaftMF.mesh = combinedShaftMesh;
            MeshRenderer shaftMR = shaftBatchObject.AddComponent<MeshRenderer>();
            shaftMR.material = material;

            // Combine arrowhead meshes
            Mesh combinedArrowheadMesh = new Mesh();
            combinedArrowheadMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            combinedArrowheadMesh.CombineMeshes(arrowheadBatches[material].ToArray(), true, true);

            GameObject arrowheadBatchObject = new GameObject($"ArrowHeadBatch_{material.name}");
            MeshFilter arrowheadMF = arrowheadBatchObject.AddComponent<MeshFilter>();
            arrowheadMF.mesh = combinedArrowheadMesh;
            MeshRenderer arrowheadMR = arrowheadBatchObject.AddComponent<MeshRenderer>();
            arrowheadMR.material = material;

            //Make the transformation of shaftBatchObject and arrowheadBatchObject a child of arrowsParent
            shaftBatchObject.transform.parent = arrowsParent.transform;
            arrowheadBatchObject.transform.parent = arrowsParent.transform;
        }
        arrowsParent.transform.eulerAngles = new Vector3(0, -90, 90);
        //pos
    }

    Material GetMaterialForTemperature(float temperature)
    {
        if (temperature < 21) return redMaterial;
        if (temperature >= 21 && temperature < 22) return lightBlueMaterial;
        if (temperature >= 22 && temperature < 23) return greenMaterial;
        if (temperature >= 23 && temperature < 24) return lightGreenMaterial;
        if (temperature >= 24 && temperature < 25) return yellowMaterial;
        if (temperature >= 25 && temperature < 27) return orangeMaterial;
        return blueMaterial; // Default for temperature >= 27
    }

    void SetMaterialTransparency(Material material, float alpha)
    {
        if (material != null)
        {
            Color color = material.color;
            color.a = alpha; // Set transparency
            material.color = color;

            // Ensure the material is in transparent mode
            material.SetFloat("_Mode", 3); // 3 corresponds to Transparent mode
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000; // Transparent queue
        }
    }

    List<ArrowData> ParseCSV(string csvText)
    {
        var arrowDataList = new List<ArrowData>();
        var lines = csvText.Split('\n');

        bool isFirstLine = true;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            var values = line.Split(',');
            if (values.Length < 19) continue; // Ensure all columns are present

            NumberFormatInfo nfi = NumberFormatInfo.InvariantInfo;

            // Parsing all fields
            if (float.TryParse(values[0], NumberStyles.Float, nfi, out float wiUX) &&
                float.TryParse(values[1], NumberStyles.Float, nfi, out float wiUY) &&
                float.TryParse(values[2], NumberStyles.Float, nfi, out float wiUZ) &&
                float.TryParse(values[3], NumberStyles.Float, nfi, out float wiVX) &&
                float.TryParse(values[4], NumberStyles.Float, nfi, out float wiVY) &&
                float.TryParse(values[5], NumberStyles.Float, nfi, out float wiVZ) &&
                float.TryParse(values[6], NumberStyles.Float, nfi, out float wiWX) &&
                float.TryParse(values[7], NumberStyles.Float, nfi, out float wiWY) &&
                float.TryParse(values[8], NumberStyles.Float, nfi, out float wiWZ) &&
                float.TryParse(values[9], NumberStyles.Float, nfi, out float originalX) &&
                float.TryParse(values[10], NumberStyles.Float, nfi, out float originalY) &&
                float.TryParse(values[11], NumberStyles.Float, nfi, out float originalZ) &&
                float.TryParse(values[12], NumberStyles.Float, nfi, out float u) &&
                float.TryParse(values[13], NumberStyles.Float, nfi, out float v) &&
                float.TryParse(values[14], NumberStyles.Float, nfi, out float w) &&
                float.TryParse(values[15], NumberStyles.Float, nfi, out float uEnergy) &&
                float.TryParse(values[16], NumberStyles.Float, nfi, out float vEnergy) &&
                float.TryParse(values[17], NumberStyles.Float, nfi, out float wEnergy) &&
                float.TryParse(values[18], NumberStyles.Float, nfi, out float pressure) &&
                float.TryParse(values[19], NumberStyles.Float, nfi, out float temperature)) // Parse temperature
            {
                // Compute position using W'I(U), W'I(V), W'I(W) as measurement location
                Vector3 position = new Vector3(wiUX, wiUY, wiUZ);

                // Original position (X, Y, Z)
                Vector3 originalPosition = new Vector3(originalX, originalY, originalZ);

                // Velocity vector
                Vector3 direction = new Vector3(u, v, w);
                float length = direction.magnitude;

                // Average energy (or choose a specific component if needed)
                float averageEnergy = (uEnergy + vEnergy + wEnergy) / 3f;

                if (length > 0f)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction / length);

                    arrowDataList.Add(new ArrowData
                    {
                        position = position,
                        originalPosition = originalPosition,
                        rotation = rotation,
                        length = length,
                        energy = averageEnergy,
                        pressure = pressure,
                        temperature = temperature // Add temperature to the ArrowData
                    });
                }
            }
        }

        return arrowDataList;
    }

    struct ArrowData
    {
        public Vector3 position; // Measurement position (W'I)
        public Vector3 originalPosition; // Original position of the arrow
        public Quaternion rotation;
        public float length;
        public float energy; // Energy value
        public float pressure; // Scalar pressure
        public float temperature; // Temperature value
    }
}
