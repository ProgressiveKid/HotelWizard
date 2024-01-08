using HotelWizard.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelWizard.ViewModels
{
    public class UserOfficeViewModel
    {
     
        public string FIO { get; set; }

        public string Email { get; set; }

        public ICollection<Order> ListOrders { get; set; }
    }
}
