using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PXELDAR
{
    public class LevelManager : Singleton<LevelManager>
    {
        //===================================================================================

        public int collectibleLayer { get; private set; }
        public int obstacleLayer { get; private set; }
        public int gateLayer { get; private set; }
        public int stairLayer { get; private set; }
        public const string collidableTag = "Collidable";

        private const string _layerCollectible = "Collectible";
        private const string _layerObstacle = "Obstacle";
        private const string _layerGate = "Gate";
        private const string _layerStair = "Stair";

        //===================================================================================

        public delegate void OnThemeSetDelegate();
        public event OnThemeSetDelegate OnThemeSet;

        private const string _mainColor = "_Color";
        private const string _skyboxTintColor = "_TintColor";

        //===================================================================================

        public LevelController controller { get; private set; }
        public LevelData data { get; private set; }
        public LevelCreator creator { get; private set; }


        [Header("Level Object Holders")]
        public Transform environmentHolder;
        public Transform platformHolder;
        public Transform playerHolder;
        public Transform chunkHolder;
        public Transform poolHolder;
        public Transform temporaryHolder;

        [Space]

        [Header("Level Theme")]
        public LevelTheme[] levelThemes;

        public LevelTheme chosenlevelTheme { get; private set; }
        public int[] collectibleColorIndexes { get; private set; }
        public Material[] groundMaterials;
        public Material skyboxMaterial, obstacleWithSameColorInAllSceneMaterial;

        private int _level;

        public Camera _camera;

        private bool _initializedTimeController;

        //===================================================================================

        private void Awake()
        {
            controller = gameObject.AddComponent<LevelController>();
            data = gameObject.AddComponent<LevelData>();
            creator = GetComponent<LevelCreator>();

            collectibleLayer = LayerMask.NameToLayer(_layerCollectible);
            obstacleLayer = LayerMask.NameToLayer(_layerObstacle);
            gateLayer = LayerMask.NameToLayer(_layerGate);
            stairLayer = LayerMask.NameToLayer(_layerStair);
            _camera = Camera.main;

#if UNITY_EDITOR
            if (!creator)
            {
                Debug.LogError("Assign level creator component to: ", this);
            }
#endif
        }

        //===================================================================================

        private void OnEnable()
        {
            GameMotor.Instance.OnPrepareNewGame += OnPrepareNewGame;
            GameMotor.Instance.OnFinishGame += OnFinishGame;

            controller.OnLevelFailed += OnLevelFailed;
            controller.OnLevelCompleted += OnLevelCompleted;
            controller.OnLevelDraw += OnLevelDraw;
        }

        //===================================================================================

        private void OnDisable()
        {
            GameMotor.Instance.OnPrepareNewGame -= OnPrepareNewGame;
            GameMotor.Instance.OnFinishGame -= OnFinishGame;

            controller.OnLevelFailed -= OnLevelFailed;
            controller.OnLevelCompleted -= OnLevelCompleted;
            controller.OnLevelDraw -= OnLevelDraw;
        }

        //===================================================================================

        private void Start()
        {
            GameMotor.Instance.SetState(GameState.INITIALIZED);
            GameMotor.Instance.StartGameInstantly();
        }

        //===================================================================================

        private void OnFinishGame(bool bWin = true)
        {
            controller.UpdateScoreValue();

            // Possibility of sending some analitycs
        }

        //===================================================================================

        private void OnPrepareNewGame(bool bIsRematch = false)
        {
            _level = data.GetCurrentLevel();

            PrepareLevel();
        }

        //===================================================================================

        private void OnLevelFailed()
        {
            controller.UpdateScoreValue();
            StartCoroutine(PlayFailAnimation());
            GameMotor.Instance.FinishGame(false);
        }

        //===================================================================================

        private void OnLevelCompleted()
        {
            controller.UpdateScoreValue();
            GameMotor.Instance.FinishGame();
        }

        //===================================================================================

        private void OnLevelDraw()
        {
            GameMotor.Instance.FinishGame();
        }

        //===================================================================================

        private void PrepareLevel()
        {
            ClearPreviousLevel();
            CreateLevelObjects();
        }

        //===================================================================================

        private void ClearPreviousLevel()
        {
            ClearPreviousLevelObjects();
        }

        //===================================================================================

        private void ClearPreviousLevelObjects()
        {
            PoolingManager.Instance.ReleaseAllPooledObjects();

            DestroyAllChildrenIn(environmentHolder);
            DestroyAllChildrenIn(platformHolder);
            DestroyAllChildrenIn(playerHolder);
            DestroyAllChildrenIn(chunkHolder);
            DestroyAllChildrenIn(temporaryHolder);
        }

        //===================================================================================

        private void DestroyAllChildrenIn(Transform holder)
        {
            if (holder)
            {
                Transform[] children = holder.GetComponentsInChildren<Transform>(true);

                foreach (Transform child in children)
                {
                    if (child.parent == holder)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }

        //===================================================================================

        private void CreateLevelObjects()
        {
            SetRandomLevelTheme();
            creator.GenerateNewLevel();
        }

        //===================================================================================

        private void SetRandomLevelTheme()
        {
            if (levelThemes == null)
            {
                return;
            }

            int levelThemesCount = levelThemes.Length;
            int randIndex = Random.Range(0, levelThemesCount);
            chosenlevelTheme = (levelThemes[randIndex] != null) ? levelThemes[randIndex] : levelThemes[0];

            collectibleColorIndexes = ExtensionMethods.FillStartingFromToCount(0, chosenlevelTheme.collectibleMainColors.Length);
            collectibleColorIndexes.Shuffle<int>();

            SetRandomLevelThemeGround();
        }

        //===================================================================================

        private void SetRandomLevelThemeGround()
        {
            if (chosenlevelTheme == null)
            {
                return;
            }

            int levelThemeSkyColorsCount = chosenlevelTheme.skyboxColors.Length;
            int randomIndex = Random.Range(0, levelThemeSkyColorsCount);
            if (_level == 1)
            {
                randomIndex = 1;
            }
            Color32 colCurrentSkybox = chosenlevelTheme.skyboxColors[randomIndex];

            randomIndex = Random.Range(0, chosenlevelTheme.groundColors.Length);
            foreach (Material groundMaterial in groundMaterials)
            {
                groundMaterial.SetColor(_mainColor, chosenlevelTheme.groundColors[randomIndex]);
            }

            obstacleWithSameColorInAllSceneMaterial.SetColor(_mainColor, chosenlevelTheme.obstacleColors[Random.Range(0, chosenlevelTheme.obstacleColors.Length)]);
            skyboxMaterial.SetColor(_skyboxTintColor, colCurrentSkybox);
            RenderSettings.fogColor = colCurrentSkybox;

            OnThemeSet?.Invoke();
        }


        //===================================================================================

        private IEnumerator PlayFailAnimation()
        {
            CameraController.Instance.ShakeCamera(0, 0.5f, 3.0f, 100, 100);

            yield return null;
        }

        //===================================================================================

    }
}