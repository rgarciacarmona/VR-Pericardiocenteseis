using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SAT_VideoPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    [SerializeField]
    private VideoClip[] videoClips;
    public int videoClipIndex;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoClipIndex = 0;

        SetClip(videoClipIndex);
    }

    public void SetClip(int index)
    {
        if (index >= videoClips.Length)
            return;

        videoPlayer.clip = videoClips[index];
        videoPlayer.Play();
    }
}
