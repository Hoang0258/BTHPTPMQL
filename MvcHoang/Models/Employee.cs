using System.ComponentModel.DataAnnotations;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
namespace  MvcHoang.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public string PersonId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string EmployeeId { get; set; }
        public int Age { get; set; }
    }
}