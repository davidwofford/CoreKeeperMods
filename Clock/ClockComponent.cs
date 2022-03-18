using System;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Type = Il2CppSystem.Type;

namespace Clock
{
    public class ClockComponent : MonoBehaviour
    {
        public static GameObject gameObj = null;
        public static ClockComponent instance;
        public static Text textComponent;

        private static GameObject canvas = null;
        private static GameObject uiPanel = null;
        private static string _formatString;
        private static bool initialized = false;

        public ClockComponent(IntPtr ptr) : base(ptr)
        {
            instance = this;
        }

        private static void Initialize()
        {
            instance.createUI();

            initialized = true;
        }

        internal static GameObject Create()
        {
            gameObj = new GameObject("ClockObject");
            DontDestroyOnLoad(gameObj);

            var component = new ClockComponent(gameObj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<ClockComponent>()).Pointer);

            _formatString = "HH:mm";
            if (BepInExLoader.TwelveHourFormat.Value)
            {
                _formatString = "hh:mm tt";
            }

            return gameObj;
        }

        public void Update()
        {
            if (!initialized) { Initialize(); }

            string timeString = DateTime.Now.ToString(_formatString);
            if (textComponent.text != timeString)
            {
                BepInExLoader.log.LogInfo("Updating text string");
                textComponent.text = timeString;
                textComponent.enabled = true;
                textComponent.color = Color.white;
            }
        }

        private void createUI()
        {
            if (canvas == null)
            {
                canvas = instance.createCanvas();
                DontDestroyOnLoad(canvas);

                uiPanel = instance.createUIPanel(canvas, "500", "900");

                GameObject uiText = instance.createUIText(uiPanel);
                uiText.GetComponent<RectTransform>().localPosition = new Vector3(-1125, 630, 0);

                BepInExLoader.log.LogInfo("Created canvas");
            }

            canvas.SetActive(true);
        }

        private GameObject createCanvas()
        {
            GameObject canvasObj = new GameObject("ClockCanvasObject");
            DontDestroyOnLoad(canvasObj);

            Canvas canvas = new Canvas(canvasObj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<Canvas>()).Pointer);

            canvas.renderMode = RenderMode.ScreenSpaceCamera;

            CanvasScaler scalar = new CanvasScaler(canvasObj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<CanvasScaler>()).Pointer);

            scalar.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            scalar.referencePixelsPerUnit = 100f;
            scalar.referenceResolution = new Vector2(1024f, 788f);

            GraphicRaycaster raycaster = new GraphicRaycaster(canvasObj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<GraphicRaycaster>()).Pointer);

            return canvasObj;
        }

        public GameObject createUIPanel(GameObject canvas, Il2CppSystem.String height, Il2CppSystem.String width)
        {
            GameObject uiPanel = instance.createPanel();
            uiPanel.transform.SetParent(canvas.transform, false);

            RectTransform rectTransform = uiPanel.GetComponent<RectTransform>();

            float size;
            size = Il2CppSystem.Single.Parse(height);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            size = Il2CppSystem.Single.Parse(width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);

            return uiPanel;
        }

        public GameObject createPanel()
        {
            GameObject panel = instance.createUIRootElement("ClockPanel");

            RectTransform component = panel.GetComponent<RectTransform>();
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.anchoredPosition = Vector2.zero;
            component.sizeDelta = Vector2.zero;

            /*Image image = panel.AddComponent<Image>();
            image.color = Color.white;*/

            return panel;
        }

        public GameObject createUIRootElement(string name)
        {
            GameObject uiElement = new GameObject(name);
            RectTransform rectTransform = uiElement.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160f, 50f);

            return uiElement;
        }

        public GameObject createUIText(GameObject parent)
        {
            GameObject uiText = instance.createText();
            uiText.transform.SetParent(parent.transform, false);

            LayoutElement layoutElement = uiText.GetComponent<LayoutElement>();
            if (!(bool)(UnityEngine.Object)layoutElement)
            {
                layoutElement = uiText.AddComponent<LayoutElement>();
            }

            layoutElement.minWidth = 50;
            layoutElement.minHeight = 25;

            return uiText;
        }

        public GameObject createText()
        {
            GameObject uiText = instance.createUIRootElement("ClockText");
            textComponent = uiText.AddComponent<Text>();
            textComponent.text = "12:00";
            textComponent.color = Color.black;
            textComponent.alignment = TextAnchor.UpperLeft;
            textComponent.fontSize = 35;
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");;
            textComponent.verticalOverflow = VerticalWrapMode.Overflow;

            return uiText;
        }
    }
}