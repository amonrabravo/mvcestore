using Microsoft.AspNetCore.Identity;

namespace MVCEStoreData
{
    public class Role : IdentityRole<int>
    {
        #region Properties

        public string FriendlyName { get; set; }

        #endregion

        #region Navigation

        #endregion

    }
}