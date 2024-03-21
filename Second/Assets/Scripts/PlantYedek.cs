using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FieldControllerYedek : MonoBehaviour
{
    public GameObject wheatPrefab;
    public GameObject cornPrefab;
    public Button harvestWheatButton;
    public Button harvestCornButton;
    public Button wheatButton;
    public Button cornButton;
    public TextMeshProUGUI wheatCountdownText;
    public TextMeshProUGUI cornCountdownText;
    public GameObject remainingTimePanel;


    private GameObject currentCrop;
    private bool isGrowing = false;
    [SerializeField] private float wheatGrowthTime = 30f;
    [SerializeField] private float cornGrowthTime = 60f;

    private void Start()
    {
        harvestCornButton.onClick.AddListener(() => Harvest("Corn"));
        harvestWheatButton.onClick.AddListener(() => Harvest("Wheat"));
        wheatButton.onClick.AddListener(() => PlantCrop(wheatPrefab));
        cornButton.onClick.AddListener(() => PlantCrop(cornPrefab));
    }

    private void Update()
    {
        if (isGrowing)
        {
            if (currentCrop.CompareTag("Wheat"))
            {
                UpdateGrowthTime(ref wheatGrowthTime, wheatCountdownText, harvestWheatButton);
            }
            else if (currentCrop.CompareTag("Corn"))
            {
                UpdateGrowthTime(ref cornGrowthTime, cornCountdownText, harvestCornButton);
            }
        }
    }

    private void UpdateGrowthTime(ref float growthTime, TextMeshProUGUI countdownText, Button harvestButton)
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

    private void PlantCrop(GameObject cropPrefab)
    {
        if (!isGrowing)
        {
            if (currentCrop != null)
                Destroy(currentCrop);

            currentCrop = Instantiate(cropPrefab, transform.position, Quaternion.identity);
            isGrowing = true;
            if (cropPrefab == wheatPrefab)
                wheatGrowthTime = 30f;
            else if (cropPrefab == cornPrefab)
                cornGrowthTime = 60f;

            ActivateCountdownPanel(cropPrefab);
        }
    }

    private void ActivateCountdownPanel(GameObject cropPrefab)
    {
        if (cropPrefab == wheatPrefab)
        {
            wheatCountdownText.gameObject.SetActive(true);
            cornCountdownText.gameObject.SetActive(false);
        }
        else if (cropPrefab == cornPrefab)
        {
            wheatCountdownText.gameObject.SetActive(false);
            cornCountdownText.gameObject.SetActive(true);
        }
        remainingTimePanel.SetActive(true);
    }

    private void Harvest(string cropType)
    {
        Destroy(currentCrop);
        isGrowing = false;
        wheatCountdownText.gameObject.SetActive(false);
        cornCountdownText.gameObject.SetActive(false);
        remainingTimePanel.SetActive(false);

        Level level = FindObjectOfType<Level>();
        if (level != null)
        {
            // level.coin += 100;
            // level.current += 500;
        }

        if (cropType == "Wheat")
            harvestWheatButton.gameObject.SetActive(false);
        else if (cropType == "Corn")
            harvestCornButton.gameObject.SetActive(false);

        wheatButton.interactable = true;
        cornButton.interactable = true;
    }
}