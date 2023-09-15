using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefenderFileScanNotifier.Function.Core.ValidationHelper
{
    public static class GuardHelper
    {
        /// <summary>
        /// Throws Argument Null Exception when the parameter is null.
        /// </summary>
        /// <typeparam name="T">Type parameter.</typeparam>
        /// <param name="value">Parameter value.</param>
        /// <param name="paramName">Name of the parameter which has null value.</param>
        public static void AgainstNull<T>(T value, string paramName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName, $"{typeof(T)} object value must not be null.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentException"/> when the value is null or empty or white space.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static void AgainstNullOrWhiteSpace(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("string value must not be null or empty or white-space.", paramName);
            }
        }

        /// <summary>
        /// Throws if invalid.
        /// </summary>
        /// <typeparam name="T">Any reference type.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <param name="message">The error message for <see cref="ArgumentException"/> only.</param>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        /// <exception cref="ArgumentException">Invalid argument value.</exception>
        public static void ThrowIfInvalid<T>(string parameterName, T parameterValue, string message = "Invalid argument value.")
            where T : class
        {
            if (parameterValue is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (parameterValue is string valueStr && string.IsNullOrWhiteSpace(valueStr) ||
                parameterValue is IValidation checkValue && !checkValue.Validate())
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>
        /// Throws exception if parameter value is zero or negative.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <param name="message">The message for <see cref="ArgumentException"/>.</param>
        /// <exception cref="ArgumentException">The provided message.</exception>
        public static void ThrowIfZeroOrNegative(string parameterName, int parameterValue, string message = "Invalid value.")
        {
            if (parameterValue <= 0)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

    }
}
