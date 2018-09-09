using ProtoBuf;

namespace Hive.Contracts
{
    [ProtoContract]
    public class Beat
    {
        [ProtoMember(1)]
        public Channel Channel { get; set; }
        
        [ProtoMember(2)]
        public MessageType MessageType { get; set; }
        
        [ProtoMember(3)]
        public int MessageId { get; set; }
    }
}