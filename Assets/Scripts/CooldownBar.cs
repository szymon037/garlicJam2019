using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public Slider slider = null;
   	public string skillName;
   	public Image img;
   	public Skill skillRef;
   	public Text t = null;
   	public string textToDisplay;
    void Start() {
    	//slider = GetComponent<Slider>();
    	skillRef = PlayerStats.GetInstance().FindSkillWithName(skillName);
    	//img = slider.fillRect.gameObject.GetComponent<Image>();
    }

    void Update() {
    	//slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, 0.5f);
    	if(Time.timeScale == 0) return;
    	//slider.value = PlayerStats.GetInstance().timers[skillName] / skillRef.cooldown;
    	//img.fillAmount = slider.value;
    	t.text = textToDisplay + (PlayerStats.GetInstance().timers[skillName] <= 0f ? "Ready!" : (((int)PlayerStats.GetInstance().timers[skillName]).ToString()) + "s");
    }
}
