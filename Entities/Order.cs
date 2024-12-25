using IdentityProject.Constants;

namespace IdentityProject.Entities
{
    public class Order : BaseEntity
    {
        public OrderStatus Status { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
