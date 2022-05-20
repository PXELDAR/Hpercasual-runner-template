using System;
using UnityEngine;

namespace PXELDAR
{
    public class PlayerAnimationController : MonoBehaviour
    {
        //===================================================================================

        private Animator _animator;

        private readonly int _idleAnimation = Animator.StringToHash("Idle");
        private readonly int _walkAnimation = Animator.StringToHash("Walk");
        private readonly int _victoryAnimation = Animator.StringToHash("Victory");

        //===================================================================================

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (!_animator)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }

        //===================================================================================

        private void Start()
        {
            SetIdleAnimation();
        }

        //===================================================================================

        private void OnEnable()
        {
            InputController.OnFirstInput += OnFirstInput;
            LevelManager.Instance.controller.OnPlayerReachedEndOfSpline += OnPlayerReachedEndOfSpline;
            LevelManager.Instance.controller.OnLevelFailed += OnLevelFailed;
        }

        private void OnDisable()
        {
            InputController.OnFirstInput -= OnFirstInput;
            LevelManager.Instance.controller.OnPlayerReachedEndOfSpline -= OnPlayerReachedEndOfSpline;
            LevelManager.Instance.controller.OnLevelFailed -= OnLevelFailed;
        }

        //===================================================================================

        private void OnFirstInput()
        {
            SetWalkAnimation();
        }

        //===================================================================================

        private void OnPlayerReachedEndOfSpline()
        {
            SetVictoryAnimation();
        }

        //===================================================================================

        private void OnLevelFailed()
        {
            throw new NotImplementedException();
        }

        //===================================================================================

        private void OnFinishGame(bool b)
        {
            SetVictoryAnimation();
        }

        //===================================================================================

        private void SetIdleAnimation()
        {
            _animator.SetTrigger(_idleAnimation);
        }

        //===================================================================================

        private void SetWalkAnimation()
        {
            _animator.SetTrigger(_walkAnimation);
        }

        //===================================================================================

        private void SetVictoryAnimation()
        {
            _animator.SetTrigger(_victoryAnimation);
        }

        //===================================================================================

    }
}