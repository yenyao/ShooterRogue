using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI _ammoText;
    public TextMeshProUGUI _stateText;
    public TextMeshProUGUI _equipText;
    public ProgressBar progressBar;
    public void updateAmmo(int currentAmmo, int maxAmmo, bool isReloading, bool isGunEquipped) {
        string ammoState = currentAmmo.ToString() + " | " + maxAmmo.ToString();
        if(isReloading) {
            ammoState = "Reloading";
            progressBar.showReloadBar(isReloading);
        }
        if(!isGunEquipped) ammoState = "";
        progressBar.showReloadBar(isReloading);
        _ammoText.SetText(ammoState);
    }

    public void setProgressBarTimer(float duration) {
        progressBar.setDuration(duration);
    }

    public void updateInteraction(bool equip) {
        string interactionText = "Press E to equip";
        if(!equip) interactionText = "";
        _equipText.SetText(interactionText);
    }

    public void updateStates(bool canFire, bool isReloading, bool isEquipabble, int numGuns, string equippedGun) {
        _stateText.SetText("canFire: " + canFire + "\nisReloading: " + isReloading + "\nisEquipabble: " + isEquipabble + "\nnumGuns: " + numGuns + "\nequippedGun: " + equippedGun);
    }
}
