using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CardsController : MonoBehaviour
{
    private Camera _camera;
    public List<GameObject> cardsPattern, cardInstances;
    // The dictionary is the local database of this controller that store all individual cards state.
    // So we can easily get all value of the card when the user touch the card triggered by a raycast.
    private readonly Dictionary<GameObject, CardController> _singleCardControllersDict = new();
    public bool IsCardsSpawned { get; private set; }

    private void Start()
    {
        _camera = Camera.main;
        InitialiseCards();
    }

    private void FixedUpdate()
    {
        DetectTouch();
    }

    private void InitialiseCards()
    {
        var incrementVector = -0.5f;
        foreach (var card in cardsPattern)
        {
            card.SetActive(false);
            var spawnedCard = Instantiate(card, transform.position + new Vector3(1,0,0) * incrementVector, Quaternion.Euler(-90,-90,0));
            cardInstances.Add(spawnedCard);
            _singleCardControllersDict.Add(spawnedCard, spawnedCard.GetComponent<CardController>()); // store the gameObject (automatically convert to hash) as key and set controller as value
            incrementVector += 0.5f;
        }
    }
    
    public IEnumerator SpawnCards()
    {
        foreach (var card in cardInstances)
        {
            card.SetActive(true);
            yield return new WaitForSeconds(1);
        }
        IsCardsSpawned = true;
    }
    

    private void DetectTouch()
    {
        if (Input.touchCount <= 0) return;
        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;
        
        var ray = _camera.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out var hit))
        {
            var card = _singleCardControllersDict[hit.transform.gameObject];
            Debug.Log($"{card.Force} {card.CardName} {card.Type}");
        }
    }

    private void SelectCard(GameObject cardSelected)
    {
        var card = _singleCardControllersDict[cardSelected];
        // for each cards in playerCards :
        // verify if the category of type of the selected card is > 0 in playerCards
        // 
    }
}
