using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

    public class CurrencySystem : MonoBehaviour
    {
        private static Dictionary<CurrencyType, int> CurrencyAmounts = new Dictionary<CurrencyType, int>();

        [SerializeField] private List<GameObject> texts;

        private Dictionary<CurrencyType, TextMeshProUGUI>
        currencyTexts = new Dictionary<CurrencyType, TextMeshProUGUI>();

        private void Awake()
        {
            for (int i = 0; i < texts.Count; i++)
            {
                CurrencyAmounts.Add((CurrencyType)i, 0);
                currencyTexts.Add((CurrencyType)i, texts[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
            }
        }

        private void Start()
        {
            EventManager.Instance.AddListener<GameEvent.CurrencyChangeGameEvent>(OnCurrencyChange);
            EventManager.Instance.AddListener<GameEvent.NotEnoughCurrencyGameEvent>(OnNotEnough);
            
        }

        private void OnCurrencyChange(GameEvent.CurrencyChangeGameEvent info)
        {
           
            CurrencyAmounts[info.currencyType] += info.Amount;
            currencyTexts[info.currencyType].text = CurrencyAmounts[info.currencyType].ToString();
        }

        private void OnNotEnough(GameEvent.NotEnoughCurrencyGameEvent info)
        {
            Debug.Log($"Yeteri kadar {info.Amount} {info.currencyType} yok");
        }
    }


    public enum CurrencyType
    {
        Coins,
        Diamonds

    }


