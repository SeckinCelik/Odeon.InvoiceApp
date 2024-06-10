using System.Net.Mail;
using System.Net;
using System.Security;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using MimeKit;

namespace Odeon.InvoiceApp;

public class SmtpService
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;

    public SmtpService(string smtpServer, int port, string username, string password)
    {
        _smtpServer = smtpServer;
        _port = port;
        _username = username;
        _password = password;
    }

    public async Task SendEmailAsync(string to, string subject, string body, string[] attachmentFilePath = null)
    {
        try
        {
            using (var client = new SmtpClient(_smtpServer, _port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_username, GetSecureString(_password));
                client.EnableSsl = true;

                var message = new MailMessage(_username, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                // Attach the CSV file if provided
                if (attachmentFilePath != null)
                {
                    foreach (var file in attachmentFilePath)
                    {
                        if (File.Exists(file))
                        {
                            var attachment = new Attachment(file);
                            message.Attachments.Add(attachment);
                        }
                        else
                        {
                            throw new FileNotFoundException("Attachment file not found.", file);
                        }
                    }
                }
#if DEBUG

                string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string assemblyDirectory = System.IO.Path.GetDirectoryName(assemblyPath);
                SerializeMailMessage(message, Path.Combine(assemblyDirectory, "email_with_attachments.eml"));
#else
                  await client.SendMailAsync(message);
#endif

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
            // You can handle the exception here as needed
        }
    }
    void SerializeMailMessage(MailMessage message, string filePath)
    {
        MimeMessage mimeMessage = MimeMessage.CreateFromMailMessage(message);

        // Save MimeMessage to a file
        using (var stream = File.Create(filePath))
        {
            mimeMessage.WriteTo(stream);
        }
    }

    public SecureString GetSecureString(string password)
    {
        SecureString securePassword = new SecureString();

        foreach (char c in password)
        {
            securePassword.AppendChar(c);
        }

        // Make the SecureString read-only to prevent modification
        securePassword.MakeReadOnly();

        return securePassword;
    }
}