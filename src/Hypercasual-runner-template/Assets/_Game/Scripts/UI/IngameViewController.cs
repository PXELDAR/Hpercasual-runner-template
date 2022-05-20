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
        [SerializeField] private TextMeshProUGUI _moneyText;
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
            GameMotor.Instance.OnStartGame += OnStartGame;
            GameMotor.Instance.OnPrepareNewGame += OnPrepareNewGame;
            LevelManager.Instance.controller.OnLevelIsCreated += OnLevelIsCreated;
            LevelManager.Instance.controller.OnMoneyChanged += OnMoneyChanged;
        }

        private void OnDisable()
        {
            GameMotor.Instance.OnStartGame -= OnStartGame;
            GameMotor.Instance.OnPrepareNewGame -= OnPrepareNewGame;
            LevelManager.Instance.controller.OnLevelIsCreated -= OnLevelIsCreated;
            LevelManager.Instance.controller.OnMoneyChanged -= OnMoneyChanged;
        }

        //===================================================================================

        private void OnStartGame()
        {
            //bool showControlsHelpers = LevelManager.Instance.data.GetCurrentLevel() <= 3;

            StartCoroutine(ShowControlsHelpers(true));
        }

        //===================================================================================

        private void OnPrepareNewGame(bool isRematch)
        {
            UpdateLevelLabels();
        }

        //===================================================================================

        private void OnLevelIsCreated()
        {

        }

        //===================================================================================

        private void OnMoneyChanged(double newAmount, double previousAmount)
        {
            if (_moneyIncreaseTween != null)
            {
                if (_moneyIncreaseTween.IsPlaying())
                {
                    _moneyIncreaseTween.Kill();
                }
            }

            _moneyIncreaseTween = DOVirtual.Float((float)previousAmount, (float)newAmount, 1f, OnMoneyValueChanged);

            double difference = newAmount - previousAmount;
            string operative = string.Empty;

            if (difference > 0)
            {
                operative = _plus;
            }

            if (operative != "")
            {
                _moneyIncreaseText.text = operative + difference;
            }
            else
            {
                _moneyIncreaseText.text = difference.ToString();
            }

            MoveMoneyIncreaseText();
        }

        //===================================================================================

        private void OnMoneyValueChanged(float value)
        {
            if (_moneyText)
            {
                _moneyText.SetText(value.ToString("F0"));
            }
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
            int currentLevel = LevelManager.Instance.data.GetCurrentLevel();

            if (_levelText)
            {
                _levelText.SetText(_level + " " + currentLevel);
            }
        }

        //===================================================================================

        private void MoveMoneyIncreaseText()
        {
            _moneyIncreaseText.rectTransform
                .DOAnchorPos(new Vector2(35, -25), 0);

            _moneyIncreaseText
                .DOFade(1, 0).SetEase(Ease.InQuint);

            _moneyIncreaseText.rectTransform
                .DOAnchorPos(new Vector2(75, -75), 3)
                .SetEase(Ease.OutQuint);

            _moneyIncreaseText
                .DOFade(0, 1.5f).SetEase(Ease.InQuint);
        }

        //===================================================================================

    }
}