using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Assets_present4D_UnitTests_Editor_Tests_NewTestScript {

    [Test]
    public void Assets_present4D_UnitTests_Editor_Tests_NewTestScriptSimplePasses() {
        // Use the Assert class to test conditions.
        string test1 = "equal";
        string test2 = "equal";
        Assert.AreEqual(test1, test2);
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator Assets_present4D_UnitTests_Editor_Tests_NewTestScriptWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
        string test1 = "equal";
        string test2 = "equal";
        Assert.AreEqual(test1, test2);
    }
}
