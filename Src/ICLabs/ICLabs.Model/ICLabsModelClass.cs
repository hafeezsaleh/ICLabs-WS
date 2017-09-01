using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace ICLabs.Model
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
        /// Class for getting application
        /// </summary>
    public class ClsGetApplication : ClsBase
        {
            /// <summary>
            /// Gets or sets appId
            /// </summary>
            public string appId { get; set; }

           
        }

        /// <summary>
        /// Class for application
        /// </summary>
        public class ClsApplication
        {
            /// <summary>
            /// Gets or sets appId
            /// </summary>
            public string appId { get; set; }

            /// <summary>
            /// Gets or sets clientSecret
            /// </summary>
            public string orgId { get; set; }

            /// <summary>
            /// Gets or sets clientSecret
            /// </summary>
            public string clientSecret { get; set; }

            /// <summary>
            /// Gets or sets clientSecret
            /// </summary>
            public string accessLevel { get; set; }

            /// <summary>
            /// Gets or sets clientSecret
            /// </summary>
            public string ipAddress { get; set; }

            /// <summary>
            /// Gets or sets clientSecret
            /// </summary>
            public string lastAccessed { get; set; }

        }

        /// <summary>
        /// Class for order
        /// </summary>
        public class ClsOrder : ClsBase
        {

            public override string ToString()
            {                
                return "orderId:" + orderId ;
            }

            /// <summary>
            /// Gets or sets locationId
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required.")]
            [MaxLength(4, ErrorMessage = "Must be 4 characters or less")]
            public string locationId { get; set; }

            /// <summary>
            /// Gets or sets appId
            /// </summary>
            /// 
            //[Required(ErrorMessage = "Field required.")]
            public string appId { get; set; }

            /// <summary>
            /// Gets or sets Alt1
            /// </summary>
            /// 
            [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt1 { get; set; }

            /// <summary>
            /// Gets or sets Alt10
            /// </summary>
            /// 
            [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt10 { get; set; }

            /// <summary>
            /// Gets or sets Alt2
            /// </summary>
            /// 
            [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt2 { get; set; }

            /// <summary>
            /// Gets or sets Alt3
            /// </summary>
            /// 
            [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt3 { get; set; }

            /// <summary>
            /// Gets or sets Alt4
            /// </summary>
            /// 
            [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt4 { get; set; }

            /// <summary>
            /// Gets or sets Alt5
            /// </summary>
            /// 
            [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt5 { get; set; }

            /// <summary>
            /// Gets or sets Alt6
            /// </summary>
            /// 
             [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt6 { get; set; }

            /// <summary>
            /// Gets or sets Alt7
            /// </summary>
            /// 
             [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt7 { get; set; }

            /// <summary>
            /// Gets or sets Alt9
            /// </summary>
            /// 
             [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
            public string alt9 { get; set; }

            /// <summary>
            /// Gets or sets ClientComments
            /// </summary>
            /// 
            [MaxLength(1000, ErrorMessage = "Must be 1000 characters or less")]
            public string clientComments { get; set; }

            /// <summary>
            /// Gets or sets ClientFileNumber
            /// </summary>
            /// 
            [MaxLength(15, ErrorMessage = "Must be 15 characters or less")]
            public string clientFileNumber { get; set; }

            /// <summary>
            /// Gets or sets ClientId
            /// </summary>
            /// 

             [Required(ErrorMessage = "Field required.")]
             [MaxLength(8, ErrorMessage = "Must be 8 characters or less")]
            public string clientId { get; set; }

            /// <summary>
            /// Gets or sets ClientName
            /// </summary>
            /// 

             [Required(ErrorMessage = "Field required.")]
             [MaxLength(30, ErrorMessage = "Must be 30 characters or less")]
            public string clientName { get; set; }

            /// <summary>
            /// Gets or sets CollectionDate
            /// </summary>
            ///
            [Required(ErrorMessage = "Field required")]
            [RegularExpression(@"^(19|20)\d\d(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "In the format of YYYYMMDD (YYYY-year;MM-month,DD-day)")]    
            public string collectionDate { get; set; }

            /// <summary>
            /// Gets or sets CollectionTime
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required")]
            [RegularExpression(@"^([0-1]?[0-9]|2[0-3])([0-5][0-9])([0-5][0-9])?$", ErrorMessage = "In format of HHMM (HH-24hour format e.g. 01 for 1 AM 22 for 10PM; MM-minutes)")]
            public string collectionTime { get; set; }

            /// <summary>
            /// Gets or sets DOB
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required")]
            [RegularExpression(@"^(19|20)\d\d(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "In the format of YYYYMMDD (YYYY-year;MM-month,DD-day)")]
            public string dOB { get; set; }

            /// <summary>
            /// Gets or sets FirstName
            /// Patient First Name
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required")]
            [MaxLength(25, ErrorMessage = "Must be 25 characters or less")]
            public string firstName { get; set; }

            /// <summary>
            /// Gets or sets LastName
            /// Patient Last Name
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required")]
            [MaxLength(25, ErrorMessage = "Must be 25 characters or less")]
            public string lastName { get; set; }

            /// <summary>
            /// Gets or sets MiddleName
            /// Patient Middle Names
            /// </summary>
            ///             
            [MaxLength(25, ErrorMessage = "Patient Middle Name must be 25 characters or less")]
            public string middleName { get; set; }

            /// <summary>
            /// Gets or sets OrderId
            /// ICL Accession number
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required")]
            [MaxLength(20, ErrorMessage = "Must be 20 characters")]
            public string orderId { get; set; }

            /// <summary>
            /// Gets or sets Orders
            /// </summary>
            /// 
             [MaxLength(1000, ErrorMessage = "Must be 1000 characters or less")]
            public string orders { get; set; }

            /// <summary>
            /// Gets or sets PatientAddress1
            /// </summary>
            /// 
            [MaxLength(30, ErrorMessage = "Must be 30 characters or less")]
            public string patientAddress1 { get; set; }

            /// <summary>
            /// Gets or sets PatientAddress2
            /// </summary>
            /// 
            [MaxLength(30, ErrorMessage = "Must be 30 characters or less")]
            public string patientAddress2 { get; set; }


            /// <summary>
            /// Gets or sets PatientAddressCity
            /// </summary>
            /// 

            [MaxLength(30, ErrorMessage = "Must be 30 characters or less")]
            public string patientAddressCity { get; set; }

            /// <summary>
            /// Gets or sets PatientAddressCountry
            /// </summary>
            /// 
            [MaxLength(12, ErrorMessage = "Must be 12 characters or less")]
            public string patientAddressCountry { get; set; }

            /// <summary>
            /// Gets or sets PatientAddressPostalCode
            /// </summary>
            /// 
            [MaxLength(12, ErrorMessage = "Must be 12 characters or less")]
            public string patientAddressPostalCode { get; set; }

            /// <summary>
            /// Gets or sets PatientAddressProvince
            /// </summary>
            /// 
            [MaxLength(2, ErrorMessage = "Must be 2 characters or less")]
            public string patientAddressProvince { get; set; }

            /// <summary>
            /// Gets or sets PatientCellPhone
            /// </summary>
            /// 
            [MaxLength(20, ErrorMessage = "Must be 20 characters or less")]
            public string patientCellPhone { get; set; }

            /// <summary>
            /// Gets or sets PatientHomePhone
            /// </summary>
            /// 
            [MaxLength(20, ErrorMessage = "Must be 20 characters or less")]
            public string patientHomePhone { get; set; }

            /// <summary>
            /// Gets or sets PatientPHN
            /// </summary>
            /// 
            [MaxLength(12, ErrorMessage = "Must be 12 characters or less")]
            public string patientPHN { get; set; }

            /// <summary>
            /// Gets or sets Priority
            /// </summary>
            /// 

             [Required(ErrorMessage = "Field required")] 
            public string priority { get; set; }

                      
            /// <summary>
            /// Gets or sets Sex
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required")]            
            public string sex { get; set; }

         }


        /// <summary>
        /// Class for AuditLog
        /// </summary>
        public class ClsAuditLog
        {
            /// <summary>
            /// Gets or sets appId
            /// </summary>
            public string appId { get; set; }

            /// <summary>
            /// Gets or sets actDate
            /// </summary>
            public DateTime actDate { get; set; }

            /// <summary>
            /// Gets or sets recordId
            /// </summary>
            public string recordId { get; set; }

            /// <summary>
            /// Gets or sets recordType
            /// </summary>
            public string recordType { get; set; }

            /// <summary>
            /// Gets or sets eventData
            /// </summary>
            public string eventData { get; set; }

        }


        /// <summary>
        /// Class for GetOrderStatus
        /// </summary>
        public class ClsGetResultStatus : ClsBase
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
            /// Gets or sets test
            /// </summary>
            /// 
            public string test { get; set; }


            public string appId { get; set; }
        }

        /// <summary>
        /// Class for ResultsStatus
        /// </summary>
        public class ClsResultsStatus : ClsBase
        {
            /// <summary>
            /// Gets or sets Id
            /// </summary>
            //public string Id { get; set; }            

            /// <summary>
            /// Gets or sets orderId
            /// </summary>
            ///
            [MaxLength(10, ErrorMessage = "Must be 10 characters or less")]
            public string orderId { get; set; }

            /// <summary>
            /// Gets or sets resultsStatus
            /// </summary>
            /// 
            [MaxLength(8, ErrorMessage = "Must be 8 characters or less")]
            public string resultsStatus { get; set; }

            /// <summary>
            /// Gets or sets test
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required.")]
            public string test { get; set; }

        }



        /// <summary>
        /// Class for GetOrderStatus
        /// </summary>
        public class ClsGetOrderStatus : ClsBase
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


            public string appId { get; set; }
        }




        /// <summary>
        /// Class for OrderStatus
        /// </summary>
        public class ClsOrderStatus : ClsBase
        {
            ///// <summary>
            ///// Gets or sets Id
            ///// </summary>
            //public string Id { get; set; }

            /// <summary>
            /// Gets or sets orderId
            /// </summary>
            ///
            [MaxLength(20, ErrorMessage = "Must be 20 characters or less")]
            public string orderId { get; set; }


            public string locationId { get; set; }


            /// <summary>
            /// Gets or sets accessioned
            /// </summary>
            /// 
            [MinLength(1, ErrorMessage = "VALUELIST = N,Y")]
            [MaxLength(1, ErrorMessage = "VALUELIST = N,Y")]
            public string accessioned { get; set; }


            /// <summary>
            /// Gets or sets demographicChange
            /// </summary>
            /// 
            [MinLength(1, ErrorMessage = "VALUELIST = N,Y")]
            [MaxLength(1, ErrorMessage = "VALUELIST = N,Y")]
            public string demographicChange { get; set; }


            /// <summary>
            /// Gets or sets ordersModified
            /// </summary>
            /// 
            [MinLength(1, ErrorMessage = "VALUELIST = N,Y")]
            [MaxLength(1, ErrorMessage = "VALUELIST = N,Y")]
            public string ordersModified { get; set; }



            /// <summary>
            /// Gets or sets partialResults
            /// </summary>
            /// 
            [MinLength(1, ErrorMessage = "VALUELIST = N,Y")]
            [MaxLength(1, ErrorMessage = "VALUELIST = N,Y")]
            public string partialResults { get; set; }


            /// <summary>
            /// Gets or sets referredtosupplier
            /// </summary>
            /// 
            [MinLength(1, ErrorMessage = "VALUELIST = N,Y")]
            [MaxLength(1, ErrorMessage = "VALUELIST = N,Y")]
            public string referredToSupplier { get; set; }


            /// <summary>
            /// Gets or sets resultsfinal
            /// </summary>
            /// 
            [MinLength(1, ErrorMessage = "VALUELIST = N,Y")]
            [MaxLength(1, ErrorMessage = "VALUELIST = N,Y")]
            public string resultsFinal { get; set; }

        }


        /// <summary>
        /// Class for Test
        /// </summary>
        public class ClsTest : ClsBase
        {
            /// <summary>
            /// Gets or sets id
            /// </summary>
            //public int id { get; set; }

            /// <summary>
            /// Gets or sets test
            /// </summary>
            ///
            [MaxLength(10, ErrorMessage = "Must be 10 characters or less")]
            public string test { get; set; }

            /// <summary>
            /// Gets or sets testName
            /// </summary>
            ///           
            public string testName { get; set; }

            /// <summary>
            /// Gets or sets testPrice
            /// </summary>
            ///            
            public object testPrice { get; set; }
        }

           /// <summary>
        /// Class for Result
        /// </summary>
        public class ClsResult : ClsBase
        {
           

            /// <summary>
            /// Gets or sets clientId
            /// </summary>
            ///
            [MaxLength(8, ErrorMessage = "Must be 8 characters or less")]
            public string clientId { get; set; }

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
            /// Gets or sets orderCode
            /// </summary>
            ///
            [MaxLength(3, ErrorMessage = "Must be 3 characters or less")]
            [MinLength(1, ErrorMessage = "Must be 1 characters or greater")]
            public string resultFlag { get; set; }
         

            /// <summary>
            /// Gets or sets resultDate
            /// </summary>
            ///
            public string resultDate { get; set; }

            /// <summary>
            /// Gets or sets resultName
            /// </summary>
            ///           
            public string resultName { get; set; }

            /// <summary>
            /// Gets or sets resultText
            /// </summary>
            ///            
            public string resultText { get; set; }

            /// <summary>
            /// Gets or sets resultType
            /// </summary>
            ///            
            public string resultType { get; set; }

            /// <summary>
            /// Gets or sets resultUnits
            /// </summary>
            ///            
            public string resultUnits { get; set; }
          

            /// <summary>
            /// Gets or sets Results Normal Range
            /// </summary>
            ///            
            public string normalRange { get; set; }

            /// <summary>
            /// Gets or sets resCode
            /// </summary>
            ///            
            public string resCode { get; set; }

             /// <summary>
            /// Gets or sets supplierSite
            /// </summary>
            ///            
            public string supplierSite { get; set; }
            
        }

        /// <summary>
        /// Class for ClsGetResult
        /// </summary>
        public class ClsGetResult : ClsBase
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
            //[Required(ErrorMessage = "Field required.")]
            [MaxLength(10, ErrorMessage = "Must be 10 characters or less")]
            public string test { get; set; }

            public string appId { get; set; }
        }

        public class ClsPostResult : ClsBase
        {

            /// <summary>
            /// Gets or sets locationId
            /// </summary>
            /// 
            [Required(ErrorMessage = "Field required.")]
            [MaxLength(4, ErrorMessage = "Must be 4 characters or less")]
            public string locationId { get; set; }

            public string appId { get; set; }

            /// <summary>
            /// Gets or sets clientId
            /// </summary>
            ///            
            public string clientId { get; set; }

            /// <summary>
            /// Gets or sets orderId
            /// </summary>
            ///           
            public string orderId { get; set; }

            /// <summary>
            /// Gets or sets orderCode
            /// </summary>
            ///
            [MaxLength(10, ErrorMessage = "Must be 10 characters or less")]
            public string orderCode { get; set; }

            /// <summary>
            /// Gets or sets orderCode
            /// </summary>
            ///
            [MaxLength(3, ErrorMessage = "Must be 3 characters or less")]
            [MinLength(1, ErrorMessage = "Must be 1 characters or greater")]
            public string resultFlag { get; set; }


            /// <summary>
            /// Gets or sets resultDate
            /// </summary>
            ///
            public string resultDate { get; set; }

            /// <summary>
            /// Gets or sets resultName
            /// </summary>
            ///           
            public string resultName { get; set; }

            /// <summary>
            /// Gets or sets resultText
            /// </summary>
            ///            
            public string resultText { get; set; }

            /// <summary>
            /// Gets or sets resultType
            /// </summary>
            ///            
            public string resultType { get; set; }

            /// <summary>
            /// Gets or sets resultUnits
            /// </summary>
            ///            
            public string resultUnits { get; set; }


            /// <summary>
            /// Gets or sets Results Normal Range
            /// </summary>
            ///            
            public string normalRange { get; set; }

            /// <summary>
            /// Gets or sets resultBinaryStream
            /// </summary>
            ///            
            public byte[] resultBinaryStream { get; set; }


            public string fileName { get; set; }
        }


      
}
