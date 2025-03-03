using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Profile;
using UnityEngine.Assertions;

namespace Unity.DedicatedServer.Editor.Internal
{
    internal static class InternalUtility
    {
        public static BuildProfile GetProfileForClassicTarget(BuildTarget buildTarget, StandaloneBuildSubtarget subtarget)
        {
            return BuildProfileContext.instance.GetForClassicPlatform(buildTarget, subtarget);
        }

        public static BuildProfile GetActiveOrClassicProfile()
        {
            var profile = BuildProfile.GetActiveBuildProfile();
            if (profile != null)
                return profile;

            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var subtarget = BuildPipeline.GetBuildTargetGroup(buildTarget) == BuildTargetGroup.Standalone
                ? EditorUserBuildSettings.standaloneBuildSubtarget
                : StandaloneBuildSubtarget.Default;

            return BuildProfileContext.instance.GetForClassicPlatform(buildTarget, subtarget);
        }

        public static void SetActiveOrClassicProfile(BuildProfile profile)
        {
            if (BuildProfileContext.IsClassicPlatformProfile(profile))
            {
                var buildTarget = profile.buildTarget;
                if (BuildPipeline.GetBuildTargetGroup(buildTarget) == BuildTargetGroup.Standalone)
                    EditorUserBuildSettings.standaloneBuildSubtarget = profile.subtarget;

                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(buildTarget), buildTarget);
                BuildProfile.SetActiveBuildProfile(null);
            }
            else
                BuildProfile.SetActiveBuildProfile(profile);
        }

        public static bool IsServerProfile(BuildProfile profile)
        {
            return NamedBuildTarget.FromTargetAndSubtarget(profile.buildTarget, (int)profile.subtarget) == NamedBuildTarget.Server;
        }

        public static bool IsClassicProfile(BuildProfile profile)
        {
            return BuildProfileContext.IsClassicPlatformProfile(profile);
        }

        public static string GetUniqueKeyForClassicProfile(BuildProfile profile)
        {
            Assert.IsTrue(IsClassicProfile(profile));
            return profile.platformId;
        }
    }
}
