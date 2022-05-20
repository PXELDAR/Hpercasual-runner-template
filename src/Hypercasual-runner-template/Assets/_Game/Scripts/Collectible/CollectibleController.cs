using UnityEngine;
using DG.Tweening;

namespace PXELDAR
{
    public class CollectibleController : MonoBehaviour, ICollidable
    {
        //===================================================================================

        [SerializeField][Range(0, 10)] private int _moneyValue;
        [SerializeField][Range(0, 3.0f)] private float _scaleFeedbackValue;
        [SerializeField][Range(0, 1.5f)] private float _feedbackTime;

        //===================================================================================

        public void OnCollision()
        {
            SendMoneyInfo();
        }

        //===================================================================================

        private void SendMoneyInfo()
        {
            LevelManager.Instance.data.IncreaseLevelMoney(_moneyValue);
        }

        //===================================================================================

    }
}