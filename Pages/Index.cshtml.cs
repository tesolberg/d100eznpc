using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace D100EZNPC.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}


		public IActionResult OnGet()
		{
			return Page();
		}

		public void OnPost()
		{
			_logger.LogInformation("On Post recieved on Index");
		}
	}

}