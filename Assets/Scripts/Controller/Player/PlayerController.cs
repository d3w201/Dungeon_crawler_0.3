using Controller.Dungeon;
using Entity;
using UnityEngine;

namespace Controller.Player
{
    public class PlayerController : RootController
    {
        public bool smoothTransition = false;
        public float transitionSpeed = 10f;
        public float transitionRotationSpeed = 500f;

        [SerializeField] private Vector3 _targetGridPos;
        private Vector3 _prevTargetGridPos;
        [SerializeField] private Vector3 _targetRotation;
        [SerializeField] private GameObject dc;

        private void Start()
        {
            _targetGridPos = Vector3Int.RoundToInt(transform.position);
        }

        private void FixedUpdate()
        {
            if (!GameStatus.Play.Equals(GameController.GetStatus())) return;
            MovePlayer();
        }

        private void MovePlayer()
        {
            var targetTile = GameController.GetTile(new Vector2Int(
                Vector3Int.RoundToInt(_targetGridPos).x,
                Vector3Int.RoundToInt(_targetGridPos).z)
            );

            if (targetTile.Walkable)
            {
                _targetGridPos = Vector3Int.RoundToInt(_targetGridPos);
                _prevTargetGridPos = _targetGridPos;
                var targetPosition = _targetGridPos;

                if (_targetRotation.y > 270f && _targetRotation.y < 361f)
                    _targetRotation.y = 0f;
                if (_targetRotation.y < 0f)
                    _targetRotation.y = 270f;
                if (!smoothTransition)
                {
                    transform.position = targetPosition;
                    transform.rotation = Quaternion.Euler(_targetRotation);
                }
                else
                {
                    transform.position =
                        Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_targetRotation),
                        Time.deltaTime * transitionRotationSpeed);
                }
            }
            else
            {
                _targetGridPos = _prevTargetGridPos;
            }
        }

        public void RotateLeft()
        {
            if (AtRest())
                _targetRotation -= Vector3.up * 90f;
        }

        public void RotateRight()
        {
            if (AtRest())
                _targetRotation += Vector3.up * 90f;
        }

        public void MoveForward()
        {
            if (AtRest())
                _targetGridPos += transform.forward;
        }

        public void MoveBackward()
        {
            if (AtRest())
                _targetGridPos -= transform.forward;
        }

        public void MoveLeft()
        {
            if (AtRest())
                _targetGridPos -= transform.right;
        }

        public void MoveRight()
        {
            if (AtRest())
                _targetGridPos += transform.right;
        }

        private bool AtRest()
        {
            return Vector3.Distance(transform.position, _targetGridPos) < 0.05f &&
                   Vector3.Distance(transform.eulerAngles, _targetRotation) < 0.05f;
        }
    }
}