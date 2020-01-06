using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
    public class AssistantCreateTicketViewModel
    {
        [Display(Name = "ID Zgłoszenia"), Key]
        public int TiH_ID { get; set; }

        [Required, Display(Name = "Temat")]
        public string TiH_Subject { get; set; }
        [Display(Name = "Treść"), DataType(DataType.MultilineText)]
        public string TiP_Content { get; set; }
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
        [Display(Name = "Odpowiedź")]
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
        [Display(Name = "Imię klienta")]
        public string CuS_Name { get; set; }
        [Display(Name = "Nazwisko klienta")]
        public string CuS_Surname { get; set; }
        [Display(Name = "E-mail klienta"), Key]
        public string CuS_Email { get; set; }

        [Display(Name = "Numer telefonu klienta")]
        public string CuS_PhoneNumber { get; set; }
        [Display(Name = "Firma")]
        public int CuS_CMPID { get; set; }
        public List<Company> Companies { get; set; }
    }




    public class DesctiptionViewModel
    {

        public string Version { get; set; }
        public string Content { get; set; }
    }

}