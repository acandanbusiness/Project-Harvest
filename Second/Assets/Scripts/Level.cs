using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private int level;
    public float xp;
    [SerializeField] private float xpTarget;
    [SerializeField] private Image XPProgressBar;
    public TextMeshProUGUI coinText;
    public int coin;

    private void Start()
    {
        LoadGameData();
        UpdateUI();
    }

    public void Add(int _coin, float _xp)
    {
        coin += _coin;
        xp += _xp;
        UpdateUI();
        experiencecontroller();
    }

    public void CoinAdd(int coin)
    {
        coin += coin;
        UpdateUI();
        experiencecontroller();
    }


    public void XPAdd(int xp)
    {
        xp += 500;
        UpdateUI();
        experiencecontroller();
    }

    private void experiencecontroller()
    {
        levelText.text = level.ToString();
        XPProgressBar.fillAmount = (xp / xpTarget);

        if (xp >= xpTarget)
        {
            level++;
            xp = xp - xpTarget;

            if (level < 5)
            {
                xpTarget = xpTarget * 2;
            }
            else
            {
                xpTarget += xpTarget / 10;
            }
        }
        SaveGameData();
    }

    public void UpdateUI()
    {
        experienceText.text = xp + " / " + xpTarget;
        coinText.text = coin.ToString();
    }

    private void SaveGameData()
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetFloat("CurrentXP", xp);
        PlayerPrefs.SetFloat("TargetXP", xpTarget);
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.Save();
    }

    private void LoadGameData()
    {
        level = PlayerPrefs.GetInt("Level", 1);
        xp = PlayerPrefs.GetFloat("CurrentXP", 0f);
        xpTarget = PlayerPrefs.GetFloat("TargetXP", 1000f);
        coin = PlayerPrefs.GetInt("Coin", 0);
    }

    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
        level = 1;
        xp = 0;
        xpTarget = 1000f;
        coin = 0;
        UpdateUI();
        SaveGameData();
    }


}