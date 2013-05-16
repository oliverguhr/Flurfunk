using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Flurfunk.Data.Model
{
    public class User : Validatable
    {
        /// <summary>
        /// User Id
        /// </summary>
        public ObjectId _id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        
        [StringLength(40,ErrorMessage = "Must be between 3 and 40 characters", MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// OAuth Provider Name
        /// </summary>
        [Required]
        public string ProviderName { get; set; }

        /// <summary>
        /// OAuthProvider Id
        /// </summary>
        [Required]
        public string ProviderId { get; set; }

        /// <summary>
        /// Groups where the user is a member
        /// </summary>
        public Dictionary<ObjectId, string> Groups { get; set; }

        /// <summary>
        /// saved searches, shown as filter to the user
        /// </summary>
        public List<string> Filter { get; set; } 
    }
}
