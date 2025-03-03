using System.Collections.Generic;

namespace Unity.Services.Multiplayer
{
    /// <summary>
    /// An interface that allows to modify the properties of a Player.
    /// </summary>
    public interface IPlayer : IReadOnlyPlayer
    {
        /// <summary>
        /// Set the &#x60;allocationId&#x60; returned by the networking solution which associates this player in this Session with a persistent connection.
        /// </summary>
        /// <param name="allocationId">This value is used to identify the associated member in a Session.</param>
        public void SetAllocationId(string allocationId);

        /// <summary>
        /// Modifies a set of properties of the player.
        /// </summary>
        /// <param name="properties">A dictionary of properties that can be added/modified.</param>
        public void SetProperties(Dictionary<string, PlayerProperty> properties);

        /// <summary>
        /// Modifies a single property of the player.
        /// </summary>
        /// <param name="key">The key in the player properties that will be added/modified</param>
        /// <param name="property">The property that will be added/modified</param>
        public void SetProperty(string key, PlayerProperty property);
    }
}
