using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AwesomeDesk.Models
{
    
    public class Company
    {
        public virtual ICollection<Customer> Customers { get; set; }
        [Key]
        public int CmP_ID { get; set; }
        [Display(Name = "Firma"),StringLength(60, MinimumLength = 3)]
        public string CmP_Name { get; set; }
        [Display(Name="Telefon (sekretariat)")]
        public string CmP_PhoneNumber { get; set; }
        [Display(Name = "Adres strony")]
        public string CmP_PageAdress { get; set;}
    }
    public class Operator : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Operator> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<OperatorCustomizationDefinition> OperatorCustomizationDefinitions { get; set; }
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        public override string Email { get => base.Email; set => base.Email = value; }
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        
    }
    public class Customer:Operator
    {
        public virtual ICollection<TicketPosition> TicketPositions { get; set; }
        public virtual ICollection<TicketHeaderCustomer> TicketHeaderCustomers { get; set; }

        [Display(Name = "Firma"),ForeignKey("Company")]
        public int CuS_CMPID { get; set; }
        public Company Company { get; set; }
        [Display(Name = "Imię")]
        public string CuS_Name { get; set; }
        [Display(Name = "Nazwisko")]
        public string CuS_Surname { get; set; }
        public string CuS_Image { get; set; }
        [Display(Name = "Adres e-mail")]
        public string CuS_Email { get => base.Email; set => base.Email = value; }
        [Display(Name = "Login")]
        public string CuS_Login { get => base.UserName; set => base.UserName = value; }
        [Display(Name = "Numer telefonu")]

        public string CuS_PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
    }
    public class Assistant:Operator
    {
        public virtual ICollection<TicketPosition> TicketPositions { get; set; }
        public virtual ICollection<TicketHeaderAssistant> TicketHeaderAssistants { get; set; }

        [Display(Name = "Imię")]
        public string AsS_Name { get; set; }
        [Display(Name = "Nazwisko")]
        public string AsS_Surname { get; set; }        
        public string AsS_Image { get; set; }
        [Display(Name = "Adres e-mail")]
        public string AsS_Email { get => base.Email; set => base.Email=value; }
        [Display(Name = "Login")]
        public string AsS_Login { get => base.UserName; set => base.UserName = value; }
        [Display(Name = "Numer telefonu")]

        public string AsS_PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
    }
    public class TicketHeader
    {
        public virtual ICollection<TicketPosition> TicketPositions { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketWorkLog> TicketWorkLogs { get; set; }
        public virtual ICollection<TicketHeaderAssistant> TicketHeaderAssistants { get; set; }
        public virtual ICollection<TicketHeaderCustomer> TicketHeaderCustomers { get; set; }

        [Key,Display(Name = "ID Zgłoszenia")]
        public int TiH_ID { get; set; }
        [Display(Name = "Temat"),StringLength(150, MinimumLength = 8)]
        public string TiH_Subject { get; set; }

        [ForeignKey("TicketState")]
        public int TiH_TiTID { get; set; }
        public TicketState TicketState { get; set; }


        [ForeignKey("TicketType")]
        public int TiH_TiSID { get; set; }
        public TicketType TicketType { get; set; }         

    }
    public class TicketPosition 
    {
        [Key]
        public int TiP_ID { get; set; }
        [ForeignKey("TicketHeader")]
        public int TiP_TiHID { get; set; }
        public TicketHeader TicketHeader { get; set; }

        public int TiP_LP { get; set; }
        [Display(Name = "Treść zgłoszenia"),StringLength(3000, MinimumLength = 8)]
        public string TiP_Content {get;set;}            

        public DateTime TiP_Date { get; set; }

        public Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public string TiP_CUSID { get; set; }

        public Assistant Assistant { get; set; }
        [ForeignKey("Assistant")]
        public string TiP_ASSID { get; set; }
    }
    public class TicketState
    {
        public virtual ICollection<TicketHeader> TicketHeaders { get; set; }

        [Key]
        public int TiS_ID { get; set; }        
        [Display(Name = "Nazwa statusu"),StringLength(50,MinimumLength =5), ]
        public string TiS_Name { get; set; }

        [Display(Name = "Opis statusu"), StringLength(250, MinimumLength = 5)]        
        public string TiS_Description { get; set; }
    }
    public class TicketType
    {
        public virtual ICollection<TicketHeader> TicketHeaders { get; set; }
        [Key]
        public int TiT_ID { get; set; }

        [Display(Name = "Nazwa typu"), StringLength(50, MinimumLength = 5)]        
        public string TiT_Name { get; set; }

        [Display(Name = "Opis typu"), StringLength(250, MinimumLength = 5)]        
        public string TiT_Description { get; set; }
    }
    public class TicketHistory
    {
        [Key]
        public int ThS_ID { get; set; }
        [ForeignKey("TicketHeader")]        
        public int ThS_TiHID { get; set; }
        public TicketHeader TicketHeader { get; set; }
        [Display(Name = "LP")]
        public int ThS_LP { get; set; }
        public string ThS_Description { get; set; }
    }
    public class TicketWorkLog
    {
        [Key]
        public int TwL_ID { get; set; }
        [ForeignKey("TicketHeader")]
        public int? TwL_TiHID { get; set; }
        public TicketHeader TicketHeader { get; set; }

        [Display(Name = "Data rozpoczęcia")]
        public DateTime TwL_StartDate { get; set; }
        [Display(Name = "Data zakończenia")]
        public DateTime TwL_EndDate { get; set; }
        public int TwL_SpendMinutes { get; set; }
        [Display(Name = "Opis")]
        public string TwL_Description { get; set; }
        [Display(Name = "Czy opis ma być widoczny dla klienta?")]
        public bool TwL_PublicDescription { get; set; }

    }
    public class SystemConfig
    {
        [Key]
        public int SyC_ID { get; set; }
        public string SyC_Value { get; set; }
        public string SyC_Description { get; set; }
    }
    public class CustomizationDefinition
    {
        public virtual ICollection<OperatorCustomizationDefinition> OperatorCustomizationDefinitions { get; set; }
        [Key]
        public int CsD_ID { get; set; }
        public string CsD_DefinitionName { get; set; }
        public string CsD_Value { get; set; }
        public string CsD_Description { get; set; }
    }
    public class OperatorCustomizationDefinition
    {
        [Key, Column(Order = 1),ForeignKey("Operator")]
        public string OcD_OPEID { get; set; }
        public Operator Operator { get; set; }

        [Key, Column(Order = 2),ForeignKey("CustomizationDefinition")]
        public int OcD_CSDID { get; set; }
        public CustomizationDefinition CustomizationDefinition { get; set; }


    }
    public class TicketHeaderAssistant
    {
        [Key, Column(Order = 1), ForeignKey("Assistant")]
        public string TiA_AsSID { get; set; }
        public Assistant Assistant { get; set; }

        [Key, Column(Order = 2), ForeignKey("TicketHeader")]
        public int TiA_TiHID { get; set; }
        public TicketHeader TicketHeader { get; set; }
    }
    public class TicketHeaderCustomer
    {
        [Key]
        public int TiC_ID { get; set; }
        [ ForeignKey("Customer")]
        public string TiC_CuSID { get; set; }
        public Customer Customer { get; set; }

        [ ForeignKey("TicketHeader")]
        public int TiC_TiHID { get; set; }
        public TicketHeader TicketHeader { get; set; }

    }
}