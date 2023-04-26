using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// ## TO USE: FindObjectOfType<AudioManager>().Play("TrainHorn"); (in other script); later object reference


// Declare a serializable class for sound properties (configurable in inspector)
[System.Serializable]
public class Sound
{
    public string name; // The name of the sound
    public AudioClip clip; // The audio clip for the sound

    [Range(0f, 1f)]
    public float volume = 1f; // The volume of the sound

    [Range(0.1f, 3f)]
    public float pitch = 1f; // The pitch of the sound

    public bool loop; // Whether or not the sound should loop endlessly

    [HideInInspector]
    public AudioSource source; // The audio source for the sound
}

// Define the audio manager class.
public class AudioController : MonoBehaviour
{
    public Sound[] sounds; // An array of sounds to manage.

    void Awake()
    {
        // For each sound in the array...
        foreach (Sound sound in sounds)
        {
            // Add an audio source component to the game object
            sound.source = gameObject.AddComponent<AudioSource>();

            // Set the audio clip for the sound
            sound.source.clip = sound.clip;

            // Set the volume, pitch, and loop props for the sound
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }
}