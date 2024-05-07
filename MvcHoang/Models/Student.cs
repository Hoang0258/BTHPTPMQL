using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MvcHoang.Models
{
 [Table("Student")]
 public class Student  {
   [Key]
   public string StudentID { get; set; } 
   public string FullName { get; set; }
   public string Age { get; set; }

    
    
   

 }    
  }