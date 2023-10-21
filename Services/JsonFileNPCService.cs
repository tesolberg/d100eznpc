using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using D100EZNPC.Models;
using Microsoft.AspNetCore.Hosting;

namespace D100EZNPC.Services
{
	public class JsonFileNPCService : INPCService
	{
		public JsonFileNPCService(IWebHostEnvironment webHostEnvironment)
		{
			WebHostEnvironment = webHostEnvironment;
		}

		public IWebHostEnvironment WebHostEnvironment { get; }

		private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "codex.json");

		public IEnumerable<NPC>? GetAllNPCs()
		{
			using var jsonFileReader = File.OpenText(JsonFileName);
			return JsonSerializer.Deserialize<List<NPC>>(jsonFileReader.ReadToEnd(),
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
		}

		public void AddNewNPC(NPC npc)
		{
			npc.Id = GetNextNPCId();

			List<NPC> npcs = (List<NPC>)GetAllNPCs()!;
			if (npcs == null) npcs = new List<NPC>();

			npcs.Add(npc);

			JsonSerializerOptions options = new JsonSerializerOptions
			{
				WriteIndented = true, // Format the JSON with indentation
			};

			string jsonString = JsonSerializer.Serialize(npcs, options);

			string path = Path.Combine(WebHostEnvironment.WebRootPath, "data", "codex.json");

			File.WriteAllText(path, jsonString);
		}

		public void EditNPC(NPC updatedNpc)
		{
			List<NPC> npcs = (List<NPC>)GetAllNPCs()!;
			if (npcs == null) return;

			// Removes old NPC entry
			npcs = npcs.Where(n => n.Id != updatedNpc.Id).ToList();

			// Adds updated entry
			npcs.Add(updatedNpc);

			JsonSerializerOptions options = new JsonSerializerOptions
			{
				WriteIndented = true, // Format the JSON with indentation
			};

			string jsonString = JsonSerializer.Serialize(npcs, options);

			string path = Path.Combine(WebHostEnvironment.WebRootPath, "data", "codex.json");

			File.WriteAllText(path, jsonString);
		}

		public NPC GetNPC(int id)
		{
			return ((List<NPC>)GetAllNPCs()!)?.Where(n => n.Id == id).FirstOrDefault()!;
		}

		public void DeleteNPC(int id)
		{
			List<NPC> npcs = (List<NPC>)GetAllNPCs()!;

			npcs = npcs.Where(n => n.Id != id).ToList();

			JsonSerializerOptions options = new JsonSerializerOptions
			{
				WriteIndented = true, // Format the JSON with indentation
			};

			string jsonString = JsonSerializer.Serialize(npcs, options);

			string path = Path.Combine(WebHostEnvironment.WebRootPath, "data", "codex.json");

			File.WriteAllText(path, jsonString);
		}

		int GetNextNPCId()
		{
			IEnumerable<NPC>? list = GetAllNPCs();

			int next = 0;

			foreach (var npc in list!)
			{
				if (npc.Id >= next) next = npc.Id + 1;
			}

			return next;
		}
	}
}