using System;

public class PlayerEvents
{
    // Player Health events
    public event Action<float> OnPlayerHealthChanged;

    public void PlayerHealthChanged(float healthGained)
    {
        OnPlayerHealthChanged?.Invoke(healthGained);
    }

    // Game Over Event
    public event Action OnGameOver;
    public void GameOver()
    {
        OnGameOver?.Invoke();
    }

    internal void ClearAll()
    {
        OnGameOver = null;
        OnPlayerHealthChanged = null;
    }
}
