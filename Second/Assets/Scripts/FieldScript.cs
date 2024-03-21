using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    public enum BitkiTuru { Bugday, Misir }

    public BitkiTuru bitkiTuru;
    private bool ekili;
    private float ekimZamani;
    private float buyumeSuresi;

    private void Start()
    {
        ekili = false;
        ekimZamani = 0f;
        buyumeSuresi = (bitkiTuru == BitkiTuru.Bugday) ? 60f : 600f; // Buðday için 60 saniye, mýsýr için 600 saniye büyüme süresi
    }

    private void Update()
    {
        if (ekili && Time.time >= ekimZamani + buyumeSuresi)
        {
            Hasat();
        }
    }

    public void Ekim()
    {
        if (!ekili)
        {
            ekili = true;
            ekimZamani = Time.time;
            Debug.Log(bitkiTuru.ToString() + " ekildi.");
        }
        else
        {
            Debug.Log("Bu tarla zaten ekili.");
        }
    }

    private void Hasat()
    {
        ekili = false;
        ekimZamani = 0f;
        Debug.Log(bitkiTuru.ToString() + " hasat edildi.");
        // Hasat edilen bitkileri iþleme ekleme veya baþka bir þey yapma
    }
}