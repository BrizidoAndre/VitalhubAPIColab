﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utils.Mail;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : ControllerBase
    {
        private readonly IEmailService emailService;


        public SendMailController(IEmailService service)
        {
            emailService = service;
        }



        private string GetHtmlContentRecovery(int codigo)
        {
            string Response = @"
<div style=""width:100%; background-color:rgba(96, 191, 197, 1); padding: 20px;"">
    <div style=""max-width: 600px; margin: 0 auto; background-color:#FFFFFF; border-radius: 10px; padding: 20px;"">
        <img src=""https://blobvitalhub.blob.core.windows.net/containervitalhub/logotipo.png"" alt="" Logotipo da Aplicação"" style="" display: block; margin: 0 auto; max-width: 200px;"" />
        <h1 style=""color: #333333;text-align: center;"">Recuperação de senha</h1>
        <p style=""color: #666666;font-size: 24px; text-align: center;"">Código de confirmação <strong>" + codigo + @"</strong></p>
    </div>
</div>";

            return Response;
        }
        private string GetHtmlContent(string username)
        {
            string Response = @"
<div style=""width:100%; background-color:rgba(96, 191, 197, 1); padding: 20px;"">
    <div style=""max-width: 600px; margin: 0 auto; background-color:#FFFFFF; border-radius: 10px; padding: 20px;"">
        <img src=""https://blobvitalhub.blob.core.windows.net/containervitalhub/logotipo.png"" alt="" Logotipo da Aplicação"" style="" display: block; margin: 0 auto; max-width: 200px;"" />
        <h1 style=""color: #333333; text-align: center;"">Bem-vindo ao VitalHub!</h1>
        <p style=""color: #666666; text-align: center;"">Olá <strong>" + username + @"</strong>,</p>
        <p style=""color: #666666;text-align: center"">Estamos muito felizes por você ter se inscrito na plataforma VitalHub.</p>
        <p style=""color: #666666;text-align: center"">Explore todas as funcionalidades que oferecemos e encontre os melhores médicos.</p>
        <p style=""color: #666666;text-align: center"">Se tiver alguma dúvida ou precisar de assistência, nossa equipe de suporte está sempre pronta para ajudar.</p>
        <p style=""color: #666666;text-align: center"">Aproveite sua experiência conosco!</p>
        <p style=""color: #666666;text-align: center"">Atenciosamente,<br>Equipe VitalHub</p>
    </div>
</div>";

            // Retorna o conteúdo HTML do e-mail
            return Response;
        }


        [HttpPost]
        public async Task<IActionResult> SendMail(string email, string username)
        {
            try
            {
                //Cria obj para receber os dados do e-mail a ser enviado
                MailRequest mailRequest = new MailRequest();

                //Define o endereco, assunto e corpo do email
                mailRequest.ToEmail = email;
                mailRequest.Subject = "Obrigado por se inscrever em nossa empresa!";
                mailRequest.Body = GetHtmlContent(username);

                //chamar o método para o envio do e-mail
                await emailService.SendEmailAsync(mailRequest);

                return Ok("Email enviado com sucesso");

            }
            catch (Exception)
            {
                return BadRequest("Falha ao enviar o e-mail");
                throw;
            }
        }


        [HttpPost("RecoveryEmail")]
        public async Task SendRecoveryPassword(string email, int codigo)
        {
            try
            {
                MailRequest request = new MailRequest
                {
                    ToEmail = email,
                    Subject = "Bem vindo ao VitalHub",
                    Body = GetHtmlContentRecovery(codigo)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
