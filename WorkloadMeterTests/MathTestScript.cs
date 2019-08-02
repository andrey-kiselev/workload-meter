using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace WorkloadMeter {

    public class MathTestScript {

        [Test]
        public void MathTaskType1Passes() {
            MathTaskGenerator m = new MathTaskGenerator();
            for (int i = 2; i <= 9; i++){
                for (int j = 0; j < 10; j++) {
                    Task t = m.GetTask(2, 1, i);
                    Debug.Log(i + ":" + t.Text + "=" + t.Answer);
                    Assert.AreEqual(i, t.Answer); // This does not make much sense becasue there is nothig to fail in this task. 
                }
            }
        }

        [Test]
        public void MathTaskType2Passes() {
            MathTaskGenerator m = new MathTaskGenerator();
            for (int i = 0; i <= 8; i++){
                for (int j = 0; j < 10; j++) {
                    Task t = m.GetTask(2, 2, i);
                    Debug.Log(i + ":" + t.Text + "=" + t.Answer);
                    Assert.AreEqual(i, t.Answer); // This does not make much sense becasue there is nothig to fail in this task. 
                }
            }
        }

        [Test]
        public void MathTaskType3Passes() {
            MathTaskGenerator m = new MathTaskGenerator();
            for (int i = 0; i <= 9; i++){
                for (int j = 0; j < 10; j++) {
                    Task t = m.GetTask(2, 3, i);
                    Debug.Log(i + ":" + t.Text + "=" + t.Answer);
                    Assert.AreEqual(i, t.Answer); // This does not make much sense becasue there is nothig to fail in this task. 
                }
            }
        }

        [Test]
        public void MathTaskType4Passes() {
            MathTaskGenerator m = new MathTaskGenerator();
            for (int i = 0; i <= 7; i++){
                for (int j = 0; j < 10; j++) {
                    Task t = m.GetTask(2, 4, i);
                    Debug.Log(i + ":" + t.Text + "=" + t.Answer);
                    Assert.AreEqual(i, t.Answer); // This does not make much sense becasue there is nothig to fail in this task. 
                }
            }
        }

        [Test]
        public void MathTestScriptSimplePasses() {
            // Use the Assert class to test conditions.
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator MathTestScriptWithEnumeratorPasses() {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}