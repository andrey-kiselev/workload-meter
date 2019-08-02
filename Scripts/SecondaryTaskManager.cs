/**
 * Workload meter in VR using secondary math task.
 * Copyright (C) 2019 Andrey Kiselev (andrey.kiselev@oru.se)
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Windows.Speech;
using System.IO;

namespace SecondaryTask {

	public class SecondaryTaskManager : MonoBehaviour {

		[Header("Tasks")]
		[Tooltip("Offset from the start of the script, after which to start measurements.")]
		[Range(0.0f, 60.0f)]
		public float m_StartOffset = 2.0f;
		[Tooltip("Duration of task presentation. Note, that keyword spotting may take time, so answers given in the end of the task may not be recorded. Restart needed to update value!")]
		[Range(1.0f, 10.0f)]
		public float m_TaskDuration = 5.0f;
		[Tooltip("Text object for task presentation.")]
		public Text m_TextObject = null;
		[Tooltip("Overall task difficulty. Level 2 recommended for most cases.")]
		[Range(1,2)]
		public int m_Difficulty = 2;

		[Header("Answers")]
		[Tooltip("Duration of result/empty line. Restart needed to update value!")]
		public float m_TaskFixation = 1.0f;
		[Tooltip("Show response or just an empty field after timeout.")]
		public bool m_ShowCorrect = true;
		[Tooltip("Text to be used as a correct response feedback.")]
		public string m_ResponseTextCorrect = "Correct!";
		[Tooltip("Text to be used as an incorrect response feedback.")]
		public string m_ResponseTextIncorrect = "Wrong!";
		[SerializeField]
		[Tooltip("Keywords representing digits from 0 to 9. Depend on the system speech recognition settings.")]
		public string[] m_Keywords = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

		// Timer for tracking current task duration.
		private float m_TaskTimer = 0.0f;
		// Current task and expected answer.
		private String m_CurrentTask = "";
		private String m_ExpectedAnswer = "";

		// Keyword spotting engine. Currently Windows Speech.
		private KeywordRecognizer m_Recognizer;
		volatile private String m_LastRecognizedAnswer = "";

		[Header("Statistics")]
		[Tooltip("Sliding window size for computing real time performance. Restart needed to update the value.")]
		[Range(1,25)]
		public int m_SlidingWindowSize = 10;
		private int[] m_Answers;

		[Header("Logging")]
		[Tooltip("Save results to file.")]
		public bool m_SaveLog = false;
		[Tooltip("Path to log file. If does not exist, will be created. If exists will be cleared!")]
		public string m_LogFilePath = "";
		private StreamWriter m_LogFile = null;

		// Counter of all answers; will overflow eventually...
		private int m_AnswerCounter = 0;

		// Task generator has to be non-static to have consistent random number generation.
		private MathTaskGenerator m_mtg;

		// Use this for initialization
		void Start () {
			
			m_mtg = new MathTaskGenerator();

			m_Answers = new int[m_SlidingWindowSize];

			if( (!String.Equals(m_LogFilePath, "")) && (m_SaveLog == true) ) {
				m_LogFile = new StreamWriter(m_LogFilePath);
				// Create header in log file
				m_LogFile.WriteLine("Timestamp,Current Task,User Answer,Correct Rate");
			}

			m_Recognizer = new KeywordRecognizer(m_Keywords, ConfidenceLevel.Low);
			m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;

			// start procedure
			InvokeRepeating("ShowNewTask", m_StartOffset, m_TaskDuration + m_TaskFixation);

		}

		// Request new task from the generator, display it on the text field and trigger the keyword spotter.
		private void ShowNewTask() {
			Task t = m_mtg.GetTask(m_Difficulty);
			m_TextObject.text = t.Text;
			m_CurrentTask = t.Text;
			m_ExpectedAnswer = m_Keywords[t.Answer];
			m_TaskTimer = Time.fixedUnscaledTime;
			m_Recognizer.Start();
		}

		// returns current answer rate
		private float GetAnswerRate(){
			float sum = 0.0f;
			foreach (int i in m_Answers) {
				sum += (float) i;
			}
			return sum / Math.Min(m_AnswerCounter, m_SlidingWindowSize);
		}

		// Update is called once per frame
		void Update () {
			// if there was any task given before
			if (!String.Equals(m_ExpectedAnswer, "")) {
				// if time to check the answer and show fixation
				if (Time.fixedUnscaledTime > (m_TaskTimer + m_TaskDuration)){
					// respect privacy, do not listen after the task duration is over
					m_Recognizer.Stop();
					// reset the timer
					m_TaskTimer = Time.fixedUnscaledTime + m_TaskFixation;
					// show the response
					m_TextObject.text = "";
					if(String.Equals(m_ExpectedAnswer, m_LastRecognizedAnswer, StringComparison.OrdinalIgnoreCase)){
						if (m_ShowCorrect == true ) {
							m_TextObject.text = m_ResponseTextCorrect;
						} else {
							m_TextObject.text = "";
						}
						m_Answers[m_AnswerCounter++ % m_SlidingWindowSize] = 1;
					} else {
						if (m_ShowCorrect == true ) {
							m_TextObject.text = m_ResponseTextIncorrect;
						} else {
							m_TextObject.text = "";
						}
						m_Answers[m_AnswerCounter++ % m_SlidingWindowSize] = 0;
					}

					// store log if needed
					if ((m_SaveLog == true) && (m_LogFile != null) ) {
						m_LogFile.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK") + ", " + m_CurrentTask + ", " + m_LastRecognizedAnswer + ", " + GetAnswerRate());
					}
					// output to debug console
					Debug.Log(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK") + ", " + m_CurrentTask + ", " + m_LastRecognizedAnswer + ", " + GetAnswerRate());

					m_LastRecognizedAnswer = "";
				}
			}
		}

		// keyword recognition callback
		private void OnPhraseRecognized(PhraseRecognizedEventArgs args) {
			m_LastRecognizedAnswer = args.text;

		}

		// clean up everything and close files
		private void OnApplicationQuit() {
			
			if (m_Recognizer != null && m_Recognizer.IsRunning)	{
				m_Recognizer.OnPhraseRecognized -= OnPhraseRecognized;
				m_Recognizer.Stop();
			}

			if (m_LogFile != null) {
				m_LogFile.Close();
			}
			
			m_LastRecognizedAnswer = "";
		}

	}
}