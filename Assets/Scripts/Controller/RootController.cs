using Controller.Game;
using UnityEngine;

namespace Controller
{
    public class RootController : MonoBehaviour
    {
        //Game-Objects
        protected GameObject GameManager;
        
        //Components
        protected GameController GameController;

        //Awake
        private void Awake()
        {
            if (!GameManager)
            {
                GameManager = GameObject.FindGameObjectWithTag("GameManager");
            }
            
            //Components
            GameController = GameManager.GetComponent<GameController>();
        }
    }
}