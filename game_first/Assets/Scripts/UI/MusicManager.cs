using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] public List<AudioClip> musicClips = new();
    private int currentTrackIndex = 5;

    private void Start()
    {
        PlayRandomTrack();
    }

    private IEnumerator Wait()
    {
        while (audioSource.isPlaying || Time.timeScale == 0)
        {
            yield return null;
        }

        PlayRandomTrack();
    }

    private void PlayTrack(int index)
    {
        currentTrackIndex = index;
        audioSource.clip = musicClips[currentTrackIndex];
        audioSource.Play();
        StartCoroutine(Wait());
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