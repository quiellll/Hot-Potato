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
            // create a new texture for the captured photo
            Texture2D photo = new Texture2D(cameraTexture.width, cameraTexture.height);
            photo.SetPixels(cameraTexture.GetPixels());
            photo.Apply();

            // apply the captured photo to the placeholder's raw image
            placeholderImage.texture = photo;

            // stop the camera
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
        string playerName = nameInput.text;
        if (string.IsNullOrEmpty(playerName)) return;

        Sprite backgroundSprite = playerBackground.sprite;
        Texture faceTexture = placeholderImage.texture; // directly get texture from RawImage

        Player newPlayer = gameObject.AddComponent<Player>();
        config.players.Add(newPlayer);
    }
}
