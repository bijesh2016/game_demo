using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiddenNepal.Quest
{
    [Serializable]
    public class QuestObjective
    {
        public string description;
        public bool isCompleted;
    }

    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        [Header("Active Quest Status")]
        public string questTitle = "Secret of the Waterfall";
        [TextArea(2, 4)]
        public string questDescription = "Pasang, the Village Elder, spoke of an ancient hidden relic near the waterfall north-east of Sundari Gaun.";
        
        public List<QuestObjective> objectives = new List<QuestObjective>()
        {
            new QuestObjective { description = "Talk to Village Elder Pasang", isCompleted = true },
            new QuestObjective { description = "Follow the mountain trail north-east to the Waterfall", isCompleted = false },
            new QuestObjective { description = "Take a Photograph of the Ancient Waterfall Shrine [P]", isCompleted = false }
        };

        public bool isQuestCompleted { get; private set; } = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CompleteObjective(int index)
        {
            if (index >= 0 && index < objectives.Count)
            {
                objectives[index].isCompleted = true;
                CheckQuestCompletion();
            }
        }

        private void CheckQuestCompletion()
        {
            foreach (var obj in objectives)
            {
                if (!obj.isCompleted) return;
            }

            if (!isQuestCompleted)
            {
                isQuestCompleted = true;
                Debug.Log("🎉 QUEST COMPLETED: " + questTitle);
            }
        }

        private void OnGUI()
        {
            // Quest HUD Banner on Top-Right
            float width = 280f;
            float height = 110f;
            float posX = Screen.width - width - 20f;
            float posY = 20f;

            GUI.Box(new Rect(posX, posY, width, height), $"<b>📜 QUEST: {questTitle}</b>");

            for (int i = 0; i < objectives.Count; i++)
            {
                string status = objectives[i].isCompleted ? "<color=green>✓</color>" : "<color=yellow>○</color>";
                GUI.Label(new Rect(posX + 10f, posY + 28f + (i * 24f), width - 20f, 22f), $"{status} {objectives[i].description}");
            }
        }
    }
}
