using UnityEngine;
using TMPro;

public class ResourceCounter : MonoBehaviour
{
    public TextMeshProUGUI fruitText;
    public TextMeshProUGUI oreText;

    private int fruits;
    private int ores;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        fruitText.text = "Fruits: " + fruits;
        oreText.text = "Ores: " + ores;
    }

    public void AddResource(string resourceName, int amount)
    {
        if (resourceName == "Fruit")
        {
            fruits += amount;
        }
        else if (resourceName == "Ore")
        {
            ores += amount;
        }

        UpdateUI();
    }
}