using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private KeyCode pauseKey = KeyCode.Keypad0; 
        [SerializeField] private KeyCode speedDownKey = KeyCode.Keypad1; 
        [SerializeField] private KeyCode speedNormalKey = KeyCode.Keypad2; 
        [SerializeField] private KeyCode speedUpKey = KeyCode.Keypad3;

        [SerializeField] private float[] timeValues = { 0, 0.25f, 0.5f, 1, 2, 3, 4, 6, 8, 16, 32, 64 };

        public TextMeshProUGUI timeSpeedText;

        public static float t;

        private int normalSpeedIndex;
        private int index;
        private int lastIndex;
        private int lastChangeIndex;

        private void Start()
        {
            normalSpeedIndex = timeValues.ToList().IndexOf(1);
            index = normalSpeedIndex;
            lastIndex = normalSpeedIndex;
            lastChangeIndex = -1; 
        }

        private void Update()
        {
            if(Input.GetKeyDown(pauseKey))
            {
                Pause();
            }
            if(Input.GetKeyDown(speedDownKey))
            {
                SetDownSpeed();
            }
            if(Input.GetKeyDown(speedNormalKey))
            {
                SetUpNormalSpeed();
            }
            if(Input.GetKeyDown(speedUpKey))
            {
                SetUpSpeed();
            }
        }

        private void SetUpNormalSpeed()
        {
            index = normalSpeedIndex; 
            SetTime();
        }


        private void SetDownSpeed()
        {
            index = Mathf.Clamp(index - 1, 0, timeValues.Length - 1);
            SetTime();
        }
        private void SetUpSpeed()
        {
            index = Mathf.Clamp(index + 1, 0, timeValues.Length - 1);
            SetTime();
        }

        private void Pause()
        {
            if (Time.timeScale == 0)
            {
                index = lastIndex;
            }
            else
            {
                lastIndex = index;
                index = 0;
             
            }
            SetTime();
        }

        void SetTime()
        {
            if (lastChangeIndex != index)
            {
                lastChangeIndex = index;
                Time.timeScale = timeValues[index];
                SetSpeedText();
            }
        }

        void SetSpeedText()
        {
            if (timeSpeedText != null)
            {
                timeSpeedText.text = "x" + Time.timeScale;
            }
        }
    }
