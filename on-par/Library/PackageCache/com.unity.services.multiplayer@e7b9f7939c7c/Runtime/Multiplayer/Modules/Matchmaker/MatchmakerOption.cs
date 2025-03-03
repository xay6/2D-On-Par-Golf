using System;
using Unity.Services.Multiplayer;

namespace Unity.Services.Multiplayer
{
    class MatchmakerOption : IModuleOption
    {
        public Type Type => typeof(MatchmakerOption);
        internal readonly BackfillingConfiguration Options;


        public MatchmakerOption(BackfillingConfiguration options)
        {
            Options = options;
        }

        public void Process(SessionHandler session)
        {
            if (session.IsHost)
            {
                var module = session.GetModule<MatchmakerModule>();

                if (module == null)
                {
                    throw new Exception("Trying to setup connection in session but the module isn't registered.");
                }

                module.SetBackfillingConfiguration(Options);
            }
        }
    }
}
