using UnityEngine;

namespace PXELDAR
{
    public class PlayerCollisionController : MonoBehaviour
    {
        //===================================================================================

        private bool _canDetectCollision;

        private Transform _previousCollectible;
        private Transform _previousObstacle;
        private Transform _previousGate;
        private Transform _previousStair;

        //===================================================================================

        private void OnEnable()
        {
            InputController.OnFirstInput += OnFirstInput;
            GameMotor.Instance.OnFinishGame += OnFinishGame;
        }

        private void OnDisable()
        {
            InputController.OnFirstInput -= OnFirstInput;
            GameMotor.Instance.OnFinishGame -= OnFinishGame;
        }

        //===================================================================================

        private void OnFirstInput()
        {
            _canDetectCollision = true;
        }

        //==========================================================================

        private void OnFinishGame(bool b)
        {
            _canDetectCollision = false;
        }

        //===================================================================================

        private void OnTriggerEnter(Collider other)
        {
            if (!GameMotor.Instance.IsPlaying()) return;
            if (!_canDetectCollision) return;

            DetectTrigger(other);
        }

        //===================================================================================

        private void DetectTrigger(Collider other)
        {
            if (other)
            {
                if (other.gameObject.CompareTag(LevelManager.collidableTag))
                {
                    Transform triggeredTransform = other.transform;

                    ICollidable collidable = triggeredTransform.GetComponent<ICollidable>();
                    collidable?.OnCollision();

                    if (other.gameObject.layer == LevelManager.Instance.collectibleLayer)
                    {
                        if (!Equals(_previousCollectible, triggeredTransform))
                        {
                            HandleCollectibleTriggerFor(triggeredTransform);
                            return;
                        }
                    }

                    if (other.gameObject.layer == LevelManager.Instance.obstacleLayer)
                    {
                        if (!Equals(_previousObstacle, triggeredTransform))
                        {
                            HandleObstacleTriggerFor(triggeredTransform);
                            return;
                        }
                    }

                    if (other.gameObject.layer == LevelManager.Instance.gateLayer)
                    {
                        if (!Equals(_previousGate, triggeredTransform))
                        {
                            _previousGate = other.transform;

                            HandleGateTriggerFor(triggeredTransform);
                            return;
                        }
                    }

                    if (other.gameObject.layer == LevelManager.Instance.stairLayer)
                    {
                        if (!Equals(_previousStair, triggeredTransform))
                        {
                            _previousStair = other.transform;
                            //Stair Logic
                        }
                    }
                }
            }
        }

        //===================================================================================

        private void HandleCollectibleTriggerFor(Transform other)
        {
            if (other)
            {
                CollectibleController collectibleController = other.GetComponent<CollectibleController>();

                if (collectibleController)
                {
                    if (!collectibleController.IsCreatedViaDebugMenu())
                    {
                        LevelManager.Instance.controller.PlayerCollidedWithCollectible(collectibleController);
                        _previousCollectible = other;
                    }
                }
            }
        }

        //===================================================================================

        private void HandleObstacleTriggerFor(Transform other)
        {
            if (other)
            {
                ObstacleController obstacleController = other.GetComponent<ObstacleController>();

                if (obstacleController)
                {
                    LevelManager.Instance.controller.PlayerCollidedWithObstacle(obstacleController);

                    _previousObstacle = other;
                }
            }
        }

        //===================================================================================

        private void HandleGateTriggerFor(Transform other)
        {
            // if (other)
            // {
            //     GateController gateController = other.GetComponent<GateController>();

            //     if (gateController)
            //     {
            //         LevelManager.Instance.controller.PlayerCollidedWithGate(gateController);

            //         _previousGate = other;
            //     }
            // }
        }

        //===================================================================================
    }
}