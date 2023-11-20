using System;
using Picker.Enums;
using Picker.Utilities;


namespace Picker.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public GameState gameState { get; private set; } = GameState.Wait;

        public void ChangeGameState(GameState changedState)
        {
            gameState = changedState;
        }
    }
}
