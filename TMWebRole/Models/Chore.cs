using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TMWebRole.Models
{
    /// <summary>
    /// Enum for Priority types
    /// </summary>
    public enum Priority
    {
        None,
        Low,
        Medium,
        High
    }

    /// <summary>
    /// Enum for Status types
    /// </summary>
    public enum Status
    {
        Incomplete,
        Completed
    }

    /// <summary>
    /// Enum for Recurrence types
    /// </summary>
    public enum Recurrence
    {
        None,
        [Display(Name = "Every Day")]
        Every_Day,
        [Display(Name = "Every Week")]
        Every_Week,
        [Display(Name = "Every Month")]
        Every_Month,
        [Display(Name = "Every Year")]
        Every_Year
    }

    /// <summary>
    /// This is the Entity representing a Chore
    /// </summary>
    public class Chore
    {
        public int ChoreId { get; set; }     //Id of the task

        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }    //Name of the task

        [StringLength(256)]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }    //Notes

        [Display(Name = "Begins")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; } //Date when to begin the task

        [Display(Name = "Ends")]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }   //Date when to end the task

        [DataType(DataType.DateTime)]
        public DateTime Reminder { get; set; }  //Date and time for a reminder

        public Recurrence? Recurrence { get; set; } //Task recurrency

        public Priority? Priority { get; set; } //Task priority

        public Status? Status { get; set; } //Task status

        public string Location { get; set; }    //Short friendly location to perform the task

        public string Attachment { get; set; }  //File attachment to the task

        public string User { get; set; }  //User that the chore belongs to

        public virtual Category Category { get; set; }  //Task category

    }
}