using UnityEngine;

namespace Controller.Npc
{
    public class NpcController : MonoBehaviour
    {
        public float transitionSpeed = 10f;

        private Vector3 _targetGridPos;
    
        private void Start()
        {
            _targetGridPos =  transform.position;
            InvokeRepeating("Patrol", 1, 1);
        }

        private void FixedUpdate()
        {
            MoveNpc();
        }

        //TODO:This function have to be called when player move
        private void MoveNpc()
        {
            var targetPosition = _targetGridPos;
            transform.position =
                Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
        }

        private int c = 0;
        private int cc = 0;
    
        void Patrol() {
            if (c < 3)
            {
                _targetGridPos -= transform.right;
                c++;
            }
            else
            {
                _targetGridPos += transform.right;
                cc++;
                if (cc < 3) return;
                c = 0; cc = 0;

            }
        }
    }
}
