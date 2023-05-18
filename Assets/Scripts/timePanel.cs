using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class timePanel : MonoBehaviour
{

    int day = 1;
    float totalElapsed = 0;
    public TextMeshProUGUI dayCounterText;
    public Image nightOverlay;
    public float howDarkIsNight = .5f;
   
    public int lengthOfDaySeconds = 120;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalElapsed += Time.deltaTime;
        transform.Rotate(new Vector3(0f, 0f, 360f / lengthOfDaySeconds * Time.deltaTime));
        if(totalElapsed > lengthOfDaySeconds)
        {
            totalElapsed -= lengthOfDaySeconds;
            day++;
            dayCounterText.text = "Day " + day.ToString();
        }
        float currentSunPos = transform.eulerAngles.z;
        if(currentSunPos > 300 || currentSunPos < 30)
        {
            float scaled = currentSunPos + 60;
            if(scaled > 100) { scaled -= 360; }
            nightOverlay.color = new Color(nightOverlay.color.r,nightOverlay.color.g,nightOverlay.color.b,(howDarkIsNight * (90 - scaled) / 90) );
        } else if (currentSunPos > 150 && currentSunPos < 240)
        {
            currentSunPos -= 150;
            nightOverlay.color = new Color(nightOverlay.color.r, nightOverlay.color.g, nightOverlay.color.b, (howDarkIsNight * currentSunPos / 90));
        }

    }
}
