﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.VisualBasic;
using Services.Interfaces;

namespace Services
{
    public class EmailUtilsService : IEmailUtilsService
    {
        public bool SendMail(string MsgFrom, string MsgTo, string MsgSubject, string MsgBody,
            bool MsgIsHTML = true, bool SuppressSignatureForMarketingReasons = false, List<string> AttachmentPathsList = null, string CCRecipients = "",
            bool DeliverViaAmazon = false, bool RequestReadReceipt = false, bool MsgHighPriority = false,
            bool IsExternalNotification = false)
        {
            string EmailHeaderLogo = "http://www.translateplus.com/images/flow-plus-internal.png";
            //string EmailFooterLogo = "http://www.translateplus.com/images/int_notification_logo.png";
            string FontColor = "#e84133";
            string AppName = "flow plus";
            string TMSFooterString = "<p style ='text-align:center;margin:0cm;' ><span style = 'font-size: 12.0pt;font-family:\"Arial\",\"sans-serif\";color:" + FontColor + ";'><b>A TMS by translate plus</b></span></p>" + Constants.vbCrLf +
                                                       "<p style='margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf;

            if (IsExternalNotification)
            {
                EmailHeaderLogo = "https://www.translateplus.com/images/flowplus_external_logo.png";
                //EmailFooterLogo = "http://www.translateplus.com/images/ext_notification_logo.png";
                FontColor = "#12cbec";
            }

            if (MsgFrom.Contains("myplus@translateplus.com")){
                EmailHeaderLogo = "https://www.translateplus.com/images/my-plus-RGB.png";
                AppName = "my plus";
                TMSFooterString = "";
            }

            string EmailBodyStartHTMLCode = Constants.vbCrLf + "<html><head><style>" + Constants.vbCrLf + "body {" + Constants.vbCrLf + "font-family: Arial, Helvetica, Sans-Serif; " + Constants.vbCrLf + "font-size: 10px; " + Constants.vbCrLf + "color: Black; " + Constants.vbCrLf + "background-color: White; " + Constants.vbCrLf + "margin: 0cm; " + Constants.vbCrLf + "}" + Constants.vbCrLf + String.Format("</style></head><body><p style='text-align:center;'><img border='0' id='header-logo' src='{0}' alt='header logo'></p><p style='margin:0cm;'>&nbsp;</p>", EmailHeaderLogo) + Constants.vbCrLf;
            string EmailBodyEndHTMLCode = "<p style='margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf + "<p style='margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf + "<p style='text-align:center;margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf + "<p style='text-align:center;margin:0cm;'><span style='font-size: 10.0pt; " + Constants.vbCrLf + " font-family:\"Arial\",\"sans-serif\";color:" + FontColor + String.Format(";'>This message was automatically generated by <b>{0}</b> at [[TIME]] GMT on [[DATE]].</span></a></p>", AppName) + Constants.vbCrLf + "<p style='text-align:center;margin:0cm;'><span style='font-size:12.0pt;font-family:\"Arial\",\"sans-serif\"; color:" + FontColor + ";'>---------------------------------------------------------------------------------------------" + Constants.vbCrLf + "</span></p>" + Constants.vbCrLf +
                                                       "<p style='margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf +
                                                       TMSFooterString + 
                                                       "<p style='text-align:center;margin:0cm;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img border='0' width='227' height='104' id='Logo' src='https://www.translateplus.com/images/translate%20plus-Publicis%20Groupe%20Company.png' alt='translate plus logo'></p><p style='margin:0cm;'>&nbsp;</p><br/><br/>" +
                                                       "<p style='text-align:center;margin:0cm;'><span style='font-size:9.0pt;font-family:\"Arial\",\"sans-serif\"; color:#575757'>As part of our Data Protection policy, if you no longer wish to hear from us please contact unregister@translateplus.com, so that we can update our records accordingly.</span></p>" +
                                                       "</body></html>";

            //string EmailBodyEndHTMLCode =String.Format("<p style='text-align:center;'><img border='0' id='header-logo' src='{0}' alt='header logo'></p>" +
            //                                           "<p style='margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf + "<p style='margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf + "<p style='margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size: 10.0pt; " + Constants.vbCrLf + " font-family:\"Arial\",\"sans-serif\";color:black'>This message was automatically generated by <b>i&nbsp;plus</b> at [[TIME]] GMT on [[DATE]].</span></a></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"; color:black'>&nbsp;</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"; color:black'>translate plus</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"; color:#575757'>2 Television Centre, 101 Wood Lane</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"; color:#575757'>London W12 7FR, UK</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"; color:#575757'>E-mail: </span><a href=\"mailto:iplus@translateplus.com\">" + Constants.vbCrLf + "<span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"'>iplus@translateplus.com</span></a>" + Constants.vbCrLf + "<span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\";color:#575757'>/ Web: </span><a href=\"http://www.translateplus.com/\">" + Constants.vbCrLf + "<span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\";color:#575757; text-decoration:none'>www.translateplus.com</span></a></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"; color:#575757'>Tel: +44 (0)20 7324 0950 / Fax: +44 (0)20 3642 9032</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"; color:black'>---------------------------------------------------------------------------------------------<br />" + Constants.vbCrLf + "<br /></span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><img border='0' width='227' height='84' id='Logo' src='http://www.translateplus.com/images/tplus_logo.png' alt='translate plus logo'></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:8.0pt;font-family:\"Arial\",\"sans-serif\";'>&nbsp;</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:9.0pt;font-family:\"Arial\",\"sans-serif\"; color:#575757'>Translate Plus Limited, registered in England &amp; Wales no. 6674541</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:9.0pt;font-family:\"Arial\",\"sans-serif\"; color:#575757'>Registered head office: 1st Floor, 2 Television Centre, 101 Wood Lane, London, W12 7FR, UK</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'><span style='font-size:9.0pt;font-family:\"Arial\",\"sans-serif\"; color:#575757'>As part of our Data Protection policy, if you no longer wish to hear from us please contact unregister@translateplus.com, so that we can update our records accordingly.</span></p>" + Constants.vbCrLf + "<p style='margin:0cm;'>&nbsp;</p>" + Constants.vbCrLf + "<p style='margin:0cm;'>&nbsp;</p>" +
            //                                           "<p style='text-align:center;'><img border='0' id='tms-footer' src='{1}' alt='tms footer'></p>" +
            //                                           "<p style='text-align:center;'><img border='0' id='ext-notification-logo' src='{2}' alt='ext notification'></p></body></html>",
            //                                           EmailHeaderLogo, TMSFooter, EmailFooterLogo);

            if (Strings.Trim(MsgFrom) == "")
                throw new Exception("Cannot send message with a blank From header.");

            if (Strings.Trim(MsgTo) == "")
                throw new Exception("Cannot send message with a blank To header.");

            if (Strings.Trim(MsgSubject) == "")
                throw new Exception("Cannot send message with a blank Subject header.");
            // help us to see internally that messages are send from the DEV/QA systems and not the live system
            //switch (TPCommonGeneralUtils.CurrentAppMode)
            //{
            //    case object _ when TPCommonGeneralUtils.AppModes.DEV:
            //        {
            //            MsgSubject += " [SENT FROM DEV SYSTEM, NOT PRODUCTION SYSTEM]";
            //            break;
            //        }

            //    case object _ when TPCommonGeneralUtils.AppModes.QAandCustomerWSTesting:
            //        {
            //            MsgSubject += " [SENT FROM QA/TEST SYSTEM, NOT PRODUCTION SYSTEM]";
            //            break;
            //        }
            //}

            if (Strings.Trim(MsgBody) == "")
                throw new Exception("Cannot send message with no body content.");

            // strip out any duplicate recipients from the To field
            string[] Recipients = Strings.Split(Strings.Replace(Strings.LCase(MsgTo), ";", ","), ",");
            List<string> RecipientsDeduplicated = new List<string>();
            foreach (string Recipient in Recipients)
            {
                if (RecipientsDeduplicated.Contains(Strings.Trim(Recipient)) == false)
                    RecipientsDeduplicated.Add(Strings.Trim(Recipient));
            }
            MsgTo = string.Join(",", RecipientsDeduplicated.ToArray());

            // strip out any duplicate recipients from the To field
            if (CCRecipients != "")
            {
                string[] CCRecipientsArray = Strings.Split(Strings.Replace(Strings.LCase(CCRecipients), ";", ","), ",");
                List<string> CCRecipientsDeduplicated = new List<string>();
                foreach (string CCRecipient in CCRecipientsArray)
                {
                    // also remove them from the cc line if they've already been included
                    // in the "to" line
                    if ((CCRecipientsDeduplicated.Contains(Strings.Trim(CCRecipient)) == false) & (RecipientsDeduplicated.Contains(Strings.Trim(CCRecipient)) == false))
                        CCRecipientsDeduplicated.Add(Strings.Trim(CCRecipient));
                }
                CCRecipients = string.Join(",", CCRecipientsDeduplicated.ToArray());
            }


            MsgBody = MsgBody.Replace("<table>", "<table align='center'>");

            //add the content in the div and table with single cell
            MsgBody = "<div align='center'>" + Constants.vbCrLf +
                      "<table><tr><td>" + Constants.vbCrLf +
                      MsgBody + Constants.vbCrLf +
                      "</td></tr></table>" + Constants.vbCrLf +
                      "</div>";
            // add standard style info and formatted signature if formatted
            if (MsgIsHTML == true)
            {
                // for marketing e-mails suppress the "generated by i plus" standard info 
                // and the standard "translate plus" signature, since this would typically
                // be tailored for a particular sender and would already form part of the body
                if (SuppressSignatureForMarketingReasons == true)
                    MsgBody = EmailBodyStartHTMLCode + MsgBody;
                else
                {
                    string MsgBody2 = EmailBodyEndHTMLCode;
                    if (MsgFrom == "TRANSLATE@translate.prodigiouscloud.com")
                        MsgBody2 = EmailBodyEndHTMLCode.Replace("i&nbsp;plus", "TRANSLATE").Replace("iplus@translateplus.com", "TRANSLATE@translate.prodigiouscloud.com");
                    MsgBody = EmailBodyStartHTMLCode + MsgBody + MsgBody2;
                }

                // log date and time sent if appropriate
                TimeZoneInfo tmzi;
                DateTime dt;
                // Dim ss As DateTime
                // ss = TPTimeZonesLogic.GetCurrentGMT()
                // Dim ff As DateTime
                // ff = DateTime.UtcNow
                // Dim ee As DateTime
                // ee = TPTimeZonesLogic.GetCurrentGMT().ToUniversalTime()
                tmzi = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

                dt = TimeZoneInfo.ConvertTime(GeneralUtils.GetCurrentGMT(), tmzi);

                MsgBody = Strings.Replace(MsgBody, "[[DATE]]", dt.ToString("d MMMM yyyy"));
                MsgBody = Strings.Replace(MsgBody, "[[TIME]]", dt.ToString("HH:mm"));

                // use standardised styles
                MsgBody = Strings.Replace(MsgBody, "<p>", "<p style='margin:0cm; font-size:10.0pt;font-family:\"Arial\",\"sans-serif\"; color:black'>");
                MsgBody = MsgBody.Replace("<b>", "<span style='font-size: 10.0pt; font-family:\"Arial\",\"sans-serif\";color:" + FontColor + ";'><b>");
                MsgBody = MsgBody.Replace("</b>", "</b></span>");

            }


            MailMessage MsgToSend = new MailMessage(MsgFrom, MsgTo, MsgSubject, MsgBody);

            // delete any trailing comma
            if (CCRecipients != "" && CCRecipients.LastIndexOf(",") == CCRecipients.Length - 1)
                CCRecipients = CCRecipients.Substring(0, CCRecipients.Length - 1);

            if (CCRecipients != "")
            {
                try
                {
                    MsgToSend.CC.Add(CCRecipients);
                }
                catch
                {
                }
            }


            // add any attachments if needed
            //if (AttachmentPathsList != null && AttachmentPathsList.Count > 0)
            //{
            //    foreach (string AttachmentPath in AttachmentPathsList)
            //    {
            //        // throw an error if the attachment does not exist, e.g. may not be accessible
            //        // via the network
            //        string UnmappedAttachmentPath = TPFileSystem.UnMapTPNetworkPath(AttachmentPath);
            //        if (File.Exists(UnmappedAttachmentPath) == false)
            //            throw new Exception(string.Format("Could not attach file {0} to the e-mail message because the file could not be found.", AttachmentPath));
            //        else
            //            MsgToSend.Attachments.Add(new System.Net.Mail.Attachment(UnmappedAttachmentPath));
            //    }
            //}

            MsgToSend.IsBodyHtml = MsgIsHTML;

            if (RequestReadReceipt == true)
            {
                MsgToSend.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                MsgToSend.Headers.Add("Return-Receipt-To", MsgFrom);
                MsgToSend.Headers.Add("Disposition-Notification-To", MsgFrom);
            }

            if (MsgHighPriority == true)
                MsgToSend.Priority = MailPriority.High;

            SmtpClient SendingClient = new SmtpClient();
            if (DeliverViaAmazon == true)
            {
                SendingClient.Host = "email-smtp.eu-west-1.amazonaws.com";
                SendingClient.Port = 587;
                SendingClient.Credentials = new System.Net.NetworkCredential("AKIAIKBSQJCWKRI52VKA", "AosnBw8qDsFr5F154Wcbf2AfD5ZK+WKBw+fPo28jrI8x");
                SendingClient.EnableSsl = true;
            }
            else
            {
                SendingClient.Host = "81.200.177.12";                                  // needed for server sent
                SendingClient.DeliveryMethod = SmtpDeliveryMethod.Network;                                // needed for server sent
            }

            try
            {
                MsgToSend.BodyEncoding = System.Text.Encoding.UTF8;
                SendingClient.Send(MsgToSend);
                return true;
            }
            catch (Exception ex)
            {
                // Throw New Exception(ex.Message)
                return false;
            }
            return false;
        }
    }
}
