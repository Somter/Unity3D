using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static Dictionary<string, int> bag { get; } = new Dictionary<string, int>();



    #region float effectsVolume
    private static float _effectsVolume = 0.073f;
    public static float effectsVolume
    {
        get => _effectsVolume;
        set
        {
            if (_effectsVolume != value)
            {
                _effectsVolume = value;
                Notify(nameof(effectsVolume));
            }
        }
    }
    #endregion

    #region float musicVolume
    private static float _musicVolume = 0.008f;
    public static float musicVolume
    {
        get => _musicVolume;
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                Notify(nameof(musicVolume));
            }
        }
    }
    #endregion



    #region bool isDay
    private static bool _isDay = true;
    public static bool isDay
    {
        get => _isDay;
        set
        {
            if (_isDay != value)
            {
                _isDay = value;
                Notify(nameof(isDay));
            }
        }
    }
    #endregion

    #region bool isFpv
    private static bool _isFpv = true;
    public static bool isFpv
    {
        get => _isFpv;
        set
        {
            if (_isFpv != value)
            {
                _isFpv = value;
                Notify(nameof(isFpv));
            }
        }
    }
    #endregion

    #region Change Notifier
    private static List<Action<string>> listeners = new List<Action<string>>();
    public static void AddListener(Action<string> listener)
    {
        listeners.Add(listener);
    }
    public static void RemoveListener(Action<string> listener)
    {
        listeners.Remove(listener);
    }
    private static void Notify(string fieldName)
    {
        foreach (var listener in listeners)
        {
            listener.Invoke(fieldName);
        }
    }
    #endregion

    public static object GetProperty(string propertyName)
    {
        var prop = typeof(GameState).GetProperty(
            propertyName,
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static
        );
        if (prop == null)
        {
            Debug.LogError($"Property {propertyName} not found.");
            return null;
        }
        return prop.GetValue(null);
    }

    public static void SetProperty(string propertyName, object value)
    {
        var prop = typeof(GameState).GetProperty(
            propertyName,
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static
        );
        if (prop == null)
        {
            Debug.LogError($"Property not found: '{propertyName}' setting value");
           
        }
        else prop.SetValue(null, value);
    }

}