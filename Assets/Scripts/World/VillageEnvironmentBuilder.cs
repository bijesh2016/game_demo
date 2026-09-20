using UnityEngine;

namespace HiddenNepal.World
{
    [ExecuteAlways]
    public class VillageEnvironmentBuilder : MonoBehaviour
    {
        [Header("Build Controls")]
        [SerializeField] private bool buildOnStart = true;
        [SerializeField] private Material villageGroundMat;
        [SerializeField] private Material houseWallMat;
        [SerializeField] private Material houseRoofMat;
        [SerializeField] private Material stupaMat;
        [SerializeField] private Material waterMat;
        [SerializeField] private Material rockMat;
        [SerializeField] private Material woodMat;

        private void Start()
        {
            if (Application.isPlaying && buildOnStart)
            {
                BuildSundariGaun();
            }
        }

        [ContextMenu("Build Sundari Gaun Village")]
        public void BuildSundariGaun()
        {
            // Clear previous generated environment under this object
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            CreateMaterialsIfNeeded();

            GameObject villageContainer = new GameObject("Sundari_Gaun_Village");
            villageContainer.transform.SetParent(transform);

            // 1. Village Houses (Sundari Gaun Homes)
            CreateHouse(new Vector3(-12f, 0f, 10f), new Vector3(6f, 4f, 8f), "Gurung_House_1", villageContainer.transform);
            CreateHouse(new Vector3(12f, 0f, 12f), new Vector3(7f, 4.5f, 9f), "Sherpa_House_2", villageContainer.transform);
            CreateHouse(new Vector3(-14f, 0f, -8f), new Vector3(8f, 5f, 7f), "Village_Lodge", villageContainer.transform);
            CreateHouse(new Vector3(10f, 0f, -10f), new Vector3(6f, 4f, 6f), "Tea_House", villageContainer.transform);

            // 2. Central Stupa / Chorten (Nepalese Shrine)
            CreateStupa(new Vector3(0f, 0f, 5f), villageContainer.transform);

            // 3. Mani Wall (Stone Prayer Wall)
            CreateManiWall(new Vector3(0f, 0f, -15f), villageContainer.transform);

            // 4. Trail Signpost
            CreateSignpost(new Vector3(5f, 0f, -14f), "➔ Waterfall & Cave", villageContainer.transform);

            // 5. Mountain Trail & Rocks
            CreateMountainTrail(villageContainer.transform);

            // 6. Waterfall & Cliff Nook (North-East)
            CreateWaterfallNook(new Vector3(25f, 0f, 35f), villageContainer.transform);

            // 7. Cave Entrance (South-West)
            CreateCaveEntrance(new Vector3(-30f, 0f, -30f), villageContainer.transform);

            // 8. Surrounding Forest / Trees
            CreateForestPerimeter(villageContainer.transform);

            Debug.Log("🏔️ Sundari Gaun Village Environment built successfully!");
        }

        private void CreateMaterialsIfNeeded()
        {
            if (houseWallMat == null)
            {
                houseWallMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                houseWallMat.color = new Color(0.72f, 0.52f, 0.38f); // Mud / Stone terracotta
            }
            if (houseRoofMat == null)
            {
                houseRoofMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                houseRoofMat.color = new Color(0.25f, 0.2f, 0.18f); // Slate / Wooden tile
            }
            if (stupaMat == null)
            {
                stupaMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                stupaMat.color = new Color(0.95f, 0.95f, 0.92f); // White dome
            }
            if (waterMat == null)
            {
                waterMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                waterMat.color = new Color(0.15f, 0.55f, 0.85f, 0.8f); // Glacial water blue
            }
            if (rockMat == null)
            {
                rockMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                rockMat.color = new Color(0.42f, 0.45f, 0.46f); // Himalayan slate rock
            }
            if (woodMat == null)
            {
                woodMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                woodMat.color = new Color(0.4f, 0.26f, 0.13f); // Timber wood
            }
        }

