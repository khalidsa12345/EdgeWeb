using EdgeWEB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        public async Task<IActionResult> SubmitServiceRequest(RequestServiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill all required fields.";
                return View("RequestServices", model);
            }

            try
            {
                // Build services list
                var services = new List<string>();
                if (model.StrategyPlanning) services.Add("Strategic Planning & Performance");
                if (model.PMO) services.Add("Project Management (PMO)");
                if (model.Governance) services.Add("Governance & Risk Management");
                if (model.ISO) services.Add("ISO & Excellence Models");
                if (model.Digital) services.Add("Digital & Technology Solutions");
                if (model.Financial) services.Add("Financial Advisory");
                if (model.Sustainability) services.Add("Sustainability & Energy");
                if (model.HR) services.Add("HR & Restructuring");
                if (model.BusinessProcess) services.Add("Business Process Management");
                if (model.AI) services.Add("Artificial Intelligence");
                if (model.CyberSecurity) services.Add("Cyber Security");

                string servicesHtml = "";
                foreach (var s in services)
                    servicesHtml += $"<li>{s}</li>";

                // Email body
                string body = $@"
                <html>
                <body style='font-family: Arial; background:#f4f6f8; padding:20px;'>
                <div style='max-width:600px; margin:auto; background:#fff; border-radius:10px; overflow:hidden;'>
                    <div style='background:#0b1f3a; color:#fff; padding:20px; text-align:center;'>
                        <h2>Edge Consulting</h2>
                        <p>New Service Request</p>
                    </div>
                    <div style='padding:20px;'>
                        <h3>Selected Services</h3>
                        <ul>{servicesHtml}</ul>
                        <hr/>
                        <h3>Contact Info</h3>
                        <p><b>Name:</b> {model.PersonName}</p>
                        <p><b>Email:</b> {model.Email}</p>
                        <p><b>Mobile:</b> {model.MobileNumber}</p>
                        <p><b>Company:</b> {model.CompanyName}</p>
                        <hr/>
                        <h3>Notes</h3>
                        <p>{model.Notes}</p>
                    </div>
                    <div style='background:#eee; padding:10px; text-align:center; font-size:12px;'>
                        {DateTime.Now}
                    </div>
                </div>
                </body>
                </html>";

                // ✅ Resend API via HttpClient
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer re_3Sgxiids_KVuMyaWz76933aFQauWTb4LX");

                var payload = new
                {
                    from = "Edge Website <onboarding@resend.dev>",
                    to = new[] { "info@edgesline.com" },
                    reply_to = model.Email,
                    subject = $"New Request from {model.PersonName}",
                    html = body
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await httpClient.PostAsync("https://api.resend.com/emails", content);

                if (!result.IsSuccessStatusCode)
                {
                    var error = await result.Content.ReadAsStringAsync();
                    throw new Exception("Resend error: " + error);
                }

                TempData["SuccessMessage"] = "Request sent successfully!";
                return RedirectToAction("RequestServices");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "ERROR: " + ex.Message + " | " + ex.InnerException?.Message;
                return View("RequestServices", model);
            }
        }
    }
}