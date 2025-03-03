namespace Unity.Services.Multiplayer
{
    /// <summary>
    /// Indicates for whom the property should be visible.  If &#x60;public&#x60;, the property will be visible to everyone and will be included in query results.  If &#x60;member&#x60;, the data will only be visible to users who are members of the Session (i.e. those who have successfully joined).  If &#x60;private&#x60;, the metadata will only be visible to the Session member.
    /// </summary>
    /// <value>Indicates for whom the property should be visible.  If &#x60;public&#x60;, the property will be visible to everyone and will be included in query results.  If &#x60;member&#x60;, the data will only be visible to users who are members of the Session (i.e. those who have successfully joined).  If &#x60;private&#x60;, the metadata will only be visible to the Session member.</value>
    public enum VisibilityPropertyOptions
    {
        /// <summary>
        /// Enum Public for value: public
        /// </summary>
        Public = 1,
        /// <summary>
        /// Enum Member for value: member
        /// </summary>
        Member = 2,
        /// <summary>
        /// Enum Private for value: private
        /// </summary>
        Private = 3
    }
}
