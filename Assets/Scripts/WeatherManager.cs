using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherManager : MonoBehaviour
{
    [Tooltip("List of weather particle system prefabs")]
    public List<GameObject> weathers;

    [Tooltip("Time interval in min between weather changes.")]
    public float changeIntervalMinutes = 5f;

    [Tooltip("Number of upcoming weathers to forecast.")]
    public int forecastCount = 3;

    public TextMeshProUGUI forecastText;

    private GameObject currentWeather;
    private float changeIntervalSeconds;
    private Queue<GameObject> upcomingWeathers = new Queue<GameObject>();

    void Start()
    {
        changeIntervalSeconds = changeIntervalMinutes * 60f;

        InitializeForecast();

        StartCoroutine(ChangeWeatherPeriodically());
    }

    void InitializeForecast()
    {
        for (int i = 0; i < forecastCount; i++)
        {
            upcomingWeathers.Enqueue(weathers[Random.Range(0, weathers.Count)]);
        }
        UpdateForecastUI();
    }

    IEnumerator ChangeWeatherPeriodically()
    {
        while (true)
        {
            ChangeWeather();

            yield return new WaitForSeconds(changeIntervalSeconds);
        }
    }

    void ChangeWeather()
    {
        if (currentWeather != null)
        {
            Destroy(currentWeather);
        }

        GameObject nextWeather = upcomingWeathers.Dequeue();
        currentWeather = Instantiate(nextWeather, transform);
        upcomingWeathers.Enqueue(weathers[Random.Range(0, weathers.Count)]);

        UpdateForecastUI();
    }

    void UpdateForecastUI()
    {
        if (forecastText != null)
        {
            forecastText.text = "Upcoming Weathers:\n";
            foreach (GameObject weather in upcomingWeathers)
            {
                forecastText.text += weather.name + "\n";
            }
        }
    }
}