using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIHandler : MonoBehaviour
{
    [SerializeField] private Image healthBarImg; 
    [SerializeField] private Image characterIcon; 
    private Character _character;
    private float maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<Character>();
        _character.ONTakenDamage += SetHealthBar;
        maxHealth = _character.MaxHealth;
        characterIcon.sprite = _character.characterStats.characterIcon;
    }


    public void SetHealthBar(float damage)
    {
        healthBarImg.fillAmount = _character.MaxHealth / maxHealth;
    }
}
