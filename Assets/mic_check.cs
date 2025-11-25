using UnityEngine;
using Oculus.Voice;   // AppVoiceExperience
using UnityEngine.InputSystem;
using Meta.WitAi.Json;

public class PushToTalk : MonoBehaviour
{
    public AppVoiceExperience voice;
    public InputActionProperty buttonAction;
    public Animator characterAnimator;

    public AvatarFollowPlayer avatar;

    private bool isHeld = false;

    private void OnEnable()
    {
        buttonAction.action.Enable(); 
        buttonAction.action.started += OnPressed;
        buttonAction.action.canceled += OnReleased;

        voice.VoiceEvents.OnStoppedListening.AddListener(OnStoppedListening);
        voice.VoiceEvents.OnFullTranscription.AddListener(OnFull);
        voice.VoiceEvents.OnPartialTranscription.AddListener(OnPartial);

        voice.VoiceEvents.OnResponse.AddListener(OnWitResponse);
    }

    private void OnDisable()
    {
        buttonAction.action.started -= OnPressed;
        buttonAction.action.canceled -= OnReleased;

        voice.VoiceEvents.OnStoppedListening.RemoveListener(OnStoppedListening);
        voice.VoiceEvents.OnFullTranscription.RemoveListener(OnFull);
        voice.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartial);
    }

    private void OnPressed(InputAction.CallbackContext ctx)
    {
        isHeld = true;
        voice.Activate();
    }

    private void OnReleased(InputAction.CallbackContext ctx)
    {
        isHeld = false;
        voice.Deactivate();
    }

    private void OnStoppedListening()
    {
        if (isHeld)
        {
            voice.Activate();
        }
    }

    private void OnPartial(string text)
    {
        Debug.Log("🟡 PARTIAL: " + text);
    }

    private void OnFull(string text)
    {
        Debug.Log("🟢 FULL: " + text);
    }

    private void OnWitResponse(WitResponseNode response)
    {
        Debug.Log("📥 FULL WIT RESPONSE: " + response.ToString());

        var greetings = response["traits"]["wit$greetings"];
        if (greetings != null && greetings.Count > 0)
        {
            string value = greetings[0]["value"];
            if (value == "true")
            {
                Debug.Log("Greeting detected!");
                characterAnimator.SetTrigger("Wave");
            }
        }

        string[] sadKeywords = { "hate you", "don't like you", "mean", "leave me alone", "annoying", "you suck"};

        string transcript = response["text"];
        if (!string.IsNullOrEmpty(transcript))
        {
            transcript = transcript.ToLower(); // normalize
            if (transcript.Contains("sit"))
            {
                Debug.Log("Detected 'sit'");
                avatar.sitting = true;
                characterAnimator.SetTrigger("Sit");
                
            }
            else if (transcript.Contains("set"))
            {
                Debug.Log("Detected 'sit'");
                avatar.sitting = true;
                characterAnimator.SetTrigger("Sit");
                
            }
            else if (transcript.Contains("stand"))
            {
                Debug.Log("Detected 'stand'");
                avatar.sitting = false;
                characterAnimator.SetTrigger("Stand");
                
            }
            else if (transcript.Contains("dance"))
            {
                Debug.Log("Detected 'dance'");
                avatar.sitting = false;
                characterAnimator.SetTrigger("Dance");
                
            }
            else if (transcript.Contains("love you") || (transcript.Contains("like you") && !transcript.Contains("don't like")))
            {
                Debug.Log("Detected 'love'");
                avatar.sitting = false;
                characterAnimator.SetTrigger("Love");
                
            }
            foreach (string keyword in sadKeywords)
            {
                if (transcript.Contains(keyword))
                {
                    Debug.Log($"Detected 'sad");
                    avatar.sitting = false;
                    characterAnimator.SetTrigger("Sad");
                    break;
                }
            }
        }
    }
}
