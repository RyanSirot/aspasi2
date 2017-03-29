using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ZenithSociety.Models.Business
{
    [ModelMetadataType(typeof(ActivityMetaData))]
    public partial class Activity
    {
        [Key]
        public int ActivityId { get; set; }


        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public virtual List<Event> Events { get; set; }

    }

    public class ActivityMetaData
    {
        [Required]
        public object Description { get; set; }
    }
}
