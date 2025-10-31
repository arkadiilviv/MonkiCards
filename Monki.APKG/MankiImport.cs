using Microsoft.Data.Sqlite;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

namespace Monki.APKG
{
	// --- Models ---
	public class AnkiModel
	{
		[JsonConverter(typeof(StringOrNumberConverter))]
		public string Id { get; set; } = "";
		public string Name { get; set; }
		public List<AnkiField> Flds { get; set; } = new();
		public List<AnkiTemplate> Tmpls { get; set; } = new();
	}
	public class AnkiField { public string Name { get; set; } = ""; }
	public class AnkiTemplate
	{
		public string Name { get; set; } = "";
		public string Qfmt { get; set; } = "";
		public string Afmt { get; set; } = "";
	}

	public class ApkgCard
	{
		public long Id { get; set; }
		public string SideA { get; set; } = "";
		public string SideB { get; set; } = "";
		public string? ExampleA { get; set; }
		public string? ExampleB { get; set; }
	}

	public class StringOrNumberConverter : JsonConverter<string>
	{
		public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return reader.TokenType switch
			{
				JsonTokenType.String => reader.GetString()!,
				JsonTokenType.Number => reader.GetInt64().ToString(),
				_ => throw new JsonException($"Unexpected token {reader.TokenType}")
			};
		}

		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value);
		}
	}
	public class MankiImport
	{
		public async Task<List<ApkgCard>> ReadApkgAsync(string apkgPath)
		{
			List<ApkgCard> result = new List<ApkgCard>();
			try
			{
				result = await ReadApkgFacadedAsync(apkgPath);
			} catch (Exception)
			{
				result.Add(new ApkgCard()
				{
					SideA = "Error reading .apkg file."
				});
			}
			return result;
		}

		private async Task<List<ApkgCard>> ReadApkgFacadedAsync(string apkgPath)
		{
			var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Directory.CreateDirectory(tempDir);

			try
			{
				ZipFile.ExtractToDirectory(apkgPath, tempDir, true);

				var dbPath = Path.Combine(tempDir, "collection.anki2");
				if (!File.Exists(dbPath))
					throw new FileNotFoundException("Invalid .apkg (missing collection.anki2)");

				var cards = new List<ApkgCard>();

				using (var connection = new SqliteConnection($"Data Source={dbPath};Mode=ReadOnly"))
				{
					connection.Open();

					//var models = GetModels(connection);

					using var command = connection.CreateCommand();

					var cmd = new SqliteCommand("SELECT models FROM col LIMIT 1", connection);
					var modelsJson = (string)cmd.ExecuteScalar();

					var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
					var models = JsonSerializer.Deserialize<Dictionary<string, AnkiModel>>(modelsJson, options)!;

					var cardCmd = connection.CreateCommand();
					cardCmd.CommandText = @"
						SELECT c.id as cardId, n.flds, n.mid
						FROM cards c
						JOIN notes n ON c.nid = n.id;
					";

					using var reader = cardCmd.ExecuteReader();
					while (reader.Read())
					{
						long cardId = reader.GetInt64(0);
						string fldsRaw = reader.GetString(1);
						long modelId = reader.GetInt64(2);

						if (!models.TryGetValue(modelId.ToString(), out var model))
							continue;

						string[] fieldValues = fldsRaw.Split('\x1F');
						var fieldMap = model.Flds
							.Select((f, i) => new { f.Name, Value = i < fieldValues.Length ? fieldValues[i] : "" })
							.ToDictionary(f => f.Name, f => f.Value);

						foreach (var tmpl in model.Tmpls)
						{
							var frontFields = ExtractTemplateFields(tmpl.Qfmt);
							var backFields = ExtractTemplateFields(tmpl.Afmt);

							// Detect language prefix
							string frontLangPrefix = DetectLangPrefix(frontFields);
							string backLangPrefix = DetectLangPrefix(backFields);

							// Split front/back fields into "words" and "sentences"
							var (frontWord, frontExamples) = SplitWordAndSentences(fieldMap, frontFields, frontLangPrefix);
							var (backWord, backExamples) = SplitWordAndSentences(fieldMap, backFields, backLangPrefix);

							// Render templates (full HTML, just for reference)
							string frontHtml = RenderTemplate(tmpl.Qfmt, fieldMap);
							string backHtml = RenderTemplate(tmpl.Afmt, fieldMap);

							cards.Add(new ApkgCard
							{
								Id = cardId,
								SideA = frontWord,
								SideB = backWord,
								ExampleA = frontExamples.Count > 0 ? string.Join("\n", frontExamples) : null,
								ExampleB = backExamples.Count > 0 ? string.Join("\n", backExamples) : null
							});
						}
					}
				}
				cards = cards.DistinctBy(c => (c.Id)).ToList();
				return cards;
			} finally
			{
				SqliteConnection.ClearAllPools();
				await SafeDelete(tempDir);
			}
		}

		// --- Helpers ---
		List<string> ExtractTemplateFields(string template) =>
		  Regex.Matches(template, "{{(.*?)}}")
			  .Select(m => m.Groups[1].Value.Replace("type:", "").Trim())
			  .Where(n => !string.IsNullOrWhiteSpace(n))
			  .Distinct()
			  .ToList();

		string RenderTemplate(string template, Dictionary<string, string> fields) =>
		  Regex.Replace(template, "{{(.*?)}}", m =>
		  {
			  string name = m.Groups[1].Value.Replace("type:", "").Trim();
			  return fields.TryGetValue(name, out var val) ? val : "";
		  });

		string CleanHtml(string text)
		{
			text = Regex.Replace(text, "<.*?>", "").Trim();
			text = text.Replace("&nbsp;", " ");

			text = Regex.Replace(text, @"\[sound:.*?\]", "");

			return text;
		}

		string DetectLangPrefix(List<string> fieldNames)
		{
			var prefixes = fieldNames
				.Select(f => f.Contains('_') ? f.Split('_')[0] : "")
				.Where(p => !string.IsNullOrEmpty(p))
				.GroupBy(p => p)
				.OrderByDescending(g => g.Count())
				.Select(g => g.Key)
				.ToList();

			return prefixes.FirstOrDefault() ?? "";
		}

		// 🧠 NEW: separate word + sentences
		(string Word, List<string> Sentences) SplitWordAndSentences(
		  Dictionary<string, string> fieldMap, List<string> candidateFields, string langPrefix)
		{
			string word = "";
			var sentences = new List<string>();

			var relevantFields = candidateFields
				.Where(f => fieldMap.ContainsKey(f))
				.ToList();

			foreach (var name in relevantFields)
			{
				string value = fieldMap[name];
				if (string.IsNullOrWhiteSpace(value))
					continue;

				string lower = name.ToLower();
				bool isSentence = lower.Contains("sent") || lower.Contains("example") ||
								  lower.Contains("phrase") || Regex.IsMatch(value, @"[.!?]");

				if (isSentence)
					sentences.Add(CleanHtml(value));
				else if (string.IsNullOrEmpty(word))
					word = CleanHtml(value);
			}

			// fallback: first field = word if none detected
			if (string.IsNullOrEmpty(word) && relevantFields.Count > 0)
				word = CleanHtml(fieldMap[relevantFields[0]]);

			return (word, sentences);
		}

		async Task SafeDelete(string path, int retries = 5)
		{
			for (int i = 0; i < retries; i++)
			{
				try
				{
					Directory.Delete(path, true);
					return;
				} catch (IOException) when (i < retries - 1)
				{
					Thread.Sleep(1000); // wait before retry
				} catch (UnauthorizedAccessException) when (i < retries - 1)
				{
					Thread.Sleep(1000);
				}
			}
		}
	}
}
