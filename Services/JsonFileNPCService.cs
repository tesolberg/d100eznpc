﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using D100EZNPC.Models;
using Microsoft.AspNetCore.Hosting;

namespace D100EZNPC.Services
{
	public class JsonFileNPCService
	{
		public JsonFileNPCService(IWebHostEnvironment webHostEnvironment) 
		{
			WebHostEnvironment = webHostEnvironment;
		}

		public IWebHostEnvironment WebHostEnvironment { get; }

		private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "codex.json");

		public IEnumerable<NPC>? GetNPCs()
		{
			using var jsonFileReader = File.OpenText(JsonFileName);
			return JsonSerializer.Deserialize<List<NPC>>(jsonFileReader.ReadToEnd(),
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
		}

		public void AddNPC(NPC npc)
		{
			npc.GenerateSkills();
			npc.Id = GetNextNPCId();

			List<NPC> npcs = (List<NPC>)GetNPCs()!;
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
		
		public void DeleteNPC(int id)
		{
            List<NPC> npcs = (List<NPC>)GetNPCs()!;
            
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
			IEnumerable<NPC>? list = GetNPCs();
			
			int next = 0;

			foreach (var npc in list!)
			{
				if (npc.Id >= next) next = npc.Id + 1;
			}

			return next;
		}
	}
}