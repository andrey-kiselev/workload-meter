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

namespace WorkloadMeter {

	/**
	 * Task is a pair of string and int, where:
	 * - Text is what is displayed on screen, and
	 * - Answer is a digit to be recognized.
	 */
	public class Task {
		public string Text = "0+0";
		public int Answer = 0;
	}

	/**
	 * Class to generate math tasks.
	 * Must be instantiated to get consistent Random.
	 */
	public class MathTaskGenerator {

		// instantiate Random with a system clock seed value
		private System.Random m_random = new System.Random();

		/**
		 * Get new task of givel difficulty level. 
		 * If type is not 0, then return the task of the specifies type.
		 * 
		 */
		public Task GetTask (int difficulty, int type = 0, int ans = 10) {

			int taskType;
			if(type == 0){
				// pre-generate a type of the task based on difficulty
				switch (difficulty){
				case 1:
					taskType = m_random.Next(1, 2 + 1);
					break;
				case 2:
					taskType = m_random.Next(1, 4 + 1);
					break;
				// case 3:
				// 	taskType = m_random.Next(1, 7 + 1);
				// 	break;
				default:
					taskType = m_random.Next(1, 4 + 1);
					break;
				}
			} else {
				taskType = type;
			}

			// declare the answer
			int answer = ans;

			Task task = new Task();
			Pair ab_c = new Pair();
			Pair a_b = new Pair();

			switch (taskType) {
				case 1: // "a + b = c", c >= 2
					if (ans >= 10) {
						answer = m_random.Next(2, 10);
					}
					a_b = m_getSum(answer);
					task.Text = a_b.a + "+" + a_b.b;
					break;
				case 2: // "a - b = c", c <= 8
					if (difficulty <= 2) {
						if (ans >= 10) {
							answer = m_random.Next(9);
						}
						a_b = m_getSimpleSub(answer);
					} else {
						if (ans >= 10) {
							answer = m_random.Next(10);
						}
						a_b = m_getSub(answer);
					}
					task.Text = a_b.a + "-" + a_b.b;
					break;
				case 3: // "a + b - c = d"
					if (difficulty <= 2) {
						if (ans >= 10) {
							answer = m_random.Next(9);
						}
						a_b = m_getSimpleSub(answer, 2);
					} else {
						if (ans >= 10) {
							answer = m_random.Next(10);
						}
						a_b = m_getSub(answer, 2);
					}
					ab_c = m_getSum(a_b.a);
					task.Text = ab_c.a + "+" + ab_c.b + "-" + a_b.b;
					break;
				case 4: // "a - b - c = d"
					if (difficulty <= 2) {
						if (ans >= 10) {
							answer = m_random.Next(8); //0..8
						}
						a_b = m_getSimpleSub(answer, 1, 8);
						ab_c = m_getSimpleSub(a_b.a);
					} else {
						if (ans >= 10) {
							answer = m_random.Next(10);
						}
						a_b = m_getSub(answer);
						ab_c = m_getSub(a_b.a);
					}
					task.Text = ab_c.a + "-" + ab_c.b + "-" + a_b.b;
					break;
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
			p.a = m_random.Next(1, c);
			p.b = c - p.a;
			return p;
		}

		// find integer a and b such as a - b = c
		// c must be LESS than 99
		private Pair m_getSub(int c, int min_a = 1) {
			Pair p = new Pair();
			p.a = m_random.Next(Math.Max(min_a, c + 1), 99 + 1);
			p.b = p.a - c;
			return p;
		}

		// finds integers a and b such as a - b = c, and a and b are less than 10
		// c should be less than 8, otherwize getSub will be used
		private Pair m_getSimpleSub(int c, int min_a = 1, int max_a = 9) {
			Pair p = new Pair();
			p.a = m_random.Next(Math.Max(min_a, c + 1), Math.Min(max_a + 1, 9 + 1));
			p.b = p.a - c;
			return p;
		}
	}
}