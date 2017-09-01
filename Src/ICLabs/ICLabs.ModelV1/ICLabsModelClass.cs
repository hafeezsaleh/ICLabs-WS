using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace ICLabs.ModelV1
{
    /// <summary>
    /// Class for getting Base class
    /// </summary>
    public class ClsBase
    {
        /// <summary>
        /// Gets or sets appId
        /// </summary>
        /// 
        [JsonIgnore]
        public string remoteIP { get; set; }


    }
   
        /// <summary>
        /// Class for ClsGetResultFile
        /// </summary>
    public class ClsGetResultFile : ClsBase
        {

            /// <summary>
            /// Gets or sets locationId
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required.")]
            [MaxLength(4, ErrorMessage = "Must be 4 characters or less")]
            public string locationId { get; set; }

            /// <summary>
            /// Gets or sets orderId
            /// </summary>
            ///
            [Required(ErrorMessage = "Field required.")]
            [MaxLength(20, ErrorMessage = "Must be 20 characters or less")]
            public string orderId { get; set; }

            /// <summary>
            /// Gets or sets orderCode
            /// </summary>
            ///
            [Required(ErrorMessage = "Field required.")]
            [MaxLength(10, ErrorMessage = "Must be 10 characters or less")]
            public string test { get; set; }

            /// <summary>
            /// Gets or sets resultName
            /// </summary>
            ///
            [Required(ErrorMessage = "Field required.")]   
            public string resultName { get; set; }


            public string appId { get; set; }
        }

        /// <summary>
        /// Class for ClsResultFile
        /// </summary>
    public class ClsResultFile : ClsBase
        {

            /// <summary>
            /// Gets or sets orderId
            /// </summary>
            ///
            [MaxLength(20, ErrorMessage = "Must be 20 characters or less")]
            public string orderId { get; set; }

            /// <summary>
            /// Gets or sets orderCode
            /// </summary>
            ///
            [MaxLength(10, ErrorMessage = "Must be 10 characters or less")]
            public string orderCode { get; set; }

            /// <summary>
            /// Gets or sets resultBinaryFile
            /// </summary>
            ///           
            public byte[] resultBinaryFile { get; set; }

            /// <summary>
            /// Gets or sets resultBinaryStream
            /// </summary>
            ///            
            public byte[] resultBinaryStream { get; set; }

            
            public string fileName { get; set; }

        }

  }
