using System;
using Unity.Services.Multiplayer;

namespace Unity.Services.Multiplayer
{
    class MatchmakerInitializationOption : IModuleOption
    {
        readonly bool m_BackfillIsAllowed;
        readonly string m_Connection;
        public Type Type => typeof(MatchmakerInitializationOption);

        internal MatchmakerInitializationOption(bool backfillIsAllowed = false, string connection = null)
        {
            m_BackfillIsAllowed = backfillIsAllowed;
            m_Connection = connection;
        }

        public void Process(SessionHandler session)
        {
            var module = session.GetModule<MatchmakerModule>();

            if (module == null)
            {
                throw new Exception("Trying to setup matchmaker in session but the module isn't registered.");
            }

            module.InitializeMatchmakingModule(m_BackfillIsAllowed, m_Connection);
        }
    }
}
