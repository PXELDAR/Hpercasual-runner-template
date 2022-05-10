namespace PXELDAR
{
    public class PlayerController : Singleton<PlayerController>
    {
        //===================================================================================

        public PlayerMovementController movementController { get; private set; }
        public PlayerCollisionController collisionController { get; private set; }
        public PlayerAnimationController animationController { get; private set; }
        public PlayerStackController stackController { get; private set; }

        private const string _lvlUpParticle = "LvlUpParticle";

        //===================================================================================

        private void Awake()
        {
            movementController = GetComponent<PlayerMovementController>();
            collisionController = GetComponent<PlayerCollisionController>();
            stackController = GetComponent<PlayerStackController>();

            animationController = GetComponent<PlayerAnimationController>()
                                  ?? GetComponentInChildren<PlayerAnimationController>();
        }

        //===================================================================================
    }
}