using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI _ammoText;
    public void updateAmmo(int currentAmmo, int maxAmmo, bool isReloading) {
        string ammoState = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        if(isReloading) ammoState = "Reloading";
        _ammoText.SetText(ammoState);
    }
}
