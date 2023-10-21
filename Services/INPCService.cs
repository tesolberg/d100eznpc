using D100EZNPC.Models;

namespace D100EZNPC.Services
{
	public interface INPCService
	{
		//public JsonFileNPCService(IWebHostEnvironment webHostEnvironment)
		//{
		//	WebHostEnvironment = webHostEnvironment;
		//}

		IWebHostEnvironment WebHostEnvironment { get; }

		void AddNewNPC(NPC npc);
		NPC GetNPC(int id);
		IEnumerable<NPC>? GetAllNPCs();
		void EditNPC(NPC updatedNpc);
		void DeleteNPC(int id);
	}
}