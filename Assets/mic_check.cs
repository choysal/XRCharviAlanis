using UnityEngine;
using Oculus.Voice;   // AppVoiceExperience
using UnityEngine.InputSystem;
using Meta.WitAi.Json;

public class PushToTalk : MonoBehaviour
{
    public AppVoiceExperience voice;
    public InputActionProperty buttonAction;
    public Animator characterAnimator;

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
        Debug.Log("🎤 Voice ACTIVATED");
        voice.Activate();
    }

    private void OnReleased(InputAction.CallbackContext ctx)
    {
        isHeld = false;
        Debug.Log("🛑 Voice DEACTIVATED");
        voice.Deactivate();
    }

    private void OnStoppedListening()
    {
        // If still holding the button, keep listening
        if (isHeld)
        {
            Debug.Log("🔄 Auto-restarting mic...");
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

        // Check if trait exists
        var greetings = response["traits"]["wit$greetings"];
        if (greetings != null && greetings.Count > 0)
        {
            string value = greetings[0]["value"];
            if (value == "true")
            {
                Debug.Log("👋 Greeting detected!");
                characterAnimator.SetTrigger("Wave");
            }
        }
    }
}
