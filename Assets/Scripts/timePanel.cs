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
    }
}
