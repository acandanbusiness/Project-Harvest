using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


    public class LevelSystem : MonoBehaviour
    {
        private int xpNow;
        private int Level;
        private int xpToNext;

        [SerializeField] private GameObject levelPanel;
        [SerializeField] private GameObject lvlWindowPrefab;

        private Slider slider;
        private TextMeshProUGUI xpText;
        private TextMeshProUGUI lvlText;
        private Image starImage;

        private static bool initialized;
        private static Dictionary<int, int> xpToNextLevel = new Dictionary<int, int>();
        private static Dictionary<int, int[]> lvlReward = new Dictionary<int, int[]>();



        private void Awake()
        {
            slider = levelPanel.GetComponent<Slider>();
            xpText = levelPanel.transform.Find("XP text").GetComponent<TextMeshProUGUI>();
            starImage = levelPanel.transform.Find("Star").GetComponent<Image>();

            if (!initialized)
            {
                Initialize();
            }

            xpToNextLevel.TryGetValue(Level, out xpToNext);
        }


        private static void Initialize()
        {
            try
            {
                string path = "levelsXP";

                TextAsset textAsset = Resources.Load<TextAsset>(path);
                string[] lines = textAsset.text.Split('\n');

                xpToNextLevel = new Dictionary<int, int>(lines.Length - 1);

                for (int i = 1; i < lines.Length - 1; i++)
                {
                    string[] columns = lines[i].Split(',');

                    int lvl = -1;
                    int xp = -1;
                    int curr1 = -1;
                    int curr2 = -1;

                    int.TryParse(columns[0], out lvl);
                    int.TryParse(columns[1], out xp);
                    int.TryParse(columns[2], out curr1);
                    int.TryParse(columns[3], out curr2);

                    if (lvl >= 0 && xp > 0)
                    {
                        if (!xpToNextLevel.ContainsKey(lvl))
                        {
                            xpToNextLevel.Add(lvl, xp);
                            lvlReward.Add(lvl, new[] { curr1, curr2 });
                        }
                    }

                }
            }
            catch (UnityException ex)
            {
                Debug.Log(ex.Message);
            }

            initialized = true;

        }

        private void Start()
        {
            EventManager.Instance.AddListener<GameEvent.XPAddedGameEvent>(OnXPAdded);
            EventManager.Instance.AddListener<GameEvent.LevelChangedGameEvent>(OnLevelChanged);

            UpdateUI();

        }

        private void UpdateUI()
        {
            float fill = (float)xpNow / xpToNext;
            slider.value = fill;
            xpText.text = xpNow + "/" + xpToNext;
        }

        private void OnXPAdded(GameEvent.XPAddedGameEvent info)
        {
            xpNow += info.Amount;

            UpdateUI();

            if (xpNow >= xpToNext)
            {
                Level++;
            GameEvent.LevelChangedGameEvent levelChange = new GameEvent.LevelChangedGameEvent(Level);
                EventManager.Instance.QueueEvent(levelChange);
            }
        }

        private void OnLevelChanged(GameEvent.LevelChangedGameEvent info)
        {
            xpNow -= xpToNext;
            xpToNext = xpToNextLevel[info.NewLvl];
            lvlText.text = (info.NewLvl + 1).ToString();
            UpdateUI();

            GameObject window = Instantiate(lvlWindowPrefab, GameManager.current.canvas.transform);

            window.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Destroy(window); });

        GameEvent.CurrencyChangeGameEvent currencyInfo =
                new GameEvent.CurrencyChangeGameEvent(lvlReward[info.NewLvl][0], CurrencyType.Coins);
            EventManager.Instance.QueueEvent(currencyInfo);

            currencyInfo =
                new GameEvent.CurrencyChangeGameEvent(lvlReward[info.NewLvl][1], CurrencyType.Coins);
            EventManager.Instance.QueueEvent(currencyInfo);

        }
    }


