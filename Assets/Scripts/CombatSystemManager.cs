using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystemManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCamSocket;
    public GameObject enemy;
    public GameObject panelVictory;
    public GameObject panelDefeat;

    [SerializeField] private InputHandler _inputHandlerPlayer;
    [SerializeField] private InputHandlerAI _inputHandlerEnemy;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Character _playerCharacter;
    [SerializeField] private Character _EnemyCharacter;

    public enum CombatState
    {
        PlayerTurn,
        EnemyTurn,
        PlayerWon,
        PlayerLost
    }

    public CombatState state;
    
    // Start is called before the first frame update
    void Start()
    {
        Projectile.OnEnteredCollision += OnProjectileCollision;

        _playerCharacter = player.GetComponent<Character>();
        _playerCharacter.OnProjectileShoot += CharacterHasShot;
        
        _EnemyCharacter = enemy.GetComponent<Character>();
        _EnemyCharacter.OnProjectileShoot += CharacterHasShot;
        
        state = CombatState.PlayerTurn;
        StartCoroutine(HandlePlayerTurn());
    }

    private void OnDisable()
    {
        _playerCharacter.OnProjectileShoot -= CharacterHasShot;
        
        _EnemyCharacter.OnProjectileShoot -= CharacterHasShot;
        
        Projectile.OnEnteredCollision -= OnProjectileCollision;
    }

    public void OnProjectileCollision(GameObject projectile)
    {
        if (state == CombatState.PlayerTurn)
        {
            state = CombatState.EnemyTurn;
            StartCoroutine(HandleEnemyTurn());
        }
        else
        {
            state = CombatState.PlayerTurn;
            StartCoroutine(HandlePlayerTurn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerCharacter.MaxHealth <= 0.0f)
        {
            state = CombatState.PlayerLost;
            _inputHandlerPlayer.EnableInput = false;
            panelDefeat.SetActive(true);
        }
        else if (_EnemyCharacter.MaxHealth <= 0.0f)
        {
            state = CombatState.PlayerWon;
            _inputHandlerPlayer.EnableInput = false;
            panelVictory.SetActive(true);
        }
    }

    public void CharacterHasShot(GameObject projectile)
    {
        _cameraController.TargetTransform = projectile.transform;
        _inputHandlerPlayer.EnableInput = false;
        _cameraController.followSpeed = 6.0f;
    }
    public IEnumerator HandlePlayerTurn()
    {
        yield return new WaitForSeconds(1.0f);
        _cameraController.followSpeed = 2.0f;
        _inputHandlerPlayer.EnableInput = true;
        _cameraController.TargetTransform = playerCamSocket.transform;
        _playerCharacter.SetCollidersActive(false);
        _EnemyCharacter.SetCollidersActive(true);
        yield return null;
    }
    
    public IEnumerator HandleEnemyTurn()
    {
        yield return new WaitForSeconds(2.0f);
        _cameraController.TargetTransform = enemy.transform;
        _EnemyCharacter.SetCollidersActive(false);
        _playerCharacter.SetCollidersActive(true);
        yield return new WaitForSeconds(2.0f);
        _EnemyCharacter.Shoot();
        yield return null;
    }
}
