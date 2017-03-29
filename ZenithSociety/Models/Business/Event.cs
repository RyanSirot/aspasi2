using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZenithSociety.Models.CustomValidation;

namespace ZenithSociety.Models.Business
{
    [ModelMetadataType(typeof(EventMetaData))]
    public partial class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Event From")]
        public DateTime EventFrom { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Event To")]
        public DateTime EventTo { get; set; }

        [DataType(DataType.Date)]
        [HiddenInput(DisplayValue = false)]
        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public int ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public virtual Activity Activity { get; set; }
    }

    public class EventMetaData
    {
      
        [DateRange]
        public object EventFrom { get; set; }

     
        [DateTimeFromTo(ErrorMessage = "{0} is greater than the Date of Event From")]
        [DateRange]
        public object EventTo { get; set; }
    }
}
