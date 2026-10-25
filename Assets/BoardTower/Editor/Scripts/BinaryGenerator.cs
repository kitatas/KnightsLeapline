using System.Collections.Generic;
using System.IO;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using Cysharp.Text;
using FastEnumUtility;
using MessagePack;
using MessagePack.Resolvers;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace BoardTower.Editor
{
    public static class BinaryGenerator
    {
        [MenuItem("Tools/MasterMemory/" + nameof(BuildBinary))]
        private static void BuildBinary()
        {
            StaticCompositeResolver.Instance.Register(
                MasterMemoryResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance
            );

            var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
            MessagePackSerializer.DefaultOptions = options;

            var databaseBuilder = new DatabaseBuilder();
            databaseBuilder.Append(DeserializeJson<BoardPatternMaster>("board_pattern"));
            databaseBuilder.Append(DeserializeJson<RoundMaster>("round"));
            databaseBuilder.Append(DeserializeJson<RelicMaster>("relic"));
            databaseBuilder.Append(DeserializeJson<ScoreRateMaster>("score_rate"));
            databaseBuilder.Append(GetChessmenMovementRuleMaster());

            var path = "Assets/Externals/Binary/MasterMemory.bytes";
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllBytes(path, databaseBuilder.Build());
            AssetDatabase.Refresh();

            Debug.Log("[MasterMemory] Build bytes completed.");
        }

        private static List<T> DeserializeJson<T>(string fileName)
        {
            var json = File.ReadAllText(ZString.Format("Master/Jsons/{0}.json", fileName));
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        private static IEnumerable<ChessmenMovementRuleMaster> GetChessmenMovementRuleMaster()
        {
            return new[]
            {
                new ChessmenMovementRuleMaster(ChessmenType.King.ToInt32(), ChessmenMovementType.Leaper.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 0),
                        (dx: -1, dy: 1),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 0),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleMaster(ChessmenType.Queen.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 0),
                        (dx: -1, dy: 1),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 0),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleMaster(ChessmenType.Rook.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: 0),
                        (dx: 1, dy: 0),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                    }),
                new ChessmenMovementRuleMaster(ChessmenType.Bishop.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleMaster(ChessmenType.Knight.ToInt32(), ChessmenMovementType.Leaper.ToInt32(),
                    new[]
                    {
                        (dx: -2, dy: -1),
                        (dx: -2, dy: 1),
                        (dx: -1, dy: -2),
                        (dx: -1, dy: 2),
                        (dx: 1, dy: -2),
                        (dx: 1, dy: 2),
                        (dx: 2, dy: -1),
                        (dx: 2, dy: 1),
                    }),
            };
        }
    }
}