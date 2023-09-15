using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefenderFileScanNotifier.Function.Core.ValidationHelper
{
    /// <summary>
    /// The <see cref="IValidation"/> provides mechanism to validate the object.
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>true</c> if the instance is valid; otherwise <c>false</c>.</returns>
        bool Validate();
    }
}
