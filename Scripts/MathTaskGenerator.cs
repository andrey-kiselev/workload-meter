/**
 * Math Task Generator based on Montreal Imaging Stress Test algorithms.
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondaryTask {

	/**
	* Task is a pair of string and int, where:
	* - Text is what is displayed on screen, and
	* - Answer is a digit to be recognized.
	*/
	public class Task {
		public string Text = "0+0";
		public int Answer = 0;
	}

	public class MathTaskGenerator {
		private System.Random RND = new System.Random();

		public Task GetTask (int dif) {
			
			int taskType;
			switch (dif){
			case 1:
				taskType = RND.Next(1, 2 + 1);
				break;
			case 2:
				taskType = RND.Next(1, 4 + 1);
				break;
			case 3:
				taskType = RND.Next(1, 7 + 1);
				break;
			default:
				taskType = RND.Next(1, 7 + 1);
				break;
			}

			int answer;

			Pair pair = new Pair();
			Task task = new Task();

			switch (taskType) {
				case 1: // "a + b = c", c >= 2
					answer = RND.Next(2, 10);
					pair = m_getSum(answer);
					task.Text = pair.a + "+" + pair.b;
					break;
				case 2: // "a - b = c", c <= 8
					if (dif <= 2) {
						answer = RND.Next(9);
						pair = m_getSimpleSub(answer);
					} else {
						answer = RND.Next(10);
						pair = m_getSub(answer);
					}
					task.Text = pair.a + "-" + pair.b;
					break;
				case 3: // "a + b - c = d"
					Pair pair1 = new Pair();
					if (dif <= 2) {
						answer = RND.Next(9);
						pair1 = m_getSimpleSub(answer, 2);
					} else {
						answer = RND.Next(10);
						pair1 = m_getSub(answer, 2);
					}
					pair = m_getSum(pair1.a);
					task.Text = pair.a + "+" + pair.b + "-" + pair1.b;
					break;
				case 4: // "a - b - c = d"
					Pair pair2 = new Pair();
					if (dif <= 2) {
						answer = RND.Next(9);
						pair2 = m_getSimpleSub(answer, 0, 8);
						pair = m_getSimpleSub(pair2.a);
					} else {
						answer = RND.Next(10);
						pair2 = m_getSub(answer);
						pair = m_getSub(pair2.a);
					}
					task.Text = pair.a + "-" + pair.b + "-" + pair2.b;
					break;
				//case 5: // "a x b = d"
				//	answer = RND.Next(1, 10);
				//	pair = m_getMul(answer);
				//	task.Text = pair.a + "*" + pair.b;
				// 	break;
				//case 6: // "a x b + c = d"
				//	answer = RND.Next(2, 10);
				//	Pair pair3 = m_getSum(answer);
				//	pair = m_getMul(pair3.a);
				//	task.Text = pair.a + "*" + pair.b + "+" + pair3.b;
				// 	break;
				//case 7: // "a x b - c = d"
				//	answer = RND.Next(10);
				//	Pair pair4 = m_getSub(answer);
				//	pair = m_getMul(pair4.a);
				//	task.Text = pair.a + "*" + pair.b + "-" + pair4.b;
				// 	break;
				default:
					answer = 0;
					task.Text = "0+0";
					break;
			}

			task.Answer = answer;
			return task;
		}

		// helper functions
		/* Note: Unity does not support tuples at the moment of writing, therefore, using Pair */
		class Pair {
			public int a = 0, b = 0;
		}

		// find integer a and b such as a + b = c
		// c should be GREATER OR EQUAL than 2
		private Pair m_getSum(int c) {
			Pair p = new Pair();
			p.a = RND.Next(1, c);
			p.b = c - p.a;
			return p;
		}

		// find integer a and b such as a - b = c
		// c must be LESS than 99
		private Pair m_getSub(int c, int min_a = 1) {
			Pair p = new Pair();
			p.a = RND.Next(Math.Max(min_a, c + 1), 99 + 1);
			p.b = p.a - c;
			return p;
		}

		// finds integer a and b such as a - b = c, and a and b are less than 10
		// c should be less than 8, otherwize getSub will be used
		private Pair m_getSimpleSub(int c, int min_a = 1, int max_a = 9) {
			if (c < 9) {
				Pair p = new Pair();
				p.a = RND.Next(Math.Max(min_a, c + 1), Math.Min(max_a + 1, 9 + 1));
				p.b = p.a - c;
				return p;
			} else {
				return m_getSub(c);
			}
		}
	}
}