namespace BlazorCookieAuth.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DataItem : ICloneable
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        [Range(0, 60, ErrorMessage = "Value A must be between 0 and 60.")]
        public int ValueA { get; set; }

        public int ValueB { get; set; }

        [Required]
        public string Code { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
