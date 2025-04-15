using HelpDesk.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    [Table("Cat_Plaza")]
    public class Cat_Plaza
    {
        [Key]
        [Column("IdPlaza")]
        public int IdPlaza { get; set; }

        [Column("Nombre")]
        [MaxLength(100)]
        public required string Nombre { get; set; }

        [Column("Activo")]
        public bool? Activo { get; set; }
    }
}