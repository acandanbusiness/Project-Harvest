using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FieldController : MonoBehaviour
{
    private enum FieldStage { 
        IlkEkim= 1,
        Buyume1 = 2,
        Buyume2 = 2,
        HasataHazir = 10
    }

    [Serializable]
    public class Fields
    {
        [SerializeField] public string name;
        [SerializeField] public string displayName;
        [SerializeField] public float buyumeSuresi = 30;
        [SerializeField] public int coin = 100;
        [SerializeField] public float xp = 10;
        [SerializeField] public List<FieldsStage> stage = new();
    }

    [Serializable]
    public class FieldsStage
    {
        [SerializeField] FieldStage stage;
        [SerializeField] public Texture texture;
    }

    [SerializeField] public Fields currentField;
    private Button itemButton;

    private bool isGrowing = false;
    [SerializeField] private float growthTime;

    public Button harvestButton;
    public TextMeshProUGUI countdownText;
    public GameObject remainingTimePanel; 


    [SerializeField] List<Fields> fieldList = new();


    private void Start()
    {
        float butonX = 900;
        foreach (var field in fieldList) {
            GameObject go = Instantiate(this.transform.parent.Find("Field Settings Canvas/Plant Settings/SampleButton").gameObject, this.transform.parent.Find("Field Settings Canvas/Plant Settings").transform);
            go.SetActive(true);
            go.GetComponentInChildren<TMP_Text>().text = field.displayName;
            Vector3 pos = go.GetComponentInChildren<Button>().transform.position;
            pos.x = butonX;
            butonX += 191;
            go.GetComponentInChildren<Button>().transform.position = pos;
            go.GetComponentInChildren<Button>().onClick.AddListener(() => {
                Debug.Log(field.name);
                itemButton = go.GetComponentInChildren<Button>();
                currentField = field;
                PlantCrop();
            });
            go.name = field.name;
        }
    }

    private void Update()
    {
        if (isGrowing) UpdateGrowthTime();
    }

    private void UpdateGrowthTime()
    {
        if (growthTime > 1f)
        {
            growthTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(growthTime / 60);
            int seconds = Mathf.FloorToInt(growthTime % 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            isGrowing = false;
            harvestButton.gameObject.SetActive(true);
            harvestButton.interactable = true;
        }
    }

    private void PlantCrop()
    {
        if (!isGrowing)
        {
            isGrowing = true;
            harvestButton.interactable = false;
            harvestButton.transform.parent.gameObject.SetActive(true);
            growthTime = currentField.buyumeSuresi;

            ActivateCountdownPanel();
        }
        else {
            harvestButton.transform.parent.gameObject.SetActive(true);
        }
    }

    private void ActivateCountdownPanel()
    {
        remainingTimePanel.SetActive(true);
    }

    public void Harvest()
    {
        isGrowing = false;
        countdownText.gameObject.SetActive(false);
        remainingTimePanel.SetActive(false);

        Level level = FindObjectOfType<Level>();
        if (level != null)
        {
            level.Add(currentField.coin, currentField.xp);
        }
        harvestButton.gameObject.SetActive(false);
    }
}