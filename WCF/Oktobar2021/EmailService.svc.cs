﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oktobar2021
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmailService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmailService.svc or EmailService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class EmailService : IEmailService
    {
        private static Dictionary<string, List<IEmailCallback>> Users
        {
            get
            {
                if (_users == null)
                    _users = new Dictionary<string, List<IEmailCallback>>();
                return _users;
            }
        }

        private static Dictionary<string, List<IEmailCallback>> _users;

        private string Nickname { get; set; }

        public EmailService()
        {
            Nickname = null;
        }
        public void Register(string nickname)
        {
            if (!Users.ContainsKey(nickname))
                Users.Add(nickname, new List<IEmailCallback>());
            Users[nickname].Add(OperationContext.Current.GetCallbackChannel<IEmailCallback>());
        }

        public void SendEmail(Email email)
        {
            email.From = Nickname;
            foreach(string s in email.For)
            {
                if(Users.ContainsKey(s))
                {
                    foreach(IEmailCallback cb in Users[s])
                    {
                        if (cb != null)
                            cb.OnEmail(email);
                    }
                }
            }
        }
    }
}
