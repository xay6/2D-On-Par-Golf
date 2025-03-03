using System;
using System.Threading.Tasks;
using Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.AdminApi.Shared;

namespace Unity.Services.Multiplayer.Editor.Matchmaker.Authoring.AdminApi.Network
{
    /// <summary>
    /// Policy that determines if an operation must be retried based on the exception and number of retry attempts.
    /// </summary>
    interface IRetryPolicy
    {
        /// <summary>
        /// Function that determines if and when an error must be retried based on the type and number of retry attempts.
        /// The task can implement exponential backoff if desired.
        /// </summary>
        Func<ApiResponse, int, Task<bool>> Policy { get; set; }
    }
}
