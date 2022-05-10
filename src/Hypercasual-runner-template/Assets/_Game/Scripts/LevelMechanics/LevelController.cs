using UnityEngine;

namespace PXELDAR
{
    public class LevelController : MonoBehaviour
    {
        //===================================================================================

        //
        // Level
        //
        public delegate void OnLevelFailedDelegate();
        public event OnLevelFailedDelegate OnLevelFailed;

        public delegate void OnLevelCompletedDelegate();
        public event OnLevelCompletedDelegate OnLevelCompleted;

        public delegate void OnLevelDrawDelegate();
        public event OnLevelDrawDelegate OnLevelDraw;

        public delegate void OnStageCompletedDelegate(int nStage);
        public event OnStageCompletedDelegate OnStageCompleted;

        public delegate void OnStageChangedDelegate(int nStage);
        public event OnStageChangedDelegate OnStageChanged;

        public delegate void OnLevelChangedDelegate(int nLevel);
        public event OnLevelChangedDelegate OnLevelChanged;

        //
        // Generic
        //
        public delegate void OnLevelProgressValueChangedDelegate(float fMin, float fMax, float fVal);
        public event OnLevelProgressValueChangedDelegate OnLevelProgressValueChanged;

        public delegate void OnScoreValueChangedDelegate();
        public event OnScoreValueChangedDelegate OnScoreValueUpdated;

        public delegate void OnDiamondAmountChangedDelegate(int amount);
        public event OnDiamondAmountChangedDelegate OnDiamondAmountChanged;

        private bool isLevelEnded;
        private int diamondAmount;

        //
        // In Game
        //
        public delegate void OnLevelIsCreatedDelegate();
        public event OnLevelIsCreatedDelegate OnLevelIsCreated;

        public delegate void OnMoneyChangedDelegate(double newAmount = 0, double previousAmount = 0);
        public event OnMoneyChangedDelegate OnMoneyChanged;

        //
        // Player
        //
        // public delegate void OnPlayerCollidedWithObstacleDelegate(ObstacleController obstacle);
        // public event OnPlayerCollidedWithObstacleDelegate OnPlayerCollidedWithObstacle;

        // public delegate void OnPlayerCollidedWithCollectibleDelegate(CollectibleController collectible);
        // public event OnPlayerCollidedWithCollectibleDelegate OnPlayerCollidedWithCollectible;

        public delegate void OnPlayerStackChangedDelegate(int currentCount, int previousCount);
        public event OnPlayerStackChangedDelegate OnPlayerStackChanged;

        public delegate void OnPlayerStackLevelChangedDelegate(int currentLevel = 0, int previousLevel = 0);
        public event OnPlayerStackLevelChangedDelegate OnPlayerStackLevelChanged;

        public delegate void OnPlayerTitleChangedDelegate(string currentTitle);
        public event OnPlayerTitleChangedDelegate OnPlayerTitleChanged;

        public delegate void OnPlayerReachedEndOfSplineDelegate();
        public event OnPlayerReachedEndOfSplineDelegate OnPlayerReachedEndOfSpline;

        // public delegate void OnPlayerCollidedWithGateDelegate(GateController gate);
        // public event OnPlayerCollidedWithGateDelegate OnPlayerCollidedWithGate;

        //
        // Idle Section
        //
        public delegate void OnAwayProfitsCalculatedDelegate(double earnings);
        public event OnAwayProfitsCalculatedDelegate OnAwayProfitsCalculated;

        public delegate void OnTimeStampIsReadyDelegate();
        public event OnTimeStampIsReadyDelegate OnTimeStampIsReady;

        public delegate void OnIdleNewSessionTimeDifferenceCalculatedDelegate(int secondsGone);
        public event OnIdleNewSessionTimeDifferenceCalculatedDelegate OnIdleNewSessionTimeDifferenceCalculated;

        public delegate void OnHotelIsReadyDelegate();
        public event OnHotelIsReadyDelegate OnHotelIsReady;

        // public delegate void OnHotelRoomIsPurchasedDelegate(RoomController purchasedRoom);
        // public event OnHotelRoomIsPurchasedDelegate OnHotelRoomIsPurchased;

        // public delegate void OnIdleUnitIsPurchasedDelegate(IdleUnitController purchasedIdleUnit);
        // public event OnIdleUnitIsPurchasedDelegate OnIdleUnitIsPurchased;

        public delegate void OnHotelNavMeshIsBakedDelegate();
        public event OnHotelNavMeshIsBakedDelegate OnHotelNavMeshIsBaked;

        public delegate void OnMoneyBankIsCollectedDelegate();
        public event OnMoneyBankIsCollectedDelegate OnMoneyBankIsCollected;


        //===================================================================================

        private void OnEnable()
        {
            GameMotor.Instance.OnPrepareNewGame += OnPrepareNewGame;
        }

        //===================================================================================

        private void OnDisable()
        {
            GameMotor.Instance.OnPrepareNewGame -= OnPrepareNewGame;
        }

        //===================================================================================

        private void OnPrepareNewGame(bool bIsRematch = false)
        {
            diamondAmount = PlayerPrefs.GetInt("diamond");
            ChangeDiamondAmount(diamondAmount);

            isLevelEnded = false;
        }

        //===================================================================================

