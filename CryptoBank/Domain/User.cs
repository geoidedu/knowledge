using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Login
        /// </summary>
        public string UserName { get; set; } = null!;
        /// <summary>
        /// Password
        /// </summary>
        [JsonIgnore]
        public string Password { get; set; } = null!;
    }
}
