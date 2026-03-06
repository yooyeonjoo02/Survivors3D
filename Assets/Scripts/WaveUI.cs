using System.Collections;
using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private GameObject waveTextObject;
    [SerializeField] private float showTime = 1.2f;

    private Coroutine routine;

    private void Awake()
    {
        if (waveTextObject != null)
            waveTextObject.SetActive(false);
    }

    public void ShowWaveText(int waveNumber)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(CoShow(waveNumber));
    }

    private IEnumerator CoShow(int waveNumber)
    {
        waveTextObject.SetActive(true);

        TextMeshProUGUI tmp = waveTextObject.GetComponent<TextMeshProUGUI>();
        if (tmp != null) tmp.text = $"Wave {waveNumber} Start!";

        yield return new WaitForSecondsRealtime(showTime);

        waveTextObject.SetActive(false);
    }
}