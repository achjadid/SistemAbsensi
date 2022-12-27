using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AttendanceHistory
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public string NIK { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Reason { get; set; }
        public DateTime? ResponseDate { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public enum ResponseStatus { Request, Approve, Reject }
}
