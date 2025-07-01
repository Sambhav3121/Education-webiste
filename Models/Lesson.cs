using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Education.Models
{
public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string VideoUrl { get; set; } 
    public int CourseId { get; set; }
    public Course Course { get; set; }
}
}

