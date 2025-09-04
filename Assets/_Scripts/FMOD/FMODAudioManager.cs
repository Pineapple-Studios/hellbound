using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager Instance;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _sfxBus;

    private List<EventInstance> eventInstances = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        // Carregar apenas o que realmente existe
        RuntimeManager.LoadBank("Master");
        RuntimeManager.LoadBank("Master.strings");
        // RuntimeManager.LoadBank("Music");   // ainda não criado
        // RuntimeManager.LoadBank("SFX");     // ainda não criado

        // Esperar carregar
        bool banksLoaded = false;
        while (!banksLoaded)
        {
            RuntimeManager.StudioSystem.flushCommands();
            RuntimeManager.StudioSystem.getBankList(out var banks);
            banksLoaded = banks.Length > 0;
            yield return null;
        }

        Debug.Log("FMOD Banks carregados.");

        // Agora é seguro pegar os buses
        _masterBus = RuntimeManager.GetBus("bus:/");
        //_musicBus = RuntimeManager.GetBus("bus:/Music");  // ainda não existe
        //_sfxBus   = RuntimeManager.GetBus("bus:/SFX");    // ainda não existe

        SetInitialValues();
    }


    private void Update()
    {
        // Só aplica nos Buses (não salva PlayerPrefs aqui)
        _masterBus.setVolume(masterVolume);
        //_musicBus.setVolume(musicVolume);
        //_sfxBus.setVolume(sfxVolume);
    }

    public void SaveVolumeSettings()
    {
        LocalStorage.SaveMixerValue(LocalStorage.GeneralMixerKey(), masterVolume);
        LocalStorage.SaveMixerValue(LocalStorage.MusicMixerKey(), musicVolume);
        LocalStorage.SaveMixerValue(LocalStorage.SfxMixerKey(), sfxVolume);
    }

    public void SetInitialValues()
    {
        float def = 0.6f;
        masterVolume = LocalStorage.GetMixerValue(LocalStorage.GeneralMixerKey(), def);
        //musicVolume = LocalStorage.GetMixerValue(LocalStorage.MusicMixerKey(), def);
        //sfxVolume = LocalStorage.GetMixerValue(LocalStorage.SfxMixerKey(), def);

        _masterBus.setVolume(masterVolume);
        //_musicBus.setVolume(musicVolume);
        //_sfxBus.setVolume(sfxVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 pos)
        => RuntimeManager.PlayOneShot(sound, pos);

    public void CleanUp()
    {
        foreach (var instance in eventInstances)
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }

        eventInstances.Clear();
    }

    private void OnDestroy() => CleanUp();
}