        private void CreateHouse(Vector3 pos, Vector3 size, string houseName, Transform parent)
        {
            GameObject houseObj = new GameObject(houseName);
            houseObj.transform.SetParent(parent);
            houseObj.transform.position = pos;

            // Main Walls
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Walls";
            body.transform.SetParent(houseObj.transform);
            body.transform.localPosition = new Vector3(0f, size.y / 2f, 0f);
            body.transform.localScale = size;
            body.GetComponent<Renderer>().sharedMaterial = houseWallMat;

            // Roof
            GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roof.name = "Roof";
            roof.transform.SetParent(houseObj.transform);
            roof.transform.localPosition = new Vector3(0f, size.y + 0.5f, 0f);
            roof.transform.localScale = new Vector3(size.x + 1f, 0.8f, size.z + 1f);
            roof.GetComponent<Renderer>().sharedMaterial = houseRoofMat;

            // Doorway
            GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = "Door";
            door.transform.SetParent(houseObj.transform);
            door.transform.localPosition = new Vector3(0f, 1.25f, -size.z / 2f - 0.05f);
            door.transform.localScale = new Vector3(1.4f, 2.5f, 0.1f);
            door.GetComponent<Renderer>().sharedMaterial = woodMat;
        }

        private void CreateStupa(Vector3 pos, Transform parent)
        {
            GameObject stupaObj = new GameObject("Central_Stupa_Chorten");
            stupaObj.transform.SetParent(parent);
            stupaObj.transform.position = pos;

            // Plinth (Base)
            GameObject baseBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            baseBlock.transform.SetParent(stupaObj.transform);
            baseBlock.transform.localPosition = new Vector3(0f, 0.75f, 0f);
            baseBlock.transform.localScale = new Vector3(6f, 1.5f, 6f);
            baseBlock.GetComponent<Renderer>().sharedMaterial = rockMat;

            // White Dome
            GameObject dome = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dome.transform.SetParent(stupaObj.transform);
            dome.transform.localPosition = new Vector3(0f, 3f, 0f);
            dome.transform.localScale = new Vector3(4f, 3.5f, 4f);
            dome.GetComponent<Renderer>().sharedMaterial = stupaMat;

            // Spire Top
            GameObject spire = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            spire.transform.SetParent(stupaObj.transform);
            spire.transform.localPosition = new Vector3(0f, 5.5f, 0f);
            spire.transform.localScale = new Vector3(0.6f, 2f, 0.6f);
            spire.GetComponent<Renderer>().sharedMaterial = woodMat;
        }

        private void CreateManiWall(Vector3 pos, Transform parent)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = "Mani_Stone_Wall";
            wall.transform.SetParent(parent);
            wall.transform.position = pos + new Vector3(0f, 1f, 0f);
            wall.transform.localScale = new Vector3(8f, 2f, 1.2f);
            wall.GetComponent<Renderer>().sharedMaterial = rockMat;
        }

        private void CreateSignpost(Vector3 pos, string text, Transform parent)
        {
            GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            post.name = "Trail_Signpost";
            post.transform.SetParent(parent);
            post.transform.position = pos + new Vector3(0f, 1.5f, 0f);
            post.transform.localScale = new Vector3(0.2f, 1.5f, 0.2f);
            post.GetComponent<Renderer>().sharedMaterial = woodMat;

            GameObject board = GameObject.CreatePrimitive(PrimitiveType.Cube);
            board.name = "Signboard";
            board.transform.SetParent(post.transform);
            board.transform.localPosition = new Vector3(0.8f, 0.8f, 0f);
            board.transform.localScale = new Vector3(2.5f, 0.6f, 0.1f);
            board.GetComponent<Renderer>().sharedMaterial = woodMat;
        }

