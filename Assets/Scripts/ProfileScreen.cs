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
            // la idea es pillar camara selfie pero si no la detecta que use la default
            cameraTexture = new WebCamTexture(WebCamTexture.devices.FirstOrDefault(d => d.isFrontFacing).name ?? "");
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

        // cambio la resolucion de la foto a la de la camara pero a la mitad
        rectTransform.sizeDelta = new Vector2(cameraTexture.width, cameraTexture.height);

        // si la camara es mas alta que ancha (movil), pues que se vea girada
        if (cameraTexture.width < cameraTexture.height)
        {
            rectTransform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    private Texture2D ConvertWebCamTextureToTexture2D(WebCamTexture webcamTexture)
    {
        if (webcamTexture == null || !webcamTexture.isPlaying)
        {
            Debug.LogError("WebCamTexture is not available!");
            return null;
        }

        Texture2D texture2D = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);
        texture2D.SetPixels(webcamTexture.GetPixels());
        texture2D.Apply();

        return texture2D;
    }

    private Sprite CreateSpriteFromWebCam(WebCamTexture webcamTexture)
    {
        Texture2D texture2D = capturedTexture;
        if (texture2D == null) return null;

        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
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

        // por si se olvida a quien sea pillar la foto
        if (cameraTexture.isPlaying) CapturePhoto();

        Sprite photoTexture = CreateSpriteFromWebCam(cameraTexture);

        Sprite profileSprite = Sprite.Create(
        capturedTexture,
        new Rect(0, 0, capturedTexture.width, capturedTexture.height),
        new Vector2(0.5f, 0.5f),
        100.0f
        );

        var newPlayer = new PlayerData(
            nameInput.text,
            background.sprite,
            photoTexture
        );

        newPlayer.SetImageResolution(new Vector2(cameraTexture.width, cameraTexture.height));
        
        config.players.Add(newPlayer);

        // Update display
        var playersScreen = FindAnyObjectByType<PlayersScreen>();
        if (playersScreen != null)
        {
            playersScreen.UpdateNamesVisibility();

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
