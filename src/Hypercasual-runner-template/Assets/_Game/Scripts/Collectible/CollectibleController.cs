using UnityEngine;

namespace PXELDAR
{
    public class CollectibleController : MonoBehaviour, ICollidable
    {
        //===================================================================================

        private bool _isCreatedViaDebugMenu;
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

        public void SetCreationViaDebugMenu()
        {
            _isCreatedViaDebugMenu = true;
        }

        //===================================================================================

        public bool IsCreatedViaDebugMenu()
        {
            return _isCreatedViaDebugMenu;
        }

        //===================================================================================

    }
}