using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Agri_Energy_Connect_Platform.Models
{
    public class User
    {
        [Key]
        public int userID {  get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string surname { get; set; }
        
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string phoneNumber { get; set; }

        [Required]
        public string role {  get; set; }

        public ICollection<Product> farmerProducts { get; set; }
    }
}
