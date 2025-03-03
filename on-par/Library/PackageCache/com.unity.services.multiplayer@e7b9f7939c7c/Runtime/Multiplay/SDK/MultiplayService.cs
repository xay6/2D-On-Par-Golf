using System;

namespace Unity.Services.Multiplay
{
    /// <summary>
    /// Here is the first point and call for accessing Multiplay Package's features!
    /// Use the .Instance method to get a singleton of the IMultiplayService, and from there you can make various requests to the Multiplay service API.
    /// </summary>
    public static class MultiplayService
    {
        private static IMultiplayService m_Service;

        /// <summary>
        /// Provides the Multiplay Service interface for making service API requests.
        /// </summary>
        public static IMultiplayService Instance
        {
            get
            {
                if (m_Service == null)
                {
                    throw new InvalidOperationException($"Unable to get {nameof(IMultiplayService)} because Multiplay API is not initialized. Make sure you call UnityServices.InitializeAsync();");
                }

                return m_Service;
            }
            internal set
            {
                m_Service = value;
            }
        }
    }
}
