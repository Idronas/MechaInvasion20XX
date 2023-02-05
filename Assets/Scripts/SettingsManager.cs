using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager: MonoBehaviour
{
    private static SettingsManager _instance;

    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SettingsManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    public float lookSensitivity = 50f;
	public float SFXVolume = 1f;
	public float MusicVolume = 1f;
	public bool RandomEnemies = false;
	public bool DebugMode = false;
}
