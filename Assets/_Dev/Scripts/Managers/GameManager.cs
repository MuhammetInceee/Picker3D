using System;
using Picker.Enums;
using Picker.Utilities;
using UnityEngine;


namespace Picker.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public GameState gameState { get; private set; } = GameState.Wait;

        public Transform playerFirstPos;

        public void ChangeGameState(GameState changedState)
        {
            gameState = changedState;
        }
    }
}
