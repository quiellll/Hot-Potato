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

    [SerializeField] private RawImage placeholderImage;
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

        if (cameraTexture.isPlaying)
        {
            cameraTexture.Stop();
        }

        placeholderImage.texture = cameraTexture;
        placeholderImage.gameObject.SetActive(true);
        cameraTexture.Play();
    }

    public void CapturePhoto()
    {
        if (cameraTexture != null && cameraTexture.isPlaying)
        {
            // Create a new texture with the webcam dimensions
            Texture2D photo = new Texture2D(cameraTexture.width, cameraTexture.height, TextureFormat.RGB24, false);

            // Read the current camera frame
            photo.SetPixels(cameraTexture.GetPixels());
            photo.Apply();

            // Set the captured photo as the texture of the placeholder
            placeholderImage.texture = photo;

            // Stop the camera - THIS WAS MISSING
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

        Texture2D photoTexture = placeholderImage.texture as Texture2D;

        var newPlayer = new PlayerData(
            nameInput.text,
            background.sprite,
            photoTexture  
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
        background.sprite = null; 
    }

    private void OnDestroy()
    {
        if (cameraTexture != null)
        {
            if (cameraTexture.isPlaying)
                cameraTexture.Stop();
            Destroy(cameraTexture);
        }
    }

    private void OnDisable()
    {
        if (cameraTexture != null)
        {
            cameraTexture.Stop();
        }
    }

    private void OnApplicationQuit()
    {
        if (cameraTexture != null)
        {
            cameraTexture.Stop();
        }
    }

}
