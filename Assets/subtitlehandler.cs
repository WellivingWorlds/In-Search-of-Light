using UnityEngine;
using AC;

public class CustomSubtitleHandler : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnStartConversation += OnStartConversation;
        EventManager.OnEndConversation += OnEndConversation;
    }

    private void OnDisable()
    {
        EventManager.OnStartConversation -= OnStartConversation;
        EventManager.OnEndConversation -= OnEndConversation;
    }

    private void OnStartConversation(Conversation conversation)
    {
        // Logic to keep subtitles visible when conversation options appear
        KickStarter.speechManager.displayForever = true;
    }

    private void OnEndConversation(Conversation conversation)
    {
        // Resetting or adjusting logic after the conversation ends
        KickStarter.speechManager.displayForever = false;
    }
}