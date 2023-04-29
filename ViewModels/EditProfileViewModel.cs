﻿using System.ComponentModel.DataAnnotations;

namespace Wishler.ViewModels;

public class EditProfileViewModel
{
    [Required]
    [MaxLength(20)]
    [MinLength(5)]
    public string Name { get; set; } = "";

    [Required] public int AvatarId { get; set; }
}