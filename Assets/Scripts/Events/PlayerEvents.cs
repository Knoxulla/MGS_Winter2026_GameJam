using System;

public class PlayerEvents
{
    // Player Health events
    public event Action<float> OnPlayerHealthChanged;

    public void PlayerHealthChanged(float healthGained)
    {
        OnPlayerHealthChanged?.Invoke(healthGained);
    }

        // Player EXP events
    public event Action<float> OnPlayerExpChanged;

    public void PlayerExpChanged(float expGained)
    {
        OnPlayerExpChanged?.Invoke(expGained);
    }

    public event Action<float> OnPlayerCurrencyChanged;
    public void PlayerCurrencyChanged(float currenyGained)
    {
        OnPlayerCurrencyChanged?.Invoke(currenyGained);
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
