using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public GameObject canvas;


    private void Awake()
    {
        current = this;
    }

    public void GetXP(int amount)
    {
        GameEvent.XPAddedGameEvent info = new GameEvent.XPAddedGameEvent(amount);
        EventManager.Instance.QueueEvent(info);
    }

    public void GetCoins(int amount)
    {
        GameEvent.CurrencyChangeGameEvent info = new GameEvent.CurrencyChangeGameEvent(amount, CurrencyType.Coins);
        EventManager.Instance.QueueEvent(info);
    }
}
