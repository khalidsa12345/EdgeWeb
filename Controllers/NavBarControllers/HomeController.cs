using EdgeWEB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EdgeWEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult AboutUs() => View();
        public IActionResult Services() => View();
        public IActionResult Clients() => View();
        public IActionResult Contact() => View();

        [HttpGet]
        public IActionResult RequestServices()
        {
            return View(new RequestServiceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitServiceRequest(RequestServiceViewModel model)
        {
            // Check if at least one service is selected
            if (!(model.StrategyPlanning || model.PMO || model.Governance ||
                  model.ISO || model.Digital || model.Financial ||
                  model.Sustainability || model.HR || model.BusinessProcess))
            {
                ModelState.AddModelError("Services", "Please select at least one service.");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                return View("RequestServices", model);
            }

            try
            {
                // Build selected services list
                var selectedServices = new List<string>();
                if (model.StrategyPlanning) selectedServices.Add("Strategic Planning & Performance");
                if (model.PMO) selectedServices.Add("Project Management (PMO)");
                if (model.Governance) selectedServices.Add("Governance & Risk Management");
                if (model.ISO) selectedServices.Add("ISO & Excellence Models");
                if (model.Digital) selectedServices.Add("Digital & Technology Solutions");
                if (model.Financial) selectedServices.Add("Financial Advisory");
                if (model.Sustainability) selectedServices.Add("Sustainability & Energy");
                if (model.HR) selectedServices.Add("HR & Restructuring");
                if (model.BusinessProcess) selectedServices.Add("Business Process Management");
                if (model.AI) selectedServices.Add("AI & Data Analytics");
                if (model.CyberSecurity) selectedServices.Add("Cybersecurity & Compliance");

                // Build email body
                var emailBody = new StringBuilder();
                emailBody.AppendLine("NEW SERVICE REQUEST FROM EDGE CONSULTING WEBSITE");
                emailBody.AppendLine("=====================================================");
                emailBody.AppendLine();

                emailBody.AppendLine("SELECTED SERVICES:");
                emailBody.AppendLine("------------------");
                foreach (var service in selectedServices)
                {
                    emailBody.AppendLine($"✓ {service}");
                }
                emailBody.AppendLine();

                emailBody.AppendLine("CONTACT INFORMATION:");
                emailBody.AppendLine("--------------------");
                emailBody.AppendLine($"Name: {model.PersonName}");
                emailBody.AppendLine($"Email: {model.Email}");
                emailBody.AppendLine($"Mobile: {model.MobileNumber}");
                emailBody.AppendLine($"Company: {model.CompanyName ?? "Not provided"}");
                emailBody.AppendLine();

                if (model.AddNote == "Yes" && !string.IsNullOrWhiteSpace(model.Notes))
                {
                    emailBody.AppendLine("ADDITIONAL NOTES:");
                    emailBody.AppendLine("-----------------");
                    emailBody.AppendLine(model.Notes);
                    emailBody.AppendLine();
                }

                emailBody.AppendLine("=====================================================");
                emailBody.AppendLine($"Request submitted on: {DateTime.Now:dddd, MMMM dd, yyyy 'at' hh:mm tt}");

                // Create email
                var fromEmail = "kingkaloodfi@gmail.com";
                var mail = new MailMessage
                {
                    From = new MailAddress(fromEmail, "Edge Consulting Website"),
                    Subject = $"New Service Request - {model.PersonName}",
                    Body = emailBody.ToString(),
                    IsBodyHtml = false
                };

                // Send to your email
                mail.To.Add(fromEmail);

                // Configure SMTP
                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromEmail, "xybcvqqzmvvljvrm")
                };

                // Send email
                smtp.Send(mail);

                TempData["SuccessMessage"] = "Thank you! Your service request has been sent successfully. We will contact you within 24 hours.";
                return RedirectToAction(nameof(RequestServices));
            }
            catch (Exception ex)
            {
                // Log the error (you can add logging here)
                TempData["ErrorMessage"] = "An error occurred while sending your request. Please try again or contact us directly.";
                return View("RequestServices", model);
            }
        }
    }
}