        public void FailLevel()
        {
            if (!isLevelEnded)
            {
                isLevelEnded = true;
                Debug.Log("<color='purple'>LevelEventsManager OnLevelFailed</color>");
                OnLevelFailed?.Invoke();
            }
        }

        //===================================================================================

        public void CompleteLevel()
        {
            if (!isLevelEnded)
            {
                PlayerPrefs.SetInt("diamond", diamondAmount);
                isLevelEnded = true;
                Debug.Log("<color='purple'>LevelEventsManager OnLevelCompleted</color>");
                OnLevelCompleted?.Invoke();
            }

        }

        //===================================================================================

        public void DrawLevel()
        {
            if (!isLevelEnded)
            {
                isLevelEnded = true;
                Debug.Log("<color='purple'>LevelEventsManager OnLevelDraw</color>");
                OnLevelDraw?.Invoke();
            }
        }

        //===================================================================================

        public void CompleteStage(int nStage)
        {
            Debug.Log("<color='purple'>LevelEventsManager OnStageCompleted: </color>" + nStage);

            OnStageCompleted?.Invoke(nStage);
        }

        //===================================================================================

        public void ChangeStage(int nStage)
        {
            Debug.Log("<color='purple'>LevelEventsManager OnStageChanged: </color>" + nStage);

            OnStageChanged?.Invoke(nStage);
        }

        //===================================================================================

        public void ChangeLevel(int nLevel)
        {
            Debug.Log("<color='purple'>LevelEventsManager OnLevelChanged: </color>" + nLevel);

            OnLevelChanged?.Invoke(nLevel);
        }

        //===================================================================================

        public void ChangeLevelProgressValue(float fMin, float fMax, float fVal)
        {
            //Debug.Log("<color='purple'>LevelProgressValueChanged</color> " + fMin + " " + fMax + " " + fVal);

            OnLevelProgressValueChanged?.Invoke(fMin, fMax, fVal);
        }

        //===================================================================================

        public void UpdateScoreValue()
        {
            //Debug.Log("<color='purple'>ScoreValueChanged</color>");

            OnScoreValueUpdated?.Invoke();
        }

        //===================================================================================

        public void DiamondIncreased(int amount = 1)
        {
            diamondAmount++;
            ChangeDiamondAmount(diamondAmount);
        }

        //===================================================================================

        private void ChangeDiamondAmount(int amount)
        {
            OnDiamondAmountChanged?.Invoke(amount);
        }

        //===================================================================================

        public void LevelIsCreated()
        {
            OnLevelIsCreated?.Invoke();
        }

        //===================================================================================

        public void PlayerReachedEndOfSpline()
        {
            OnPlayerReachedEndOfSpline?.Invoke();
        }

        // //===================================================================================

        // public void PlayerCollidedWithObstacle(ObstacleController obstacle)
        // {
        //     OnPlayerCollidedWithObstacle?.Invoke(obstacle);
        // }

        // //===================================================================================

        // public void PlayerCollidedWithCollectible(CollectibleController collectible)
        // {
        //     OnPlayerCollidedWithCollectible?.Invoke(collectible);
        // }

        //===================================================================================

        public void PlayerStackChanged(int currentCount, int previousCount)
        {
            OnPlayerStackChanged?.Invoke(currentCount, previousCount);
        }

        //===================================================================================

        public void PlayerStackLevelChanged(int level = 0, int previousLevel = 0)
        {
            OnPlayerStackLevelChanged?.Invoke(level, previousLevel);
        }

        //===================================================================================

        public void PlayerTitleChanged(string title)
        {
            OnPlayerTitleChanged?.Invoke(title);
        }

        //===================================================================================

        public void MoneyChanged(double newAmount = 0, double previousAmount = 0)
        {
            OnMoneyChanged?.Invoke(newAmount, previousAmount);
        }

        //===================================================================================

        public void AwayProfitsCalculated(double earnings)
        {
            OnAwayProfitsCalculated?.Invoke(earnings);
        }

        //===================================================================================

        public void TimeStampIsReady()
        {
            OnTimeStampIsReady?.Invoke();
        }

        //===================================================================================

        public void IdleNewSessionTimeDifferenceCalculated(int secondsGone)
        {
            OnIdleNewSessionTimeDifferenceCalculated?.Invoke(secondsGone);
        }

        //===================================================================================

        public void HotelIsReady()
        {
            OnHotelIsReady?.Invoke();
        }

        // //===================================================================================

        // public void HotelRoomIsPurchased(RoomController purchasedRoom)
        // {
        //     OnHotelRoomIsPurchased?.Invoke(purchasedRoom);
        // }

        // //===================================================================================

        // public void IdleUnitIsPurchased(IdleUnitController purchasedIdleUnit)
        // {
        //     OnIdleUnitIsPurchased?.Invoke(purchasedIdleUnit);
        // }

        //===================================================================================

        // public void PlayerCollidedWithGate(GateController gate)
        // {
        //     OnPlayerCollidedWithGate?.Invoke(gate);
        // }

        //===================================================================================

        public void HotelNavMeshIsBaked()
        {
            OnHotelNavMeshIsBaked?.Invoke();
        }

        //===================================================================================

        public void MoneyBankIsCollected()
        {
            OnMoneyBankIsCollected?.Invoke();
        }

        //===================================================================================

    }
}