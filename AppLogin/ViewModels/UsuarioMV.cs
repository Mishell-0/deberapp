using AppLogin.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace AppLogin.ViewModels
{
    public class UsuarioMV
    {

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [RegularExpression(@"^[A-Z'\s]{1,40}$", ErrorMessage = "Debe estar en mayusculas")]
        [StringLength(40), ]
        public string NombreCompleto { get; set; }

        [CustomEmail(ErrorMessage = "El email no es válido.")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }

        [Required(ErrorMessage = "Confirmar clave es obligatorio.")]
        public string ConfirmarClave { get; set; }
    }
}
