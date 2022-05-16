using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace PXELDAR
{
    public class IngameViewController : MonoBehaviour
    {
        //===================================================================================

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _moneyIncreaseText;
        [SerializeField] private GameObject _controlHelper;
        [SerializeField] private RectTransform _controlHelperHand;

        private Tweener _moneyIncreaseTween;
        private Tweener _controlHelperTween;
        private WaitForSeconds _controlHelperShowTime = new WaitForSeconds(4);

        private const float _helperHandRightPos = 200f;
        private const float _helperHandLeftPos = -200f;
        private const float _helperHandMoveTime = 1.3f;
        private const Ease _helperEase = Ease.InOutSine;

        private const string _level = "LEVEL";
        private const string _plus = "+";
        private const string _minus = "-";

        //===================================================================================

        private void OnEnable()
        {
            GameMotor.Instance.OnPrepareNewGame += OnPrepareNewGame;
            GameMotor.Instance.OnStartGame += OnStartGame;
        }

        private void OnDisable()
        {
            GameMotor.Instance.OnPrepareNewGame -= OnPrepareNewGame;
            GameMotor.Instance.OnStartGame -= OnStartGame;
        }

        //===================================================================================

        private void OnStartGame()
        {
            //bool showControlsHelpers = LevelManager.Instance.data.GetCurrentLevel() <= 3;

            bool showControlHelpers = true;
            StartCoroutine(ShowControlsHelpers(showControlHelpers));
        }

        //===================================================================================

        private void OnPrepareNewGame(bool isRematch)
        {
            UpdateLevelLabels();
        }

        //===================================================================================

        private IEnumerator ShowControlsHelpers(bool show = true)
        {
            if (_controlHelper)
            {
                if (show)
                {
                    _controlHelper.SetActive(true);

                    _controlHelperHand
                    .DOAnchorPosX(_helperHandRightPos, _helperHandMoveTime)
                    .SetEase(_helperEase)
                    .OnComplete(() =>
                    {
                        _controlHelperHand
                        .DOAnchorPosX(_helperHandLeftPos, _helperHandMoveTime)
                        .SetEase(_helperEase)
                        .OnComplete(() =>
                        {
                            _controlHelperHand
                            .DOAnchorPosX(_helperHandRightPos, _helperHandMoveTime)
                            .SetEase(_helperEase);
                        });
                    });


                    yield return _controlHelperShowTime;
                }

                _controlHelper.SetActive(false);
            }
        }

        //===================================================================================

        private void UpdateLevelLabels()
        {
            int currentLevel = LevelManager.Instance.data.levelNumber;

            if (_levelText)
            {
                _levelText.SetText(_level + string.Empty + currentLevel);
            }
        }

        //===================================================================================


    }
}