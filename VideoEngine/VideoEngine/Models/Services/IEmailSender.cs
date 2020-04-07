using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jugnoon.Services
{
    public interface IEmailSender
    {
        /// <summary>
        /// Using Routine Email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message);

        /// <summary>
        /// Using Mandril Email Engine
        /// </summary>
        /// <param name="to_email"></param>
        /// <param name="from_email"></param>
        /// <param name="display_name"></param>
        /// <param name="subject"></param>
        /// <param name="templateName"></param>
        /// <param name="MergeVariables"></param>
        /// <returns></returns>
        Task SendEmailAsync(string to_email, string from_email, string display_name, string subject, string templateName, Dictionary<string, dynamic> MergeVariables);
    }
}
