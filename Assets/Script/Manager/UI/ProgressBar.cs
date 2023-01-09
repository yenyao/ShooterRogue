using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public float duration;
    public Image mask;
    private float timer = 0f;
    private bool active = false;
    void Start() {
        gameObject.GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        updateCurrentFill();
    }
    void updateCurrentFill() {
        if(active) {
            timer += Time.deltaTime;
            float fillAmount = 100 / duration;
            // mask.fillAmount = Mathf.Lerp(0, 100, fillAmount) / 100;
            // mask.fillAmount = timer;
            mask.fillAmount = Mathf.Lerp(0, 100, timer/(duration*100));
            print(mask.fillAmount);
        }
        // timer = 0;
    }
    // public void setCurrent(float current) {
    //     this.current = current;
    // }
    public void setDuration(float duration) {
        this.duration = duration;
    }
    public void showReloadBar(bool enabled) {
        if(enabled) resetBar();
        active = enabled;
        gameObject.GetComponent<Image>().enabled = enabled;
        transform.GetChild(0).GetComponent<Image>().enabled = enabled;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = enabled;
    }
    private void resetBar() {
        timer = 0f;
    }
}
