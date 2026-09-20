using UnityEngine;
using HiddenNepal.Quest;

namespace HiddenNepal.World
{
    [RequireComponent(typeof(Collider))]
    public class DiscoveryZone : MonoBehaviour
    {
        [Header("Discovery Settings")]
        [SerializeField] private string locationName = "Ancient Himalayan Waterfall";
        [SerializeField] private int objectiveIndexToComplete = 1;

        private bool hasDiscovered = false;
        private float bannerTimer = 0f;

        private void OnTriggerEnter(Collider other)
        {
            if (hasDiscovered) return;

            if (other.CompareTag("Player") || other.GetComponent<HiddenNepal.Player.PlayerController>() != null)
            {
                hasDiscovered = true;
                bannerTimer = 4f;
                Debug.Log("📍 Discovered Location: " + locationName);

                if (QuestManager.Instance != null && objectiveIndexToComplete >= 0)
                {
                    QuestManager.Instance.CompleteObjective(objectiveIndexToComplete);
                }
            }
        }

        private void Update()
        {
            if (bannerTimer > 0f)
            {
                bannerTimer -= Time.deltaTime;
            }
        }

        private void OnGUI()
        {
            if (bannerTimer > 0f)
            {
                float width = 420f;
                float height = 50f;
                float posX = (Screen.width - width) / 2f;
                float posY = 80f;

                GUI.Box(new Rect(posX, posY, width, height), $"<size=18><b>📍 DISCOVERED: {locationName.ToUpper()}</b></size>");
            }
        }
    }
}
