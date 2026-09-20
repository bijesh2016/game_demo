using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using HiddenNepal.Quest;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace HiddenNepal.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public string playerName = "Explorer";
        public Vector3 playerPosition;
        public Vector3 playerRotation;
        public bool isQuestCompleted;
        public List<bool> objectiveStates = new List<bool>();
        public string saveTimestamp;
    }

    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private string SaveFilePath => Path.Combine(Application.persistentDataPath, "savegame.json");

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            bool f5Pressed = false;
            bool f9Pressed = false;

#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                f5Pressed = Keyboard.current.f5Key.wasPressedThisFrame;
                f9Pressed = Keyboard.current.f9Key.wasPressedThisFrame;
            }
#else
            f5Pressed = Input.GetKeyDown(KeyCode.F5);
            f9Pressed = Input.GetKeyDown(KeyCode.F9);
#endif

            if (f5Pressed) SaveGame();
            if (f9Pressed) LoadGame();
        }

        public void SaveGame()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                var pc = FindObjectOfType<HiddenNepal.Player.PlayerController>();
                if (pc != null) player = pc.gameObject;
            }

            SaveData data = new SaveData();
            data.saveTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (player != null)
            {
                data.playerPosition = player.transform.position;
                data.playerRotation = player.transform.eulerAngles;
            }

            if (QuestManager.Instance != null)
            {
                data.isQuestCompleted = QuestManager.Instance.isQuestCompleted;
                foreach (var obj in QuestManager.Instance.objectives)
                {
                    data.objectiveStates.Add(obj.isCompleted);
                }
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SaveFilePath, json);
            Debug.Log($"💾 Game Saved successfully to {SaveFilePath} at {data.saveTimestamp}");
        }

        public void LoadGame()
        {
            if (!File.Exists(SaveFilePath))
            {
                Debug.LogWarning("⚠️ No Save File found!");
                return;
            }

            string json = File.ReadAllText(SaveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                var pc = FindObjectOfType<HiddenNepal.Player.PlayerController>();
                if (pc != null) player = pc.gameObject;
            }

            if (player != null)
            {
                var charController = player.GetComponent<CharacterController>();
                if (charController != null) charController.enabled = false;

                player.transform.position = data.playerPosition;
                player.transform.eulerAngles = data.playerRotation;

                if (charController != null) charController.enabled = true;
            }

            if (QuestManager.Instance != null && data.objectiveStates != null)
            {
                for (int i = 0; i < data.objectiveStates.Count; i++)
                {
                    if (data.objectiveStates[i])
                    {
                        QuestManager.Instance.CompleteObjective(i);
                    }
                }
            }

            Debug.Log($"📂 Game Loaded successfully from {data.saveTimestamp}!");
        }

        private void OnGUI()
        {
            float width = 220f;
            float height = 25f;
            GUI.Label(new Rect(20f, Screen.height - height - 10f, width, height), "<color=white><b>F5:</b> Quick Save | <b>F9:</b> Quick Load</color>");
        }
    }
}
