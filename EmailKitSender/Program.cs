using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

class MailKitAdvancedExample
{
    static async Task Main()
    {
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress("Vereshchagin", "sender@example.com"));
        message.To.Add(new MailboxAddress("Mykeria", "recipient@example.com"));
        message.Cc.Add(new MailboxAddress("Boss copy", "cc@example.com"));
        message.Bcc.Add(new MailboxAddress("Hiden copy", "bcc@example.com"));
        
        message.Subject = "Dificult list MAIlKIT";
        
        var bodyBuilder = new BodyBuilder();
        
        bodyBuilder.TextBody = "Hi its for poor emails systems";
        
        bodyBuilder.HtmlBody = """
            <html>
            <body style="font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;">
                <div style="max-width: 600px; background: white; padding: 20px; border-radius: 8px; border: 1px solid #ddd;">
                    <h2 style="color: #2c3e50;">Hi from MailKit! 👋</h2>
                    <p>Its popular <strong>HTML</strong>-list with <em>formatyvannam</em>,created for my lab.</p>
                    <p style="color: #7f8c8d; font-size: 12px; margin-top: 20px;">Send automaticaly through MailKit + MimeKit</p>
                </div>
            </body>
            </html>
            """;
        
        string attachmentPath = "report.txt";
        if (File.Exists(attachmentPath))
        {
            bodyBuilder.Attachments.Add(attachmentPath);
        }
        else
        {
            await File.WriteAllTextAsync(attachmentPath, "Data for this project.");
            bodyBuilder.Attachments.Add(attachmentPath);
        }
        
        message.Body = bodyBuilder.ToMessageBody();
        
        using var client = new SmtpClient();
        
        try
        {
            Console.WriteLine("Connection to local server..");
            
            await client.ConnectAsync("localhost", 1025, SecureSocketOptions.None);
            
            
            Console.WriteLine("Sending email.");
            await client.SendAsync(message);
            
            Console.WriteLine("✓ [MailKit] Email was sent!");
            Console.WriteLine("Open http://localhost:8025 .");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\u274c Exception MailKit: {ex.Message}");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}