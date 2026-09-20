using UnityEngine;
using HiddenNepal.Quest;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace HiddenNepal.Photography
{
    public class CameraPhotography : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private KeyCode toggleCameraKey = KeyCode.P;
        [SerializeField] private Camera mainCam;

        private bool isViewfinderActive = false;
        private float flashTimer = 0f;

        private void Start()
        {
            if (mainCam == null) mainCam = Camera.main;
        }

        private void Update()
        {
            bool pPressed = false;
            bool spacePressed = false;

#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                pPressed = Keyboard.current.pKey.wasPressedThisFrame;
                spacePressed = Keyboard.current.spaceKey.wasPressedThisFrame || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);
            }
#else
            pPressed = Input.GetKeyDown(toggleCameraKey);
            spacePressed = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
#endif

            if (pPressed)
            {
                isViewfinderActive = !isViewfinderActive;
            }

            if (isViewfinderActive && spacePressed)
            {
                TakePhoto();
            }

            if (flashTimer > 0f)
            {
                flashTimer -= Time.deltaTime;
            }
        }

        private void TakePhoto()
        {
            flashTimer = 0.25f;
            Debug.Log("📸 Photo Captured!");

            // Complete Photograph quest objective
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.CompleteObjective(2);
            }
        }

        private void OnGUI()
        {
            if (flashTimer > 0f)
            {
                // White Flash Effect
                Color prevColor = GUI.color;
                GUI.color = Color.white;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
                GUI.color = prevColor;
                return;
            }

            if (isViewfinderActive)
            {
                // Draw Camera Viewfinder Frame
                float frameMargin = 60f;
                float w = Screen.width;
                float h = Screen.height;

                // Viewfinder Header
                GUI.Box(new Rect(0, 0, w, 40f), "<b>📷 CAMERA VIEWFINDER</b> | Press [SPACE] or Left-Click to Capture | Press [P] to Exit");

                // Crosshair Frame Lines
                float size = 40f;
                GUI.Box(new Rect(w / 2f - size / 2f, h / 2f - size / 2f, size, size), "+");
            }
            else
            {
                // Small Camera HUD Prompt at Bottom Right
                float promptWidth = 200f;
                float promptHeight = 30f;
                GUI.Box(new Rect(Screen.width - promptWidth - 20f, Screen.height - promptHeight - 20f, promptWidth, promptHeight), "Press <b>[P]</b> to Open Camera");
            }
        }
    }
}
