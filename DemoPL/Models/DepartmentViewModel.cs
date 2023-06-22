using System.ComponentModel.DataAnnotations;

namespace DemoPL.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department Code is Required")]

        public string Code { get; set; }

        [Required(ErrorMessage = "Department Name is Required")]
        [MinLength(3, ErrorMessage = "MinLength is 3 Characters")]
        public string Name { get; set; }
    }
}
