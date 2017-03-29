using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenithSociety.Models.AccountViewModels;

namespace ZenithSociety.Models.IdentityViewModels
{
    public class UserRoleViewModel
    {
        public string Id { get; set; }

        public string RoleId { get; set; }


        public List<ApplicationUser> Users { get; set; }

        public List<AspRole> Roles { get; set; }

    }
}
