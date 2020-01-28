using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AwesomeDesk.Extensions;

namespace AwesomeDesk.Models
{
  
    public class CustomerListTicketViewModel
    {
        [Display(Name = "ID"), Key]
        public int TiH_ID { get; set; }
        [Display(Name = "Temat")]
        public string TiH_Subject { get; set; }
        [Display(Name = "Treść")]
        public string TiP_Content { get; set; }
        [Display(Name = "Data utworzenia")]
        public DateTime TiH_Date { get; set; }
        [Display(Name = "Przypisani serwisanci")]
        public List<string> Assistants { get; set; }

        [Display(Name = "Status zgłoszenia")]
        public string TiS_Name { get; set; }

        public int MailState { get; set; }
    }
    public class CustomerCreateTicketViewModel
    {
        [Display(Name = "ID Zgłoszenia"), Key]
        public int TiH_ID { get; set; }

        [Required, Display(Name = "Temat")]
        public string TiH_Subject { get; set; }
        [Display(Name = "Treść"), DataType(DataType.MultilineText)]
        public string TiP_Content { get; set; }
    }
    public class CustomerDetailsTicketViewModel
    {
        public int TiH_ID { get; set; }
        public int TiP_LP { get; set; }

        [Display(Name = "Data")]
        public DateTime TiP_Date { get; set; }

        public int AssOrCus { get; set; }
        [Display(Name = "Treść")]
        public string TiP_Content { get; set; }

        public string OperatorName { get; set; }

    }
    public class CustomerAddResponseViewModel
    {
        public List<CustomerDetailsTicketViewModel> CustomerDetailsTickets { get; set; }
        [Display(Name = "Odpowiedź")]
        public string NewPositionContent { get; set; }
    }

    public class AssistantListTicketViewModel
    {

        [Display(Name = "ID"), Key]
        public int TiH_ID { get; set; }
        [Display(Name = "Temat")]
        public string TiH_Subject { get; set; }
        [Display(Name = "Treść")]
        public string TiP_Content { get; set; }
        [Display(Name = "Data utworzenia")]
        public DateTime TiH_Date { get; set; }
        [Display(Name = "Przypisani serwisanci")]
        public List<string> Assistants { get; set; }

        [Display(Name = "Firma")]
        public string CmP_Name { get; set; }

        [Display(Name = "Przypisani klienci")]
        public List<string> Customers { get; set; }

        [Display(Name = "Status zgłoszenia")]
        public string TiS_Name { get; set; }

        public List<TicketState> TicketStates { get; set; }



        public int MailState { get; set; }

    }
    public class AssistantCreateTicketViewModel
    {
        [Display(Name = "ID Zgłoszenia"), Key]
        public int TiH_ID { get; set; }

        [Required, Display(Name = "Temat")]
        public string TiH_Subject { get; set; }
        [Display(Name = "Treść"), DataType(DataType.MultilineText)]
        public string TiP_Content { get; set; }
        public List<Assistant> Assistants { get; set; }
        [Display(Name = "Przypisz asystenta")]
        public string TiP_ASSID { get; set; }

        public List<Company> Companies { get; set; }
        [Display(Name = "Przypisz firmę")]
        public int TiH_CMPID { get; set; }
    }
    public class AssistantDetailsTicketViewModel
    {
        public int TiH_ID { get; set; }
        public int TiP_LP { get; set; }

        [Display(Name = "Data")]
        public DateTime TiP_Date { get; set; }

        public int AssOrCus { get; set; }
        [Display(Name = "Treść")]
        public string TiP_Content { get; set; }

        public string OperatorName { get; set; }

    }
    public class AssistantAddResponseViewModel
    {
        public List<AssistantDetailsTicketViewModel> AssistantDetailsTickets { get; set; }
        
        public TicketWorkLog TicketWorkLog { get; set; }
        [Required]
        [Display(Name = "Odpowiedź"), MinLength(3, ErrorMessage = "Pole musi miec minimum {1} znaków "), MaxLength(25000, ErrorMessage = "Pole musi miec maksimum {1} znaków)")]
        public string NewPositionContent { get; set; }

    }
    public class CustomerListViewModel
    { 
        [Display(Name = "Imię klienta")]
        public string CuS_Name { get; set; }
        [Display(Name = "Nazwisko klienta")]
        public string CuS_Surname { get; set; }
        [Display(Name = "E-mail klienta"),Key]
        public string CuS_Email { get; set; }

        [Display(Name = "Numer telefonu klienta")]
        public string CuS_PhoneNumber { get; set; }

        [Display(Name = "Firma")]
        public string CmP_Name{ get; set; }

     
    }
    public class CustomerCreateViewModel
    {
        [StringLength(100, ErrorMessage = " {0} musi mieć minimum {2} znaków długości", MinimumLength = 2)]
        [Display(Name = "Imię klienta")]
        public string CuS_Name { get; set; }

        [StringLength(100, ErrorMessage = " {0} musi mieć minimum {2} znaków długości", MinimumLength = 2)]
        [Display(Name = "Nazwisko klienta")]
        public string CuS_Surname { get; set; }

        [Display(Name = "E-mail klienta"), Key]
        [Required]
        [EmailAddress]
        public string CuS_Email { get; set; }
        [Display(Name = "Numer telefonu klienta"), RegularExpression("^[0-9]*$", ErrorMessage = "{0} może zawierać tylko liczby")]     
        public string CuS_PhoneNumber { get; set; }
        
        [Required]
        [Display(Name = "Hasło")]
        [StringLength(100, ErrorMessage = " {0} musi mieć minimum {2} znaków długości", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string CuS_Password { get; set; }
        public List<Company> Companies { get; set; }
        [Display(Name = "Firma")]
        public int CuS_CMPID { get; set; }
    }


    public class TicketWorkLogViewModel
    {
        [Display(Name = "Data rozpoczęcia")]
        public DateTime TwL_StartDate { get; set; }
        [Display(Name = "Data zakończenia"), DateTimeNotLessThan("TwL_StartDate", "time")]

    
        public DateTime TwL_EndDate { get; set; }
        [Display(Name = "Minuty")]
        public int TwL_SpendMinutes { get; set; }

        [Display(Name = "Godziny")]
        public int TwL_SpendHours { get; set; }
        [Display(Name = "Opis")]
        public string TwL_Description { get; set; }
        [Display(Name = "Czy opis ma być widoczny dla klienta?")]
        public bool TwL_PublicDescription { get; set; }



        public int? TwL_TIHID { get; set; }

    }



}