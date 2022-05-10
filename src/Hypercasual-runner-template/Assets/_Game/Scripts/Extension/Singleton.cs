using UnityEngine;

namespace PXELDAR
{
    [RequireComponent(typeof(Initialization))]
    public abstract class Singleton<T> : MonoBehaviour
    {
        //===================================================================================

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Singleton not registered! Make sure the GameObject running your singleton is active in your scene and has an Initialization component attached.");
                    return default(T);
                }
                return _instance;
            }
        }

        [SerializeField] bool _dontDestroyOnLoad = false;
        static T _instance;

        //===================================================================================

        /// <summary>
        /// Override this method to have code run when this singleton is initialized which is guaranteed to run before Awake and Start.
        /// </summary>
        protected virtual void OnRegistration()
        {
        }

        //===================================================================================

        /// <summary>
        /// Generic method that registers the singleton instance.
        /// </summary>
        public void RegisterSingleton(T instance)
        {
            _instance = instance;
        }

        //===================================================================================

        protected void Initialize(T instance)
        {
            if (_dontDestroyOnLoad && _instance != null)
            {
                //there is already an instance:
                Destroy(gameObject);
                return;
            }

            if (_dontDestroyOnLoad)
            {
                //don't destroy on load only works on root objects:
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }

            _instance = instance;
            OnRegistration();
        }

        //===================================================================================
    }
}