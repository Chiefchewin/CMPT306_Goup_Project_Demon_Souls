using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D playerLight;
    [SerializeField] private float dayDuration = 30.0f;
    [SerializeField] private float nightDuration = 30.0f; 
    [SerializeField] private TextMeshProUGUI messageText;
    
    public delegate void DayCycleEndEvent(int dayCount);
    public delegate void NightStartEvent();

    public static event DayCycleEndEvent OnDayCycleEnd;
    public static event NightStartEvent OnNightTime;

    
    private float _timer = 0.0f;
    private int _daysSurvivedCount = 0;
    private bool _isDay = true;
    private float _globalTargetIntensity = 1.0f;
    private float _globalInitialIntensity = 1.0f;
    private float _playerTargetIntensity = 0.0f;
    private float _playerInitialIntensity = 0.0f;
    private float _transitionSpeed;

    private bool _isInterpolating = false;

    private void Start()
    {
        if (globalLight == null || playerLight == null)
        {
            Debug.LogError("2D Global Light and Player Light are not both assigned in the Inspector.");
            enabled = false;
        }

        _transitionSpeed = 2.0f / dayDuration;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if (!_isInterpolating)
        {
            // Player light intensity slowly increases when it's night
            if (!_isDay && playerLight.intensity < 1.0f)
            {
                playerLight.intensity += Time.deltaTime / nightDuration;
            }
        }

        if (_isDay && _timer >= dayDuration)
        {
            // Time to turn off global light and turn on player light
            OnNightTime?.Invoke();
            _isDay = false;
            _timer = 0.0f;
            _globalTargetIntensity = 0.0f;
            _globalInitialIntensity = 1.0f;
            _playerTargetIntensity = 1.0f;
            _playerInitialIntensity = 0.0f;
            _isInterpolating = true;
        }
        else if (!_isDay && _timer >= nightDuration)
        {
            // Time to turn on global light and turn off player light
            _isDay = true;
            _timer = 0.0f;
            _globalTargetIntensity = 1.0f;
            _globalInitialIntensity = 0.0f;
            _playerTargetIntensity = 0.0f;
            _playerInitialIntensity = 1.0f;
            _isInterpolating = true;
            _daysSurvivedCount++; // Increment the day count after full cycle
            OnDayCycleEnd?.Invoke(_daysSurvivedCount);
            GameManager.daysSurvived = _daysSurvivedCount;
            StartCoroutine(ShowEndOfDayMessage());
        }

        if (_isInterpolating)
        {
            globalLight.intensity = Mathf.Lerp(_globalInitialIntensity, _globalTargetIntensity, _timer * _transitionSpeed);

            playerLight.intensity = Mathf.Lerp(_playerInitialIntensity, _playerTargetIntensity, _timer * _transitionSpeed);
            
            if (globalLight.intensity == _globalTargetIntensity && playerLight.intensity == _playerTargetIntensity)
            {
                _isInterpolating = false;
            }
        }
    }
    
    public void ForceNight()
    {
        if (_isDay)
        {
            // Set parameters for nighttime
            _isDay = false;
            _timer = 0.0f;
            _globalTargetIntensity = 0.0f;
            _globalInitialIntensity = 1.0f;
            _playerTargetIntensity = 1.0f;
            _playerInitialIntensity = 0.0f;
            _isInterpolating = true;
        }
    }
    
    private IEnumerator ShowEndOfDayMessage()
    { 
        const float fadeInDuration = 2.0f; // Duration of the fade-in animation
        const float displayDuration = 3.0f; // Duration the message is displayed
        const float fadeOutDuration = 2.0f; // Duration of the fade-out animation
        
        float elapsedTime = 0;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            messageText.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
            yield return null;
        }

        messageText.alpha = 1;

        messageText.text = "Day " + _daysSurvivedCount + " has ended!";

        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            messageText.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration);
            yield return null;
        }

        messageText.alpha = 0;
        messageText.text = "";
    }

    public bool IsDay()
    {
        return _isDay;
    }
    
    public bool IsNight()
    {
        return !_isDay;
    }
    
    public int GetDaysSurvived()
    {
        return _daysSurvivedCount;
    }
}