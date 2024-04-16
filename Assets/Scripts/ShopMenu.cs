using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public GameObject shopMenu;
    public DayNightManager dayNightManager;
    private static bool _shopVisible;
    [SerializeField]  private Player player;
    [SerializeField] private int HealthCost = 50;
    [SerializeField] private int NightShadeCost = 100;
    [SerializeField] private int KeyCost = 150;
    [SerializeField] private TextMeshProUGUI HealthGUIItem;
    [SerializeField] private TextMeshProUGUI NightShadeGUIItem;
    [SerializeField] private TextMeshProUGUI KeysGUIItem;
    private TextMeshProUGUI _exitNotification;
    public Button buttonHealth;
    public Button buttonNightShade;
    public Button buttonKeys;

    public TextMeshProUGUI currentKeys;

    private ScoreManager _scoreManager;
    
    // Start is called before the first frame update
    void Start()
    {
        shopMenu.SetActive(false);
        
        _scoreManager = GameObject.Find("Canvas").GetComponent<ScoreManager>();

            //buttonKeys = 
    }
    
    private void OnEnable()
    {
        DayNightManager.OnDayCycleEnd += HandleDayCycleEnd;
    }

    private void OnDisable()
    {
        DayNightManager.OnDayCycleEnd -= HandleDayCycleEnd;
    }

    private void HandleDayCycleEnd(int daysSurvived)
    {
        StartCoroutine(ShowShopAfterSeconds(7));
    }

    // Update is called once per frame
    void Update()
    {
        // update what the player currently has
        currentKeys.SetText( "x" + player.GetKeys().ToString() );
        
        //Update costs of items, and change color if player has enough souls or not
        KeysGUIItem.SetText( KeyCost.ToString() );
        NightShadeGUIItem.SetText( NightShadeCost.ToString() );
        HealthGUIItem.SetText( HealthCost.ToString() );

        KeysGUIItem.color = Color.green;
        NightShadeGUIItem.color = Color.green;
        HealthGUIItem.color = Color.green;
        buttonKeys.interactable = true;
        buttonHealth.interactable = true;
        buttonNightShade.interactable = true;

        if (_scoreManager.score < KeyCost)
        {
            KeysGUIItem.color = Color.red;
            buttonKeys.interactable = false;
        }

        if (_scoreManager.score < HealthCost)
        {
            HealthGUIItem.color = Color.red;
            buttonHealth.interactable = false;
        }

        if (_scoreManager.score < NightShadeCost)
        {
            NightShadeGUIItem.color = Color.red;
            buttonNightShade.interactable = false;
        }
    }

    IEnumerator ShowShopAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        if( PlayerCanAffordMinPrice() )
            ShowShop();
    }

    public void ShowShop()
    {
        shopMenu.SetActive(true);
        Time.timeScale = 0f;
        _shopVisible = true;
    }


    public bool ShopIsVisible()
    {
        return _shopVisible;
    }

    public void HideShop()
    {
        shopMenu.SetActive(false);
        Time.timeScale = 1f;
        _shopVisible = false;
    }

    public void Upgrade1() // Purchase a key
    {
        // give key and subtract cost from score
        player.AddKeys( 1 );
        _scoreManager.AddPoint( -KeyCost );
        CloseShopCannotAfford();
    }

    public void Upgrade2() // Make it night
    {
        _scoreManager.AddPoint( -NightShadeCost );
        dayNightManager.ForceNight();
        CloseShopCannotAfford();
    }

    public void Upgrade3() // Add health
    {
        if (player.GetHealth() < 100)
        {
            player.AddHealth(50);
            _scoreManager.AddPoint(-HealthCost);
            CloseShopCannotAfford();
        }
    }

    private void CloseShopCannotAfford()
    {
        if( !PlayerCanAffordMinPrice() )
            HideShop();
    }

    private bool PlayerCanAffordMinPrice()
    {
        int minPrice = Math.Min(HealthCost, NightShadeCost);
        minPrice = Math.Min(minPrice, KeyCost);

        return _scoreManager.score >= minPrice;
    }
}
