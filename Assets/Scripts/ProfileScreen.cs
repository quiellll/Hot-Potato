using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System.Linq;

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

    private Texture2D capturedTexture;
    private bool isPortrait = false;


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

    public void SelectColor(Button clickedButton)
    {
        var childImage = clickedButton.transform.GetChild(0).GetComponent<Image>();

        if (childImage != null && background != null)
        {
            background.sprite = childImage.sprite; 
        }
    }

    public void OpenCamera()
    {
        if (cameraTexture == null)
        {
            // Try to get the front-facing camera
            WebCamDevice[] devices = WebCamTexture.devices;
            WebCamDevice? frontCamera = null;

            // Look for front camera
            foreach (var device in devices)
            {
                if (device.isFrontFacing)
                {
                    frontCamera = device;
                    break;
                }
            }

            // If found, use front camera, otherwise use the first available camera
            string deviceName = frontCamera?.name ?? devices.FirstOrDefault().name;
            cameraTexture = new WebCamTexture(deviceName, 1280, 720, 30);
            placeholderImage.texture = cameraTexture;
        }

        if (cameraTexture.isPlaying)
        {
            cameraTexture.Stop();
        }

        placeholderImage.texture = cameraTexture;
        placeholderImage.gameObject.SetActive(true);
        cameraTexture.Play();

        StartCoroutine(ResizeImageAfterCameraInit());
    }

    private IEnumerator ResizeImageAfterCameraInit()
    {
        yield return new WaitForEndOfFrame();

        // pillar la resolucion de la foto
        RectTransform rectTransform = placeholderImage.GetComponent<RectTransform>();

        isPortrait = cameraTexture.width < cameraTexture.height;

        // Set the correct size
        if (isPortrait)
        {
            // In portrait mode, swap width and height to maintain aspect ratio
            rectTransform.sizeDelta = new Vector2(cameraTexture.height / 2, cameraTexture.width / 2);
            placeholderImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(cameraTexture.width / 2, cameraTexture.height / 2);
            placeholderImage.rectTransform.localRotation = Quaternion.identity;
        }

        // Apply the correct video rotation based on the camera
        int videoRotationAngle = cameraTexture.videoRotationAngle;
        placeholderImage.rectTransform.localRotation *= Quaternion.Euler(0, 0, -videoRotationAngle);
    }

    private Texture2D ConvertWebCamTextureToTexture2D(WebCamTexture webcamTexture)
    {
        if (webcamTexture == null || !webcamTexture.isPlaying)
        {
            Debug.LogError("WebCamTexture is not available!");
            return null;
        }

        Texture2D texture2D;
        if (isPortrait)
        {
            // In portrait mode, swap width and height
            texture2D = new Texture2D(webcamTexture.height, webcamTexture.width, TextureFormat.RGB24, false);

            // Get the pixels and rotate them
            Color[] pixels = webcamTexture.GetPixels();
            Color[] rotatedPixels = new Color[pixels.Length];

            for (int i = 0; i < webcamTexture.height; i++)
            {
                for (int j = 0; j < webcamTexture.width; j++)
                {
                    // Rotate 90 degrees counterclockwise
                    rotatedPixels[i * webcamTexture.width + j] =
                        pixels[(webcamTexture.width - 1 - j) * webcamTexture.height + i];
                }
            }

            texture2D.SetPixels(rotatedPixels);
        }
        else
        {
            texture2D = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);
            texture2D.SetPixels(webcamTexture.GetPixels());
        }

        texture2D.Apply();
        return texture2D;
    }

    private Sprite CreateSpriteFromWebCam(WebCamTexture webcamTexture)
    {
        Texture2D texture2D = capturedTexture;
        if (texture2D == null) return null;

        // Create sprite with the correct orientation
        return Sprite.Create(
            texture2D,
            new Rect(0, 0, texture2D.width, texture2D.height),
            new Vector2(0.5f, 0.5f),
            100f);
    }

    

    public void CapturePhoto()
    {
        if (cameraTexture != null && cameraTexture.isPlaying)
        {
            capturedTexture = ConvertWebCamTextureToTexture2D(cameraTexture);
            cameraTexture.Stop();

            if (capturedTexture != null)
            {
                placeholderImage.texture = capturedTexture; // assign it to the UI texture
            }
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

        // Capture photo if still in preview mode
        if (cameraTexture.isPlaying) CapturePhoto();

        Sprite photoTexture = CreateSpriteFromWebCam(cameraTexture);

        var newPlayer = new PlayerData(
            nameInput.text,
            background.sprite,
            photoTexture
        );

        // Set the correct resolution based on orientation
        Vector2 imageResolution = isPortrait ?
            new Vector2(cameraTexture.height / 2, cameraTexture.width / 2) :
            new Vector2(cameraTexture.width / 2, cameraTexture.height / 2);

        newPlayer.SetImageResolution(imageResolution);

        config.players.Add(newPlayer);

        // Update display
        var playersScreen = FindAnyObjectByType<PlayersScreen>();
        playersScreen?.UpdateNamesVisibility();

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
