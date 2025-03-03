using System.Runtime.CompilerServices;

// Adapters
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.Adapters.MockNgo")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.Adapters.Ngo1")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.Adapters.Utp2")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.Adapters.Ngo1WithUtp2")]

// Tools
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetworkProfiler.Runtime")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetStatsMonitor.Implementation")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetworkSimulator.Editor")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetworkSimulator.Runtime")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetVis.Configuration")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetVis.Editor.UI")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetVis.Editor.Visualization")]

// Test assemblies
#if UNITY_INCLUDE_TESTS
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetworkSimulator.Tests.Editor")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetStatsMonitor.Implementation.Tests.Editor")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.NetworkSimulator.Tests.Runtime")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Unity.Multiplayer.Tools.Adapters.Tests.Editor")]

#endif
