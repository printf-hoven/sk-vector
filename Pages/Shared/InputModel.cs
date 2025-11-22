using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Pages.Shared;
public class InputModel
{
  [Required(ErrorMessage = "* required")]
  [Display(Prompt = "your text here...")]
  public string Name { get; set; } = string.Empty;

  [HiddenInput]
  public string? ChatHistory { get; set; } = string.Empty;
}
