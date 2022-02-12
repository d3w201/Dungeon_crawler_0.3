using Controller.Player;
using UnityEngine;

namespace Input
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInput : MonoBehaviour
    {
        public KeyCode forward = KeyCode.W;
        public KeyCode back = KeyCode.S;
        public KeyCode turnLeft = KeyCode.A;
        public KeyCode turnRight = KeyCode.D;

        private PlayerController _controller;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if(UnityEngine.Input.GetKeyUp(forward)) 
                _controller.MoveForward();
            if(UnityEngine.Input.GetKeyUp(back)) 
                _controller.MoveBackward();
            if(UnityEngine.Input.GetKeyUp(turnLeft)) 
                _controller.RotateLeft();
            if(UnityEngine.Input.GetKeyUp(turnRight)) 
                _controller.RotateRight();
        }
    }
}
