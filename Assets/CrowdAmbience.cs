using UnityEngine;
using System.Collections;

public class CrowdAmbience : MonoBehaviour
{
    public AudioSource crowdSource;
    public AudioClip[] reactionClips;
    public float minDelay = 5f;
    public float maxDelay = 12f;

    void Start()
    {
        StartCoroutine(PlayRandomCrowdReactions());
    }

    IEnumerator PlayRandomCrowdReactions()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            AudioClip clip = reactionClips[Random.Range(0, reactionClips.Length)];
            crowdSource.PlayOneShot(clip, Random.Range(0.4f, 0.8f));
        }
    }
}