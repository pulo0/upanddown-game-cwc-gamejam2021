using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private TextMeshProUGUI _tutorialText;

    private float _timeSinceChange;
    private const float TimeToChange = 0.5f;
    private void Awake()
    {
        _tutorialText = GetComponent<TextMeshProUGUI>();
    }

    
    private void Update()
    {
        _timeSinceChange += Time.deltaTime;

        if (_timeSinceChange >= TimeToChange)
        {
            var col = new Color(Random.value, Random.value, Random.value);

            _tutorialText.color = col;
            _timeSinceChange = 0;
        }
    }
}
