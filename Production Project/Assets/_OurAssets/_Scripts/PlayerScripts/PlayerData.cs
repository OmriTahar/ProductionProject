using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Weapon { Trap, Wall }
public class PlayerData : Unit
{
    public static PlayerData Instance;
    internal Queue<GameObject> activeTraps = new Queue<GameObject>();
    internal Queue<GameObject> activeWalls = new Queue<GameObject>();
    [SerializeField] internal GameObject _trapPrefab, _wallPrefab;
    [SerializeField] internal int _maxTrapAmmo = 3, _maxWallAmmo = 3, _currentCoverAmount, _currentTrapAmount;
    [SerializeField] internal Weapon CurrentWeapon;

    [Header("UI")]
    public TextMeshProUGUI CurrentAmmoUI;
    public Image CurrentWeaponImage;
    [SerializeField] Sprite _coverImage;
    [SerializeField] Sprite _trapImage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _currentCoverAmount = _maxWallAmmo;
        _currentTrapAmount = _maxTrapAmmo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeaponPrefab();
            UpdateUI();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
            UpdateUI();
        }
    }

    void Attack()
    {
        switch (CurrentWeapon)
        {
            case Weapon.Trap:

                if (activeTraps.Count == _maxTrapAmmo)
                {
                    GameObject firstTrap = activeTraps.Dequeue();
                    Destroy(firstTrap);
                }

                if (PlayerAim.Instance._canShoot)
                {
                    GameObject trap = Instantiate(_trapPrefab, PlayerAim.Instance._outline.transform.position, Quaternion.identity);
                    activeTraps.Enqueue(trap);
                    _currentTrapAmount--;
                }

                break;

            case Weapon.Wall:

                if (activeWalls.Count == _maxWallAmmo)
                {
                    GameObject firstWall = activeWalls.Dequeue();
                    Destroy(firstWall);
                }

                if (PlayerAim.Instance._canShoot)
                {
                    GameObject wall = Instantiate(_wallPrefab, PlayerAim.Instance._outline.transform.position, Quaternion.identity);
                    activeWalls.Enqueue(wall);
                    _currentCoverAmount--;
                }

                break;
            default:
                break;
        }
    }

    void SwitchWeaponPrefab()
    {
        switch (CurrentWeapon)
        {
            case Weapon.Trap:
                CurrentWeapon = Weapon.Wall;
                Instance._trapPrefab.SetActive(false);
                Instance._wallPrefab.SetActive(true);
                break;
            case Weapon.Wall:
                CurrentWeapon = Weapon.Trap;
                Instance._wallPrefab.SetActive(false);
                Instance._trapPrefab.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void UpdateUI()
    {
        switch (CurrentWeapon)
        {
            case Weapon.Trap:
                CurrentWeaponImage.sprite = _trapImage;
                CurrentAmmoUI.text = _currentTrapAmount.ToString();
                break;
            case Weapon.Wall:
                CurrentWeaponImage.sprite = _coverImage;
                CurrentAmmoUI.text = _currentCoverAmount.ToString();
                break;
            default:
                CurrentWeaponImage.sprite = _trapImage;
                CurrentAmmoUI.text = _currentTrapAmount.ToString();
                break;
        }
    }
}