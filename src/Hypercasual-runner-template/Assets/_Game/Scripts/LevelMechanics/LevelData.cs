using UnityEngine;

namespace PXELDAR
{
    public class LevelData : MonoBehaviour
    {
        //===================================================================================

        public int nLevelNumber = 0;
        public int nStageNumber = 1;
        public int nStagesCount = 3;

        public int nLevelSize = 1;
        public int nThemeNumber = 1;
        public int nGamePrepareCount = 0;

        public bool bIsLevelFinished = false;

        public int nScore = 0;

        public float fRunDuration;
        public int nReviveCount;
        public int nCoins;
        public int nGems;
        public int nMagicBoxes;



        //===================================================================================

        void Awake()
        {
            ResetValues(true);
        }

        //===================================================================================

        void OnEnable()
        {
            GameMotor.Instance.OnPrepareNewGame += OnPrepareNewGame;
            GameMotor.Instance.OnStartGame += OnStartGame;
            GameMotor.Instance.OnFinishGame += OnFinishGame;
        }

        //===================================================================================

        void OnDisable()
        {
            GameMotor.Instance.OnPrepareNewGame -= OnPrepareNewGame;
            GameMotor.Instance.OnStartGame -= OnStartGame;
            GameMotor.Instance.OnFinishGame -= OnFinishGame;
        }

        //===================================================================================

        void OnPrepareNewGame()
        {
            ResetValues();
        }

        //===================================================================================

        void ResetValues(bool bIncreaseLevel = false)
        {
            nScore = 0;

            nStageNumber = 1;

            bIsLevelFinished = false;

            fRunDuration = 0;

            if (bIncreaseLevel)
            {
                // int nMaxFinishedLevel = PlayerStatsManager.Instance.GetMaxFinishedLevel();
                // SetCurrentLevel(++nMaxFinishedLevel);
            }
        }

        //===================================================================================

        public void SetCurrentLevel(int nVal)
        {
            nLevelNumber = nVal;
        }

        //===================================================================================

        public int GetCurrentLevel()
        {
            return nLevelNumber;
        }


        //===================================================================================
        //
        // UPDATE PLAYER STATS
        //
        //===================================================================================

        public void UpdatePlayerStats()
        {
            Debug.Log("GameInstanceData: UpdatePlayerStats");

            int nLastFinishedLevel = nLevelNumber;

            // register last finished level
            // PlayerStatsManager.Instance.SetLastFinishedLevel(nLastFinishedLevel);

            // register if max level
            // PlayerStatsManager.Instance.SetMaxFinishedLevel(nLastFinishedLevel);

            SetCurrentLevel(nLastFinishedLevel + 1);

            // register if highscore
            int nCurrentScore = nScore;
            // PlayerStatsManager.Instance.UpdateIfHighscore(nCurrentScore);
        }

        //===================================================================================
        //
        // CURRENTs INSTANCE EARNINGS
        //
        //===================================================================================

        public void SaveEarningsOfCurrentInstance()
        {
            Debug.Log("GameInstanceData: SaveEarningsOfCurrentInstance");

            // save coins
            // InventoryManager.Instance.IncreaseCoinsCount(nCoins);

            // save gems
            // InventoryManager.Instance.IncreaseGemsCount(nGems);
        }


        //===================================================================================
        //
        // SCORE
        //
        //===================================================================================

        public int GetScore()
        {
            return nScore;
        }

        //===================================================================================

        public void SetScore(int nValue)
        {
            nScore = nValue;
        }

        //===================================================================================

        public void IncreaseScore(int nIncOrDec = 1)
        {
            nScore += nIncOrDec;
        }

        //===================================================================================

        public bool IsHighscore()
        {
            int nCurrentScore = GetScore();
            // bool bRet = PlayerStatsManager.Instance.IsHighscore(nCurrentScore);

            // return bRet;

            return false;
        }


        //===================================================================================


        //===================================================================================
        //
        // COINS
        //
        //===================================================================================

        public int GetCoins()
        {
            return nCoins;
        }

        //===================================================================================

        public void SetCoins(int nValue)
        {
            nCoins = nValue;
        }

        //===================================================================================

        public void IncreaseCoins(int nIncOrDec = 1)
        {
            nCoins += nIncOrDec;
        }

        //===================================================================================

        //===================================================================================
        //
        // MAGIC BOXES
        //
        //===================================================================================

        public int GetMagicBoxes()
        {
            return nMagicBoxes;
        }

        //===================================================================================

        public void SetMagicBoxes(int nValue)
        {
            nMagicBoxes = nValue;
        }

        //===================================================================================

        public void IncreaseMagicBoxes(int nIncOrDec = 1)
        {
            nMagicBoxes += nIncOrDec;
        }


        //===================================================================================
        //
        // KEYS
        //
        //===================================================================================

        public int GetGems()
        {
            return nGems;
        }

        //===================================================================================

        public void SetGems(int nValue)
        {
            nGems = nValue;
        }

        //===================================================================================

        public void IncreaseGems(int nIncOrDec = 1)
        {
            nGems += nIncOrDec;
        }


        //===================================================================================
        //
        // REVIVE COUNT
        //
        //===================================================================================

        public int GetReviveCount()
        {
            return nReviveCount;
        }

        //===================================================================================

        public void SetReviveCount(int nValue)
        {
            nReviveCount = nValue;
        }

        //===================================================================================

        public void IncreaseReviveCount(int nIncOrDec = 1)
        {
            nReviveCount += nIncOrDec;
        }

        //===================================================================================


        //===================================================================================
        //
        // EVENTS TO IMPLEMENT
        //
        //===================================================================================

        void OnPrepareNewGame(bool bIsRematch = false)
        {
            ResetValues(!bIsRematch);
        }

        //===================================================================================


        void OnStartGame()
        {
        }

        //===================================================================================

        void OnFinishGame(bool bWin = true)
        {
            if (bWin)
            {
                Debug.Log("UpdatePlayerStats level:" + GetCurrentLevel());
                UpdatePlayerStats();
            }

            // PlayerStatsManager.Instance.IncreasePlayedGamesCount();

            // PlayerStatsManager.Instance.IncreaseTotalPlayDuration(fRunDuration);
        }

        //===================================================================================





    }
}