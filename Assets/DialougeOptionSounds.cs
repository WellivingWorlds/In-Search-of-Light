using UnityEngine;
using AC;

public class DialogueOptionSounds : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] private DialogueOptionAudio[] dialogueOptionAudios = new DialogueOptionAudio[0];

    // Subscribe to the event
    private void OnEnable()
    {
        EventManager.OnMouseOverMenu += OnMouseOverMenu;
    }

    // Unsubscribe from the event
    private void OnDisable()
    {
        EventManager.OnMouseOverMenu -= OnMouseOverMenu;
    }

    // Event handler for when the mouse hovers over a menu element
    private void OnMouseOverMenu(AC.Menu menu, MenuElement element, int slot)
    {
        // Check if the hovered element is a dialog list
        if (element is MenuDialogList dialogList)
        {
            // Get the dialog option for the hovered slot
            ButtonDialog dialogueOption = dialogList.GetDialogueOption(slot);

            // Check if the dialogue option is valid
            if (dialogueOption != null)
            {
                // Iterate through the list of audio options
                foreach (DialogueOptionAudio dialogueOptionAudio in dialogueOptionAudios)
                {
                    // Play the corresponding audio if the option IDs match
                    if (dialogueOptionAudio.optionID == dialogueOption.ID)
                    {
                        if (audioSource != null && dialogueOptionAudio.audioClip != null)
                        {
                            // Check if the audio clip is already playing
                            if (audioSource.clip != dialogueOptionAudio.audioClip || !audioSource.isPlaying)
                            {
                                audioSource.clip = dialogueOptionAudio.audioClip;
                                audioSource.Play();
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    // Serializable class to store dialogue option audio data
    [System.Serializable]
    private class DialogueOptionAudio
    {
        public int optionID;       // ID of the dialogue option
        public AudioClip audioClip; // Audio clip to play for the corresponding dialogue option
    }
}
