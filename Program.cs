using System;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

public class SpecificIdEgo
{
    [JsonPropertyName("id")]
    public int Id {get; set;}

    [JsonPropertyName("ego")]
    public int[][] Ego {get; set;} = Array.Empty<int[]>();
}
public class Config
{
    public List<int[][]> DefaultEgo {get; set;} = new List<int[][]>();
    public List<SpecificIdEgo> SpecificIdEgos {get; set;} = new List<SpecificIdEgo>();

    public int[][] GetData(int Id, int Idx)
    {
        var specificIdEgo = SpecificIdEgos?.FirstOrDefault(item => item.Id == Id);
        if (specificIdEgo != null) return specificIdEgo.Ego;

        int realIdx = Idx - 1;

        if (realIdx >= 0 && realIdx < DefaultEgo.Count) return DefaultEgo[realIdx];

        return new int[][] { new int[] {0, 1}, new int[] {0, 1}, new int[] {0, 1}, new int[] {0, 1} };
    }
}
class IDToAbnor
{
    static void Main()
    {
        Console.WriteLine("It's Mega Terrorist script");
        try
        {
            string path_base = AppContext.BaseDirectory;

            // Create the mod's folder

            string modName = "AbnorsAsIDs";
            string path_mod = Path.Combine(path_base, modName);

            Directory.CreateDirectory(path_mod);
            Console.WriteLine($"{modName} folder found");
            Console.WriteLine("A Sauropod a day keeps your lawyer away");

            // Create locale folders

            // string path_custom_limbus_locale = Path.Combine(path_mod, "custom_limbus_locale", "EN");

            // Directory.CreateDirectory(path_custom_limbus_locale);
            // Console.WriteLine("custom_limbus_locale/EN found");
            // Console.WriteLine("Hi mointpan");

            // string path_abnormalityContentData = Path.Combine(path_custom_limbus_locale, "abnormalityContentData");
            // string path_enemyList = Path.Combine(path_custom_limbus_locale, "enemyList");

            // Directory.CreateDirectory(path_abnormalityContentData);
            // Console.WriteLine("abnormalityContentData found");
            // Console.WriteLine("Larper, do your job");

            // Directory.CreateDirectory(path_enemyList);
            // Console.WriteLine("enemyList found");
            // Console.WriteLine("Do you know Limi is the G-O-A-T?");

            // Read config.json for custom EGO choosing
            string path_config = Path.Combine(path_base, "config.json");

            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            Config config = (File.Exists(path_config) ? JsonSerializer.Deserialize<Config>(File.ReadAllText(path_config), options) : new Config()) ?? new Config();

            // Create data folders

            string path_custom_limbus_data = Path.Combine(path_mod, "custom_limbus_data");

            Directory.CreateDirectory(path_custom_limbus_data);
            Console.WriteLine("custom_limbus_data found");
            // Console.WriteLine("You should join the Index (1337 reasons [Click Here to read more!])");
            Console.WriteLine("Hi mointpan");

            string path_abnormality_part = Path.Combine(path_custom_limbus_data, "abnormality-part");
            string path_abnormality_unit = Path.Combine(path_custom_limbus_data, "abnormality-unit");

            Directory.CreateDirectory(path_abnormality_part);
            Console.WriteLine("abnormality-part found");
            // Console.WriteLine("Love yourself :heart:");
            Console.WriteLine("Do you know Limi is the G-O-A-T?");

            Directory.CreateDirectory(path_abnormality_unit);
            Console.WriteLine("abnormality-unit found");
            Console.WriteLine("added 200 lethe coin miners");

            // Real shit starts
            // units + parts

            string path_personality = Path.GetFullPath(Path.Combine(path_base, "..", "dumpedData", "limbus_data", "personality"));

            string[] personalityFiles = Directory.GetFiles(path_personality, "*.json");

            JsonArray result_Unit = new JsonArray();
            JsonArray result_Part = new JsonArray();

            JsonObject final_Unit = new JsonObject
            {
                ["list"] = result_Unit
            };

            JsonObject final_Part = new JsonObject
            {
                ["list"] = result_Part
            };

            foreach (string fpath in personalityFiles)
            {
                string raw = File.ReadAllText(fpath);

                JsonNode idList = JsonNode.Parse(raw)["list"];

                foreach (JsonNode idData in idList.AsArray())
                {
                    int id = idData["id"].GetValue<int>();
                    int charIdx = idData["characterId"].GetValue<int>();

                    JsonArray egoList = new JsonArray();

                    int[][] egoDatas = config.GetData(id, charIdx);

                    for (int i = 0; i < egoDatas.Length; i++)
                    {
                        if (i > 4) break;
                        if (egoDatas[i].Length >= 2 && egoDatas[i][0] != 0)
                        {
                            egoList.Add(new JsonObject
                            {
                                ["egoID"] = egoDatas[i][0],
                                ["egoLevel"] = egoDatas[i][1]
                            });
                        }
                    }

                    JsonArray unitKeywordList = new JsonArray {"SHADOW_ENEMY"};

                    if (idData["unitKeywordList"] is JsonArray keywordList)
                    {
                        foreach (var unitKeyword in keywordList)
                        unitKeywordList.Add(unitKeyword.DeepClone());
                    }

                    var newAbnorUnit = new JsonObject
                    {
                        ["id"] = 2000000000 + id,
                        ["unitScriptID"] = "ShadowAbnormality_Jealousy",
                        ["unitKeywordList"] = unitKeywordList,
                        ["classType"] = "ZAYIN",
                        ["attributeType"] = idData["uniqueAttribute"].GetValue<string>(),
                        ["isOriginNormalEnemy"] = true,
                        ["hasMp"] = true,
                        ["lowMorale"] = -46,
                        ["panic"] = -46,
                        ["patternID"] = "PickLikeAPlayerHighest",
                        ["startActionSlotNum"] = 1,
                        ["maxActionSlotNum"] = 1,
                        ["abnormalityPartList"] = new JsonArray(2010000000 + id),
                        ["overridePersonalityId"] = id,
                        ["egoList"] = egoList
                    };

                    result_Unit.Add(newAbnorUnit);

                    var newAbnorPart = new JsonObject
                    {
                        ["id"] = 2010000000 + id,
                        ["defCorrection"] = idData["defCorrection"].GetValue<int>(),
                        ["isDestroyable"] = "false",
                        ["skillCancelable"] = "false",
                        ["canVanish"] = "false",
                        ["canChangeSpriteOnDestoryed"] = "false",
                        ["spreadMpEffectToAbnormality"] = "true",
                        ["partType"] = "BODY",
                        ["nameID"] = 999999
                    };

                    result_Part.Add(newAbnorPart);
                }
            }

            File.WriteAllText(Path.Combine(path_abnormality_unit, $"{modName}.json"), final_Unit.ToJsonString(new JsonSerializerOptions{WriteIndented = true}));
            Console.WriteLine("Done writing abnormality-unit");

            File.WriteAllText(Path.Combine(path_abnormality_part, $"{modName}.json"), final_Part.ToJsonString(new JsonSerializerOptions{WriteIndented = true}));
            Console.WriteLine("Done writing abnormality-part");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }

        Console.ReadKey();
    }
}