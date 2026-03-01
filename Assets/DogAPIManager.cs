using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class DogAPIManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent;
    public GameObject breedItemPrefab;
    public TextMeshProUGUI pageText;
    public Button nextButton;
    public Button previousButton;

    private int currentPage = 0;
    private int totalPages = 172; // There are ~172 breeds if loading 1 per page
    private int limit = 1; // FIX: Set this to 1 to show ONLY one dog at a time

    private const string BASE_URL = "https://api.thedogapi.com/v1/breeds";

    void Start()
    {
        LoadBreeds();
    }

    public void NextPage()
    {
        currentPage++;
        LoadBreeds();
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            LoadBreeds();
        }
    }

    void LoadBreeds()
    {
        StartCoroutine(GetBreeds());
    }

    IEnumerator GetBreeds()
    {
        // Fetch only 1 dog for the current page index
        string url = $"{BASE_URL}?limit={limit}&page={currentPage}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("x-api-key", "live_jwFNtFWDXvYEMxENDYUQVtfcqfv8Z7tjR99NOrIfugDDGLws4BoB1Dq5Sy0y5gFb");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            DogBreed[] breeds = JsonHelper.FromJson<DogBreed>(json);

            ClearUI(); // Removes the previous dog before showing the new one

            if (breeds.Length > 0)
            {
                DisplaySingleBreed(breeds[0]);
            }

            UpdatePaginationUI();
        }
    }

    void DisplaySingleBreed(DogBreed breed)
    {
        GameObject item = Instantiate(breedItemPrefab, contentParent);
        BreedItemUI ui = item.GetComponent<BreedItemUI>();

        if (ui != null)
        {
            ui.Setup(breed);
            if (breed.image != null && !string.IsNullOrEmpty(breed.image.url))
            {
                StartCoroutine(LoadImage(breed.image.url, ui.dogImage));
            }
        }
    }

    IEnumerator LoadImage(string url, Image targetImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (targetImage == null) yield break;
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            targetImage.sprite = sprite;
        }
    }

    void ClearUI()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }

    void UpdatePaginationUI()
    {
        // Displays "Page 1", "Page 2", etc.
        pageText.text = $"Page {currentPage + 1} / {totalPages}";
        previousButton.interactable = currentPage > 0;
    }
}