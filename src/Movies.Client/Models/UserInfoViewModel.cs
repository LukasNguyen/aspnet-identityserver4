using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Client.Models
{
    public class UserInfoViewModel
    {
        public Dictionary<string, string> UserInfoDirectory { get; private set; } = null;

        public UserInfoViewModel(Dictionary<string, string> userInfoDirectory)
        {
            UserInfoDirectory = userInfoDirectory;
        }
    }
}
