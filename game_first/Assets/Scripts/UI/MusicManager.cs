using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public List<AudioClip> musicClips = new();
    private int currentTrackIndex = 5;

    private static MusicManager instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        PlayRandomTrack();
    }

    private void Update()
    {
        if (audioSource.isPlaying || Time.timeScale == 0) return;
        PlayRandomTrack();
    }

    public void PlayTrack(int index)
    {
        currentTrackIndex = index;
        audioSource.clip = musicClips[currentTrackIndex];
        audioSource.Play();
    }

    private void PlayRandomTrack()
    {
        var randomIndex = Random.Range(0, musicClips.Count);
        PlayTrack(randomIndex);
    }

    public void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicClips.Count;
        PlayTrack(currentTrackIndex);
    }

    public void PreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + musicClips.Count) % musicClips.Count;
        PlayTrack(currentTrackIndex);

    }
}