using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PXELDAR
{
    public class PlayerStackController : MonoBehaviour
    {
        //===================================================================================

        [SerializeField][Range(1, 10)] private int _requiredStackForLevel;
        public int requiredStackForLevel => _requiredStackForLevel;

        [SerializeField][Range(-2f, 2f)] private float _segmentHeight = 0.3f;
        [SerializeField][Range(0, 1f)] private float _segmentsDistanceFactor = 0.1f;
        [SerializeField][Range(0, 100f)] private float _segmentFollowSpeed = 30;
        [SerializeField][Range(0, 1f)] private float _collectedFeedbackScaleMultiplier = 0.5f;
        [SerializeField][Range(0, 1f)] private float _collectedFeebackDuration = 0.5f;

        private List<CollectibleController> _stackedCollectibleList;
        private const int _startingLevel = 1;
        private const int _minLevel = 1;
        private const int _maxLevel = 5;
        private const string _collectible = "Collectible";
        private const string _popParticle = "PopParticle";
        private int _stackCount => _stackedCollectibleList?.Count ?? 0;
        private int _stackLevel;

        //===================================================================================

        private void Awake()
        {
            _stackedCollectibleList = new List<CollectibleController>();
            // _feedbackList = new List<MMFeedbacks>();
        }

        //===================================================================================

        private void OnEnable()
        {
            LevelManager.Instance.controller.OnPlayerCollidedWithCollectible += OnPlayerCollidedWithCollectible;
            LevelManager.Instance.controller.OnPlayerCollidedWithObstacle += OnPlayerCollidedWithObstacle;
            // LevelManager.Instance.controller.OnPlayerCollidedWithGate += OnPlayerCollidedWithGate;
        }

        private void OnDisable()
        {
            LevelManager.Instance.controller.OnPlayerCollidedWithCollectible -= OnPlayerCollidedWithCollectible;
            LevelManager.Instance.controller.OnPlayerCollidedWithObstacle -= OnPlayerCollidedWithObstacle;
            // LevelManager.Instance.controller.OnPlayerCollidedWithGate -= OnPlayerCollidedWithGate;
        }

        //===================================================================================

        private void Start()
        {
            SetStackLevel(_startingLevel);
        }

        //===================================================================================

        private void Update()
        {
            if (_stackedCollectibleList.Count > 0)
            {
                _stackedCollectibleList[0].transform.LookAt(PlayerController.Instance.transform.position);
                _stackedCollectibleList[0].transform.position = Vector3.Lerp(
                    _stackedCollectibleList[0].transform.position,
                    PlayerController.Instance.transform.position + PlayerController.Instance.transform.forward,
                    Time.deltaTime * _segmentFollowSpeed);
                _stackedCollectibleList[0].transform.forward = PlayerController.Instance.transform.forward;

                for (int i = 1; i < _stackedCollectibleList.Count; i++)
                {
                    if (Vector3.Magnitude(
                        _stackedCollectibleList[i].transform.position - _stackedCollectibleList[i - 1].transform.position) > _segmentHeight * _segmentsDistanceFactor)
                    {
                        _stackedCollectibleList[i].transform.LookAt(_stackedCollectibleList[i - 1].transform.position);
                        _stackedCollectibleList[i].transform.position = Vector3.Lerp(
                            _stackedCollectibleList[i].transform.position,
                            _stackedCollectibleList[i - 1].transform.position - _stackedCollectibleList[i].transform.forward * _segmentHeight,
                            Time.deltaTime * _segmentFollowSpeed);
                    }
                }
            }
        }

        //===================================================================================

        private void OnPlayerCollidedWithCollectible(CollectibleController collectible)
        {
            AddStack(collectible);
        }

        //===================================================================================

        private void OnPlayerCollidedWithObstacle(ObstacleController obstacle)
        {
            LoseStack(1);
        }

        //===================================================================================

        // private void OnPlayerCollidedWithGate(GateController gate)
        // {
        //     if (gate.gateType == GateType.Positive)
        //     {
        //         AddStack(1);
        //     }
        //     else
        //     {
        //         LoseStack(1);
        //     }
        // }

        //===================================================================================

        private void AddStack(CollectibleController collectible, bool isGate = false, bool isCreatedViaDebugMenu = false)
        {
            if (collectible)
            {
                int previousStackCount = _stackCount;
                _stackedCollectibleList.Add(collectible);
                LevelManager.Instance.controller.PlayerStackChanged(_stackCount, previousStackCount);
                CheckStackLevelState();

                if (!isCreatedViaDebugMenu)
                {
                    StartCoroutine(DoCollectedFeedback());
                }
            }
        }

        public void AddStack(int count)
        {
            for (int stack = 0; stack < count; stack++)
            {
                GameObject newCollectible = PoolingManager.Instance.GetPooledObject(_collectible);

                if (newCollectible)
                {
                    newCollectible.SetActive(true);
                    CollectibleController controller = newCollectible.GetComponent<CollectibleController>();

                    if (controller)
                    {
                        AddStack(controller, true, true);
                        controller.SetCreationViaDebugMenu();
                    }
                }
            }
        }

        //===================================================================================

        private void LoseStack(CollectibleController collectible, ObstacleController obstacleController = null)
        {
            if (collectible)
            {
                if (_stackCount > 0)
                {
                    int previousStackCount = _stackCount;
                    _stackedCollectibleList.Remove(collectible);
                    // _feedbackList.Remove(collectible.feedback);
                    LevelManager.Instance.controller.PlayerStackChanged(_stackCount, previousStackCount);
                    CheckStackLevelState();
                }
            }
        }

        public void LoseStack(int count, ObstacleController obstacleController = null)
        {
            if (_stackCount > 0)
            {
                if (_stackCount > count)
                {
                    for (int i = 0; i < count; i++)
                    {
                        //If de-stacking happening from the -LAST-
                        CollectibleController controller = _stackedCollectibleList[_stackCount - 1];
                        LoseStack(controller, obstacleController);

                        //If de-stacking happening from the -FIRST-
                        //CollectibleController controller = _stackedCollectibleList[0];
                        //LoseStack(controller);
                    }
                }
                else
                {
                    for (int i = _stackCount; i > 0; i--)
                    {
                        CollectibleController controller = _stackedCollectibleList[0];
                        LoseStack(controller, obstacleController);
                    }
                }
            }
            else
            {
                LevelManager.Instance.controller.FailLevel();
            }
        }

        //===================================================================================

        private void PerformLostStackMovement(CollectibleController controller)
        {
            //Lost stack movement
        }

        //===================================================================================

        public void IncreaseStackLevel()
        {
            //Possible Level Increase Effects Here
            //

            int currentLevel = _stackLevel;
            int newLevel = currentLevel + 1;
            SetStackLevel(newLevel, currentLevel);
        }

        //===================================================================================

        public void DecreaseStackLevel()
        {
            //Possible Level Decrease Effects Here
            //

            int currentLevel = _stackLevel;
            int newLevel = currentLevel - 1;
            SetStackLevel(newLevel, currentLevel);
        }

        //===================================================================================

        private void SetStackLevel(int newLevel = 0, int previousLevel = 0)
        {
            _stackLevel = newLevel;
            _stackLevel = Mathf.Clamp(_stackLevel, _minLevel, _maxLevel);
            LevelManager.Instance.controller.PlayerStackLevelChanged(_stackLevel, previousLevel);
        }

        //===================================================================================

        private void CheckStackLevelState()
        {
            if (_stackCount > 0)
            {
                int newCurrentLevel = _stackCount / _requiredStackForLevel;
                newCurrentLevel = Mathf.FloorToInt(newCurrentLevel);
                int currentLevel = _stackLevel;

                //Add 1 to newCurrentLevel because stack count starts at 0 so newCurrentLevel starts from 0
                //while player level starts at 1
                if (newCurrentLevel + 1 > _stackLevel)
                {
                    IncreaseStackLevel();
                }
                else if (newCurrentLevel + 1 < _stackLevel)
                {
                    DecreaseStackLevel();
                }
            }
        }

        //===================================================================================

        private IEnumerator DoCollectedFeedback()
        {
            foreach (CollectibleController collectible in _stackedCollectibleList)
            {
                Transform body = collectible.transform.GetChild(0);
                DOTween.Kill(body);
                body.localScale = Vector3.one;
                body
                .DOPunchScale(Vector3.one * _collectedFeedbackScaleMultiplier, _collectedFeebackDuration, 0, 0)
                .SetEase(Ease.InSine);
                yield return new WaitForSeconds(0.05f);
            }
        }

        //===================================================================================
    }
}