using System.Collections;
using UnityEngine;


public class WeatherManager : MonoBehaviour
{
    public enum WeatherType { Sunny, Rainy }

    [Header("Current State (read-only at runtime)")]
    [SerializeField] private WeatherType currentWeather = WeatherType.Sunny;

    [Header("Timing")]
    [Tooltip("Minimum seconds before weather can change")]
    [SerializeField] private float minWeatherDuration = 60f;
    [Tooltip("Maximum seconds before weather can change")]
    [SerializeField] private float maxWeatherDuration = 180f;
    [Tooltip("Seconds to fade rain in/out")]
    [SerializeField] private float transitionDuration = 5f;

    [Header("Rain Visuals")]
    [Tooltip("Particle system used for rain. Should be parented to follow the camera/player.")]
    [SerializeField] private ParticleSystem rainParticles;
    [SerializeField] private float rainMaxEmissionRate = 800f;

    [Header("Audio")]
    [SerializeField] private AudioSource rainAudioSource; // looping rain ambience
    [SerializeField] private AudioSource ambientSunAudioSource; // optional birds/wind loop
    [SerializeField] private float audioFadeSpeed = 1f;

    [Header("Lighting (optional)")]
    [Tooltip("Directional light to dim slightly during rain")]
    [SerializeField] private Light sunLight;
    [SerializeField] private float sunnyIntensity = 1.2f;
    [SerializeField] private float rainyIntensity = 0.7f;

    [Header("Skybox / Fog (optional)")]
    [SerializeField] private bool adjustFog = true;
    [SerializeField] private Color sunnyFogColor = new Color(0.76f, 0.85f, 0.9f);
    [SerializeField] private Color rainyFogColor = new Color(0.5f, 0.5f, 0.55f);
    [SerializeField] private float sunnyFogDensity = 0.005f;
    [SerializeField] private float rainyFogDensity = 0.02f;

    private Coroutine weatherLoopRoutine;
    private Coroutine transitionRoutine;

    public WeatherType CurrentWeather => currentWeather;

    private void Start()
    {
        // Initialize to sunny state instantly (no fade on scene load)
        SetWeatherInstant(WeatherType.Sunny);
        weatherLoopRoutine = StartCoroutine(WeatherLoop());
    }

    private IEnumerator WeatherLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minWeatherDuration, maxWeatherDuration);
            yield return new WaitForSeconds(waitTime);

            WeatherType next = (currentWeather == WeatherType.Sunny)
                ? WeatherType.Rainy
                : WeatherType.Sunny;

            ChangeWeather(next);
        }
    }

    /// <summary>
    /// Smoothly transitions to the given weather type.
    /// </summary>
    public void ChangeWeather(WeatherType newWeather)
    {
        if (newWeather == currentWeather) return;

        if (transitionRoutine != null) StopCoroutine(transitionRoutine);
        transitionRoutine = StartCoroutine(TransitionTo(newWeather));
    }

    private IEnumerator TransitionTo(WeatherType newWeather)
    {
        bool goingToRain = newWeather == WeatherType.Rainy;

        if (goingToRain && rainParticles != null && !rainParticles.isPlaying)
            rainParticles.Play();

        float t = 0f;
        var emission = rainParticles != null ? rainParticles.emission : default;

        float startRate = rainParticles != null ? emission.rateOverTime.constant : 0f;
        float targetRate = goingToRain ? rainMaxEmissionRate : 0f;

        float startRainVol = rainAudioSource != null ? rainAudioSource.volume : 0f;
        float targetRainVol = goingToRain ? 1f : 0f;

        float startSunVol = ambientSunAudioSource != null ? ambientSunAudioSource.volume : 0f;
        float targetSunVol = goingToRain ? 0f : 1f;

        float startLight = sunLight != null ? sunLight.intensity : 0f;
        float targetLight = goingToRain ? rainyIntensity : sunnyIntensity;

        float startFogDensity = RenderSettings.fogDensity;
        float targetFogDensity = goingToRain ? rainyFogDensity : sunnyFogDensity;

        Color startFogColor = RenderSettings.fogColor;
        Color targetFogColor = goingToRain ? rainyFogColor : sunnyFogColor;

        if (rainAudioSource != null && goingToRain && !rainAudioSource.isPlaying)
            rainAudioSource.Play();

        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            float lerp = t / transitionDuration;

            if (rainParticles != null)
            {
                emission.rateOverTime = Mathf.Lerp(startRate, targetRate, lerp);
            }

            if (rainAudioSource != null)
                rainAudioSource.volume = Mathf.Lerp(startRainVol, targetRainVol, lerp);

            if (ambientSunAudioSource != null)
                ambientSunAudioSource.volume = Mathf.Lerp(startSunVol, targetSunVol, lerp);

            if (sunLight != null)
                sunLight.intensity = Mathf.Lerp(startLight, targetLight, lerp);

            if (adjustFog)
            {
                RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, targetFogDensity, lerp);
                RenderSettings.fogColor = Color.Lerp(startFogColor, targetFogColor, lerp);
            }

            yield return null;
        }

        // Snap to final values
        if (rainParticles != null)
        {
            emission.rateOverTime = targetRate;
            if (!goingToRain) rainParticles.Stop();
        }
        if (rainAudioSource != null)
        {
            rainAudioSource.volume = targetRainVol;
            if (!goingToRain) rainAudioSource.Stop();
        }
        if (ambientSunAudioSource != null) ambientSunAudioSource.volume = targetSunVol;
        if (sunLight != null) sunLight.intensity = targetLight;
        if (adjustFog)
        {
            RenderSettings.fogDensity = targetFogDensity;
            RenderSettings.fogColor = targetFogColor;
        }

        currentWeather = newWeather;
    }

  
    /// Sets weather immediately with no transition (used on startup).
    
    private void SetWeatherInstant(WeatherType weather)
    {
        currentWeather = weather;
        bool isRainy = weather == WeatherType.Rainy;

        if (rainParticles != null)
        {
            var emission = rainParticles.emission;
            emission.rateOverTime = isRainy ? rainMaxEmissionRate : 0f;
            if (isRainy) rainParticles.Play(); else rainParticles.Stop();
        }

        if (rainAudioSource != null)
        {
            rainAudioSource.volume = isRainy ? 1f : 0f;
            if (isRainy) rainAudioSource.Play(); else rainAudioSource.Stop();
        }

        if (ambientSunAudioSource != null)
            ambientSunAudioSource.volume = isRainy ? 0f : 1f;

        if (sunLight != null)
            sunLight.intensity = isRainy ? rainyIntensity : sunnyIntensity;

        if (adjustFog)
        {
            RenderSettings.fogDensity = isRainy ? rainyFogDensity : sunnyFogDensity;
            RenderSettings.fogColor = isRainy ? rainyFogColor : sunnyFogColor;
        }
    }

    // call these from a debug menu or console command for testing
    public void ForceRain() => ChangeWeather(WeatherType.Rainy);
    public void ForceSun() => ChangeWeather(WeatherType.Sunny);
}