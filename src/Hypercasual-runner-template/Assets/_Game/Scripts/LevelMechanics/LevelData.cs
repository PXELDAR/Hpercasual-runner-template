using UnityEngine;

namespace PXELDAR
{
    public class LevelData : MonoBehaviour
    {
        //===================================================================================

        public int levelNumber = 0;
        public int stageNumber = 1;
        public int stagesCount = 3;

        public int levelSize = 1;
        public int themeNumber = 1;
        public int gamePrepareCount = 0;

        public bool isLevelFinished = false;

        public int score = 0;

        public float runDuration;
        public int reviveCount;
        public int coins;
        public int gems;
        public int magicBoxes;


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
            score = 0;

            stageNumber = 1;

            isLevelFinished = false;

            runDuration = 0;

            if (bIncreaseLevel)
            {
                // int nMaxFinishedLevel = PlayerStatsManager.Instance.GetMaxFinishedLevel();
                // SetCurrentLevel(++nMaxFinishedLevel);
            }
        }

        //===================================================================================

        public void SetCurrentLevel(int nVal)
        {
            levelNumber = nVal;
        }

        //===================================================================================

        public int GetCurrentLevel()
        {
            return levelNumber;
        }


        //===================================================================================
        //
        // UPDATE PLAYER STATS
        //
        //===================================================================================

        public void UpdatePlayerStats()
        {
            int nLastFinishedLevel = levelNumber;

            // register last finished level
            // PlayerStatsManager.Instance.SetLastFinishedLevel(nLastFinishedLevel);

            // register if max level
            // PlayerStatsManager.Instance.SetMaxFinishedLevel(nLastFinishedLevel);

            SetCurrentLevel(nLastFinishedLevel + 1);

            // register if highscore
            int nCurrentScore = score;
            // PlayerStatsManager.Instance.UpdateIfHighscore(nCurrentScore);
        }

        //===================================================================================
        //
        // CURRENTs INSTANCE EARNINGS
        //
        //===================================================================================

        public void SaveEarningsOfCurrentInstance()
        {
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
            return score;
        }

        //===================================================================================

        public void SetScore(int nValue)
        {
            score = nValue;
        }

        //===================================================================================

        public void IncreaseScore(int nIncOrDec = 1)
        {
            score += nIncOrDec;
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
            return coins;
        }

        //===================================================================================

        public void SetCoins(int nValue)
        {
            coins = nValue;
        }

        //===================================================================================

        public void IncreaseCoins(int nIncOrDec = 1)
        {
            coins += nIncOrDec;
        }

        //===================================================================================

        //===================================================================================
        //
        // MAGIC BOXES
        //
        //===================================================================================

        public int GetMagicBoxes()
        {
            return magicBoxes;
        }

        //===================================================================================

        public void SetMagicBoxes(int nValue)
        {
            magicBoxes = nValue;
        }

        //===================================================================================

        public void IncreaseMagicBoxes(int nIncOrDec = 1)
        {
            magicBoxes += nIncOrDec;
        }


        //===================================================================================
        //
        // KEYS
        //
        //===================================================================================

        public int GetGems()
        {
            return gems;
        }

        //===================================================================================

        public void SetGems(int nValue)
        {
            gems = nValue;
        }

        //===================================================================================

        public void IncreaseGems(int nIncOrDec = 1)
        {
            gems += nIncOrDec;
        }


        //===================================================================================
        //
        // REVIVE COUNT
        //
        //===================================================================================

        public int GetReviveCount()
        {
            return reviveCount;
        }

        //===================================================================================

        public void SetReviveCount(int nValue)
        {
            reviveCount = nValue;
        }

        //===================================================================================

        public void IncreaseReviveCount(int nIncOrDec = 1)
        {
            reviveCount += nIncOrDec;
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
                UpdatePlayerStats();
            }

            // PlayerStatsManager.Instance.IncreasePlayedGamesCount();

            // PlayerStatsManager.Instance.IncreaseTotalPlayDuration(fRunDuration);
        }

        //===================================================================================





    }
}