        private void CreateWaterfallNook(Vector3 pos, Transform parent)
        {
            GameObject waterfallObj = new GameObject("Waterfall_Nook");
            waterfallObj.transform.SetParent(parent);
            waterfallObj.transform.position = pos;

            // Cliff Wall
            GameObject cliff = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cliff.name = "Mountain_Cliff";
            cliff.transform.SetParent(waterfallObj.transform);
            cliff.transform.localPosition = new Vector3(0f, 10f, 0f);
            cliff.transform.localScale = new Vector3(20f, 20f, 6f);
            cliff.GetComponent<Renderer>().sharedMaterial = rockMat;

            // Waterfall Water Column
            GameObject waterColumn = GameObject.CreatePrimitive(PrimitiveType.Cube);
            waterColumn.name = "Waterfall_Stream";
            waterColumn.transform.SetParent(waterfallObj.transform);
            waterColumn.transform.localPosition = new Vector3(0f, 9f, -3.2f);
            waterColumn.transform.localScale = new Vector3(4f, 18f, 0.4f);
            waterColumn.GetComponent<Renderer>().sharedMaterial = waterMat;

            // Pool Basin
            GameObject pool = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pool.name = "Waterfall_Pool";
            pool.transform.SetParent(waterfallObj.transform);
            pool.transform.localPosition = new Vector3(0f, 0.1f, -8f);
            pool.transform.localScale = new Vector3(12f, 0.2f, 12f);
            pool.GetComponent<Renderer>().sharedMaterial = waterMat;
        }

        private void CreateCaveEntrance(Vector3 pos, Transform parent)
        {
            GameObject caveObj = new GameObject("Cave_Entrance");
            caveObj.transform.SetParent(parent);
            caveObj.transform.position = pos;

            // Rock Arch
            GameObject archLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
            archLeft.transform.SetParent(caveObj.transform);
            archLeft.transform.localPosition = new Vector3(-3f, 4f, 0f);
            archLeft.transform.localScale = new Vector3(4f, 8f, 6f);
            archLeft.GetComponent<Renderer>().sharedMaterial = rockMat;

            GameObject archRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
            archRight.transform.SetParent(caveObj.transform);
            archRight.transform.localPosition = new Vector3(3f, 4f, 0f);
            archRight.transform.localScale = new Vector3(4f, 8f, 6f);
            archRight.GetComponent<Renderer>().sharedMaterial = rockMat;

            GameObject archTop = GameObject.CreatePrimitive(PrimitiveType.Cube);
            archTop.transform.SetParent(caveObj.transform);
            archTop.transform.localPosition = new Vector3(0f, 7f, 0f);
            archTop.transform.localScale = new Vector3(10f, 3f, 6f);
            archTop.GetComponent<Renderer>().sharedMaterial = rockMat;
        }

        private void CreateMountainTrail(Transform parent)
        {
            GameObject trailObj = new GameObject("Trail_Path");
            trailObj.transform.SetParent(parent);

            // Decorative path rocks along trail
            for (int i = 0; i < 15; i++)
            {
                float z = -20f + (i * 4f);
                float x = Mathf.Sin(i * 0.5f) * 3f;

                GameObject pathRock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                pathRock.transform.SetParent(trailObj.transform);
                pathRock.transform.position = new Vector3(x + (i % 2 == 0 ? 2f : -2f), 0.2f, z);
                pathRock.transform.localScale = new Vector3(1.2f, 0.4f, 1.2f);
                pathRock.GetComponent<Renderer>().sharedMaterial = rockMat;
            }
        }

        private void CreateForestPerimeter(Transform parent)
        {
            GameObject forestObj = new GameObject("Village_Forest");
            forestObj.transform.SetParent(parent);

            // Ring of pine tree proxies around village radius
            int treeCount = 28;
            float radius = 38f;

            for (int i = 0; i < treeCount; i++)
            {
                float angle = (i / (float)treeCount) * Mathf.PI * 2f;
                Vector3 treePos = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

                GameObject tree = new GameObject($"Pine_Tree_{i}");
                tree.transform.SetParent(forestObj.transform);
                tree.transform.position = treePos;

                // Trunk
                GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                trunk.transform.SetParent(tree.transform);
                trunk.transform.localPosition = new Vector3(0f, 2.5f, 0f);
                trunk.transform.localScale = new Vector3(0.8f, 2.5f, 0.8f);
                trunk.GetComponent<Renderer>().sharedMaterial = woodMat;

                // Foliage (Pine Cone shape)
                GameObject foliage = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                foliage.transform.SetParent(tree.transform);
                foliage.transform.localPosition = new Vector3(0f, 6f, 0f);
                foliage.transform.localScale = new Vector3(4f, 3.5f, 4f);

                Material leafMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                leafMat.color = new Color(0.12f, 0.35f, 0.18f); // Deep Himalayan Pine Green
                foliage.GetComponent<Renderer>().sharedMaterial = leafMat;
            }
        }
    }
}
