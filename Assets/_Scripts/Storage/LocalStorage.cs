using UnityEngine;

public static class LocalStorage
{
    const string GENERAL_VOLUME = "@HB_GENERAL_VOLUME";
    const string MUSIC_VOLUME = "@HB_MUSIC_VOLUME";
    const string SFX_VOLUME = "@HB_SFX_VOLUME";
    const string RESOLUTION = "@HB_RESOLUTION";
    const string INPUT_ACTIONS = "@HB_INPUT_ACTIONS";
    const string HB_PREFIX = "@HB_";

    // === ATIVOS ===
    public static void SetGeneralVolume(float v) => PlayerPrefs.SetFloat(GENERAL_VOLUME, v);
    public static float GetGeneralVolume(float def) => PlayerPrefs.GetFloat(GENERAL_VOLUME, def);

    public static void SetMusicVolume(float v) => PlayerPrefs.SetFloat(MUSIC_VOLUME, v);
    public static float GetMusicVolume(float def) => PlayerPrefs.GetFloat(MUSIC_VOLUME, def);

    public static void SetSFXVolume(float v) => PlayerPrefs.SetFloat(SFX_VOLUME, v);
    public static float GetSFXVolume(float def) => PlayerPrefs.GetFloat(SFX_VOLUME, def);

    public static void SetResolution(int index) => PlayerPrefs.SetInt(RESOLUTION, index);
    public static int GetResolution(int def) => PlayerPrefs.GetInt(RESOLUTION, def);

    // === OPCIONAIS (comentados por agora) ===
    /*
    const string BRIGHTNESS = "@HB_BRIGHTNESS";
    const string FULL_SCREEN = "@HB_FULL_SCREEN";

    public static void SetBrightness(float value) => PlayerPrefs.SetFloat(BRIGHTNESS, value);
    public static float GetBrightness(float def) => PlayerPrefs.GetFloat(BRIGHTNESS, def);

    public static void SetFullScreen(bool value) => PlayerPrefsX.SetBool(FULL_SCREEN, value);
    public static bool GetFullScreen(bool def) => PlayerPrefsX.GetBool(FULL_SCREEN, def);
    */

    public static void SetIsUsingKeyboard(bool value) => PlayerPrefsX.SetBool(INPUT_ACTIONS, value);
    public static bool GetIsUsingKeyboard(bool def) => PlayerPrefsX.GetBool(INPUT_ACTIONS, def);

    // === OPCIONAIS: Mixers de áudio ===
    private const string GENERAL_MIXER = "@HB-fmod-general-mixer";
    private const string MUSIC_MIXER = "@HB-fmod-music-mixer";
    private const string SFX_MIXER = "@HB-fmod-sfx-mixer";
    //private const string VOICE_MIXER = "@HB-fmod-voice-mixer";
    //private const string AMBIENCE_MIXER = "@HB-fmod-ambience-mixer";

    public static string GeneralMixerKey() => GENERAL_MIXER;
    public static string MusicMixerKey() => MUSIC_MIXER;
    public static string SfxMixerKey() => SFX_MIXER;
    //public static string VoiceMixerKey() => VOICE_MIXER;
    //public static string AmbienceMixerKey() => AMBIENCE_MIXER;

    public static void SaveMixerValue(string key, float value) => PlayerPrefs.SetFloat(key, value);
    public static float GetMixerValue(string key, float def) => PlayerPrefs.GetFloat(key, def);

    // Extra para bools gerais
    public static void SetBool(string key, bool value) => PlayerPrefsX.SetBool(HB_PREFIX + key, value);
    public static bool GetBool(string key) => PlayerPrefsX.GetBool(HB_PREFIX + key);
}