using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreedItemUI : MonoBehaviour
{
    public Image dogImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;

    public void Setup(DogBreed breed)
    {
        nameText.text = breed.name;

        infoText.text =
            $"Origin: {breed.origin}\n" +
            $"Life Span: {breed.life_span}\n" +
            $"Temperament: {breed.temperament}";
    }
}