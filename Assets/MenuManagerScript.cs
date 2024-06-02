using UnityEngine;
using AC;

public class SubtitleMenuManager : MonoBehaviour
{
    public string sceneName;
    private Menu subtitleMenuMainScene;
    private Menu subtitleMenuLillypad;

    void Start()
    {
        // Get the menus by name
        subtitleMenuMainScene = PlayerMenus.GetMenuWithName("Sub1");
        subtitleMenuLillypad = PlayerMenus.GetMenuWithName("Sub2");

        // Register the event
        EventManager.OnStartSpeech += OnStartSpeech;
    }

    void OnDestroy()
    {
        // Unregister the event
        EventManager.OnStartSpeech -= OnStartSpeech;
    }

    private void OnStartSpeech(AC.Char speakingCharacter, string speechText, int lineID)
    {
        // Check the scene name and enable/disable menus accordingly
        if (sceneName == "MainScene")
        {
            subtitleMenuMainScene.TurnOn();
            subtitleMenuLillypad.TurnOff();
        }
        else if (sceneName == "Lillypad")
        {
            subtitleMenuMainScene.TurnOff();
            subtitleMenuLillypad.TurnOn();
        }
    }
}