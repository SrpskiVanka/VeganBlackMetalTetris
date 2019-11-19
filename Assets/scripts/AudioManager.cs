using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager: MonoBehaviour
{
    public AudioClip[] songs;
    public AudioSource audio;
    private List<AudioClip> playedSongs = new List<AudioClip>();
    private List<AudioClip> NonPlayedSongs = new List<AudioClip>();
    

    void Start()
    {
        NonPlayedSongs.AddRange(songs);
        RandomSong();
    }

    private void Update()
    {
        if (!audio.isPlaying)
        {
            RandomSong();
        }
    }

    private void RandomSong()
    {
        if (NonPlayedSongs.Count == 0)
        {
            NonPlayedSongs.AddRange(playedSongs);
            playedSongs.Clear();
        }

        var current = Random.Range(0, NonPlayedSongs.Count);
        var song = NonPlayedSongs[current];
        audio.clip = song;
        audio.Play();
        NonPlayedSongs.RemoveAt(current);
        playedSongs.Add(song);
    }
}