using UnityEngine;
using Oculus.Voice;

public class VoiceManager : MonoBehaviour
{
    public AppVoiceExperience appVoice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        appVoice.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
