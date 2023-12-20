﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wishler.Models;

public class Group
{
    [Key] public int Id { get; set; }
    [ForeignKey("Owner")]
    public int OwnerId { get; set; }
    public string Name { get; set; } = "";
    public bool IsStarted { get; set; }
    
    public User Owner { get; set; }
}