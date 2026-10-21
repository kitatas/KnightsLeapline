using BoardTower.Game.Application;
using MasterMemory;
using MessagePack;

namespace BoardTower.Game.Data.DataStore
{
    [MemoryTable(nameof(RelicMaster)), MessagePackObject(true)]
    public sealed class RelicMaster
    {
        public RelicMaster(int type, string name, string content, int probability, int value, bool isUniq)
        {
            Type = type;
            Name = name;
            Content = content;
            Probability = probability;
            Value = value;
            IsUniq = isUniq;
        }

        [PrimaryKey(keyOrder: 0)] public int Type { get; }
        public string Name { get; }
        public string Content { get; }
        public int Probability { get; }
        public int Value { get; }
        [SecondaryKey(indexNo: 0), NonUnique] public bool IsUniq { get; }

        public RelicVO ToVO() => new(Type, Name, Content, Probability, Value, IsUniq);
    }
}