using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLib", menuName = "Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField] 
    private AudioData[] m_AudioList;

    public static List<string> audioNamesList = new List<string>();

    public AudioData GetAudioByName(string name)
    {
        AudioData value = null;

        foreach (var audio in m_AudioList)
        {
            if (audio.AudioName == name)
                value = audio;
        }

        return value;
    }

    void OnValidate()
    {
        audioNamesList.Clear();

        foreach (var audio in m_AudioList)
        {
            audioNamesList.Add(audio.AudioName);
        }
    }
}

[System.Serializable]
public class AudioGetter
{
    public string AudioName { get => AudioLibrary.audioNamesList[id]; }
    public int id;
}
