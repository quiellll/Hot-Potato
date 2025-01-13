using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public enum PlayerColor
{
    Blue,
    Yellow,
    Purple,
    Red,
    Green
}

public class ProfileScreen : MonoBehaviour
{
    [SerializeField] private Image background;

    [SerializeField] private RawImage placeholderImage; // the UI element
    [SerializeField] private WebCamTexture cameraTexture;

    [SerializeField] private List<Button> colorButtons = new();
    private GameObject selectedColor;

    //Serializacion
    [SerializeField] private GameConfiguration config;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Image playerBackground;
    [SerializeField] private RawImage playerFace;

    private void Awake()
    {
        foreach (var button in colorButtons)
        {
            button.onClick.AddListener(delegate { SelectColor(button); });
        }
    }

    public void OpenKeyboardForInput(TMP_InputField inputField)
    {
        TouchScreenKeyboard.Open(inputField.text, TouchScreenKeyboardType.Default);
    }

    public void CloseKeyboardForInput(InputField inputField)
    {
        //
    }

    public void SelectColor(Button clickedButton)
    {
        // access only the first child image of the button
        var childImage = clickedButton.transform.GetChild(0).GetComponent<Image>();

        if (childImage != null && background != null)
        {
            background.sprite = childImage.sprite; // change background sprite
        }
    }

    public void OpenCamera()
    {
        if (cameraTexture == null)
        {
            cameraTexture = new WebCamTexture();
        }

        placeholderImage.texture = cameraTexture;
        placeholderImage.gameObject.SetActive(true); // ensure visibility
        cameraTexture.Play();
    }

    public void CapturePhoto()
    {
        if (cameraTexture != null && cameraTexture.isPlaying)
        {
            // Create a new texture and copy the webcam image
            Texture2D photo = new Texture2D(cameraTexture.width, cameraTexture.height);
            photo.SetPixels(cameraTexture.GetPixels());
            photo.Apply();

            // Set it to the placeholder
            placeholderImage.texture = photo;
            cameraTexture.Stop();
        }
    }

    public void ToggleCamera()
    {
        if (cameraTexture != null && cameraTexture.isPlaying)
        {
            CapturePhoto();
        }
        else
        {
            OpenCamera();
        }
    }

    public void SaveProfile()
    {
        if (string.IsNullOrEmpty(nameInput.text)) return;

        // Just get the texture directly - it's already a Texture2D from CapturePhoto
        var newPlayer = new PlayerData(
            nameInput.text,
            background.sprite,
            placeholderImage.texture as Texture2D
        );

        config.players.Add(newPlayer);

        // Update display
        var playersScreen = FindObjectOfType<PlayersScreen>();
        if (playersScreen != null)
        {
            playersScreen.UpdatePlaceholders();
        }

        ClearProfileForm();
    }

    private void ClearProfileForm()
    {
        nameInput.text = "";
        placeholderImage.texture = null;
        background.sprite = null; // Reset background to default
    }
}
