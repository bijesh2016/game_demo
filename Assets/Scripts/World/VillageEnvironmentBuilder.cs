using UnityEngine;
using HiddenNepal.NPC;
using HiddenNepal.Quest;
using HiddenNepal.Photography;
using HiddenNepal.SaveSystem;

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
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            CreateMaterialsIfNeeded();

            GameObject villageContainer = new GameObject("Sundari_Gaun_Village");
            villageContainer.transform.SetParent(transform);

            // Setup Managers
            SetupQuestAndCamera(villageContainer.transform);

            // 1. Village Houses
            CreateHouse(new Vector3(-12f, 0f, 10f), new Vector3(6f, 4f, 8f), "Gurung_House_1", villageContainer.transform);
            CreateHouse(new Vector3(12f, 0f, 12f), new Vector3(7f, 4.5f, 9f), "Sherpa_House_2", villageContainer.transform);
            CreateHouse(new Vector3(-14f, 0f, -8f), new Vector3(8f, 5f, 7f), "Village_Lodge", villageContainer.transform);
            CreateHouse(new Vector3(10f, 0f, -10f), new Vector3(6f, 4f, 6f), "Tea_House", villageContainer.transform);

            // 2. Central Stupa / Chorten
            CreateStupa(new Vector3(0f, 0f, 5f), villageContainer.transform);

            // 3. Pasang NPC
            CreatePasangNPC(new Vector3(0f, 0f, 1f), villageContainer.transform);

            // 4. Mani Wall
            CreateManiWall(new Vector3(0f, 0f, -15f), villageContainer.transform);

            // 5. Trail Signpost
            CreateSignpost(new Vector3(5f, 0f, -14f), "➔ Waterfall & Cave", villageContainer.transform);

            // 6. Mountain Trail
            CreateMountainTrail(villageContainer.transform);

            // 7. Waterfall Nook & Discovery Zone
            CreateWaterfallNook(new Vector3(25f, 0f, 35f), villageContainer.transform);

            // 8. Cave Entrance
            CreateCaveEntrance(new Vector3(-30f, 0f, -30f), villageContainer.transform);

            // 9. Forest Perimeter
            CreateForestPerimeter(villageContainer.transform);

            Debug.Log("🏔️ Sundari Gaun Village, Quests, Photography & Waterfall Discovery loaded!");
        }

        private void SetupQuestAndCamera(Transform parent)
        {
            if (FindObjectOfType<QuestManager>() == null)
            {
                GameObject questObj = new GameObject("QuestManager");
                questObj.transform.SetParent(parent);
                questObj.AddComponent<QuestManager>();
            }

            if (FindObjectOfType<SaveManager>() == null)
            {
                GameObject saveObj = new GameObject("SaveManager");
                saveObj.transform.SetParent(parent);
                saveObj.AddComponent<SaveManager>();
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                var pc = FindObjectOfType<HiddenNepal.Player.PlayerController>();
                if (pc != null) player = pc.gameObject;
            }

            if (player != null && player.GetComponent<CameraPhotography>() == null)
            {
                player.AddComponent<CameraPhotography>();
            }
        }

        private void CreatePasangNPC(Vector3 pos, Transform parent)
        {
            GameObject npcObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            npcObj.name = "NPC_Pasang";
            npcObj.transform.SetParent(parent);
            npcObj.transform.position = pos + new Vector3(0f, 1f, 0f);

            Material npcMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            npcMat.color = new Color(0.8f, 0.15f, 0.15f);
            npcObj.GetComponent<Renderer>().sharedMaterial = npcMat;

            if (npcObj.GetComponent<NPCController>() == null)
            {
                npcObj.AddComponent<NPCController>();
            }
        }

        private void CreateMaterialsIfNeeded()
        {
            if (houseWallMat == null)
            {
                houseWallMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                houseWallMat.color = new Color(0.72f, 0.52f, 0.38f);
            }
            if (houseRoofMat == null)
            {
                houseRoofMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                houseRoofMat.color = new Color(0.25f, 0.2f, 0.18f);
            }
            if (stupaMat == null)
            {
                stupaMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                stupaMat.color = new Color(0.95f, 0.95f, 0.92f);
            }
            if (waterMat == null)
            {
                waterMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                waterMat.color = new Color(0.15f, 0.55f, 0.85f, 0.8f);
            }
            if (rockMat == null)
            {
                rockMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                rockMat.color = new Color(0.42f, 0.45f, 0.46f);
            }
            if (woodMat == null)
            {
                woodMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                woodMat.color = new Color(0.4f, 0.26f, 0.13f);
            }
        }

        private void CreateHouse(Vector3 pos, Vector3 size, string houseName, Transform parent)
        {
            GameObject houseObj = new GameObject(houseName);
            houseObj.transform.SetParent(parent);
            houseObj.transform.position = pos;

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Walls";
            body.transform.SetParent(houseObj.transform);
            body.transform.localPosition = new Vector3(0f, size.y / 2f, 0f);
            body.transform.localScale = size;
            body.GetComponent<Renderer>().sharedMaterial = houseWallMat;

            GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roof.name = "Roof";
            roof.transform.SetParent(houseObj.transform);
            roof.transform.localPosition = new Vector3(0f, size.y + 0.5f, 0f);
            roof.transform.localScale = new Vector3(size.x + 1f, 0.8f, size.z + 1f);
            roof.GetComponent<Renderer>().sharedMaterial = houseRoofMat;

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

            GameObject baseBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            baseBlock.transform.SetParent(stupaObj.transform);
            baseBlock.transform.localPosition = new Vector3(0f, 0.75f, 0f);
            baseBlock.transform.localScale = new Vector3(6f, 1.5f, 6f);
            baseBlock.GetComponent<Renderer>().sharedMaterial = rockMat;

            GameObject dome = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dome.transform.SetParent(stupaObj.transform);
            dome.transform.localPosition = new Vector3(0f, 3f, 0f);
            dome.transform.localScale = new Vector3(4f, 3.5f, 4f);
            dome.GetComponent<Renderer>().sharedMaterial = stupaMat;

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

            GameObject cliff = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cliff.name = "Mountain_Cliff";
            cliff.transform.SetParent(waterfallObj.transform);
            cliff.transform.localPosition = new Vector3(0f, 10f, 0f);
            cliff.transform.localScale = new Vector3(20f, 20f, 6f);
            cliff.GetComponent<Renderer>().sharedMaterial = rockMat;

            GameObject waterColumn = GameObject.CreatePrimitive(PrimitiveType.Cube);
            waterColumn.name = "Waterfall_Stream";
            waterColumn.transform.SetParent(waterfallObj.transform);
            waterColumn.transform.localPosition = new Vector3(0f, 9f, -3.2f);
            waterColumn.transform.localScale = new Vector3(4f, 18f, 0.4f);
            waterColumn.GetComponent<Renderer>().sharedMaterial = waterMat;

            GameObject pool = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pool.name = "Waterfall_Pool";
            pool.transform.SetParent(waterfallObj.transform);
            pool.transform.localPosition = new Vector3(0f, 0.1f, -8f);
            pool.transform.localScale = new Vector3(12f, 0.2f, 12f);
            pool.GetComponent<Renderer>().sharedMaterial = waterMat;

            // Trigger Zone for Discovery
            GameObject triggerObj = new GameObject("Waterfall_Discovery_Trigger");
            triggerObj.transform.SetParent(waterfallObj.transform);
            triggerObj.transform.localPosition = new Vector3(0f, 1f, -8f);
            BoxCollider col = triggerObj.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(15f, 6f, 15f);
            triggerObj.AddComponent<DiscoveryZone>();
        }

        private void CreateCaveEntrance(Vector3 pos, Transform parent)
        {
            GameObject caveObj = new GameObject("Cave_Entrance");
            caveObj.transform.SetParent(parent);
            caveObj.transform.position = pos;

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

            int treeCount = 28;
            float radius = 38f;

            for (int i = 0; i < treeCount; i++)
            {
                float angle = (i / (float)treeCount) * Mathf.PI * 2f;
                Vector3 treePos = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

                GameObject tree = new GameObject($"Pine_Tree_{i}");
                tree.transform.SetParent(forestObj.transform);
                tree.transform.position = treePos;

                GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                trunk.transform.SetParent(tree.transform);
                trunk.transform.localPosition = new Vector3(0f, 2.5f, 0f);
                trunk.transform.localScale = new Vector3(0.8f, 2.5f, 0.8f);
                trunk.GetComponent<Renderer>().sharedMaterial = woodMat;

                GameObject foliage = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                foliage.transform.SetParent(tree.transform);
                foliage.transform.localPosition = new Vector3(0f, 6f, 0f);
                foliage.transform.localScale = new Vector3(4f, 3.5f, 4f);

                Material leafMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                leafMat.color = new Color(0.12f, 0.35f, 0.18f);
                foliage.GetComponent<Renderer>().sharedMaterial = leafMat;
            }
        }
    }
}
