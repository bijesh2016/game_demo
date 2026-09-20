using UnityEngine;
using HiddenNepal.Interaction;

namespace HiddenNepal.NPC
{
    public class NPCController : MonoBehaviour, IInteractable
    {
        [Header("NPC Info")]
        [SerializeField] private string npcName = "Pasang (Village Elder)";
        [TextArea(3, 5)]
        [SerializeField] private string[] dialogueLines = new string[]
        {
            "Tashi Delek, traveller! Welcome to Sundari Gaun.",
            "Our village has been quiet, but rumors say an ancient relic lies hidden near the waterfall north-east of here.",
            "Take your camera, follow the mountain trail, and see what secrets you can discover!"
        };

        private int currentLineIndex = 0;
        private bool isTalking = false;

        public string GetInteractPrompt()
        {
            return $"Talk to {npcName} [E]";
        }

        public void Interact(GameObject instigator)
        {
            if (!isTalking)
            {
                isTalking = true;
                currentLineIndex = 0;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex >= dialogueLines.Length)
                {
                    isTalking = false;
                    currentLineIndex = 0;
                }
            }
        }

        private void OnGUI()
        {
            if (isTalking)
            {
                // Draw Dialogue Box
                float boxWidth = Screen.width * 0.7f;
                float boxHeight = 120f;
                float posX = (Screen.width - boxWidth) / 2f;
                float posY = Screen.height - boxHeight - 40f;

                GUI.Box(new Rect(posX, posY, boxWidth, boxHeight), $"<b>{npcName}</b>\n\n{dialogueLines[currentLineIndex]}\n\n<i>[Press E to continue]</i>");
            }
            else
            {
                // Show Prompt when looking at NPC
                var player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    var interaction = player.GetComponent<PlayerInteraction>();
                    if (interaction != null && interaction.GetCurrentInteractable() == (IInteractable)this)
                    {
                        float promptWidth = 240f;
                        float promptHeight = 35f;
                        GUI.Box(new Rect((Screen.width - promptWidth) / 2f, Screen.height / 2f + 40f, promptWidth, promptHeight), GetInteractPrompt());
                    }
                }
            }
        }
    }
}
