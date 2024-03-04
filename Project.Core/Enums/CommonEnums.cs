using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Enums
{
    public class CommonEnums
    {
        public enum Roles
        {
            None = 0,
            SuperAdmin,
            Admin,
            User
        }

        public enum RecordState
        {
            None = 0,
            Active,
            Deleted
        }

        public enum Category
        {
            None = 0,
            Electronics,
            Fashion
        }


	}
}
