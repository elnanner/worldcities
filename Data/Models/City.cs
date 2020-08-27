using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldCities.Data.Models
{
    public class City
    {
        #region Contructor
        public City()
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// The unique id and primary key for this city
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// City name (in UTF-8 format)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// City name (in ASCII format)
        /// </summary>
        public string Name_ASCII { get; set; }


        /// <summary>
        /// City latitude
        /// </summary>
        [Column(TypeName = "decimal(7,4)")]
        public decimal Lat { get; set; }

        /// <summary>
        /// City longitude
        /// </summary>
        [Column(TypeName = "decimal(7,4)")]
        public decimal Lon { get; set; }

        /// <summary>
        /// Country Id (foreing key)
        /// </summary>
        [ForeignKey("Country")]
        public int CountryId
        { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// The coountry related to this city
        /// </summary>
        public virtual Country Country { get; set; }
        #endregion
    }
}
