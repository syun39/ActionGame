using UnityEngine;
using UnityEngine.UI;

public class RarityProbabilityText : MonoBehaviour
{
    [SerializeField] private Text _rarityRatesText;

    private void Start()
    {
        _rarityRatesText.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        // トグル表示
        _rarityRatesText.gameObject.SetActive(!_rarityRatesText.gameObject.activeSelf);
    }
}
