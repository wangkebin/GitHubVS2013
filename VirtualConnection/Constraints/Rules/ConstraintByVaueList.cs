using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualConnection.Constraints
{
    public class ConstraintByVaueList<T> : ConstraintBase<T>
    {
        public override bool ValidateNewValue(T newvalue)
        {
            return base.ValidateNewValue(newvalue);
        }
    }



}
