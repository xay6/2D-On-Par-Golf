using System.IO;

namespace Unity.Multiplayer.Playmode.VirtualProjects.Editor
{
    class TriggerCloneRefreshMessage
    {
        public TriggerCloneRefreshMessage(bool didDomainReload, int numAssetsChanged)
        {
            DidDomainReload = didDomainReload;
            NumAssetsChanged = numAssetsChanged;
        }

        public bool DidDomainReload { get; }
        public int NumAssetsChanged { get; }

        static void Serialize(BinaryWriter writer, object message)
        {
            var value = message as TriggerCloneRefreshMessage;
            writer.Write(value.DidDomainReload);
            writer.Write(value.NumAssetsChanged);
        }

        static object Deserialize(BinaryReader reader)
        {
            var didDomainReload = reader.ReadBoolean();
            var numAssetsChanged = reader.ReadInt32();
            return new TriggerCloneRefreshMessage(didDomainReload, numAssetsChanged);
        }


        [SerializeMessageDelegates] // ReSharper disable once UnusedMember.Global
        public static SerializeMessageDelegates SerializeMethods() => new SerializeMessageDelegates { SerializeFunc = Serialize, DeserializeFunc = Deserialize };
    }
}
