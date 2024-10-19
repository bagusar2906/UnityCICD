using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// ReSharper disable once CheckNamespace
// ReSharper disable once UnusedType.Global
[UsedImplicitly]
public class WebGLBuilder
    {
        [UsedImplicitly]
        static void Build()
        {

            // Place all your scenes here
            string[] scenes = { "Assets/Scenes/AutoPulse-sim.unity" };

            var pathToDeploy = "builds/WebGL/";

           var report =  BuildPipeline.BuildPlayer(scenes, pathToDeploy, BuildTarget.WebGL, BuildOptions.None);
           var summary = report.summary;
           switch (summary.result)
           {
               case BuildResult.Succeeded:
                   Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
                   break;
               case BuildResult.Failed:
                   Debug.Log("Build failed");
                   break;
           }
        }
    }
