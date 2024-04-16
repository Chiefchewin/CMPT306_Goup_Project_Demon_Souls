using TMPro;
using UnityEngine;

public class Status : MonoBehaviour
{
    public TextMeshProUGUI mostCatSoulsText;
    public TextMeshProUGUI catSoulsPerSessionText;
    public TextMeshProUGUI totalDamageTakenText;
    public TextMeshProUGUI coinsCollectedText;
    public TextMeshProUGUI daysSurvivedText;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        catSoulsPerSessionText.SetText( "Cats exiled: " + GameManager.totalCatsExiled );
        totalDamageTakenText.SetText( "Damage Taken: " + GameManager.totalDamageTaken );
        coinsCollectedText.SetText( "Health collected: " + GameManager.healthCollected );
        mostCatSoulsText.SetText( "Most cat souls: " + PlayerPrefs.GetInt("highScore", 0) );
        daysSurvivedText.SetText("Days Survived: " + GameManager.daysSurvived);
    }
}
