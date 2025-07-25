using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
public class AmmoText : MonoBehaviour
{
    [Inject] PlayerController _playerController;
    private TMP_Text _TextComponent;

    void Start()
    {
        _TextComponent = this.GetComponent<TMP_Text>();
        _playerController.OnAmmoChanged += OnAmmoChanged;
        OnAmmoChanged(_playerController.AmmoQuantity, _playerController.ClipSize); //recive initial values
    }    
    void OnAmmoChanged(int currentAmmo, int clipSize)
    {
        _TextComponent.text = currentAmmo+ "/" + clipSize;
    }
}
