using UnityEngine;

namespace ARMagicBar.Resources.Scripts.Debugging
{
    [ExecuteAlways]
    public class CustomLog : MonoBehaviour
    {
        [SerializeField] private CustomLogLevel LogLevel;

        public static CustomLog Instance;

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
            else if (Instance != this)
            {
                if (Application.isPlaying)
                {
                    DestroyImmediate(this.gameObject);
                }
            }
        }

        public static void EnsureInstance()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<CustomLog>();
                if (Instance == null)
                {
                    GameObject customLogObject = new GameObject("CustomLog");
                    Instance = customLogObject.AddComponent<CustomLog>();
                    Instance.LogLevel = CustomLogLevel.none; // Default log level
                }
            }
        }

        public void InfoLog(string message)
        {
            EnsureInstance();
            if (Instance.LogLevel != CustomLogLevel.info && Instance.LogLevel != CustomLogLevel.all || Instance.LogLevel == CustomLogLevel.none) return;
            Debug.Log(message);
        }

        public void ErrorLog(string message)
        {
            EnsureInstance();
            if (Instance.LogLevel != CustomLogLevel.error && Instance.LogLevel != CustomLogLevel.all || Instance.LogLevel == CustomLogLevel.none) return;
            Debug.LogError(message);
        }
    }

    public enum CustomLogLevel
    {
        none,
        info,
        error,
        all
    }
}