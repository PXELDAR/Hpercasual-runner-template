using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PXELDAR
{
    public class LevelManager : Singleton<LevelManager>
    {
        //===================================================================================

        public const string collidableTag = "Collidable";
        private const string layerCollectible = "Collectible";
        private const string layerObstacle = "Obstacle";
        private const string layerGate = "Gate";
        private const string layerStair = "Stair";

        public int collectibleLayer;
        public int obstacleLayer;
        public int gateLayer;
        public int stairLayer;

        //===================================================================================

        public delegate void OnThemeSetDelegate();
        public event OnThemeSetDelegate OnThemeSet;

        const string strMainColor = "_Color";
        const string strSkyboxTintColor = "_TintColor";

        //===================================================================================

        public LevelController controller { get; private set; }
        public LevelData datas { get; private set; }
        public LevelCreator creator { get; private set; }


        [Header("Level Object Holders")]
        public Transform environmentHolder;
        public Transform platformHolder;
        public Transform playerHolder;
        public Transform chunkHolder;
        public Transform collectiblePersonHolder;
        public Transform suitCaseHolder;
        public Transform poolHolder;
        public Transform temporaryHolder;

        [Header("Level Objects")]
        public Transform trGround;

        [Space]

        [Header("Level Theme")]
        public LevelTheme[] aLevelThemes;

        [HideInInspector]
        public LevelTheme chosenlevelTheme;
        [HideInInspector]
        public int[] collectibleColorIndexes;
        public Material[] aGroundMaterials;
        public Material skyboxMaterial, obstacleWithSameColorInAllSceneMaterial;

        private int level;

        public Camera _camera;

        private bool _initializedTimeController;

        //===================================================================================

        private void Awake()
        {
            controller = gameObject.AddComponent<LevelController>();
            datas = gameObject.AddComponent<LevelData>();
            creator = GetComponent<LevelCreator>();

            if (!creator)
            {
                Debug.LogError("Assign level creator component to: ", this);
            }

            collectibleLayer = LayerMask.NameToLayer(layerCollectible);
            obstacleLayer = LayerMask.NameToLayer(layerObstacle);
            gateLayer = LayerMask.NameToLayer(layerGate);
            stairLayer = LayerMask.NameToLayer(layerStair);

            _camera = Camera.main;
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
            level = datas.GetCurrentLevel();

            PrepareLevel();
        }

        //===================================================================================

        private void OnLevelFailed()
        {
            datas.bIsLevelFinished = true;
            datas.SetScore(0);
            controller.UpdateScoreValue();
            StartCoroutine(PlayFailAnimation());
            GameMotor.Instance.FinishGame(false);
        }

        //===================================================================================

        private void OnLevelCompleted()
        {
            datas.bIsLevelFinished = true;
            controller.UpdateScoreValue();
            datas.SaveEarningsOfCurrentInstance();
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
            DestroyAllChildrenIn(collectiblePersonHolder);
            DestroyAllChildrenIn(suitCaseHolder);
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
            if (aLevelThemes == null)
            {
                return;
            }

            int nLevelThemesCount = aLevelThemes.Length;
            int nRandIndex = Random.Range(0, nLevelThemesCount);
            chosenlevelTheme = (aLevelThemes[nRandIndex] != null) ? aLevelThemes[nRandIndex] : aLevelThemes[0];

            collectibleColorIndexes = ExtensionMethods.FillStartingFromToCount(0, chosenlevelTheme.acolCollectibleMainColors.Length);   //sıralı liste doldur
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

            int nLevelThemeSkyColorsCount = chosenlevelTheme.acolSkyboxColors.Length;
            int nRandIndex = Random.Range(0, nLevelThemeSkyColorsCount);
            if (level == 1)
            {
                nRandIndex = 1;
            }
            Color32 colCurrentSkybox = chosenlevelTheme.acolSkyboxColors[nRandIndex];

            nRandIndex = Random.Range(0, chosenlevelTheme.acolGroundColors.Length);
            foreach (Material groundMaterial in aGroundMaterials)
            {
                groundMaterial.SetColor(strMainColor, chosenlevelTheme.acolGroundColors[nRandIndex]);
            }

            obstacleWithSameColorInAllSceneMaterial.SetColor(strMainColor, chosenlevelTheme.acolObstacleColors[Random.Range(0, chosenlevelTheme.acolObstacleColors.Length)]);
            skyboxMaterial.SetColor(strSkyboxTintColor, colCurrentSkybox);
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