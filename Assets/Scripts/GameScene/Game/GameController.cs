using UnityEngine;

namespace GameScene.Game
{
    public class GameController : MonoBehaviour
    {
        public enum EGameState
        {
            Idle,
            Playing,
            Pausing,
            Ending,
        }

        private EGameState _gameState;
        
        public static float GameTimer;
        public EGameState GameState => _gameState;

        private void Start()
        {
            _gameState = EGameState.Playing;
        }

        private void Update()
        {
            if (_gameState != EGameState.Playing)
            {
                return;
            }
            GameTimer += Time.deltaTime;
        }
    }
}