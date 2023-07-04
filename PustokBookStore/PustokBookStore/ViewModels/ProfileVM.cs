using PustokBookStore.Entities;

namespace PustokBookStore.ViewModels
{
    public class ProfileVM
    {
        public MemberUpdateVM Member { get; set; }
        public List<Order> Orders { get; set; }
    }
}
