using System.ComponentModel.DataAnnotations;
namespace One.Models;
public class Lophoc
{
    [Key]
    [Display(Name = "Mã lớp")]
    public string Malop {get; set;}
    [Display(Name = "Họ tên")]
    public string Hoten {get; set;}

}