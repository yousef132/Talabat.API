using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Core.Entities.Identity
{
    public class UserAddress : BaseEntity
    {
        #region Customer Name

        public string FristName { get; set; }
        public string LastName { get; set; }
        #endregion
        public string City { get; set; }
        public string Country { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]

        public ApplicationUser User { get; set; }
    }
}