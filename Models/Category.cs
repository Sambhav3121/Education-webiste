using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Education.Models
{
    public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
}
