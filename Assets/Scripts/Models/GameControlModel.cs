using System;

namespace Models
{
    public class GameControlModel
    {
        public event Action OnRestart;
        public void Restart()
        {
            OnRestart?.Invoke();
        }
    }
}